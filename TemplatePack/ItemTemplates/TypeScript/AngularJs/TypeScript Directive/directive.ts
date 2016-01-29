// Install the angularjs.TypeScript.DefinitelyTyped NuGet package
module App {
    "use strict";

    interface I$safeitemname$ extends ng.IDirective {
    }

    interface I$safeitemname$Scope extends ng.IScope {
    }

    interface I$safeitemname$Attributes extends ng.IAttributes {
    }

    $safeitemname$.$inject = ["$window"];
    function $safeitemname$($window: ng.IWindowService): I$safeitemname$ {
        // Usage:
        //     <$directiveUsage$></$directiveUsage$>
        // Creates:
        // 
        return {
            restrict: "EA",
            link: link
        }

        function link(scope: I$safeitemname$Scope, element: ng.IAugmentedJQuery, attrs: I$safeitemname$Attributes) {

        }
    }

    angular.module("app").directive("$safeitemname$", $safeitemname$);
}