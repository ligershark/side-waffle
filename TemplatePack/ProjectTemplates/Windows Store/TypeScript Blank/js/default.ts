///<reference path="../Scripts/typings/lib.d.ts" />
///<reference path="../Scripts/typings/winjs.d.ts" />
///<reference path="../Scripts/typings/winrt.d.ts" />
module Default {
    "use strict"; 

    WinJS.Application.addEventListener("activated",
        (args: WinJS.Application.ApplicationActivationEvent) => {
            if (args.detail.kind === Windows.ApplicationModel.Activation.ActivationKind.launch) {
                if (args.detail.previousExecutionState !== Windows.ApplicationModel.Activation.ApplicationExecutionState.terminated) {
                // TODO: This application has been newly launched. Initialize
                // your application here.
            } else {
                // TODO: This application has been reactivated from suspension.
                // Restore application state here.
            }

            args.setPromise(WinJS.UI.processAll().then(function () {
                
            }));
        };
    });

    WinJS.Application.oncheckpoint = function (args) {
        // TODO: This application is about to be suspended. Save any state
        // that needs to persist across suspensions here. If you need to 
        // complete an asynchronous operation before your application is 
        // suspended, call args.setPromise().
    };

    WinJS.Application.start();
}