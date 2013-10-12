// Import the angularjs.TypeScript.DefinitelyTyped NuGet package to resovle the reference paths
/// <reference path='../Scripts/typings/angularjs/angular.d.ts'/>
/// <reference path='../Scripts/typings/angularjs/angular-resource.d.ts'/>

interface I$safeitemname$Scope extends ng.IScope {
    vm: $safeitemname$;
}

interface II$safeitemname$ {
    greeting: string;
    controllerId: string;
    changeGreeting: () => void;
}

class $safeitemname$ implements I$safeitemname$ {

    static $safeitemname$Id: string = "$safeitemname$";
    greeting = "Hello";

    constructor(private $scope: I$safeitemname$Scope, private $http: ng.IHttpService, private $resource: ng.resource.IResourceService) {
    }

    changeGreeting() {
        this.greeting = "Bye";
    }
}

app.controller($safeitemname$.$safeitemname$Id, function ($scope, $http, $resource) {
    return new $safeitemname$($scope, $http, $resource);
});
