// Update the reference to app1.ts to be that of your module file.
// Install the angularjs.TypeScript.DefinitelyTyped NuGet package to resovle the .d.ts reference paths,
// then adjust the path value to be relative to this file
/// <reference path="app1.ts" />
/// <reference path='../Scripts/typings/angularjs/angular.d.ts'/>
/// <reference path='../Scripts/typings/angularjs/angular-resource.d.ts'/>
var $safeitemname$ = (function () {
    function $safeitemname$($scope, $http, $resource) {
        var _this = this;
        this.$scope = $scope;
        this.$http = $http;
        this.$resource = $resource;
        $scope.greeting = "Hello";
        $scope.changeGreeting = function () {
            return _this.changeGreeting();
        };
    }
    $safeitemname$.prototype.changeGreeting = function () {
        this.$scope.greeting = "Bye";
    };
    $safeitemname$.controllerId = "$safeitemname$";
    return $safeitemname$;
})();

// Update the app variable name to be that of your module variable
app.controller($safeitemname$.controllerId, [
    '$scope',
    '$http',
    '$resource',
    function ($scope, $http, $resource) {
        return new $safeitemname$($scope, $http, $resource);
    }
]);
