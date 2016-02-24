// Install the angularjs.TypeScript.DefinitelyTyped NuGet package
module App {
    "use strict";

    interface I$safeitemname$ {
        title: string;
        activate: () => void;
    }

    class $safeitemname$ implements I$safeitemname$ {
        title: string = "$safeitemname$";

        static $inject: string[] = ["$location"];

        constructor(private $location: angular.ILocationService) {
            this.activate();
        }

        activate() {

        }
    }

    angular.module("app").controller("$safeitemname$", $safeitemname$);
}