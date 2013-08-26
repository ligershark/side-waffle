/*
* John Papa
* http://johnpapa.net
* AngularJS App Module File
**/

(function () {
    'use strict';

    // Module name is handy for logging
    var id = '$safeitemname$';

    // Create the module and define its dependencies.
    var $safeitemname$ = angular.module('$safeitemname$', [
        // Angular modules 
        'ngAnimate',        // animations
        'ngRoute'           // routing

        // Custom modules 

        // 3rd Party Modules
        
    ]);

    // Execute bootstrapping code and any dependencies.
    $safeitemname$.run(['$q', '$rootScope',
        function ($q, $rootScope) {

        }]);
})();