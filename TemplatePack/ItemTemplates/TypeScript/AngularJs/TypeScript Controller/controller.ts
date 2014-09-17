// Update the reference to app1.ts to be that of your module file.
// Install the angularjs.TypeScript.DefinitelyTyped NuGet package to resolve the .d.ts reference paths,
// then adjust the path value to be relative to this file.
// Sample html:
// <div ng-controller="$safeitemname$" >
// greeting: {{vm.greeting}}
// <input type="button" name="btn" value="Click" ng-click="vm.changeGreeting()" />
// </div>
/// <reference path="app1.ts" />
/// <reference path='/Scripts/typings/angularjs/angular.d.ts'/>
/// <reference path='/Scripts/typings/angularjs/angular-resource.d.ts'/>

interface I$safeitemname$Scope extends ng.IScope {
    vm: $safeitemname$;
}

interface I$safeitemname$ {
    greeting: string;
    changeGreeting: () => void;
}

class $safeitemname$ implements I$safeitemname$ {
    static controllerId: string = "$safeitemname$";
    greeting = "Hello";
    
    constructor(private $scope: I$safeitemname$Scope, private $http: ng.IHttpService, private $resource: ng.resource.IResourceService) {
        $scope.vm = this;
    }

    changeGreeting() {
        this.greeting = "Bye";
    }
}

// Update the app1 variable name to be that of your module variable
app1.controller($safeitemname$.controllerId, ['$scope', '$http', '$resource', ($scope, $http, $resource) =>
    new $safeitemname$($scope, $http, $resource)
]);
