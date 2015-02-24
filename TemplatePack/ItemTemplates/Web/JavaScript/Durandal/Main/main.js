// Map the files so Durandal knows where to find these.
require.config({
    paths: {
        'text': '../Scripts/text',
        'durandal': '../Scripts/durandal',
        'plugins': '../Scripts/durandal/plugins',
        'transitions': '../Scripts/durandal/transitions'
    }
});

// Durandal 2.x assumes no global libraries. It will ship expecting 
// Knockout and jQuery to be defined with requirejs. .NET 
// templates by default will set them up as standard script
// libs and then register them with require as follows: 
define('jquery', function () { return jQuery; });
define('knockout', ko);


define(function (require) {
    var app = require('durandal/app');
    var viewLocator = require('durandal/viewLocator');
    var system = require('durandal/system');

    system.debug(config.debugEnabled());

    // Set the app title
    app.title = '$safeprojectname$';
    // This is new for v2.x, we use this to turn the 
    // plug-ins on and configure them. 
    app.configurePlugins({
        router: true
    });

    // Now we will bring down the plug-ins
    app.start().then(function () {
        // When finding a viewmodel module, replace the viewmodel string 
        // with view to find it partner view.
        // [viewmodel]s/sessions --> [view]s/sessions.html
        // Defaults to viewmodels/views/views. 
        // Otherwise you can pass paths for modules, views, partials
        viewLocator.useConvention();

        // Show the app by setting the root view model for our application.
        // root, transition, application host ID
        // TODO: Put your root here
        app.setRoot('viewmodels/shell', 'entrance');
    });
});