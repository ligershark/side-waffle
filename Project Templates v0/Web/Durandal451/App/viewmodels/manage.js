define(['plugins/router', 'services/security', 'global/session', 'services/logger', 'jquery', 'knockout', 'jquery.utilities', 'knockout.validation'],
    function (router, security, session, logger, $, ko) {
        // Internal properties and functions
        function updateExternalLoginProviderStatus(name, isInUse)
        {
            $.each(vm.externalLoginProviders(), function (i, v) {
                if (v.name() === name) {
                    v.isInUse(isInUse);
                }
            });
        }

        function AddExternalLoginProviderViewModel(data) {
            var self = this;

            // Data
            self.name = ko.observable(data.name);
            self.isInUse = ko.observable(data.isInUse);

            // Operations
            self.login = function () {
                sessionStorage["state"] = data.state;
                sessionStorage["associatingExternalLogin"] = true;
                // IE doesn't reliably persist sessionStorage when navigating to another URL. Move sessionStorage temporarily
                // to localStorage to work around this problem.
                session.archiveSessionStorageToLocalStorage();
                window.location = data.url;
            };
        }

        function addExternalLogin(externalAssociationResult) {
            var dfd = $.Deferred();

            if (externalAssociationResult.externalError !== null || externalAssociationResult.externalAccessToken === null) {
                logger.log({
                    message: "Error associating external login: " + externalAssociationResult.externalError,
                    data: externalAssociationResult.externalError,
                    showToast: true,
                    type: "error"
                });              
                dfd.resolve();
            } else {
                security.addExternalLogin(externalAssociationResult)
                    .done(function (data) {
                        dfd.resolve();
                    })
                    .failJSON(function (data) {
                        var errors = security.toErrorString(data);
                                               
                        if (errors) {
                            logger.log({
                                message: "One or more errors occurred associating external login:  " + errors,
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

                        dfd.resolve();
                    });
            }

            return dfd.promise();
        }

        function SetPasswordViewModel(parent) {
            var self = this;

            // Data
            self.newPassword = ko.observable("").extend({ required: true });
            self.confirmPassword = ko.observable("").extend({ required: true, equal: self.newPassword });

            // Other UI state
            self.validationErrors = ko.validation.group([self.newPassword, self.confirmPassword]);

            // Operations
            self.set = function () {           
                session.isBusy(true);

                security.setPassword({
                    newPassword: self.newPassword(),
                    confirmPassword: self.confirmPassword()
                }).done(function (data) {                    
                    parent.logins.push(new RemoveLoginViewModel({
                        loginProvider: parent.localLoginProvider(),
                        providerKey: parent.userName()
                    }, parent));
                    logger.log({
                        message: "Your password has been set.",                        
                        showToast: true,
                        type: "info"
                    });                    
                }).always(function () {
                    session.isBusy(false);
                }).failJSON(function (data) {
                    var errors = security.toErrorString(data);

                    if (errors) {
                        logger.log({
                            message: "One or more errors occurred setting the password:  " + errors,
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
            };
        }

        function RemoveLoginViewModel(data, parent) {
            // Private state
            var vm = this,
                providerKey = ko.observable(data.providerKey);

            // Data
            vm.loginProvider = ko.observable(data.loginProvider);

            // Operations
            vm.remove = function () {                
                session.isBusy(true);
                security.removeLogin({
                    loginProvider: vm.loginProvider(),
                    providerKey: providerKey()
                }).done(function (data) {
                    parent.logins.remove(vm);
                    updateExternalLoginProviderStatus(vm.loginProvider(), false);

                    logger.log({
                        message: "The login was removed.",                        
                        showToast: true,
                        type: "info"
                    });                    
                }).always(function () {
                    session.isBusy(false);
                }).failJSON(function (data) {
                    var errors = security.toErrorString(data);

                    if (errors) {
                        logger.log({
                            message: "One or more errors occurred removing the login:  " + errors,
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
            };
        }

        function ChangePasswordViewModel(parent, name) {
            var self = this;

            // Private operations
            function reset() {                
                self.oldPassword(null);
                self.newPassword(null);
                self.confirmPassword(null);                
                self.validationErrors.showAllMessages(false);
            }

            // Data
            self.name = ko.observable(name);
            self.oldPassword = ko.observable("").extend({ required: true });
            self.newPassword = ko.observable("").extend({ required: true });
            self.confirmPassword = ko.observable("").extend({ required: true, equal: self.newPassword });

            self.validationErrors = ko.validation.group([self.oldPassword, self.newPassword, self.confirmPassword]);

            // Operations
            self.change = function () {     
                if (self.validationErrors().length > 0) {
                    self.validationErrors.showAllMessages();
                    return;
                }
                session.isBusy(true);

                security.changePassword({
                    oldPassword: self.oldPassword(),
                    newPassword: self.newPassword(),
                    confirmPassword: self.confirmPassword()
                }).done(function (data) {                    
                    reset();
                    logger.log({
                        message: "Your password has been changed.",                        
                        showToast: true,
                        type: "info"
                    });
                    parent.message("Your password has been changed.");
                }).always(function () {
                    session.isBusy(false);
                }).failJSON(function (data) {
                    var errors = security.toErrorString(data);

                    if (errors) {
                        logger.log({
                            message: "One or more errors occurred setting the password:  " + errors,
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
            };
        }

        function load() {
            var dfd = $.Deferred();

            security.getManageInfo(security.returnUrl, true /* generateState */)
               .done(function (data) {
                   if (typeof (data.localLoginProvider) !== "undefined" &&
                       typeof (data.userName) !== "undefined" &&
                       typeof (data.logins) !== "undefined" &&
                       typeof (data.externalLoginProviders) !== "undefined") {
                       vm.userName(data.userName);
                       vm.localLoginProvider(data.localLoginProvider);

                       for (var i = 0; i < data.logins.length; i++) {
                           vm.logins.push(new RemoveLoginViewModel(data.logins[i], vm));
                       }
                       
                       for (var i = 0; i < data.externalLoginProviders.length; i++) {
                           

                           data.externalLoginProviders[i].isInUse = $.arrayContains(data.logins, function (item) {
                               return item.loginProvider === data.externalLoginProviders[i].name;
                           });

                           vm.externalLoginProviders.push(new AddExternalLoginProviderViewModel(
                               data.externalLoginProviders[i]));                           
                       }

                       dfd.resolve();
                   } else {
                       logger.log({
                           message: "Error retrieving user information.",                           
                           showToast: true,
                           type: "warning"
                       });

                       dfd.reject();
                   }                  
               }).always(function () {
                   session.isBusy(false);
               }).failJSON(function (data) {
                   var errors = security.toErrorString(data);

                   if (errors) {
                       logger.log({                                                              
                           message: "Error retrieving user information.",
                           data: errors,
                           showToast: true,
                           type: "error"
                       });                       
                   } else {
                       logger.log({
                           message: "Error retrieving user information.",                          
                           showToast: true,
                           type: "error"
                       });                       
                   }

                   dfd.reject();
               });

            return dfd.promise();
        }

        // Reveal the bindable properties and functions
        var vm = {
            activate: activate,
            goBack: goBack,
            title: 'manage',
            session: session,
            userName: ko.observable(),
            logins: ko.observableArray(),
            localLoginProvider: ko.observable(),
            externalLoginProviders: ko.observableArray(),            
            message: ko.observable(),       
        };

        vm.hasLocalPassword = ko.computed(function () {
            var logins = vm.logins();

            for (var i = 0; i < logins.length; i++) {
                if (logins[i].loginProvider() === vm.localLoginProvider()) {
                    return true;
                }
            }

            return false;
        });

        vm.changePassword = ko.computed(function () {
            if (!vm.hasLocalPassword()) {
                return null;
            }

            return new ChangePasswordViewModel(vm, vm.userName());
        });

        vm.setPassword = ko.computed(function () {
            if (vm.hasLocalPassword()) {
                return null;
            }

            return new SetPasswordViewModel(vm, security);
        });

        vm.hasExternalLogin = ko.computed(function () {
            return vm.externalLoginProviders().length > 0;
        });

        vm.canRemoveLogin = ko.computed(function () {
            return vm.logins().length > 1;
        });

        return vm;

        function activate() {

            session.isBusy(true);

            vm.externalLoginProviders.removeAll();
            vm.logins.removeAll();
            
            if (sessionStorage["externalAssociationResult"]) {
                var externalAssociationResult = JSON.parse(sessionStorage["externalAssociationResult"]);
                sessionStorage.removeItem("externalAssociationResult");

                addExternalLogin(externalAssociationResult).done(function () {
                    return load();
                });
            } else {
                return load();
            }
        }

        function goBack(complete) {
            router.navigateBack();
        }
    });