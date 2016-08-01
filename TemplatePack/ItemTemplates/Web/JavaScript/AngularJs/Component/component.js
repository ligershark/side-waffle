(function () {
    'use strict';

    angular
        .module('app')
        .component('$safeitemname$', $safeitemname$Component);

    $safeitemname$Component.$inject = ['$http']; 

    function $safeitemname$Component($http) {
        var $ctrl = this;

        $ctrl.$onInit = function(){
            $ctrl.title = $safeitemname$;
        }
    }
})();
