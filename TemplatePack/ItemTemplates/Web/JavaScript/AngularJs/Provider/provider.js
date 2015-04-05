(function () {
    'use strict';

    angular
        .module('app')
        .provider('$safeitemname$', $safeitemname$Provider);

    function $safeitemname$Provider() {
        var configValue = false;

        this.setConfigValue = function (value) {
            configValue = value;
        };

        this.$get = $safeitemname$Factory;

        $safeitemname$Factory.$inject = ['$http'];
        function $safeitemname$Factory($http) {
            var service = {
                getData: getData
            };

            return service;
            

            function getData() {                
                return configValue;
            }
        }
    }
})();