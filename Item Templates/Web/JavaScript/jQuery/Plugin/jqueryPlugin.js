//Template based on http://www.queness.com/post/112/a-really-simple-jquery-plugin-tutorial
(function ($) {
    //Your plugin's name
    var pluginName = '$safeitemname$';
    
    $.fn.extend({
        '$safeitemname$': function (options) {
            var defaults = {
                //Default values for the plugin's options here
            };

            options = $.extend(defaults, options);

            return this.each(function () {
                var opts = options;
                var jqObject = $(this);
                //Your plugin code here
            });
        }
    });
})(jQuery);