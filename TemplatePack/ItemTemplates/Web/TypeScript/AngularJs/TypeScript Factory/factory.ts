// Install the angularjs.TypeScript.DefinitelyTyped NuGet package to resovle the reference paths,
// then adjust the path value to be relative to this file
/// <reference path='../Scripts/typings/angularjs/angular.d.ts'/>
/// <reference path='../Scripts/typings/angularjs/angular-resource.d.ts'/>

interface I$safeitemname$ {
    greeting: string;
    serviceId: string;
    changeGreeting: () => void;
}

class $safeitemname$ implements I$safeitemname$ {
    static serviceId: string = "$safeitemname$";
    greeting = "Hello";

    constructor(private $http: ng.IHttpService, private $resource: ng.resource.IResourceService) {
    }

    changeGreeting() {
        this.greeting = "Bye";
    }
}

app.factory($safeitemname$.serviceId, ['$http', '$resource', function ($scope, $http, $resource) {
    return new $safeitemname$($http, $resource);
}]);
