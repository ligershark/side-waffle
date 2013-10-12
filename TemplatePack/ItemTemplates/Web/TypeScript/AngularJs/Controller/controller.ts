/// <reference path='../Scripts/typings/angularjs/angular.d.ts'/>
/// <reference path='../Scripts/typings/angularjs/angular-resource.d.ts'/>


interface IController1Scope extends ng.IScope {
    vmx: Controller1;
}

interface IController1 {
    greeting: string;
    controllerId: string;
    changeGreeting: () => void;
}

class Controller1 implements IController1 {

    static controllerId: string = "controller1";
    greeting = "Hello";
    lcbohtml: string;

    constructor(private $scope: IController1Scope, private $http: ng.IHttpService, private $resource: ng.resource.IResourceService) {
        var x = '1';
    }

    callback(data: string, status: string) {
        this.lcbohtml = data;
    }

    changeGreeting() {
        this.greeting = "Bye";
    }
}

myApp.controller(Controller1.controllerId, ['$scope', '$http', '$resource', function ($scope, $http, $resource) {
    return new Controller1($scope, $http, $resource);
}]);
