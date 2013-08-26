/*
* John Papa
* http://johnpapa.net
* AngularJS Controller File
**/
(function () {
    'use strict';

    // Controller name is handy inside of a controller for logging
    var controllerId = 'customController';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('app').controller(controllerId,
        ['$scope', customController]);

    function customController($scope) {
        // Using 'Controller As' syntax, so we assign this to the vm variable (for viewmodel).
        var vm = this;

        // Bindable properties and functions are plaed on vm.
        vm.activate = activate;

        function activate() {
        }

        //#region Internal Methods        

        //#endregion
    }
})();
