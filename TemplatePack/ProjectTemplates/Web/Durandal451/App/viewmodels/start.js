define(['services/logger', 'global/session'], function (logger, session) {
    var vm = {
        session: session,
        displayName: 'Durandal 451',
        description: 'Durandal is a cross-device, cross-platform client framework written in JavaScript and designed to make Single Page Applications (SPAs) easy to create and maintain. This project template is based on the ASP.NET 4.5.1 Single Page Application template',
        features: [
            'OWIN OAuth support (with 3rd party authentication providers.',
            'Remember user using local storage.',
            'Configure routing by what role(s) a user should be in in order to navigate to a view.',
            'Customise view for the user dependent on what roles membership.'
        ]
    };        

    return vm;
});