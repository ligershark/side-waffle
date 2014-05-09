(function () {
    'use strict';

    var controllerId = '$safeitemname$';

    // TODO: replace app with your module name
    angular.module('app')
        .controller(controllerId, ['$scope', $safeitemname$]);

    function $safeitemname$($scope) {
        $scope.title = '$safeitemname$';

        activate();

        function activate() { }
    }
})();
