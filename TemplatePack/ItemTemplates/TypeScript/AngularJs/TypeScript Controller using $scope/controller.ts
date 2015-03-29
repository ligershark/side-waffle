// Install the angularjs.TypeScript.DefinitelyTyped NuGet package
module App {
    "use strict";

    interface I$safeitemname$Scope extends ng.IScope {
        title: string;
    }

    interface I$safeitemname$ {
        activate: () => void;
    }

    class $safeitemname$ implements I$safeitemname$ {
        static $inject: string[] = ["$scope"];

        constructor(private $scope: I$safeitemname$Scope) {
            $scope.title = "$safeitemname$";

            this.activate();
        }

        activate() {

        }
    }

    angular.module("app").controller("$safeitemname$", $safeitemname$);
}