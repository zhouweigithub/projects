function getLocalTime(timestamp) {
    return new Date(parseInt(timestamp) * 1000).toLocaleString().replace(/:\d{1,2}$/, ' ');
}

function goods_price(_number) {
    return _number.toFixed(2);
}

function randomWord(randomFlag, min, max) {
    var str = "",
        range = min,
        arr = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];

    // 随机产生
    if (randomFlag) {
        range = Math.round(Math.random() * (max - min)) + min;
    }
    for (var i = 0; i < range; i++) {
        pos = Math.round(Math.random() * (arr.length - 1));
        str += arr[pos];
    }
    return str;
}

function strToint(_number) {
    var _int = 0;
    try {
        _int = parseInt(_number);

    } catch (err) {
        _int = 0;
    }

    if (isNaN(_int)) _int = 0;

    return _int;
}

var yunmallIframe = new YunmallIFrame();


var yunmessage = {
    msg: function (_message) {
        $(document).dialog({
            type: 'notice',
            infoText: _message,
            autoClose: 2500,
            position: 'bottom'  // center: 居中; bottom: 底部
        });
    },
    loading: function () {
        this.loadingdom = $(document).dialog({
            type: 'toast',
            infoIcon: '/static/dialog/images/icon/loading.gif',
            infoText: '正在加载中',
            autoClose: 2500
        });
    },
    error: function (_msg, _time) {
        if (yunmessage.art) yunmessage.art.close();
        art.dialog({ icon: 'error', id: "yunmessage", lock: true, content: _msg, title: this._title, time: this._time(_time) });
    },
    warning: function (_msg, _time) {
        if (yunmessage.art) yunmessage.art.close();
        art.dialog({ icon: 'warning', id: "yunmessage", lock: true, content: _msg, title: this._title, time: this._time(_time) });
    },
    success: function (_msg, _time) {
        if (yunmessage.art) yunmessage.art.close();
        art.dialog({ icon: 'successed', id: "yunmessage", lock: true, content: _msg, title: this._title, time: this._time(_time) });
    },
    title: function (_title) {
        if (typeof (_title) == 'undefined') {
            this._title = '消息提示';
        } else {
            this._title = _title;
        }
        return this;
    },
    _title: '消息提示',
    _time: function (_time) {
        if (typeof (_time) == 'undefined') {
            return 1;
        }
        return strToint(_time);
    },
    loadingdom: null,
    timeout: null,
    art: null
}

var yunmallOpenDialog = function () {
    var that = this;
    this.dialogdom;
    this.times;

    this.setTitle = function () {

    }

    this.setUrl = function (_callback) {

    };

    this.open = function (_title, _url) {
        open(_title, _url);

    }

    this.error = function (message) {
        if (that.dialogdom) {
            that.dialogdom.close();
        }
        art.dialog({ icon: 'face-sad', lock: true, content: message, title: '消息提示', time: 5 });
    };

    this.success = function (message) {
        if (that.dialogdom) {
            that.dialogdom.close();
        }
        art.dialog({ icon: 'face-sad', lock: true, content: message, title: '消息提示', time: 5 });
    };

    this.close = function () {

        if (that.dialogdom) {
            that.dialogdom.close();
        }

    }

    this.select = function () {

    }

    this.reload = function () {
        if (that.dialogdom) {
            that.dialogdom.close();
        }
        window.location.reload();
    }

    this.callback = function (_data) {
        return this;
    };

    function open(_config) {
        that.times = new Date().getTime();

        var config = { title: "编辑信息", uri: "", width: 851, height: 475 };

        for (var Key in _config) {
            config[Key] = _config[Key];
        }

        that.dialogdom = art.dialog.open(config['uri'], {
            title: config['title'],
            width: config['width'],
            lock: true,
            height: config['height']

        });

    };

}

var OpenDialog = new yunmallOpenDialog();

function yunmalldialog(_dom) {

    _dom.bind('click', function () {

        var _config = {};
        if ($(this).attr('dialog_title') != '') {
            _config['title'] = $(this).attr('dialog_title');
        }

        _config['uri'] = $(this).attr('uri');

        if (_config['uri'] == '') {
            alert("URI未设置");
            return;
        }

        if ($(this).attr('dialog_width')) {
            if ($(this).attr('dialog_width').indexOf("%") > 0) {
                _config['width'] = $(this).attr('dialog_width');
            } else {
                _config['width'] = parseInt($(this).attr('dialog_width'));
            }
        }

        if ($(this).attr('dialog_height')) {
            if ($(this).attr('dialog_height').indexOf("%") > 0) {
                _config['height'] = $(this).attr('dialog_height');
            } else {
                _config['height'] = parseInt($(this).attr('dialog_height'));
            }
        }
        OpenDialog.open(_config);

    });

};

