(function () {
    'use strict';

    angular
        .module('app')
        .controller('$safeitemname$', ['$location', $safeitemname$]);

    function $safeitemname$($location) {
        var vm = this;
        vm.title = '$safeitemname$';

        activate();

        function activate() { }
    }
})();
