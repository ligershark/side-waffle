(function () {
    'use strict';

    angular
        .module('app')
        .factory('$safeitemname$', ['$http', $safeitemname$]);

    function $safeitemname$($http) {
        var service = {
            getData: getData
        };

        return service;

        function getData() { }
    }
})();