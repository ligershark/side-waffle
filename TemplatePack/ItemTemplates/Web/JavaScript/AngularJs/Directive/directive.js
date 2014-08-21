(function() {
    'use strict';

    angular
        .module('app')
        .directive('$safeitemname$', $safeitemname$);

    $safeitemname$.$inject = ['$window'];
    
    function $safeitemname$ ($window) {
        // Usage:
        // 
        // Creates:
        // 
        var directive = {
            link: link,
            restrict: 'EA'
        };
        return directive;

        function link(scope, element, attrs) {
        }
    }

})();