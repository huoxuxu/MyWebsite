/**
* js

*/
(function ($) {

    $.fn.snow = function (options) {

        var $flake = $('<div id="snowbox-k' + 'eleyi-com" />').css({ 'position': 'absolute', 'top': '-50px' }).html('<img src="http://fuliquimg.qiniudn.com/QQ%E5%9B%BE%E7%89%8720140214095013.png">'),
documentHeight = $(document).height(),
documentWidth = $(document).width(),
defaults = {
    minSize: 10, 
    maxSize: 20, 
    newOn: 1000, 
    flakeColor: "#FFFFFF"	
},
options = $.extend({}, defaults, options);

        var interval = setInterval(function () {
            var startPositionLeft = Math.random() * documentWidth - 100,
startOpacity = 0.5 + Math.random(),
sizeFlake = options.minSize + Math.random() * options.maxSize,
endPositionTop = documentHeight - 40,
endPositionLeft = startPositionLeft - 100 + Math.random() * 500,
durationFall = documentHeight * 10 + Math.random() * 5000;
            $flake.clone().appendTo('body').css({
                left: startPositionLeft,
                opacity: startOpacity,
                'font-size': sizeFlake,
                color: options.flakeColor
            }).animate({
                top: endPositionTop,
                left: endPositionLeft,
                opacity: 0.2
            }, durationFall, 'linear', function () {
                $(this).remove()
            }
);

        }, options.newOn);

    };

})(jQuery);