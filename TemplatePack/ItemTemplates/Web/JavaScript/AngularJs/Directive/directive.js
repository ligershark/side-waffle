(function() {
    'use strict';

    // TODO: replace app with your module name
    angular.module('app')
        .directive('$safeitemname$', ['$window', $safeitemname$]);
    
    function $safeitemname$ ($window) {
        // Usage:
        // 
        // Creates:
        // 
        var directive = {
            link: link,
            restrict: 'A'
        };
        return directive;

        function link(scope, element, attrs) {
        }
    }

})();