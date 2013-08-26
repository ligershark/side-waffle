/*
* John Papa
* http://johnpapa.net
* DurandalJS ViewModel
**/
define(['durandal/app', 'durandal/system', 'plugins/router'],
    function (app, system, datacontext, router) {
        // Internal properties and functions
        var canLeave = true;
       
        // Bindable properties and functions
        var vm = {
            activate: activate,
            canDeactivate: canDeactivate,
            goBack: goBack,
            title: '$itemname$'
        };

        return vm;
        
        function activate (id, querystring) {
            //TODO: Initialize lookup data here.
            
            //TODO: Get the data for this viewmodel, return a promise.
            // return datacontext.getSessionById(id, session);
        }

        function canDeactivate() {
            // Prompt use to confirm if they want to leave this page.
            if (canLeave) {
                var title = 'Confirm';
                var msg = 'Do you want to leave?';
                return app.showMessage(title, msg, ['Yes', 'No'])
                    .then(confirm);
            }
            ;
            return true;

            function confirm(selectedOption) {
                if (selectedOption === 'Yes') {
                    //TODO: Custom action
                }
                return selectedOption;
            }
        }

        function goBack(complete) {
            router.navigateBack();
        }
    });