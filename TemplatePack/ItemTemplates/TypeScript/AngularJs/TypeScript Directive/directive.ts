// Install the angularjs.TypeScript.DefinitelyTyped NuGet package
module App {
    "use strict";

    interface I$safeitemname$ extends angular.IDirective {
    }

    interface I$safeitemname$Scope extends angular.IScope {
    }

    interface I$safeitemname$Attributes extends angular.IAttributes {
    }

    $safeitemname$.$inject = ["$window"];
    function $safeitemname$($window: angular.IWindowService): I$safeitemname$ {
        // Usage:
        //     <$directiveUsage$></$directiveUsage$>
        // Creates:
        // 
        return {
            restrict: "EA",
            link: link
        }

        function link(scope: I$safeitemname$Scope, element: angular.IAugmentedJQuery, attrs: I$safeitemname$Attributes) {

        }
    }

    angular.module("app").directive("$safeitemname$", $safeitemname$);
}