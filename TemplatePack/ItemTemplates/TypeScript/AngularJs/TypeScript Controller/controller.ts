// Install the angularjs.TypeScript.DefinitelyTyped NuGet package
module App {

    interface I$safeitemname$ {
        title: string;
        activate: () => void;
    }

    class $safeitemname$ implements I$safeitemname$ {
        title: string = "$safeitemname$";

        static $inject: string[] = ["$location"];

        constructor(private $location: ng.ILocationService) {
            this.activate();
        }

        activate() {

        }
    }

    angular.module("app").controller("$safeitemname$", $safeitemname$);
}