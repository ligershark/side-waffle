/*
* John Papa
* http://johnpapa.net
* AngularJS Factory
**/

(function () {
    'use strict';

    // Factory name is handy for logging
    var serviceId = '$safeitemname$';

    // Define the factory on the module.
    // Inject the dependencies. 
    // Point to the factory definition function.
    angular.module('app').factory(serviceId, ['config', $safeitemname$]);

    function $safeitemname$(config) {
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