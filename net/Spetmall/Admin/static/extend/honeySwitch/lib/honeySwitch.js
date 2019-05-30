var honeySwitch = {};
honeySwitch.themeColor = "rgb(100, 189, 99)";
honeySwitch.init = function () {
    var s = "<span class='slider'><input name='' type='hidden' value='' /></span>";
    $("[class^=switch]").each(function () {
        $(this).append(s);
        if (typeof ($(this).attr("name")) != "undefined") {
            var _name = $.trim($(this).attr('name')), _input = $(this).find('input');
            _input.attr('name', _name), _value = "";

            if (typeof ($(this).attr("data-value")) != "undefined") {
                _value = $.trim($(this).attr("data-value"));
            }
            if (_value == '1') {
                $(this).removeClass('switch-off').addClass('switch-on');
            } else if (_value == '0') {
                $(this).removeClass('switch-on').addClass('switch-off');
            } else if ($(this).hasClass("switch-on")) {
                _input.val(1);
            } else if ($(this).hasClass("switch-off")) {
                _input.val(0);
            }
        }
    });

    $("[class^=switch]").unbind('click').click(function () {

        var _this = $(this);
        if ($(this).hasClass("switch-disabled")) {
            return;
        }

        var _input = $(this).find('input');

        if ($(this).hasClass("switch-on")) {
            $(this).removeClass("switch-on").addClass("switch-off");
            $(".switch-off").css({
                'border-color': '#dfdfdf',
                'box-shadow': 'rgb(223, 223, 223) 0px 0px 0px 0px inset',
                'background-color': 'rgb(255, 255, 255)'
            });
            if (_input.size() > 0) {
                _input.val(0);
            }
        } else {
            $(this).removeClass("switch-off").addClass("switch-on");
            if (honeySwitch.themeColor) {
                var c = honeySwitch.themeColor;
                $(this).css({
                    'border-color': c,
                    'box-shadow': c + ' 0px 0px 0px 16px inset',
                    'background-color': c
                });
            }
            if ($(this).attr('themeColor')) {
                var c2 = $(this).attr('themeColor');
                $(this).css({
                    'border-color': c2,
                    'box-shadow': c2 + ' 0px 0px 0px 16px inset',
                    'background-color': c2
                });
            }
            if (_input.size() > 0) {
                _input.val(1);
            }
        }


        if ($(this).hasClass("switch-attribute")) {
            var _url,
                _id = _this.attr('data-id'),
                _field = $.trim(_this.attr('name')),
                _value = _input.val();

            if (typeof (_this.attr("field")) != "undefined") {
                _field = $.trim(_this.attr('field'));
            }

            if (typeof (_this.attr("url")) != "undefined") {
                _url = $.trim(_this.attr("url"));
            }

            $.post(_url + (_url.indexOf('?') > 0 ? "&" : "?" + "isAjax=" + new Date().getTime()), { id: _id, field: _field, value: _value }, function (_result) {

                if (!_result['status'] || _result['status'] < 0) {
                    alert(_result['msg']);
                    //global_message_error(_result['msg']);
                    //HXJMessage(_result['msg'],'warning');		    	
                }

            }, 'json');


        }

    });
    window.switchEvent = function (ele, on, off) {
        $(ele).click(function () {
            if ($(this).hasClass("switch-disabled")) {
                return;
            }
            if ($(this).hasClass('switch-on')) {
                if (typeof on == 'function') {
                    on();
                }
            } else {
                if (typeof off == 'function') {
                    off();
                }
            }
        });
    }
    if (this.themeColor) {
        var c = this.themeColor;
        $(".switch-on").css({
            'border-color': c,
            'box-shadow': c + ' 0px 0px 0px 16px inset',
            'background-color': c
        });
        $(".switch-off").css({
            'border-color': '#dfdfdf',
            'box-shadow': 'rgb(223, 223, 223) 0px 0px 0px 0px inset',
            'background-color': 'rgb(255, 255, 255)'
        });
    }
    if ($('[themeColor]').length > 0) {
        $('[themeColor]').each(function () {
            var c = $(this).attr('themeColor') || honeySwitch.themeColor;
            if ($(this).hasClass("switch-on")) {
                $(this).css({
                    'border-color': c,
                    'box-shadow': c + ' 0px 0px 0px 16px inset',
                    'background-color': c
                });
            } else {
                $(".switch-off").css({
                    'border-color': '#dfdfdf',
                    'box-shadow': 'rgb(223, 223, 223) 0px 0px 0px 0px inset',
                    'background-color': 'rgb(255, 255, 255)'
                });
            }
        });
    }
};
honeySwitch.showOn = function (ele) {
    $(ele).removeClass("switch-off").addClass("switch-on");
    if (honeySwitch.themeColor) {
        var c = honeySwitch.themeColor;
        $(ele).css({
            'border-color': c,
            'box-shadow': c + ' 0px 0px 0px 16px inset',
            'background-color': c
        });
    }
    if ($(ele).attr('themeColor')) {
        var c2 = $(ele).attr('themeColor');
        $(ele).css({
            'border-color': c2,
            'box-shadow': c2 + ' 0px 0px 0px 16px inset',
            'background-color': c2
        });
    }
}
honeySwitch.showOff = function (ele) {
    $(ele).removeClass("switch-on").addClass("switch-off");
    $(".switch-off").css({
        'border-color': '#dfdfdf',
        'box-shadow': 'rgb(223, 223, 223) 0px 0px 0px 0px inset',
        'background-color': 'rgb(255, 255, 255)'
    });
}
$(function () {
    honeySwitch.init();
}); 