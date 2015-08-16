(function () {
    'use strict';

    angular
        .module('app')
        .filter('$safeitemname$', $safeitemname$);
    
    function $safeitemname$() {
        return $safeitemname$Filter;

        function $safeitemname$Filter(input) {
            return input;
        }
    }
})();