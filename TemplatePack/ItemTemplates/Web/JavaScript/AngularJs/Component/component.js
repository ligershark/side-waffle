(function () {
    'use strict';

    angular
        .module('app')
        .component('$safeitemname$', {
            template: '<h1>$saveitemname$</h1>',
            controller: $safeItemname$Controller
        });

    $safeitemname$Controller.$inject = ['$http']; 

    function $safeitemname$Controller($http) {
        var ctrl = this;

        ctrl.$onInit = function(){
            ctrl.title = $safeitemname$;
        }
    }
})();
