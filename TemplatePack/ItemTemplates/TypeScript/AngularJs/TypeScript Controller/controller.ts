// Update the reference to app1.ts to be that of your module file.
// Install the angularjs.TypeScript.DefinitelyTyped NuGet package to resolve the .d.ts reference paths,
// then adjust the path value to be relative to this file.
/// <reference path="app1.ts" />
/// <reference path='/Scripts/typings/angularjs/angular.d.ts'/>
/// <reference path='/Scripts/typings/angularjs/angular-resource.d.ts'/>

interface I$safeitemname$ {
    greeting: string;
    changeGreeting: () => void;
}

function $safeitemname$($scope: ng.IScope, $http: ng.IHttpService, $resource: ng.resource.IResourceService){
    var vm: I$safeitemname$ = this;
    vm.greeting = "Hello";

    vm.changeGreeting=(): void =>{
        vm.greeting = "Bye";
    }
}


// Update the app1 variable name to be that of your module variable
app1.controller("$safeitemname$", ['$scope', '$http', '$resource', 
    $safeitemname$
]);
