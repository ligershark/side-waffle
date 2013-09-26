(function () {
    'use strict';

    // Factory name is handy for logging
    var serviceId = '$safeitemname$';

    // Define the factory on the module.
    // Inject the dependencies. 
    // Point to the factory definition function.
    // TODO: replace app with your module name
    angular.module('app').factory(serviceId, ['$http', $safeitemname$]);

    function $safeitemname$($http) {
        // Define the functions and properties to reveal.
        var service = {
            getData: getData
        };

        return service;

        function getData() {

        }

        //#region Internal Methods        

        //#endregion
    }
})();