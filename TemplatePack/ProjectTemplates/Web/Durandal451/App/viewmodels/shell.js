define(['plugins/router', 'durandal/app', 'global/session', 'services/security', 'services/logger', 'jquery', 'jquery.utilities'],
    function (router, app, session, security, logger, $) {
    
    function verifyStateMatch(fragment) {
        var state;

        if (typeof (fragment.access_token) !== "undefined") {
            state = sessionStorage["state"];
            sessionStorage.removeItem("state");

            if (state === null || fragment.state !== state) {
                fragment.error = "invalid_state";
            }
        }
    }

    function setupRouter() {
        router.map([
                { route: '', title: 'Welcome', moduleId: 'viewmodels/welcome', nav: true},
                { route: 'welcome', title: 'Welcome', moduleId: 'viewmodels/welcome' },                
                { route: 'register', moduleId: 'viewmodels/register', nav: false},
                { route: 'login', moduleId: 'viewmodels/login', nav: false},
                { route: 'registerExternal', moduleId: 'viewmodels/registerExternal', nav: false},
                { route: 'manage', moduleId: 'viewmodels/manage', nav: false, requiredRoles: ['RegisteredUsers'] },
                { route: 'start', title: 'Get started', moduleId: 'viewmodels/start', nav: true, requiredRoles: ['RegisteredUsers']}
        ]).buildNavigationModel();

        router.guardRoute = function (routeInfo, params, instance) {
            if (typeof (params.config.requiredRoles) !== "undefined") {
                var res = session.userIsInRole(params.config.requiredRoles);

                if (!res)
                {
                    logger.log({
                        message: "Access denied. Navigation cancelled.",
                        showToast: true,
                        type: "warning"
                    });
                }

                return res;
            } else {
                return true;
            }

        };

        return router.activate();
    }

    function init() {
        var dfd = $.Deferred(), fragment = $.getFragment(), externalAccessToken, externalError, loginUrl;

        verifyStateMatch(fragment);

        window.location.hash = "";

        if (sessionStorage["associatingExternalLogin"]) {
            sessionStorage.removeItem("associatingExternalLogin");

            var externalAssociationResult = {};

            if (typeof (fragment.error) !== "undefined") {
                externalAssociationResult.externalAccessToken = null;
                externalAssociationResult.externalError = fragment.error;
            } else if (typeof (fragment.access_token) !== "undefined") {
                externalAssociationResult.externalAccessToken = fragment.access_token;
                externalAssociationResult.externalError = null;
            } else {
                externalAssociationResult.externalAccessToken = null;
                externalAssociationResult.externalError = null;
            }

            //save this for the manage VM to use
            sessionStorage["externalAssociationResult"] = JSON.stringify(externalAssociationResult);

            security.getUserInfo()
                .done(function (data) {
                    if (data.userName) {
                        session.setUser(data);

                        window.location.href = "#manage";
                        setupRouter().done(function () {
                            dfd.resolve();
                        });
                    } else {
                        logger.log({
                            message: "Error retrieving user information.",
                            showToast: true,
                            type: "error"
                        });

                        window.location.href = "#login";
                        setupRouter().done(function () {
                            dfd.resolve();
                        });
                    }
                })
                .fail(function () {
                    logger.log({
                        message: "Error retrieving user information.",
                        showToast: true,
                        type: "error"
                    });

                    window.location.href = "#login";
                    setupRouter().done(function () {
                        dfd.resolve();
                    });
                });
        } else if (typeof (fragment.error) !== "undefined") {
            logger.log({
                message: "External login failed.",
                showToast: true,
                type: "error"
            });

            window.location.href = "#login";
            setupRouter().done(function () {
                dfd.resolve();
            });

        } else if (typeof (fragment.access_token) !== "undefined") {
            security.getUserInfo(fragment.access_token)
                .done(function (data) {
                    if (typeof (data.userName) !== "undefined" && typeof (data.hasRegistered) !== "undefined"
                        && typeof (data.loginProvider) !== "undefined") {
                        if (data.hasRegistered) {
                            data.accessToken = fragment.access_token;
                            session.setUser(data, false);
                            setupRouter().done(function () {
                                dfd.resolve();
                            });
                        } else if (typeof (sessionStorage["loginUrl"]) !== "undefined") {
                            sessionStorage["registerExternal"] = JSON.stringify({
                                userName: data.userName,
                                loginProvider: data.loginProvider,
                                externalAccessToken: fragment.access_token,
                                loginUrl: sessionStorage["loginUrl"],
                                state: fragment.state
                            });

                            sessionStorage.removeItem("loginUrl");

                            window.location.href = "#registerExternal";

                            setupRouter().done(function () {
                                dfd.resolve();
                            });
                        } else {
                            logger.log({
                                message: "Login failed.",
                                showToast: true,
                                type: "error"
                            });

                            window.location.href = "#login";
                            setupRouter().done(function () {
                                dfd.resolve();
                            });
                        }
                    } else {
                        logger.log({
                            message: "Login failed.",
                            showToast: true,
                            type: "error"
                        });

                        window.location.href = "#login";
                        setupRouter().done(function () {
                            dfd.resolve();
                        });
                    }
                })
                .fail(function () {
                    logger.log({
                        message: "Login failed.",
                        showToast: true,
                        type: "error"
                    });

                    window.location.href = "#login";
                    setupRouter().done(function () {
                        dfd.resolve();
                    });
                });
        } else if (session.rememberedToken()) {
            security.getUserInfo()
                .done(function (data) {
                    if (data.userName) {
                        session.setUser(data);
                        setupRouter().done(function () {
                            dfd.resolve();
                        });
                    } else {
                        logger.log({
                            message: "Login failed.",
                            showToast: true,
                            type: "error"
                        });

                        window.location.href = "#login";
                        setupRouter().done(function () {
                            dfd.resolve();
                        });
                    }
                })
                .fail(function () {
                    logger.log({
                        message: "Login failed.",
                        showToast: true,
                        type: "error"
                    });

                    window.location.href = "#login";
                    setupRouter().done(function () {
                        dfd.resolve();
                    });
                });
        } else {
            setupRouter().done(function () {
                dfd.resolve();
            });
        }

        return dfd.promise();
    }

    return {
        router: router,
        session: session,
        search: function () {
            //It's really easy to show a message box.
            //You can add custom options too. Also, it returns a promise for the user's response.
            app.showMessage('Search not yet implemented...');
        },
        activate: function () {
            return init();
        },
        logout: function () {
            security.logout().done(function () {
                logger.log({
                    message: "Signed out.",
                    showToast: true,
                    type: "info"
                });
            }).fail(function () {
                logger.log({
                    message: "Logout failed.",
                    showToast: true,
                    type: "error"
                });
            }).always(function () {
                session.clearUser();
                router.navigate('#/welcome', 'replace');
            });
        }
    };
});