// Install the angularjs.TypeScript.DefinitelyTyped NuGet package to resovle the reference paths,
// then adjust the path value to be relative to this file
/// <reference path='../Scripts/typings/angularjs/angular.d.ts'/>
/// <reference path='../Scripts/typings/angularjs/angular-resource.d.ts'/>

interface I$safeitemname$Scope extends ng.IScope {
    greeting: string;
    changeGreeting: () => void;
}

interface I$safeitemname$ {
    controllerId: string;
}

class $safeitemname$ implements I$safeitemname$ {
    static controllerId: string = "$safeitemname$";
    
    constructor(private $scope: I$safeitemname$Scope, private $http: ng.IHttpService, private $resource: ng.resource.IResourceService) {
        $scope.greeting = "Hello";
        $scope.changeGreeting = () => this.changeGreeting();
    }

    private changeGreeting() {
        this.$scope.greeting = "Bye";
    }
}

app.controller($safeitemname$.controllerId, ['$scope', '$http', '$resource', function ($scope, $http, $resource) {
    return new $safeitemname$($scope, $http, $resource);
}]);
