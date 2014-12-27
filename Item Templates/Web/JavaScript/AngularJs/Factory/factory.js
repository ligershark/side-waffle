(function () {
    'use strict';

    angular
        .module('app')
        .factory('$safeitemname$', $safeitemname$);

    $safeitemname$.$inject = ['$http'];

    function $safeitemname$($http) {
        var service = {
            getData: getData
        };

        return service;

        function getData() { }
    }
})();