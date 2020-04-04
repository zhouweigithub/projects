function IsValueNotEmpty(elementid, errmsgelementid, name) {
    if ($("#" + elementid).val() == null || $("#" + elementid).val() == "") {
        $("#" + errmsgelementid).text(name + "不能为空！").show();
        return false;
    }
    else {
        $("#" + errmsgelementid).text("").hide();
        return true;
    }
}

function IsValueNumber(elementid, errmsgelementid, name) {
    if (!/^\d+$/.test($("#" + elementid).val())) {
        $("#" + errmsgelementid).text(name + "只能输入数字！").show();
        return false;
    }
    else {
        $("#" + errmsgelementid).text("").hide();
        return true;
    }
}

function IsValueDecimal(elementid, errmsgelementid, name) {
    if ($("#" + elementid).val() == null || $("#" + elementid).val() == "" || isNaN($("#" + elementid).val())) {
        $("#" + errmsgelementid).text(name + "只能输入数字！").show();
        return false;
    }
    else {
        $("#" + errmsgelementid).text("").hide();
        return true;
    }
}
function IsValue1Or0(elementid, errmsgelementid, name) {
    if ($("#" + elementid).val() != "1" && $("#" + elementid).val() != "0") {
        $("#" + errmsgelementid).text(name + "只能为1或0！").show();
        return false;
    }
    else {
        $("#" + errmsgelementid).text("").hide();
        return true;
    }
}
function IsValueNot0(elementid, errmsgelementid, name) {
    if ($("#" + elementid).val() == 0) {
        $("#" + errmsgelementid).text(name + "不能0！").show();
        return false;
    }
    else {
        $("#" + errmsgelementid).text("").hide();
        return true;
    }
}

function IsMobile(mobile) {
    debugger;
    var myreg = /^(((13[0-9]{1})|(14[0-9]{1})|(15[0-9]{1})|(17[0-9]{1})|(18[0-9]{1}))+\d{8})$/; 
    return myreg.test(mobile);
}