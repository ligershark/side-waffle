// Update the reference to app1.ts to be that of your module file.
// Install the angularjs.TypeScript.DefinitelyTyped NuGet package to resovle the .d.ts reference paths,
// then adjust the path value to be relative to this file
/// <reference path="app1.ts" />
/// <reference path='/Scripts/typings/angularjs/angular.d.ts'/>
/// <reference path='/Scripts/typings/angularjs/angular-resource.d.ts'/>

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

// Update the app1 variable name to be that of your module variable
app1.factory($safeitemname$.serviceId, ['$http', '$resource', ($http, $resource) =>
    new $safeitemname$($http, $resource)
]);