$(function () {

    yunmalldialog($("[yun_type=dialog]"));

});


/*确认删除*/
function yunmall_confirm_delete(_url, _title) {
    if (typeof (_title) == "undefined") {
        _title = "你确定要删除这条数据吗?";
    }
    art.dialog.confirm(_title, function () {
        window.location.href = _url;
    }, function () { });
}


function yunmall_confirm_dialog(_title, _url, _request_type) {
    var art_dialog = null, Callback = null;
    if (typeof (_title) == "undefined") {
        _title = "信息确认";
    }

    if (typeof (_request_type) == "undefined") {
        _request_type = "GET";
    } else if (typeof _request_type === "function") {
        callback = _request_type;
        _request_type = "json";
    } else if (_request_type == 'reload') {
        Callback = function (_RESULT) {
            window.location.reload();
        };
        _request_type = "json";
    }



    art_dialog = art.dialog.confirm(_title, function () {
        if (_request_type == 'json') {
            $.get(_url, { isAjax: true, _t: new Date().getTime(), isAjax: new Date().getTime() }, function (_RESULT) {
                art_dialog.close();
                if (!_RESULT['status']) {
                    alert(_RESULT.msg);
                }
                if (Callback) Callback(_RESULT);

            }, "JSON");
        } else {
            window.location.href = _url;
        }

    }, function () { });
}



yunmallIframe.IframeCallback = function (_redirect) {
    if (OpenDialog) {
        OpenDialog.close();
    }
    //window.location.reload();
    if (_redirect != '') {
        window.location.href = _redirect;
    } else {
        window.location.reload();
    }
};



yunmallIframe.Callback = function (_RESULT) {

    if (_RESULT['status'] == true || _RESULT['status'] > 0) {
        iframe_Callback.success(_RESULT);
    } else {
        iframe_Callback.error(_RESULT);
    }
};


var iframe_Callback = {
    error: function (_RESULT) {
        var dialog = art.dialog({ icon: 'warning', id: 'submitloading', lock: true, title: "提示", content: _RESULT['msg'] }).title('3秒后关闭').time(3);
        dialog.shake && dialog.shake();
    },
    success: function (_RESULT) {
        var _buttons = [];
        if (typeof _RESULT['redirects'] === 'object') {

            for (var j in _RESULT['redirects']) {
                var _btn = {
                    name: _RESULT['redirects'][j][0], callback: function (_redirects, _url_type) {

                        return function () {
                            if (_url_type == 1 && parent.yunmallIframe.IframeCallback) {
                                parent.yunmallIframe.IframeCallback('');
                            } else if (_url_type == 0) {
                                if (OpenDialog.dialogdom && OpenDialog.dialogdom.iframe) {
                                    if (_redirects != '') {
                                        $(OpenDialog.dialogdom.iframe).attr('src', _redirects);
                                    } else {
                                        $(OpenDialog.dialogdom.iframe).attr('src', $(OpenDialog.dialogdom.iframe).attr('src'));
                                    }
                                    return true;
                                } else {
                                    window.location.href = _redirects;
                                }
                                //window.location.href = _redirects;
                            } else {
                                window.location.href = _redirects;
                            }

                            return true;
                        };
                    }(_RESULT['redirects'][j][1], _RESULT['redirects'][j][2])
                };
                _buttons.push(_btn);
            }
        } else if (_RESULT['redirects'] && _RESULT['redirects'] == '') {
            _buttons.push({ name: '确定', callback: function () { if (parent.yunmallIframe.IframeCallback) parent.yunmallIframe.IframeCallback(''); else window.location.reload(); } });
        } else if (_RESULT['redirects'] && _RESULT['redirects'] != '') {
            _buttons.push({ name: '确定', callback: function () { if (parent.yunmallIframe.IframeCallback) parent.yunmallIframe.IframeCallback(_RESULT['redirects']); else window.location.href = _RESULT['redirects']; } });
        } else {
            _buttons.push({ name: '确定', callback: function () { if (parent.yunmallIframe.IframeCallback) parent.yunmallIframe.IframeCallback(''); else window.location.reload(); } });
        }

        art.dialog({
            icon: 'succeed', id: 'submitloading', lock: true, width: 200, title: "提示", content: _RESULT['msg'], button: _buttons, close: function () {
                //if(OpenDialog) OpenDialog.close();
                //window.location.reload();
            }
        });
    },



};






