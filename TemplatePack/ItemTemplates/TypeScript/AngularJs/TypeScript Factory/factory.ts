// Install the angularjs.TypeScript.DefinitelyTyped NuGet package
module App {
    "use strict";

    interface I$safeitemname$ {
        getData: () => string;
    }

    $safeitemname$.$inject = ["$http"];

    function $safeitemname$($http: angular.IHttpService): I$safeitemname$ {
        var service: I$safeitemname$ = {
            getData: getData
        };

        return service;

        function getData() {
            return "";
        }
    }


    angular.module("app").factory("$safeitemname$", $safeitemname$);
}