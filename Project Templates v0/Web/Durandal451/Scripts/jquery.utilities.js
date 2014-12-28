(function($) {
    //arguments: array, comparison (value to compare against or function which should return true/false)
    $.arrayContains = function (array, comparison) {
        if (Object.prototype.toString.call(array) === '[object Array]') {

            var res = false;
            var comparator;

            if (typeof (comparison) === 'function') {
                comparator = comparison;
            } else {
                comparator = function (value) {
                    if (value === comparison) {
                        return true;
                    } else {
                        return false;
                    }
                };
            }

            $.each(array, function (i, v) {
                res = comparator(v);
                return !res;
            });

            return res;

        } else {
            throw { name: "InvalidArgument", description: "array argument must be an array" };
        }
    };

    $.asyncEach = function (array, asyncFunction) {

        if (Object.prototype.toString.call(array) !== '[object Array]') {
            throw { name: "InvalidArgument", description: "array parameter must be an array" };
        }

        if (typeof (asyncFunction) !== 'function') {
            throw { name: "InvalidArgument", description: "asyncFunction parameter must be a function" };
        }

        var dfd = $.Deferred();

        if (array.length === 0)
        {
            return dfd.resolve();
        }

        $.each(array, function (i, v) {
            $.when(asyncFunction(v)).then(function () {
                i++;
                if (i === array.length) {
                    return dfd.resolve();
                }
            });
        });

        return dfd.promise();
    };

    $.getTodayIsoFormat = function (minusDays) {

        var today = new Date();

        if (typeof minusDays !== 'undefined') {
            today.setDate(today.getDate() - minusDays);
        }

        return today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
    };

    $.getFragment = function () {
        if (window.location.hash.indexOf("#") === 0) {
            return $.parseQueryString(window.location.hash.substr(1));
        } else {
            return {};
        }
    };

    $.parseQueryString = function (queryString) {
        var data = {},
            pairs, pair, separatorIndex, escapedKey, escapedValue, key, value;

        if (queryString === null) {
            return data;
        }

        pairs = queryString.split("&");

        for (var i = 0; i < pairs.length; i++) {
            pair = pairs[i];
            separatorIndex = pair.indexOf("=");

            if (separatorIndex === -1) {
                escapedKey = pair;
                escapedValue = null;
            } else {
                escapedKey = pair.substr(0, separatorIndex);
                escapedValue = pair.substr(separatorIndex + 1);
            }

            key = decodeURIComponent(escapedKey);
            value = decodeURIComponent(escapedValue);

            data[key] = value;
        }

        return data;
    };

    $.arrayIntersect = function (array1, array2) {
        return $.grep(array1, function (i) {
            return $.inArray(i, array2) > -1;
        });
    };

    $.getBasePath = function () {
        var path = window.location.pathname;

        if (path.substring(path.length - 1) !== "/") {
            return path + "/";
        } else {
            return path;
        }
    };
})(jQuery);