yunmallIframe.error = function (_message) {

    art.dialog({ icon: 'warning', lock: true, content: _message, title: '消息提示', time: 5 });
};

yunmallIframe.success = function (_message) {
    if (OpenDialog) OpenDialog.close();
    art.dialog({ icon: 'succeed', lock: true, content: _message, title: '消息提示', time: 5 });
};

var yunmall_reqeust = function (formsubmits, formobj, _callbackName) {
    var that = this;

    this.callbackName = "parent.yunmallIframe.Callback";

    //提交前先验证数据
    if (typeof (window.isValid) == "function") {
        if (!window.isValid())
            return;
    }

    if (typeof (formobj) === 'undefined' || formobj == null) {
        formobj = formsubmits.closest('form');
    }

    if (typeof (_callbackName) !== 'undefined' && formobj != null) {
        this.callbackName = _callbackName;
    }



    var _action = $.trim(formobj.attr('action')), _yun_submit = false;

    if (_action == '' || _action == '#') {
        return;
    }
    _action += _action.indexOf("?") > -1 ? "&" : "?" + "isIframe=1";

    //event.stopPropagation();
    //event.preventDefault();


    if (_yun_submit) return;


    _yun_submit = true;

    var PostData = {}, formobjdata = []; //formobj.serializeArray();


    formobj.find("input").each(function () {
        var _type = $(this).attr('type'), _name = $(this).attr('name');
        if (_type) { _type = _type.toLowerCase(); }
        if (_type == 'checkbox' && $(this).is(":checked")) {
            formobjdata.push({ 'name': _name, 'value': $(this).val(), 'type': _type });
        } else if (_type == 'radio' && $(this).is(":checked")) {
            formobjdata.push({ 'name': _name, 'value': $(this).val(), 'type': _type });
        } else if (_type == 'text') {
            formobjdata.push({ 'name': _name, 'value': $(this).val(), 'type': _type });
        } else if (_type == 'hidden') {
            formobjdata.push({ 'name': _name, 'value': $(this).val(), 'type': _type });
        } else if (_type == 'password') {
            var _password_val = $(this).val();
            if ($(this).attr('encryption') && $(this).attr('encryption') == 'md5') {
                _password_val = hex_md5($(this).val());
            }
            formobjdata.push({ 'name': _name, 'value': _password_val, 'type': _type });
        }

    });

    formobj.find("select").each(function () {
        var _name = $(this).attr('name');
        formobjdata.push({ 'name': _name, 'value': $(this).val(), 'type': 'select' });
    });

    formobj.find("textarea").each(function () {
        var _name = $(this).attr('name');
        formobjdata.push({ 'name': _name, 'value': $(this).val(), 'type': 'textarea' });
    });


    this.setCallbackName = function (_callbackName) {
        this.callbackName = _callbackName;
        return this;
    };




    yunmallIframe.loginFormId = "myIframeFormId_" + parseInt(new Date().getTime());
    yunmallIframe.setUrl(_action);
    yunmallIframe.setExtra({ callback: this.callbackName, isIframe: 'yunmallIframe', _t: new Date().getTime() });
    yunmallIframe.setFormData(formobjdata);
    yunmallIframe.init({});


    /*formsubmitstipstimeout =  setTimeout(function(){
            formsubmitstips = art.dialog({id:'submitloading',title:"提示",content:'<div class="artdialogloading"></div>'});
    },300);*/



}



