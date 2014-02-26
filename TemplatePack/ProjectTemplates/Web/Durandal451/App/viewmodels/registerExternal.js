define(['plugins/router', 'durandal/app', 'services/security', 'global/session', 'services/logger', 'jquery', 'knockout', 'knockout.validation'],
    function (router, app, security, session, logger, $, ko) {
        // Internal properties and functions

        // Reveal the bindable properties and functions
        var vm = {
            activate: activate,
            goBack: goBack,
            title: 'registerExternal',
            session: session,
            loginProvider: ko.observable(),
            userName: ko.observable(null).extend({ required: true }),    
            externalAccessToken: null,
            state: null,
            loginUrl: null,     
            register: register
        };

        vm.validationErrors = ko.validation.group([vm.userName]);

        return vm;

        function activate() {            

            if (sessionStorage["registerExternal"]) {
                var registerExternal = JSON.parse(sessionStorage["registerExternal"]);
                sessionStorage.removeItem("registerExternal");

                vm.loginProvider(registerExternal.loginProvider);
                vm.userName(registerExternal.userName);
                vm.externalAccessToken = registerExternal.externalAccessToken;
                vm.state = registerExternal.state;
                vm.loginUrl = registerExternal.loginUrl;
            }
            else {
                router.navigate("#/login", "replace");
            }
        }

        function goBack() {
            router.navigateBack();
        }

        function register() {           

            if (vm.validationErrors().length > 0) {
                vm.validationErrors.showAllMessages();
                return;
            }

            session.isBusy(true);

            security.registerExternal(vm.externalAccessToken, {
                userName: vm.userName()
            }).done(function (data) {
                sessionStorage["state"] = vm.state;
                // IE doesn't reliably persist sessionStorage when navigating to another URL. Move sessionStorage
                // temporarily to localStorage to work around this problem.
                session.archiveSessionStorageToLocalStorage();
                window.location = vm.loginUrl;
            }).always(function () {
                session.isBusy(false);
            }).failJSON(function (data) {
                var errors = security.toErrorString(data);

                if (errors) {
                    logger.log({
                        message: "One or more errors occurred:  " + errors,
                        data: errors,
                        showToast: true,
                        type: "error"
                    });
                } else {
                    logger.log({
                        message: "An unknown error occurred.",
                        showToast: true,
                        type: "error"
                    });
                }
            });
        }
    });