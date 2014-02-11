(function () {
    'use strict';

    var controllerId = '$safeitemname$';

    // TODO: replace app with your module name
    angular.module('app').controller(controllerId,
        ['$scope', $safeitemname$]);

    function $safeitemname$($scope) {
        var vm = this;

        vm.activate = activate;
        vm.title = '$safeitemname$';

        function activate() { }
    }
})();
