// Update the reference to app1.ts to be that of your module file.
// Install the angularjs.TypeScript.DefinitelyTyped NuGet package to resovle the .d.ts reference paths,
// then adjust the path value to be relative to this file
/// <reference path="app1.ts" />
/// <reference path='/Scripts/typings/angularjs/angular.d.ts'/>
/// <reference path='/Scripts/typings/angularjs/angular-resource.d.ts'/>

interface I$safeitemname$ extends ng.IDirective {
    directiveId: string;
}

interface I$safeitemname$Scope extends ng.IScope {
    greeting: string;
    changeGreeting: () => void;
}

class $safeitemname$ implements I$safeitemname$ {
    static directiveId: string = "$safeitemname$";
    restrict: string = "A";

    constructor(private $window: ng.IWindowService) {
    }

    link(scope: I$safeitemname$Scope, element, attrs) {
        scope.greeting = "Hi!";
        scope.changeGreeting = () => {
            scope.greeting = "See ya!";
        };
    }
}

// Update the app1 variable name to be that of your module variable
app1.directive($safeitemname$.directiveId, ['$window', $window =>
    new $safeitemname$($window)
]);
