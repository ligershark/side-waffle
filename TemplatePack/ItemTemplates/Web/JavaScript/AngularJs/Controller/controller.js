(function () {
    'use strict';

    var controllerId = '$safeitemname$';

    // TODO: replace app with your module name
    angular.module('app')
        .controller(controllerId, ['$location', $safeitemname$]);

    function $safeitemname$($location) {
        var vm = this;

        vm.title = '$safeitemname$';

        function activate() { }
    }
})();
