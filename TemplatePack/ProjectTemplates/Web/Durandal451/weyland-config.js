exports.config = function (weyland) {
    weyland.build('main')
        .task.jshint({
            include: 'App/**/*.js'
        })
        .task.uglifyjs({
            include: ['App/**/*.js', 'Scripts/durandal/**/*.js']
        })
        .task.rjs({
            include: ['App/**/*.{js,html}', 'Scripts/durandal/**/*.js'],
            loaderPluginExtensionMaps: {
                '.html': 'text'
            },
            rjs: {
                name: '../Scripts/almond-custom', //to deploy with require.js, use the build's name here instead
                insertRequire: ['main'], //not needed for require
                baseUrl: 'App',
                wrap: true, //not needed for require
                paths: {
                    'text': '../Scripts/text',
                    'durandal': '../Scripts/durandal',
                    'plugins': '../Scripts/durandal/plugins',
                    'transitions': '../Scripts/durandal/transitions',
                    'knockout': '../Scripts/knockout-2.3.0',
                    'knockout.validation': '../Scripts/knockout.validation',
                    'bootstrap': '../Scripts/bootstrap',
                    'jquery': '../Scripts/jquery-1.10.2',
                    'jquery.utilities': '../Scripts/jquery.utilities',
                    'toastr': '../Scripts/toastr'
                },
                shim: {
                    'bootstrap': {
                        deps: ['jquery'],
                        exports: 'jQuery'
                    },
                    'knockout.validation': {
                        deps: ['knockout']
                    },
                    'jquery.utilities': {
                        dep: ['jquery']
                    }
                },
                inlineText: true,
                optimize: 'none',
                pragmas: {
                    build: true
                },
                stubModules: ['text'],
                keepBuildDir: true,
                out: 'App/main-built.js'
            }
        });
};