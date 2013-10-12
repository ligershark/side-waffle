// Import the angularjs.TypeScript.DefinitelyTyped NuGet package to resovle the reference paths
/// <reference path='../Scripts/typings/angularjs/angular.d.ts'/>
/// <reference path='../Scripts/typings/angularjs/angular-resource.d.ts'/>

interface I$safeitemname$ extends ng.IModule { }

// Create the module and define its dependencies.
var $safeitemname$: I$safeitemname$ = angular.module('$safeitemname$', [
    // Angular modules 
    'ngResource',       // $resource for REST queries
    'ngAnimate',        // animations
    'ngRoute'           // routing

    // Custom modules 

    // 3rd Party Modules
]);

// Execute bootstrapping code and any dependencies.
$safeitemname$.run(['$q', '$rootScope',
    function ($q, $rootScope) {

    }]);
