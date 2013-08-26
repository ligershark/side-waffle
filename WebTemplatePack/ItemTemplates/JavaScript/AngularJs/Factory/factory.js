/*
* John Papa
* http://johnpapa.net
* AngularJS Factory File
**/

(function () {
    'use strict';

    // Factory name is handy inside of a factory for logging
    var serviceId = '$safeitemname$Service';

    // Define the factory on the module.
    // Inject the dependencies. 
    // Point to the factory definition function.
    angular.module('app').factory(serviceId, ['config', $safeitemname$Service]);

    function $safeitemname$Service(config) {
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