(function () {
    'use strict';

    angular
        .module('app')
        .controller('$safeitemname$', $safeitemname$);

    $safeitemname$.$inject = ['$scope']; 

    function $safeitemname$($scope) {
        $scope.title = '$safeitemname$';

        activate();

        function activate() { }
    }
})();
