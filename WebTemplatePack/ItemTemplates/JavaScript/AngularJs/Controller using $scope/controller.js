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
        // Bindable properties and functions are placed on vm.
        $scope.title = '$safeitemname$';
        $scope.activate = activate;

        function activate() {
        }

        //#region Internal Methods        

        //#endregion
    }
})();
