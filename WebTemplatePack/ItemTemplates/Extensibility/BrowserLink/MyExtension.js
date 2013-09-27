/// <reference path="../intellisense/browserlink.intellisense.js" />

(function (browserLink, $) {
    /// <param name="browserLink" value="bl" />
    /// <param name="$" value="jQuery" />

    function output(message) { // Helper for the 'greeting' function
        alert(message);

        if (console) {
            console.log(message);
        }
    }

    return {

        greeting: function (message) { // Can be called from the server-side extension
            output(message);
        },

        onConnected: function () { // Optional. Is called when a connection is established
            browserLink.invoke("SendText", "Hello from " + browserLink.initializationData.appName);
        }
    };
});