//属性更改器
var UpdateAttribute = function (_ElementStr, _url, _config) {

    var _this = $(this);



    $(_ElementStr).bind('blur', function () {

        update($(this));
    });


    function update(_this) {
        var _data = {}, _value = $.trim(_this.attr('data-value'));
        _data['id'] = _this.attr('data-id'),
            _data['field'] = $.trim(_this.attr('name')),
            _data['value'] = _this.val();


        if (typeof (_url) != "undefined") {
            _requesturl = $.trim(_url);
        }

        if (typeof (_config) != "undefined") {
            for (var j in _config) {
                _data[j] = _config[j];
            }
        }

        if (typeof (_this.attr("url")) != "undefined") {
            _url = $.trim(_this.attr("url"));
        }



        if (_data['value'] !== '' && _data['value'] != _value) {
            $.post(_url, _data, function (_result) {
                if (!_result['status'] || _result['status'] < 0) {
                    alert(_result['msg']);
                } else {
                    _this.attr('data-value', _data['value']);
                }
            }, 'json');

        }

    }

}


/**
 *联动选择菜单
 */
var SelectLinkage = function () {
    var that = this;
    this.url = "";
    this.name = [];
    this.append;
    this.container;
    this.selectdata = [];

    /*设置联动控件名称*/
    this.setName = function (_name) {
        this.name = _name
        return this;
    }

    /*获取数据地址*/
    this.setUrl = function (_url) {
        this.url = _url
        return this;
    }

    this.add = function (_val) {
        if (_val) {
            this.selectdata.push(_val);
        }
    };

    this.init = function (_container, _parentid, _data) {
        this.container = _container;
        if (_data) {
            that.selectdata = _data;
        }
        request(1, _parentid);
    }

    function request(_hierarchy, _parentid) {
        $.ajax({
            type: "GET",
            url: that.url,
            data: { parentid: _parentid, hierarchy: _hierarchy },
            datatype: 'json',
            //成功返回之后调用的函数             
            success: function (_data) {
                //if(callback){ that.hierarchy+=1;  callback(_data);}
                if (_data && _data.status) {
                    createSelect(_hierarchy, _data['categorys']);
                } else {
                    error_msg(_data.msg);
                    return false;
                }
            },
        });
    }

    function createSelect(_hierarchy, _data) {

        if (_data.length < 1) return;
        var _default = 0;
        if (that.selectdata.length > 0) {
            _default = that.selectdata.shift();

        }

        var _selectName = that.name[_hierarchy - 1] || "";
        var _SELSTR = '<div class="select2container"><select name="' + _selectName + '" class="myselect2linkage" data-hierarchy="' + _hierarchy + '">';
        _SELSTR += '<option value="">请选择</option>';
        for (var j = 0; j < _data.length; j++) {
            _SELSTR += '<option value="' + _data[j]['id'] + '" ' + (_default == _data[j]['id'] ? 'selected="selected"' : '') + '>' + _data[j]['name'] + '</option>';
        }
        _SELSTR += '</select></div>';
        var _DOM = $(_SELSTR);
        $(that.container).append(_DOM);

        //_DOM.appendTo($(that.container));


        _DOM.find("select").select2({
            width: 156,
            placeholder: '请选择',
            //minimumResultsForSearch: -1,
            allowClear: true
        });

        _DOM.find("select").bind('change', function (_element) {
            var _hierarchy = parseInt($(this).attr('data-hierarchy'));
            if ($(this).val() != '') {
                while ($(this).closest('.select2container').next().size() > 0) {
                    $(this).closest('.select2container').next().remove();
                };
                request(_hierarchy + 1, $(this).val());
            }
        });


        if (_default != 0) {
            _DOM.find("select").trigger('change');
        }


    }
}


/**
 *系统解锁
 */
function yunmallUnlock() {

}

var syslock = {

    lock: function () {
        var _template = ['<div class="lockbackground">',
            '   <div class="sysunlock">',
            '       <div class="lock_title">欢迎使用圣宠宠物收银管理系统-已锁定</div>',
            '       <div class="systexts">',
            '       <div class="lock_message">',
            '          <span class="l_msg error" id="art_lock_pass_msg"><em>你输入的密码错误解锁</em></span>',
            '       </div>',
            '       <div class="systext">',
            '          <input name="lockpassword" type="password" placeholder="请输入登录密码解锁" autocomplete="off" class="unlocktxt" /> ',
            '          <input type="button" class="sysunbtn"  onclick="syslock.unlock();" value="解锁">',
            '       </div>',
            '       </div>',
            '       ',
            '   </div>',
            '</div> '].join("\n");
        $.get('/main/lock?_t=' + new Date().getTime(), { isAjax: 1, t: new Date().getTime() }, function (_result) {
            if (_result['status']) {
                $('body').find('.lockbackground').remove();
                $('body').append(_template);
            } else {
                yunmallIframe.error(_result['msg']);
            }
        }, 'json');

    },
    unlock: function () {
        var _password = hex_md5($.trim($("input[name=lockpassword]").val()));
        $.post('/main/unlock?_t=' + new Date().getTime(), { password: _password }, function (_result) {

            if (_result['status']) {
                $(".lockbackground").remove();
            } else {
                $(".lock_message").show().find('em').html(_result['msg']);
            }

        }, 'json');
    }
}



