/*
* John Papa
* http://johnpapa.net
* AngularJS Controller
**/
(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = '$safeitemname$';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('app').controller(controllerId,
        ['$scope', $safeitemname$]);

    function $safeitemname$($scope) {
        // Using 'Controller As' syntax, so we assign this to the vm variable (for viewmodel).
        var vm = this;

        // Bindable properties and functions are placed on vm.
        vm.activate = activate;

        function activate() {
        }

        //#region Internal Methods        

        //#endregion
    }
})();
