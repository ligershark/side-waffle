// Install the angularjs.TypeScript.DefinitelyTyped NuGet package
module App {
    "use strict";

    interface I$safeitemname$ {
        getData: () => string;
    }
    
    class $safeitemname$ implements I$safeitemname$ {
        static $inject: string[] = ["$http"];

        constructor(private $http: ng.IHttpService) {
        }

        getData() {
            return "";
        }
    }

    angular.module("app").service("$safeitemname$", $safeitemname$);
}