/**
 *联动选择菜单
 */
var SelectLinkage = function () {
    var that = this;
    this.url = "";
    this.name = [];
    this.append;
    this.container;
    this.selectdata = [];

    /*设置联动控件名称*/
    this.setName = function (_name) {
        this.name = _name
        return this;
    }

    /*获取数据地址*/
    this.setUrl = function (_url) {
        this.url = _url
        return this;
    }

    this.add = function (_val) {
        if (_val) {
            this.selectdata.push(_val);
        }
    };

    this.init = function (_container, _parentid, _data) {
        this.container = _container;
        if (_data) {
            that.selectdata = _data;
        }
        request(1, _parentid);
    }

    function request(_hierarchy, _parentid) {
        $.ajax({
            type: "GET",
            url: that.url,
            data: { parentid: _parentid, hierarchy: _hierarchy },
            datatype: 'json',
            //成功返回之后调用的函数             
            success: function (_data) {
                //if(callback){ that.hierarchy+=1;  callback(_data);}
                if (_data && _data.status) {
                    createSelect(_hierarchy, _data['categorys']);
                } else {
                    error_msg(data.msg);
                    return false;
                }
            },
        });
    }

    function createSelect(_hierarchy, _data) {

        if (_data.length < 1) return;
        var _default = 0;
        if (that.selectdata.length > 0) {
            _default = that.selectdata.shift();

        }

        var _selectName = that.name[_hierarchy - 1] || "";
        var _SELSTR = '<div class="select2container"><select name="' + _selectName + '" class="myselect2linkage" data-hierarchy="' + _hierarchy + '">';
        _SELSTR += '<option value="">请选择</option>';
        for (var j = 0; j < _data.length; j++) {
            _SELSTR += '<option value="' + _data[j]['id'] + '" ' + (_default == _data[j]['id'] ? 'selected="selected"' : '') + '>' + _data[j]['name'] + '</option>';
        }
        _SELSTR += '</select></div>';
        var _DOM = $(_SELSTR);
        $(that.container).append(_DOM);

        //_DOM.appendTo($(that.container));


        _DOM.find("select").select2({
            width: 156,
            placeholder: '请选择',
            //minimumResultsForSearch: -1,
            allowClear: true
        });

        _DOM.find("select").bind('change', function (_element) {
            var _hierarchy = parseInt($(this).attr('data-hierarchy'));
            if ($(this).val() != '') {
                while ($(this).closest('.select2container').next().size() > 0) {
                    $(this).closest('.select2container').next().remove();
                };
                request(_hierarchy + 1, $(this).val());
            }
        });


        if (_default != 0) {
            _DOM.find("select").trigger('change');
        }


    }
}








if (typeof yunmallAttachmentManage != "undefined") {
    var yunmallattachmentmanage = new yunmallAttachmentManage();
}

$(function () {
    $('.yunmall_button').bind('click', function () {
        new yunmall_reqeust($(this));
    });



    //上传图片
    $(".yunmall_upload_img").bind('click', function () {
        yunmallattachmentmanage.bind($(this));

    });

    $(".yunmall_cut_pictures").bind('click', function () {
        yunmallattachmentmanage.bindCutPictures($(this));
    });

    $(".yunmalldate").each(function () {
        var _id = 'yunmalldate_' + Math.floor(Math.random() * 1000000) + "_" + Math.floor(new Date().getTime());
        $(this).attr('id', _id);
        laydate.render({
            elem: '#' + _id
        });
    });

    if ($('.mySelect').size() > 0) {

        $('.mySelect').select2({
            width: 156,
            minimumResultsForSearch: -1,
            placeholder: '请选择',
            allowClear: true
        });
    }

    new UpdateAttribute(".editattribute");
});



function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return decodeURIComponent(r[2]); return ""; //返回参数值
}