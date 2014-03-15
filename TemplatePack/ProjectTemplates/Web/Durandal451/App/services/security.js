define(['jquery'],
    function ($) {
        // Routes
        addExternalLoginUrl = "/api/Account/AddExternalLogin",
        changePasswordUrl = "/api/Account/changePassword",
        loginUrl = "/Token",
        logoutUrl = "/api/Account/Logout",
        registerUrl = "/api/Account/Register",
        registerExternalUrl = "/api/Account/RegisterExternal",
        removeLoginUrl = "/api/Account/RemoveLogin",
        setPasswordUrl = "/api/Account/setPassword",
        siteUrl = "/",
        userInfoUrl = "/api/Account/UserInfo";

        // Route operations
        function externalLoginsUrl(returnUrl, generateState) {
            return "/api/Account/ExternalLogins?returnUrl=" + (encodeURIComponent(returnUrl)) +
                "&generateState=" + (generateState ? "true" : "false");
        }

        function manageInfoUrl(returnUrl, generateState) {
            return "/api/Account/ManageInfo?returnUrl=" + (encodeURIComponent(returnUrl)) +
                "&generateState=" + (generateState ? "true" : "false");
        }

        // Other private operations
        function getSecurityHeaders() {
            var accessToken = sessionStorage["accessToken"] || localStorage["accessToken"];

            if (accessToken) {
                return { "Authorization": "Bearer " + accessToken };
            }

            return {};
        }

        function toErrorString(data) {
            var errors, items;

            if (!data || !data.message) {
                return null;
            }

            if (data.modelState) {
                for (var key in data.modelState) {
                    items = data.modelState[key];

                    if (items.length) {
                        for (var i = 0; i < items.length; i++) {
                            errors += ", " + items[i];
                        }
                    }
                }
            }

            if (errors === undefined) {
                errors = data.message;
            }

            return errors;
        };

        var securityService = {
            addExternalLogin: addExternalLogin,
            changePassword: changePassword,
            getExternalLogins: getExternalLogins,
            getManageInfo: getManageInfo,
            getUserInfo: getUserInfo,
            login: login,
            logout: logout,
            register: register,
            registerExternal: registerExternal,
            removeLogin: removeLogin,
            setPassword: setPassword,
            returnUrl: siteUrl,
            toErrorString: toErrorString
        };

        $.ajaxPrefilter(function (options, originalOptions, jqXHR) {
            jqXHR.failJSON = function (callback) {
                jqXHR.fail(function (jqXHR, textStatus, error) {
                    var data;

                    try {
                        data = $.parseJSON(jqXHR.responseText);
                    }
                    catch (e) {
                        data = null;
                    }

                    callback(data, textStatus, jqXHR);
                });
            };
        });

        return securityService;
  
        // Data access operations
        function addExternalLogin(data) {
            return $.ajax(addExternalLoginUrl, {
                type: "POST",
                data: data,
                headers: getSecurityHeaders()
            });
        };

        function changePassword(data) {
            return $.ajax(changePasswordUrl, {
                type: "POST",
                data: data,
                headers: getSecurityHeaders()
            });
        };

        function getExternalLogins(returnUrl, generateState) {
            return $.ajax(externalLoginsUrl(returnUrl, generateState), {
                cache: false,
                headers: getSecurityHeaders()
            });
        };

        function getManageInfo(returnUrl, generateState) {
            return $.ajax(manageInfoUrl(returnUrl, generateState), {
                cache: false,
                headers: getSecurityHeaders()
            });
        };

        function getUserInfo(accessToken) {
            var headers;

            if (typeof (accessToken) !== "undefined") {
                headers = {
                    "Authorization": "Bearer " + accessToken
                };
            } else {
                headers = getSecurityHeaders();
            }

            return $.ajax(userInfoUrl, {
                cache: false,
                headers: headers
            });
        };

        function login(data) {
            return $.ajax(loginUrl, {
                type: "POST",
                data: data
            });
        };

        function logout() {
            return $.ajax(logoutUrl, {
                type: "POST",
                headers: getSecurityHeaders()
            });
        };

        function register(data) {
            return $.ajax(registerUrl, {
                type: "POST",
                data: data
            });
        };

        function registerExternal(accessToken, data) {
            return $.ajax(registerExternalUrl, {
                type: "POST",
                data: data,
                headers: {
                    "Authorization": "Bearer " + accessToken
                }
            });
        };

        function removeLogin(data) {
            return $.ajax(removeLoginUrl, {
                type: "POST",
                data: data,
                headers: getSecurityHeaders()
            });
        };

        function setPassword(data) {
            return $.ajax(setPasswordUrl, {
                type: "POST",
                data: data,
                headers: getSecurityHeaders()
            });
        };
    });