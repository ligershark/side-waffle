(function () {
    'use strict';

    angular
        .module('app')
        .service('$safeitemname$', $safeitemname$);

    $safeitemname$.$inject = ['$http'];

    function $safeitemname$($http) {
	    /* jshint validthis:true */
        this.getData = getData;

        function getData() { }
    }
})();