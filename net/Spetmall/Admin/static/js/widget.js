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
            url: "/Activity/GetCategorys",
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



var ArtSpecData = function () {

    var that = this, ListEle = $("#option_list_id"),
        GOODSGUIZHE_LIST_ID = $("#goodsguizhe_list_id"),
        BUTTON = $("#specbutton"),
        subtractfromfullsubmit = $("#subtractfromfullsubmit"),
        discountguizheBUTTON = $("#discountguizhebutton"),
        is_discount_type_BUTTON = $("input[name=is_discount_type]"),
        GUIZHEBUTTON = $("#guizhebutton");
    this.specconfigid = 0;
    this.specdiscountconfigid = 0;


    this.data = [];

    this.setData = function (_data) {
        this.data = _data;
    }


    this.setCreate = function (_data) {
        create(_data);
        return this;
    }


    this.setGuizhe = function (_data) {

        for (var i in _data) {
            that.specconfigid = parseInt(i);
            _data[i]['specconfigid'] = i;
            _data[i]['min_price'] = _data[i]['min_price'];
            _data[i]['reduce_price'] = _data[i]['reduce_price'];
            createGuizhe(_data[i]);
        }
        return this;
    }


    this.setDiscountGuizhe = function (_data) {

        for (var i in _data) {
            that.specdiscountconfigid = parseInt(i);
            _data[i]['specconfigid'] = i;
            _data[i]['min'] = _data[i]['min'];
            _data[i]['discount'] = _data[i]['discount'];
            createDiscountGuizhe(_data[i]);
        }
        return this;
    }



    this.init = function (_data) {
        Bind();
        if (_data) {
            create(_data)
        }


    }

    /*绑定*/
    function Bind() {
        BUTTON.bind('click', function () {
            _Dialog(function (_data) {
                create(_data);
            });
        });

        //满减
        GUIZHEBUTTON.bind('click', function () {
            that.specconfigid++;
            createGuizhe({ specconfigid: that.specconfigid });
        });


        //选择折扣类型
        is_discount_type_BUTTON.bind('click', function () {
            GOODSGUIZHE_LIST_ID.empty();
            if ($(this).val() != 1) {
                $(".goodsguizhe").show();
                $(".Discount_number").hide();
            } else {
                $(".goodsguizhe").hide();
                $(".Discount_number").show();
            }
        });


        //折扣
        discountguizheBUTTON.bind('click', function () {
            that.specdiscountconfigid++;
            createDiscountGuizhe({ specconfigid: that.specdiscountconfigid });
        });

        subtractfromfullsubmit.bind('click', function () {
            Request();
        });
    }

    function create(_data) {
        for (var J in _data) {
            var val = _data[J];
            var Ele = $('<div specid="' + val['id'] + '" class="specgoods"><div class="specs"><input type="hidden" name="specdata[]" value="' + val['id'] + '" /><input type="hidden" name="spectitledata[]" value="' + val['title'] + '" /> <em>' + val['title'] + '</em><a href="javascript:;" data-id="' + val['id'] + '" class="del">删除</a></div><div class="tips_msg"><h3>只能选择</h3><div class="tips_list"></div></div></div>');
            if (ListEle.find('div[specid=' + val['id'] + ']').size() < 1) {
                ListEle.append(Ele);
                Ele.find('a.del').bind('click', function () {
                    del($(this));
                });
            }
        }
    }

    /*创建规则*/
    function createGuizhe(_data) {
        var str = template().replace(/\{#([\w]+)\}/g, function (_a, _b) {
            return _data[_b] || '';
        });
        var Ele = $(str);
        GOODSGUIZHE_LIST_ID.append(Ele);
        Ele.find('.btn').bind('click', function () {

            var _Ele = $(this).closest('.goodsguizhe_attrs').find('.goodsguizhe_attr_list'), _indexID = $(this).attr('data-index');
            _Dialog(function (_data) {
                createImg(_Ele, _data, _indexID);
            })
        });

        if (_data['goods']) {
            createImg(Ele.find('.goodsguizhe_attr_list'), _data['goods'], _data['specconfigid']);
        }

        Ele.find('a.specdel').bind('click', function () {
            $(this).closest(".goodsguizhe_attrs").remove();
        });


    }


    /*创建折扣规则*/
    function createDiscountGuizhe(_data) {
        var is_discount_type_val = $("input[name=is_discount_type]:checked").val();

        if (is_discount_type_val == 2) {
            _data['company'] = "件";
        } else if (is_discount_type_val == 3) {
            _data['company'] = "元";
        }

        var str = Discounttemplate().replace(/\{#([\w]+)\}/g, function (_a, _b) {
            return _data[_b] || '';
        });
        var Ele = $(str);
        GOODSGUIZHE_LIST_ID.append(Ele);
        Ele.find('.btn').bind('click', function () {

            var _Ele = $(this).closest('.goodsguizhe_attrs').find('.goodsguizhe_attr_list'), _indexID = $(this).attr('data-index');
            _Dialog(function (_data) {
                createImg(_Ele, _data, _indexID);
            })
        });

        if (_data['goods']) {
            createImg(Ele.find('.goodsguizhe_attr_list'), _data['goods'], _data['specconfigid']);
        }

        Ele.find('a.specdel').bind('click', function () {
            $(this).closest(".goodsguizhe_attrs").remove();
        });


    }



    function createImg(_ELe, _list, _indexID) {
        for (var j in _list) {
            var _data = _list[j];
            _data['specconfigid'] = _indexID;
            if (_ELe.find("div[specid=" + _data['id'] + "]").size() < 1) {
                var str = templateImg().replace(/\{#([\w]+)\}/g, function (_a, _b) {
                    return _data[_b];
                });
                var Ele = $(str);
                _ELe.append(Ele);
                Ele.find('.del').bind('click', function () {
                    $(this).closest('.listimgs').remove();
                });
            }
        }
    }

    function SetCategoryOrProducts() {
        var type = $("#type").val();
        if (type == 1) {
            //选择了分类
            var categoryId = 0;
            $("#placeofdelivery select").each(function (index, item) {
                if ($(item).val() != '') {
                    categoryId = $(item).val();
                    return;
                }
            });
            $("#categoryOrProducts").val(categoryId);
        }
        else if (type == 2) {
            //选择了商品
            var productIds = "";
            $("input[name='specdata[]']").each(function () {
                //4.将每个input的值放进结果集
                productIds += $(this).val() + ",";
            });
            if (productIds.length > 0) {
                productIds = productIds.substr(0, productIds.length - 1);
                $("#categoryOrProducts").val(productIds);
            }
        }
        else {
            //选择了店铺
            $("#categoryOrProducts").val("");
        }
    }

    function SetRules() {
        var result = "";
        var list = $("#goodsguizhe_list_id .goodsguizhe_attrs");
        list.each(function (index, item) {
            txt1 = $(".guizhe1", item).val();
            txt2 = $(".guizhe2", item).val();
            if (txt1 != "" && txt2 != "") {
                result += txt1 + "|" + txt2 + ",";
            }
        });

        result = result.substr(0, result.length - 1);
        $("#rules").val(result);
    }

    /*获取信息*/
    function Request() {
        SetCategoryOrProducts();
        SetRules();

        var _url = $('#subtractfromfullForm').attr('action');
        $.ajax({
            type: "POST",
            dataType: 'json',
            async: false,
            data: $("#subtractfromfullForm").serializeArray(),
            url: _url,
            success: function (_data) {

                if (_data && _data.code >= 0) {

                    switch (parseInt(_data.code)) {
                        case 1000: ErrorMsg(_data['data']); break;
                        case 1200: ErrorMsg1200(_data['data']); alert(_data['msg']); break;
                        case 1001: alert(_data['msg']); break;
                        case 0: artDialog(_data.msg, _data.href); break;
                    }

                } else {
                    alert("请求错误");
                }
            }
        });
    }

    function artDialog(_content, _href) {
        art.dialog({
            title: '消息提示',
            id: 'view_cust',
            lock: true,
            content: _content,
            button: [
                {
                    name: '继续添加',
                    callback: function () {
                        window.location.reload();
                    }
                },
                {
                    name: '返回列表',
                    callback: function () {
                        window.location.href = _href;
                        //"/admin/subtractfromfull/index.html";
                    }
                }

            ]
        });
    }


    function ErrorMsg1200(_data) {
        var placeofdeliverymsg = $(".placeofdeliverymsg");

        placeofdeliverymsg.empty();

        $(".date_config_message").hide();

        for (var _start in _data) {
            $(".date_config_message").show();
            if (_start <= 0) {
                var _Dom = $("<span>结束时间小于" + _data[_start] + "</span>");
            } else if (_data[_start] <= 0) {
                var _Dom = $("<span>开始时间大于" + _start + "</span>");
            } else {
                var _Dom = $("<span>" + _start + " 到 " + _data[_start] + "</span>");
            }
            placeofdeliverymsg.append(_Dom);
        }

    }


    function ErrorMsg(_data) {

        for (var _specid in _data) {
            var _specElement = $("div[specid=" + _specid + "]");
            _specElement.addClass('specerror');
            _specElement.find('.tips_msg').show();
            var _spec_tips_list = _specElement.find('.tips_list');
            _spec_tips_list.empty();

            for (var _start in _data[_specid]) {
                if (_start <= 0) {
                    var _Dom = $("<span>结束时间小于" + _data[_specid][_start] + "</span>");
                } else if (_data[_specid][_start] <= 0) {
                    var _Dom = $("<span>开始时间大于" + _start + "</span>");
                } else {
                    var _Dom = $("<span>" + _start + " 到 " + _data[_specid][_start] + "</span>");
                }
                //console.log(_Dom);
                _spec_tips_list.append(_Dom);
            }
        }
    }

    function del(_Ele) {
        _Ele.closest('div.specgoods').remove();
    }

    function _Dialog(_CALLBACK) {
        windowdialog = art.dialog.open("/Activity/ChooseProducts", {
            title: '请选择商品',
            width: '60%',
            height: '40%',
            fixed: true,
            lock: true,
            background: '#000', // 背景色
            opacity: 0.4,
            cancel: true,
            ok: function () {
                var frame = this.iframe.contentWindow;
                var form = $(frame.document.getElementById('SpecData'));
                var dataEle = form.find("input.glcheck:checked");
                var ListData = [];


                if (dataEle.size() > 0) {
                    dataEle.each(function () {
                        ListData.push({ id: $(this).attr('data-id'), thumimg: $(this).attr('data-thumimg'), title: $(this).attr('data-title'), number: 1 });
                    });
                    _CALLBACK && _CALLBACK(ListData);

                }
            }
        });
    }

    //'               		<div class="goodsguizhe_attr">单笔订单满：<input name="content[min_price][{#specconfigid}]" type="text" class="txt guizhe1" value="{#min_price}" /> , 立减现金<input name="content[reduce_price][{#specconfigid}]" type="text" class="txt guizhe2" value="{#reduce_price}" />  , 送礼物 <input name="formsubmits" id="specbutton" type="button" data-index="{#specconfigid}" class="btn dssc-btn" value="选择商品"/> <a href="javascript:;" class="specdel">删除</a></div>',
    /*模版*/
    function template() {
        return ['<div class="goodsguizhe_attrs">',
            '               		<div class="goodsguizhe_attr">单笔订单满：<input name="content[min_price][{#specconfigid}]" type="text" class="txt guizhe1" value="{#min_price}" /> , 立减现金<input name="content[reduce_price][{#specconfigid}]" type="text" class="txt guizhe2" value="{#reduce_price}" />   <a href="javascript:;" class="specdel">删除</a></div>',
            '                    <div class="goodsguizhe_attr_list">',
            '                    </div>',
            '</div>'].join("\n");
    }


    /*模版*/
    function Discounttemplate() {
        return ['<div class="goodsguizhe_attrs">',
            '               		<div class="goodsguizhe_attr">订单满：<input name="content[min][{#specconfigid}]" type="text" class="txt guizhe1" value="{#min}" />{#company},折扣<input name="content[discount][{#specconfigid}]" type="text" class="txt guizhe2" value="{#discount}" /><a href="javascript:;" class="specdel">删除</a></div>',
            '                    <div class="goodsguizhe_attr_list">',
            '                    </div>',
            '</div>'].join("\n");
    }

    function templateImg() {
        return ['<div class="listimgs" specid="{#id}">',
            '    <input type="hidden" name="content[spec_liwu_thumimg][{#specconfigid}][{#id}]" value="{#thumimg}"/>',
            '    <input type="hidden" name="content[spec_liwu_title][{#specconfigid}][{#id}]" value="{#title}"/>',
            '    <img src="{#thumimg}"/>',
            '    <span class="title">{#title}</span>',
            '    <span>数量:<input name="content[spec_liwu_num][{#specconfigid}][{#id}]" value="{#number}" type="text" class="txt" /><em><a href="javascript:;" class="del">删除</a></em></span>',
            '</div>'].join("\n");
    }

}

