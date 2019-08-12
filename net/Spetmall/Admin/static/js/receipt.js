/**
 *收银工具
 */
var ReceiptCommon = function () {

    var that = this;
    this.current_page = 2;
    this.reloadStatus = false;
    this.dialogdom = null;
    this.OpenDialog = null;
    this.test = 1;
    this.MaxPage = 20;
    this.Ele = {
        'shoppingcart': $("#shoppingcart"),
        'youhui_total_price': $("#youhui_total_price"),
        'shoppingcartTotalprice': $("#shoppingcart-totalprice"),
        'shoppingcartgoodsnumber': $("#shoppingcart_goods_number"),
        'goodsitems': $('#goods-items'),
        'goodsitemscontainer': $("#goods-items-container"),
        'receiptsubmit': $('#receipt-submit'),
        'receiptconfirm': $('.receiptconfirm'),
        'receiptconfirmautoheight': $('.receiptconfirm_auto_height'),
        'keyword': $("input[name=keyword]")

    }

    this.CartData = { uid: 0, totalNumber: 0, totalPrice: 0, number: 0, activitytotalprice: 0, goodsid: 0 };

    //购物商品
    this.ShoppingGoods = {};


    this.setReceipt = function (_config) {
        that.CartData = _config;

        try {

            for (var i in _config['receiptgoodsdata']) {
                that.ShoppingGoods[i] = _config['receiptgoodsdata'][i]['number'];
                var _data = _config['receiptgoodsdata'][i];
                _data['cart_number'] = _data['number'];

                create_item(_data, false);
            }
            that.Ele.shoppingcartTotalprice.html(goods_price(_config.activitytotalprice));
            that.Ele['youhui_total_price'].html(goods_price((_config.originaltotalprice - _config.activitytotalprice)));
            that.Ele.shoppingcartgoodsnumber.html(_config.buy_number_total);

            that.updateGoodsItem();

        } catch (err) {
            console.log(err);
        }
    };

    yunmall_confirm_dialog




    this.bind = function () {



        goods_item_bind($(".goods-item"));


        $(".cashregistergoodsinfo").bind('click', function () {


        });


        that.Ele.receiptsubmit.add($('#system-guadan')).bind('click', function () {

            if (validate()) {
                open();
                //yunmallOpenDialog.open( {uri:"/Receipt/receiptconfirm.html?goods=" + encodeURIComponent(  JSON.stringify( that.ShoppingGoods ) ) , title:"收银中的商品" , width:"98%" , height:"98%" }); //PostCashRegister();
            }
        });



        $(document).bind('click', function () {
            if ($(".lockbackground").is(":visible") == false)
                that.Ele.keyword.focus();
        }).trigger('click');

        window.onclick = function () {

        }

        CheckScanningGun();

    }

    this.close = function () {
        if (that.dialogdom) that.dialogdom.close();
    }


    this.selectUserOpen = function (_callback) {
        //receiptcommon.OpenDialog =  new  yunmallOpenDialog();
        receiptcommon = this.getReceiptcommon();
        //receiptcommon = parent.receiptcommon;
        receiptcommon.OpenDialog = new yunmallOpenDialog();
        receiptcommon.OpenDialog.open({ uri: "SelectUser?callback=" + _callback + "&_t=" + new Date().getTime(), title: "选择会员", width: 500, height: 400 });
    };



    this.selectUser = function (_config) {
        var receiptcommon = this.getReceiptcommon(),
            _externalframeEle = receiptcommon.Ele['external-frame'];
        if (that.OpenDialog) that.OpenDialog.close();
        //console.log(replace_param(_externalframeEle.attr('src'),'uid',_config['id']));
        _config['memberid'] = _config['id'];
        _externalframeEle.attr('src', replace_param(_externalframeEle.attr('src'), _config));


        this.Ele['huiyuaninfo'].show();
        this.Ele['sel_huiyuan'].hide();
        this.Ele['huiyuaninfo'].find('.uname').text(_config['name']);
        this.Ele['huiyuaninfo'].find('.uphone').text(_config['phone']);
        this.Ele['huiyuaninfo'].find('.uintegral').text(_config['integral']);
        this.Ele['huiyuaninfo'].find('.utotal_balance').text(goods_price(_config['money']));
        this.Ele['huiyuaninfo'].find('.udiscount').text(_config['discount']);
        this.Ele['huiyuaninfo'].find('.uaddtime').text(getLocalTime(_config['crtime']));
        this.Ele['receipt_member'].val(_config['id']);
        if (receiptcommon.Ele['discount_type']) {
            receiptcommon.Ele['discount_type'].filter("[value=1]").attr("checked", true);
        }
        if (receiptcommon.Ele['isMemberDiscount']) {
            receiptcommon.Ele['isMemberDiscount'].val("1");
        }




    };

    //选择折扣类型
    this.selectDiscountType = function (_config) {
        var receiptcommon = this.getReceiptcommon(),
            _externalframeEle = receiptcommon.Ele['external-frame'];

        if (that.OpenDialog) that.OpenDialog.close();
        _externalframeEle.attr('src', replace_param(_externalframeEle.attr('src'), _config));
    }

    //客户类型选择
    this.selectMemberType = function (_config) {
        var receiptcommon = this.getReceiptcommon(),
            _externalframeEle = receiptcommon.Ele['external-frame'];
        if (that.OpenDialog) that.OpenDialog.close();
        _externalframeEle.attr('src', replace_param(_externalframeEle.attr('src'), _config));
    }

    //是否启用会员折扣
    this.MembershipDiscount = function (_discount_type) {
        //是否启用会员折扣
        var _externalframeEle = this.Ele['external-frame'];
        _externalframeEle.attr('src', replace_param(_externalframeEle.attr('src'), 'isDiscount', _discount_type));


    };

    this.selectUser2 = function (_config) {
        var _receiptmemberinfos = $('.receiptmemberinfos'), _notlogin = $(".notlogin");

        _receiptmemberinfos.show();
        _notlogin.hide();
        _receiptmemberinfos.find('.r_username').text(_config['name']);
        _receiptmemberinfos.find('.r_total_balance').text(goods_price(_config['money']));
        _receiptmemberinfos.find('.r_integral').text(_config['integral']);
        _receiptmemberinfos.find('.r_discount').text(_config['discount']);
        $("input[name=memberid]").val(_config['id']);
        if (that.OpenDialog) that.OpenDialog.close();

        $.post('/Receipt/receiptShoppingCart.html?_t=' + new Date().getTime(), { shoppinggoods: JSON.stringify(that.ShoppingGoods), uid: _config['id'] }, function (_RESULT) {
            that.Ele['youhui_total_price'].html(goods_price((_RESULT['originaltotalprice'] - _RESULT['activitytotalprice']) / 100));
            that.Ele.shoppingcartTotalprice.html(goods_price(_RESULT['activitytotalprice']));
        }, 'json');
    }

    //不选会员
    this.quitmember = function () {
        var _receiptmemberinfos = $('.receiptmemberinfos'), _notlogin = $(".notlogin");
        _receiptmemberinfos.hide();
        _notlogin.show();
        $("input[name=memberid]").val(0);
        $.post('/Receipt/receiptShoppingCart.html?_t=' + new Date().getTime(), { shoppinggoods: JSON.stringify(that.ShoppingGoods), uid: 0 }, function (_RESULT) {
            that.Ele['youhui_total_price'].html(goods_price((_RESULT['originaltotalprice'] - _RESULT['activitytotalprice']) / 100));
            that.Ele.shoppingcartTotalprice.html(goods_price(_RESULT['activitytotalprice']));
        }, 'json');
    }

    this.setReceiptdata = function (_config) {
        ///this.setReceipt( _config );
        this.Ele['confiem_total_amount'].html(goods_price(_config['originaltotalprice']));
        this.Ele['confiem_preferential_amount'].html(goods_price(_config['discounttotalprice']));
        this.Ele['need_pay_amount'].html(goods_price(_config['originaltotalprice'] - _config['discounttotalprice']));
        this.Ele['pay_amount'].val(goods_price(_config['originaltotalprice'] - _config['discounttotalprice']));
        this.Ele['totalMoney'].val(goods_price(_config['originaltotalprice']));
        this.Ele['totalNeedMoney'].val(goods_price(_config['originaltotalprice'] - _config['discounttotalprice']));
    }

    this.Receiptresult = function (_result) {
        if (_result.status) {
            that.dialogdom && that.dialogdom.close();

            that.Ele.shoppingcart.empty();
            that.Ele.shoppingcartTotalprice.html((0).toFixed(2));
            that.Ele['youhui_total_price'].html(goods_price(0));
            that.Ele.shoppingcartgoodsnumber.html(0);

            that.updateGoodsItemClear();
            that.CartData = {};
            that.ShoppingGoods = {};

            var _OS = getSystemType();
            //打印小票
            if (_OS == 'android') {
                that.PrintAndroidPrinter(_result);
            } else if (_OS == 'pc') {
                that.PrintSmallTicket(_result);
            }
            art.dialog({
                icon: 'succeed', lock: true, content: _result.msg, title: '消息提示', time: 1,
                cancel: function () {
                    window.top.location.href = '/Receipt/Index';
                }
            });
        } else {
            art.dialog({ icon: 'face-sad', lock: true, content: _result.msg, title: '消息提示', time: 5 });
        }
    }

    //打印小票
    this.PrintSmallTicket = function (_result) {

        for (var J in _result['receiptgoodsdata']) {
            var _data = _result['receiptgoodsdata'][J];

			/*
			_data['title']
			_data['myprice']
			_data['number'] 
			_data['activitytotalprice']
			_data['province'] _data['city'] _data['district']  _data['street'] _data['address']
			_data['business']['title']
			_data['order_number']
			*/

        }
    }

    //拉起android打印机
    this.PrintAndroidPrinter = function (_data) {
        try {
            var _String = encodeURIComponent(JSON.stringify(_data));
            var _result = yunmall.androidPrinter(_String);
            if (typeof (_result) == 'string') {
                _result = decodeURIComponent(_result);
                _result = JSON.parse(_result);
            }
            if (_result && !_result['status']) {
                yunmallIframe.error(_result['msg']);
            }



        } catch (err) {
            yunmallIframe.error(err);
        }

        return false;
    }

    this.AndroidMsg = function (_result) {
        try {

            if (typeof (_result) == 'string') {
                _result = decodeURIComponent(_result);
                _result = JSON.parse(_result);
            }

            if (_result && !_result['status']) {
                yunmallIframe.error(_result['msg']);
            }

        } catch (err) {
            yunmallIframe.error("JSON格式错误");
        }
    }


    this.guadan = function (_dom) {

        art_dialog = art.dialog.confirm("是否保存挂单？", function () {
            new yunmall_reqeust(_dom);
        }, function () {

        });
    }


    this.qudan = function (orderid) {
        window.location.href = "/Receipt/index?orderid=" + orderid;
    }

    function getSystemType() {
        if (/(iPhone|iPad|iPod|iOS)/i.test(navigator.userAgent)) {  //判断iPhone|iPad|iPod|iOS
            return "ios";
        } else if (/(Android)/i.test(navigator.userAgent)) {   //判断Android
            return "android";
        } else {
            return "pc";
        };
    }

    function replace_param(_src, _queryConfig) {
        var _query = "", _host = _src;
        if (_src.indexOf('?') > -1) {
            _query = _src.substr(_src.indexOf('?') + 1);
            _host = _src.substr(0, _src.indexOf('?'));
        }

        var _paramlist = _query.split("&"), _paramdict = {};
        for (var i in _paramlist) {
            var _tmp = _paramlist[i].split("=");
            if (_tmp[0] != '') {
                _paramdict[_tmp[0]] = _tmp[1];
            }
        }

        for (var J in _queryConfig) {
            _paramdict[J] = _queryConfig[J];
        }


        var _querylist = [];
        for (var _key in _paramdict) {
            _querylist.push(_key + "=" + _paramdict[_key]);
        }



        return _host + "?" + _querylist.join("&");
    }

    this.getReceiptcommon = function () {
        return window.top.receiptcommon;
    }

    //扫码抢
    function CheckScanningGun() {
        var _start_microtime = 0, _keyupNumber = 0, Timeout = null, keyword_content = $.trim(that.Ele.keyword.val());
        that.Ele.keyword.bind('keyup', function () {
            _keyupNumber += 0;
            if (Timeout) window.clearTimeout(Timeout);
            Timeout = setTimeout(function () {
                _keyupNumber = 0;
                _start_microtime = 0;
                keyword_content = $.trim(that.Ele.keyword.val());

            }, 50)
        });

        that.Ele.keyword.bind('keypress', function () {
            if (_start_microtime <= 0) {
                start_microtime = new Date().getTime();
            }

        });

        $(document).keyup(function (event) {

            if (event.keyCode == 13) {
                var _keyword_val = $.trim(that.Ele.keyword.val());

                var reg = new RegExp("^" + keyword_content);
                _keyword_val = _keyword_val.replace(reg, '');
                if (/^[\d]{8,}$/.test(_keyword_val) && (new Date().getTime() - start_microtime) < 50) {
                    that.Ele.keyword.val(_keyword_val);
                    that.filterGoods({ keyword: _keyword_val, scanninggun: 'yes' });
                    //console.log("这是扫码抢",_keyword_val);
                } else {
                    $(".receipt-keyword-btn").trigger("click");
                }

            }
        });

    }

    function open() {
        var _uid = 0;
        if ($(".receiptmemberinfos").is(":visible")) {
            _uid = $("input[name=memberid]").val();
        }
        var orderid = $("#orderid").val();
        var _shoppinggoods = encodeURIComponent(JSON.stringify(that.ShoppingGoods));
        that.dialogdom = art.dialog.open("Confirm?products=" + _shoppinggoods + "&memberid=" + _uid + "&orderid=" + orderid, {
            title: '收银中的商品',
            width: '100%',
            lock: true,
            height: '98%',
        });
    }


    function goods_item_bind(_dom) {
        _dom.bind('click', function () {
            var _data = JSON.parse($(this).attr('data'));
            if (_data.store > 0) {
                _data['cart_number'] = 1;
                create_item(_data, true);
            }
            else {
                yunmallIframe.error("没有库存");
            }
        });
    }




    function create_item(_data, _reload) {

        _data['unitprice'] = (_data['price']);


        if (that.Ele.shoppingcart.find('li[data-goods-id=' + _data['id'] + ']').size() > 0) {
            updateNumber(that.Ele.shoppingcart.find('li[data-goods-id=' + _data['id'] + ']'), 1);
            updateTotalPrice(that.Ele.shoppingcart.find('li[data-goods-id=' + _data['id'] + ']'), 'edit');
            return;
        }

        var _str = parse_template(_data, template()),
            _dom = Append($(_str), _reload);

        _dom.find('.plus').bind('click', function () {

            removeDisabled(_dom, updateNumber(_dom, 1));

        });

        _dom.find('.min').bind('click', function () {
            removeDisabled(_dom, updateNumber(_dom, -1));

        });

        _dom.find('.delect').bind('click', function () {
            _dom.remove();
            updateTotalPrice(_dom, 'delete');
        });

    }


    function create_goods(_data) {
        _data['my_price'] = _data['price'];
        _data['mapthumimg'] = _data['thumbnail'];

        if (_data['store'] <= _data['warn']) {
            _data['goodswarning'] = 'style="display:block"';
        } else {
            _data['goodswarning'] = 'style="display:none"';
        }
        var _str = parse_template(_data, goods_template()), _dom = $(_str);
        that.Ele.goodsitems.append(_dom);
        //扫码抢
        if (_data['scanninggun'] == 'yes') {
            _data['cart_number'] = 1;
            create_item(_data, true);
        }
        goods_item_bind(_dom);
    }

    function removeDisabled(_Lidom, _cartdata) {
        var _plusEle = _Lidom.find('.plus'),
            _minEle = _Lidom.find('.min');

        if (_cartdata === false) return;

        _cartdata.num >= _cartdata.storage ? _plusEle.addClass('disabled') : _plusEle.removeClass('disabled');
        _cartdata.num <= 1 ? _minEle.addClass('disabled') : _minEle.removeClass('disabled');
        updateTotalPrice(_Lidom, 'edit');
    };

	/**
	 *计算商品总价格，商品总数量
	 */
    function updateTotalPrice(_dom, _operationtype) {

        var _NumEle = that.Ele.shoppingcart.find(".num");

        that.CartData = { totalNumber: 0, totalPrice: 0, number: 0, activitytotalprice: 0, goodsid: 0, operationtype: _operationtype };

        that.ShoppingGoods = {};
        _NumEle.each(function () {
            var _number = parseInt($(this).attr('data-number'));
            that.CartData.totalNumber += _number;
            that.CartData.totalPrice += parseInt($(this).attr('data-price')) * _number;
            that.ShoppingGoods[parseInt($(this).attr('data-goods-id'))] = _number;
        });



        that.Ele.shoppingcart.find('li').filter('[data-goods-id!=' + _dom.attr('data-goods-id') + ']').each(function () {
            that.CartData.activitytotalprice += parseInt($(this).attr('data-activityprice'));
        });

        that.CartData.number = parseInt(_dom.find('.num').attr('data-number'));

        that.CartData.goodsid = _dom.attr('data-goods-id');

        _dom.attr("data-activityprice", (that.CartData.totalPrice).toFixed(2));
        that.Ele.shoppingcartgoodsnumber.html(that.CartData.totalNumber);
        that.Ele['youhui_total_price'].html("0.00");
        that.Ele.shoppingcartTotalprice.html((that.CartData.totalPrice).toFixed(2));
        updateSingleGoodsItem(that.CartData.goodsid, that.CartData.number, that.CartData.operationtype);

        //CalculatingCommodityPrices(that.CartData, function (_result) {
        //    if (!_result['status']) {
        //        yunmessage.warning(_result.msg);
        //        return;
        //    }
        //    _dom.attr("data-activityprice", _result.goods_activity_total_price);
        //    that.Ele.shoppingcartTotalprice.html((_result.activitytotalprice).toFixed(2));
        //    that.Ele['youhui_total_price'].html(goods_price((_result['originaltotalprice'] - _result['activitytotalprice'])));
        //    that.Ele.shoppingcartgoodsnumber.html(_result.buy_number_total);
        //    updateSingleGoodsItem(_result.id, _result.number, that.CartData.operationtype);
        //});

    }



    this.page = function () {

        this.pageinit();

        if (that.Ele.goodsitemscontainer.scrollTop() <= 0) {
            paginate();
        }
        that.Ele.goodsitemscontainer.scroll(function () {
            var _containerheight = that.Ele.goodsitems.outerHeight(), _height = $(this).outerHeight(), _scrollTop = $(this).scrollTop();
            if (_height + _scrollTop + 10 >= _containerheight && !that.reloadStatus) {
                paginate();
            }
        });

    }

	/**
	 *过滤商品
	 */
    this.filterGoods = function (_config) {
        this.pageinit();
        _config && (this.pagewhere = $.extend(this.pagewhere, _config));
        paginate(function () {
            that.Ele.goodsitems.empty();
        });
    }

    this.pageinit = function () {
        this.current_page = 1;
        this.pagewhere = { _t: new Date().getTime(), isAjax: 1, page: 1, pagesize: 14 };
    }

    /*分页*/
    function paginate(_callback) {
        that.reloadStatus = true;
        $.get('/Receipt/QueryProduct', that.pagewhere, function (_RESULT) {
            that.reloadStatus = false;
            touchbottom('reload');
            _callback && _callback(_RESULT);
            if (_RESULT['data'].length > 0) {

                that.current_page += 1;
                that.pagewhere['page'] += 1;

                for (var i in _RESULT['data']) {
                    var _data = _RESULT['data'][i];
                    _data['goodsinfo'] = JSON.stringify(_data);
                    create_goods(_data);
                }




                that.updateGoodsItem();

                if (that.MaxPage-- && that.Ele.goodsitemscontainer.scrollTop() <= 0) {
                    that.Ele.goodsitemscontainer.scrollTop(1);
                    paginate();
                }


            } else {
                touchbottom('bottom');
            }



        }, 'json');
    }



    function touchbottom(_RT) {
        if (_RT == 'reload') {
            that.Ele.goodsitemscontainer.find('.noresponse').remove();
        } else {
            that.Ele.goodsitemscontainer.append('<div class="noresponse"><div class="nexplain">没有咯</div></div>');
        }


    }


	/**
	 *更改所有购买数量
	 */
    this.updateGoodsItem = function () {

        for (var _goodsid in that.ShoppingGoods) {
            $(".goods-item[data-goodsid=" + _goodsid + "]").attr('data-buy', that.ShoppingGoods[_goodsid]).addClass('buyactive').find('.buynumber').show().html(that.ShoppingGoods[_goodsid]);
        }

    }

    this.updateGoodsItemClear = function () {
        for (var _goodsid in that.ShoppingGoods) {
            $(".goods-item[data-goodsid=" + _goodsid + "]").attr('data-buy', that.ShoppingGoods[_goodsid]).removeClass('buyactive').find('.buynumber').hide().html(0);
        }
    }

	/**
	 *更新单个购买记录
	 */
    function updateSingleGoodsItem(_goodsId, _number, _operationtype) {
        var goods_item_dom = $(".goods-item[data-goodsid=" + _goodsId + "]");
        if (_operationtype == 'edit') {
            goods_item_dom.attr('data-buy', _number).addClass('buyactive').find('.buynumber').show().html(_number);
        } else {
            goods_item_dom.attr('data-buy', _number).removeClass('buyactive').find('.buynumber').hide().html(_number);
        }
    }



    function updateNumber(_Lidom, _number) {
        var _numEle = _Lidom.find('.num'), _storage = parseInt(_Lidom.attr('data-storage')), _num = parseInt(_numEle.text());
        _num = _num + _number;
        if (_num < 1) {
            _Lidom.remove();
            updateTotalPrice(_Lidom, 'delete');
            return false;
        }

        if (_num > _storage) {
            return false;
        }

        _numEle.text(_num).attr('data-number', _num);
        _Lidom.find('input.number').val(_num);

        return { num: _num, storage: _storage };
    }

    function Append(_dom, _reload) {
        var _ULEle = $("<ul/>");

        if (that.Ele.shoppingcart.find('ul').size() > 0) {
            _ULEle = that.Ele.shoppingcart.children("ul");
        } else {
            that.Ele.shoppingcart.append(_ULEle);
        }

        if (_ULEle.children('li').size()) {
            _ULEle.children('li:eq(0)').before(_dom);
        } else {

            _ULEle.append(_dom);
        }

        if (_reload) {
            updateTotalPrice(_dom, 'edit');
        }
        return _dom;
    }


    function parse_template(_data, _template) {
        return _template.replace(/\{#([\w]+)\}/g, function (_a, _b) {
            return typeof (_data[_b]) == "undefined" ? '' : _data[_b];
        });
    }

	/**
	 *提交验证
	 */
    function validate() {
        for (var j in that.ShoppingGoods) return true;

        yunmallIframe.error('请至少选择一个商品');
        return false;
    }

	/**
	 *提交收银
	 */
    function PostCashRegister() {

        that.CartData['goodsdata'] = encodeURIComponent(JSON.stringify(that.ShoppingGoods));
        $.post("/Receipt/setReceiptOrder.html?isAjax=1&_t=" + new Date().getTime(), that.CartData, function (_RESULT) {

            if (!_RESULT['status']) {
                alert(_RESULT['msg']);
            } else {
                that.Ele.shoppingcart.empty();
                CashRegisterCallback(_RESULT);
            }

        }, 'JSON');
    }

    function CashRegisterCallback(_RESULT) {
        art.dialog({
            icon: 'succeed', id: 'submitloading', lock: true, width: 200, title: "消息提示", content: _RESULT['msg'], button: [
                {
                    name: '继续收银',
                    callback: function () {
                        this.content('你同意了').time(2);
                        return false;
                    },
                    focus: true

                }, {

                    name: '查看当前收银详情',
                    callback: function () {
                        window.location.href = "/Transaction/info/id/" + _RESULT['id'] + ".html"
                        return false;
                    },


                }

            ], close: function () {
                window.location.reload();
            }
        });

    }

    //计算商品价格CalculatingCommodityPrices
    function CalculatingCommodityPrices(_CartData, _callback) {
        //{buy_number_total://总的商品数量,number://当前商品数量,originaltotalprice://总商品原始价格,Activitytotalprice:}  

        $.post("/Receipt/calculatingCommodityPrices.html?isAjax=1&_t=" + new Date().getTime(), {
            uid: strToint($("input[name=memberid]").val()),
            operationtype: _CartData.operationtype,
            goodsid: _CartData.goodsid,
            buy_number_total: _CartData.totalNumber,  //总的商品数量
            totalnumber: _CartData.number, //当前商品数量
            originaltotalprice: _CartData.totalPrice,  //总商品原始价格
            Activitytotalprice: _CartData.activitytotalprice//优惠后的商品总价

        }, function (_result) {

            if (_CartData.operationtype != 'edit') {
                delete that.ShoppingGoods[_CartData.goodsid];
            }
            _callback(_result);

        }, 'JSON');

    };


    var template = function () {
        return ['<li class="clear" data-goods-id="{#id}" data-activityprice="{#activitytotalprice}" data-storage="{#store}" >',
            '    <div class="name" title="{#name}">{#name}</div>',
            '    <div class="czbox"><input name="goodsdata[{#id}]" type="hidden" class="number" value="{#cart_number}" />',
            '        <span class="num" data-number="{#cart_number}"  data-goods-id="{#id}"  data-price="{#price}">{#cart_number}</span>',
            '        <span class="plus">+</span>',
            '        <span class="min">-</span>',
            '    </div>',
            '    <div class="prc" title="￥{#unitprice}元">￥{#unitprice}元</div>',
            '</li>'].join("\n");
    }

    var goods_template = function () {

        return ['<div class="goods-item" data=\'{#goodsinfo}\'   data-goodsid="{#id}" >',
            '        <div class="buynumber"></div>',
            '    	<a class="item-inner">',
            '             <div class="in-img"><img src="{#mapthumimg}" onerror="this.src=\'/static/images/defaultgoods.jpg\';"></div>',
            '             <div class="goods-txt">',
            '             	<div class="goods-title">{#name}</div>',
            '                <div class="goods-warning" {#goodswarning}>库存不足</div>',
            '                <div class="goods-price">￥{#my_price}元</div>',
            '             </div>',
            '        </a>',
            '</div>'].join("\n");

    }
}