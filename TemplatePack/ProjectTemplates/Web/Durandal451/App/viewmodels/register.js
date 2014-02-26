define(['plugins/router', 'durandal/app', 'services/security', 'global/session', 'services/logger', 'jquery', 'knockout', 'knockout.validation'],
    function (router, app, security, session,logger, $, ko) {

        // Internal properties and functions
        function ExternalLoginProviderViewModel(data) {
            var self = this;

            // Data
            self.name = ko.observable(data.name);

            // Operations
            self.login = function () {
                sessionStorage["state"] = data.state;
                sessionStorage["loginUrl"] = data.url;
                sessionStorage["externalLogin"] = true;
                // IE doesn't reliably persist sessionStorage when navigating to another URL. Move sessionStorage temporarily
                // to localStorage to work around this problem.
                session.archiveSessionStorageToLocalStorage();
                window.location = data.url;
            };
        }

        // Reveal the bindable properties and functions
        var vm = {
            activate: activate,
            goBack: goBack,
            title: 'register',
            session: session,
            userName: ko.observable("").extend({ required: true }),
            password: ko.observable("").extend({ required: true }),                  
            register: register,
            externalLoginProviders: ko.observableArray(),
            loaded: false,
            login: login
        };

        vm.confirmPassword = ko.observable("").extend({ required: true, equal: vm.password });
        vm.validationErrors = ko.validation.group([vm.userName, vm.password, vm.confirmPassword]);

        return vm;

        function activate() {
            var dfd = $.Deferred();

            session.isBusy(true);

            if (!vm.loaded) {
                security.getExternalLogins(security.returnUrl, true /* generateState */)
                   .done(function (data) {
                       if (typeof (data) === "object") {
                           for (var i = 0; i < data.length; i++) {
                               vm.externalLoginProviders.push(new ExternalLoginProviderViewModel(data[i]));
                           }

                           vm.loaded = true;
                       } else {
                           logger.log({
                               message: "Error loading external authentication providers.",
                               data: "",
                               showToast: true,
                               type: "warning"
                           });

                           vm.loaded = false;
                       }
                   }).fail(function () {
                       logger.log({
                           message: "Error loading external authentication providers.",
                           data: "",
                           showToast: true,
                           type: "warning"
                       });

                       vm.loaded = false;
                   }).always(function () {
                       session.isBusy(false);
                       return dfd.resolve();
                   });

                return dfd.promise();
            }
            else {
                session.isBusy(false);
                return dfd.resolve();
            }
        }

        function register() {            

            if (vm.validationErrors().length > 0) {
                vm.validationErrors.showAllMessages();
                return;
            }

            session.isBusy(true);

            security.register({
                userName: vm.userName(),
                password: vm.password(),
                confirmPassword: vm.confirmPassword()
            }).done(function (data) {
                security.login({
                    grant_type: "password",
                    username: vm.userName(),
                    password: vm.password()
                }).done(function (data) {                    
                    if (data.userName && data.access_token) {
                        session.setUser(data);
                        router.navigate('#/', 'replace');
                    } else {
                        logger.log({
                            message: "An unknown error occurred.",                            
                            showToast: true,
                            type: "error"
                        });                        
                    }
                }).always(function () {
                    session.isBusy(false);
                }).failJSON(function (data) {                    
                    if (data && data.error_description) {
                        logger.log({
                            message: "An error occurred: " + data.error_description,
                            data: data.error_description,
                            showToast: true,
                            type: "error"
                        });
                        vm.errors.push(data.error_description);
                    } else {
                        logger.log({
                            message: "An unknown error occurred.",
                            showToast: true,
                            type: "error"
                        });
                    }
                });
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

        function login() {
            router.navigate('#/login', 'replace');
        }

        function goBack(complete) {
            router.navigateBack();
        }
    });