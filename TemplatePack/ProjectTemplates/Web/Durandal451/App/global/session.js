define(['plugins/router','services/security','knockout','jquery','jquery.utilities'],
    function (router,security, ko, $) {

        function restoreSessionStorageFromLocalStorage() {
            var backupText = localStorage["sessionStorageBackup"],
                backup;

            if (backupText) {
                backup = JSON.parse(backupText);

                for (var key in backup) {
                    sessionStorage[key] = backup[key];
                }

                localStorage.removeItem("sessionStorageBackup");
            }
        }

        function setAccessToken(accessToken, persistent) {
            if (persistent) {
                localStorage["accessToken"] = accessToken;
            } else {
                sessionStorage["accessToken"] = accessToken;
            }
        }
            
        function clearAccessToken() {
            localStorage.removeItem("accessToken");
            sessionStorage.removeItem("accessToken");
        }

        function init() {
            restoreSessionStorageFromLocalStorage();
        }

        init();

        var session = {
            userName: ko.observable(undefined),
            isLoggedIn: ko.observable(false),
            isBusy: ko.observable(false),
            userRoles: ko.observableArray(),
            userIsInRole: userIsInRole,
            setUser: setUser,
            clearUser: clearUser,
            archiveSessionStorageToLocalStorage: archiveSessionStorageToLocalStorage,
            isAuthCallback: isAuthCallback,
            userRemembered: userRemembered,
            rememberedToken: rememberedToken
        };

        return session;

        function setUser(user, remember) {
            if (user) {

                session.userName(user.userName);
                
                if (user.hasOwnProperty("accessToken")) {
                    setAccessToken(user.accessToken, remember);
                } else if (user.hasOwnProperty("access_token")) {
                    setAccessToken(user.access_token, remember);
                }
                
                var roles = user.userRoles.split(",");                

                $.each(roles, function (i, v) {
                    session.userRoles.push(v);
                });

                session.isLoggedIn(true);                
            }
        }

        function clearUser() {
            clearAccessToken();
            session.userName('');
            session.userRoles.removeAll();
            session.isLoggedIn(false);
        }

        function userIsInRole(requiredRole)
        {
            if (requiredRole === undefined) {
                return true;
            } else if (session.userRoles() === undefined) {
                return false;
            } else {
                if ($.isArray(requiredRole)) {
                    if (requiredRole.length === 0) {
                        return true;
                    } else {
                        return $.arrayIntersect(session.userRoles(), requiredRole).length > 0;
                    }
                } else {
                    return $.inArray(requiredRole, session.userRoles()) > -1;
                }
            }                       
        }

        function isAuthCallback() {
            return sessionStorage["associatingExternalLogin"] || sessionStorage["externalLogin"];
        }

        function userRemembered() {
            return sessionStorage["accessToken"] !== undefined || localStorage["accessToken"] !== undefined;
        }

        function rememberedToken() {
            return sessionStorage["accessToken"] || localStorage["accessToken"];
        }
     
        function redirectCallback(redirectToManage) {
            if (redirectToManage) {
                router.navigate('#/manage', 'replace');
            } else {
                router.navigate('#/', 'replace');
            }
        }

        function archiveSessionStorageToLocalStorage() {
            var backup = {};

            for (var i = 0; i < sessionStorage.length; i++) {
                backup[sessionStorage.key(i)] = sessionStorage[sessionStorage.key(i)];
            }

            localStorage["sessionStorageBackup"] = JSON.stringify(backup);
            sessionStorage.clear();
        }      

    });