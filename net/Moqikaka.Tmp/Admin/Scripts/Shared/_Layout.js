var maxbox = false;//是否全屏
$(function () {
    //取得全屏的cookie
    if (getCookie("isfullscreen") == "1") {
        maxbox = true;
    } else {
        maxbox = false;
    }

    $("header li").on("click", function () {
        $(this).parent().find(".active").removeClass("active");
        $(this).addClass("active");
    });
    //取得全屏的cookie

    //排序相关
    var oldorder = getUrlParam("order");//排序字段
    var oldsort = getUrlParam("sort");//排序顺序
    //绑定排序字段
    if (oldorder && oldsort) {
        $(".orderbytableclass[orderbyfild='" + oldorder + "']").each(function (domindex, dom) {
            var domobj = $(dom);
            domobj.attr("sort", oldsort);
            domobj.html(domobj.html() + (oldsort == "asc" ? '<span class="glyphicon glyphicon-chevron-up" aria-hidden="true"></span>' : '<span class="glyphicon glyphicon-chevron-down" aria-hidden="true"></span>'));
        });
    }
    //可排序字段点击事件
    $(".orderbytableclass").click(function () {
        var that = this;
        tableOrderBy(that);
    });
    //排序相关

    //全屏相关
    $("#fullscreen").click(function () {
        var that = this;
        if (maxbox) {
            setCookie("isfullscreen", "0", 1);
            $("#leftmenbox").show();
            $("#page-wrapper").css("margin", "0 0 0 250px");
        }
        else {
            setCookie("isfullscreen", "1", 1);
            $("#leftmenbox").hide();
            $("#page-wrapper").css("margin", "0 0 0 0");
        }
        maxbox = !maxbox;
        //全屏切换将页面置于左上角
        $('html,body').animate({ scrollTop: '0px' }, 10);
        $('.table-responsive').animate({ scrollLeft: '0px' }, 10);
        $(".staticheadertableextenddiv").remove();
    });
    //全屏相关

    //表格头部固定
    $(".table").each(function () {
        var that = $(this);
        that.freezeHeader();
    });
    //表格头部固定

});
function AddModalShow(html) {
    $('#dialog').html(html);
    $('#dialog').modal('show');
}

//角色信息
function AddRoleShow(html) {
    $('#dialog').html(html);
    //$('#dialog').css('height', $('#DivShowFirst').css('height') + 150);
    //$('#dialog').css('width', $('#DivShowFirst').css('width'));
    //$('#dialog').css("margin-left", $('#DivShowFirst').width() / 2+ "px");
    //$('#dialog').css("margin-top", "50px");
    $('#dialog').modal('show');
}

function onDataMidff(data) {
    if (data) {
       
        if (data.Message) {
            var alertclass = data.Result ? "alert-success" : "alert-error";
            showAlert(data.Message, alertclass);
        }

        if (data.Result) {
            $('#dialog').modal('hide');
            $("#searchbtn").click();
        }
    }
}



function showAlert(msg, css) {
    var html = "<div  class='alert 0'><strong>1</strong></div>";
    html = html.replace(0, css).replace(1, msg);
    $("#dialogmsg").html(html);
    $('#myalert').modal('show');
    setTimeout(function () {
        $('#myalert').modal('hide')
        location = location;
    }, 1000);
}

///热云使用
function onDataReCloudMidff(data) {
    if (data) {

        if (data.Message) {
            var alertclass = data.Result ? "alert-success" : "alert-error";
            showPageMsg(data.Message, alertclass);
        }

        if (data.Result) {
            $('#dialog').modal('hide');
            $("#searchbtn").click();
        }
    }
}



function showPageMsg(msg, css) {
    var html = "<div  class='alert 0'><strong>1</strong></div>";
    html = html.replace(0, css).replace(1, msg);
    $("#dialogmsg").html(html);
    $('#myalert').modal('show');
    setTimeout(function () {
        $('#myalert').modal('hide')      
    }, 1000);
}

function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return decodeURIComponent(r[2]); return ""; //返回参数值
}

//表头排序点击事件
function tableOrderBy(element) {
    var obj = $(element);
    var orderbyfild = obj.attr("orderbyfild");
    var sort = "";
    if (orderbyfild && orderbyfild != "") {
        sort = obj.attr("sort");
        if (sort) {
            if (sort == "asc") {
                obj.html(obj.html().replace('<span class="glyphicon glyphicon-chevron-up" aria-hidden="true"></span>', '<span class="glyphicon glyphicon-chevron-down" aria-hidden="true"></span>'));
                obj.attr("sort", "desc");
                sort = "desc";
            }
            else {
                obj.html(obj.html().replace('<span class="glyphicon glyphicon-chevron-down" aria-hidden="true"></span>', '<span class="glyphicon glyphicon-chevron-up" aria-hidden="true"></span>'));
                obj.attr("sort", "asc");
                sort = "asc";
            }
        }
        else {
            obj.html(obj.html() + '<span class="glyphicon glyphicon-chevron-down" aria-hidden="true"></span>');
            obj.attr("sort", "desc");
            sort = "desc";
        }

        var oldorder = getUrlParam("order");
        var oldsort = getUrlParam("sort");

        if (location.href.indexOf("?") >= 0) {
            if (location.href.indexOf("order=") >= 0)
                location.href = location.href.replace("order=" + oldorder, "order=" + orderbyfild).replace("sort=" + oldsort, "sort=" + sort);
            else
                location.href += "&order=" + orderbyfild + "&sort=" + sort;
        }
        else {
            location.href += "?order=" + orderbyfild + "&sort=" + sort;
        }
        //alert(obj.attr("orderbyfild") + "___" + obj.attr("sort"));
    }
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i].trim();
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toGMTString();
    document.cookie = cname + "=" + cvalue + "; " + expires;
}

//错误验证时加红色边框提示
function AddValidationFailedClass(Id) {
    if (!Id.hasClass("input_validation-failed")) {
        Id.attr("class", Id.attr("class") + " input_validation-failed");
    }
}
//正确时去掉红色边框
function RemoveValidationFailedClass(Id) {
    if (Id.hasClass("input_validation-failed")) {
        Id.removeClass("input_validation-failed");
    }
}
