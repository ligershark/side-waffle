//TODO: Inject dependencies
define(['plugins/router'],
    function (router) {
        // Internal properties and functions
       
        // Reveal the bindable properties and functions
        var vm = {
            activate: activate,
            goBack: goBack,
            title: '$itemname$'
        };

        return vm;
        
        function activate (id, querystring) {
            //TODO: Initialize lookup data here.

            //TODO: Get the data for this viewmodel, return a promise.
        }

        function goBack(complete) {
            router.navigateBack();
        }
    });