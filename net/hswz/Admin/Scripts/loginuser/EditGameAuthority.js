
var currentgame; //当前选中的游戏
var currentappid; //当前选中的版本

//填充游戏区内容
function FillGames() {
    debugger;
    var divGames = $("#divGames");
    divGames.empty();

    var index = 0;
    for (var game in games) {
        index++;
        var tmp = '<span class="itemtxt"><input type="checkbox" id="chkGame{index}" name="games" value="{value}" onchange="chkGame_Changed(this)"/><span id="spanGame{index}" onclick="spanGame_Clicked(\'{gamevalue}\',this)">{gametext}</span></span>';
        var html = tmp.replace(/{index}/g, index).replace(/{value}/g, game).replace(/{gamevalue}/g, game).replace(/{gametext}/g, games[game]);
        divGames.append(html);
        //设置选中或不选中
        var isChecked = selectedDatas != undefined && selectedDatas[game] != undefined;
        $("#chkGame" + (index)).prop("checked", isChecked);
    }
}

//填充APPID区内容
function spanGame_Clicked(game, curElement) {
    $(".autocompleteinput").hide();
    if (!game)
        return;
    //设置当前游戏
    currentgame = game;
    //清空appid与groups版面
    var appidDiv = $("#divAppids");
    appidDiv.empty();
    var divGroups = $("#divGroups");
    divGroups.empty();
    //设置选中状态
    $(curElement).parent().addClass("active");
    $(curElement).parent().siblings().removeClass("active");

    for (var index = 0; index < gameAndVersions[game].length; index++) {
        var obj = gameAndVersions[game][index];
        var tmp = '<span class="itemtxt"><input type="checkbox" id="chkAppid{index}" name="appids" value="{value}" onchange="chkAppid_Changed(\'{gamevalue}\',this)" /><span id="spanAppid{index}" onclick="spanAppid_Clicked(\'{gamevalue}\',\'{appidvalue}\',this)">{gametext}</span></span>';
        var html = tmp.replace(/{index}/g, (index + 1)).replace(/{value}/g, obj.appid).replace(/{gamevalue}/g, game).replace(/{appidvalue}/g, obj.appid).replace(/{gametext}/g, obj.version);
        appidDiv.append(html);
        //设置选中或不选中
        var isChecked = selectedDatas[game] != undefined && selectedDatas[game][obj.appid] != undefined;
        $("#chkAppid" + (index + 1)).prop("checked", isChecked);
        //设置整体被选中
        if (currentappid == obj.appid) {
            $("#chkAppid" + (index + 1)).parent().addClass("active");
            //更新分组栏
            $("#spanAppid" + (index + 1)).click();
        }
    }
}

//填充分组区内容
function spanAppid_Clicked(game, appid, curElement) {
    var divGroups = $("#divGroups");
    divGroups.empty();
    $(".autocompleteinput").hide();

    if (!appid)
        return;

    currentappid = appid;

    //设置选中状态
    $(curElement).parent().addClass("active");
    $(curElement).parent().siblings().removeClass("active");

    if (!appidAndGroups[appid])//如果没有下级分组，则直接退出
        return;

    for (var index = 0; index < appidAndGroups[appid].length; index++) {
        debugger;
        var obj = appidAndGroups[appid][index];
        var tmp = '<span class="itemtxt"><input type="checkbox" id="chkGroup{index}" name="groups" value="{value}" onchange="chkGroup_Changed(\'{gamevalue}\',\'{appidvalue}\',this)" /><label for="chkGroup{index}" id="spanGroup{index}">{gametext}</span></span>';
        var html = tmp.replace(/{index}/g, (index + 1)).replace(/{value}/g, obj.Value).replace(/{gamevalue}/g, game).replace(/{appidvalue}/g, appid).replace(/{gametext}/g, obj.Text);
        divGroups.append(html);
        //设置选中或不选中
        var isChecked = selectedDatas[game] != undefined && selectedDatas[game][appid] != undefined && (jQuery.inArray(parseInt(obj.Value), selectedDatas[game][appid]) > -1 || jQuery.inArray(obj.Value + "", selectedDatas[game][appid]) > -1 || jQuery.inArray(obj.Value, selectedDatas[game][appid]) > -1);
        $("#chkGroup" + (index + 1)).prop("checked", isChecked);
    }
    debugger;
    if ($("#autocomplete_" + appid)) {

        if (selectedAutocompleteDatas && jQuery.inArray(appid, selectedAutocompleteDatas) > -1) {
            $("#autocomplete_" + appid).prop("checked", true);
        } else {
            $("#autocomplete_" + appid).prop("false", true);
        }
        $("#autocomplete_" + appid).show();
        $("#label_autocomplete_" + appid).show();
    }

}



//列表框被选中，更新相关数据 #####################################################
function chkGame_Changed(curElement) {
    //更新数据
    UpdateDataGame(curElement);
    //若为被第选中项，则触发下级界面更新
    if ($(curElement).parent().hasClass("active")) {
        $(curElement).siblings("span").click();
    }
}

function chkAppid_Changed(game, curElement) {
    var selectedAppidCount = $("#divAppids input:checked").length;
    if (selectedAppidCount > 0) {
        $("#divGames .active input").prop("checked", true);
    }
    //更新数据
    UpdateDataAppid(game, curElement);
    //若为被第选中项，则触发下级界面更新
    if ($(curElement).parent().hasClass("active")) {
        $(curElement).siblings("span").click();
    }
}

function chkGroup_Changed(game, appid, curElement) {
    var selectedGroupCount = $("#divGroups input:checked").length;
    if (selectedGroupCount > 0) {
        $("#divAppids .active input").prop("checked", true);
        $("#divGames .active input").prop("checked", true);
    }
    debugger;
    if (!$(curElement).prop("checked")) {
        $($(".autocompleteinput:visible")[0]).prop("checked", false);
        UpdateAutoCompleteAppId($($(".autocompleteinput:visible")[0]));
    }
    //更新数据
    UpdateDataGroup(game, appid, curElement);
}



//更新 selectedDatas 数据 #####################################################
function UpdateDataGame(curElement) {
    //更新game
    var game = $(curElement).val();
    if ($(curElement).prop("checked")) {
        selectedDatas[game] = selectedDatas[game] || {};//添加当前选中游戏
    }
    else {
        //selectedDatas.splice($.inArray(game, selectedDatas), 1);
        delete selectedDatas[game];//移除当前选中游戏

        $($(".autocompleteinput:visible")[0]).prop("checked", false);
        UpdateAutoCompleteAppId($($(".autocompleteinput:visible")[0]));
    }
}

function UpdateDataAppid(game, curElement) {
    //更新appid
    var appid = $(curElement).val();

    //若之前没选中游戏，则需要先将游戏添加进数据中
    selectedDatas[game] = selectedDatas[game] || {};

    if ($(curElement).prop("checked")) {
        selectedDatas[game][appid] = selectedDatas[game][appid] || [];//添加当前选中游戏
    }
    else {
        delete selectedDatas[game][appid];//移除当前选中游戏

        $($(".autocompleteinput:visible")[0]).prop("checked", false);
        UpdateAutoCompleteAppId($($(".autocompleteinput:visible")[0]));
    }
}

function UpdateDataGroup(game, appid, curElement) {
    //更新appid
    var group = $(curElement).val();

    //若之前没选中游戏或版本，则需要先将游戏或版本添加进数据中
    selectedDatas[game] = selectedDatas[game] || {};
    selectedDatas[game][appid] = selectedDatas[game][appid] || [];

    if ($(curElement).prop("checked")) {
        //如果不存在则添加
        if ($.inArray(parseInt(group), selectedDatas[game][appid]) < 0 && $.inArray(group, selectedDatas[game][appid]) < 0 && $.inArray(group + "", selectedDatas[game][appid]) < 0)
            selectedDatas[game][appid].push(group);
    }
    else {
        if ($.inArray(parseInt(group), selectedDatas[game][appid]) > -1) {
            selectedDatas[game][appid].splice($.inArray(parseInt(group), selectedDatas[game][appid]), 1);//移除当前选中游戏
        }
        if ($.inArray(group, selectedDatas[game][appid]) > -1) {
            selectedDatas[game][appid].splice($.inArray(group, selectedDatas[game][appid]), 1);//移除当前选中游戏
        }
        if ($.inArray(group + "", selectedDatas[game][appid]) > -1) {
            selectedDatas[game][appid].splice($.inArray(group + "", selectedDatas[game][appid]), 1);//移除当前选中游戏
        }
    }
}

function UpdateAutoCompleteAppId(curElement) {
    var curappid = $(curElement).attr("id").replace("autocomplete_", "");
    if ($(curElement).prop("checked")) {
        //如果不存在则添加
        if ($.inArray(curappid, selectedAutocompleteDatas) < 0)
            selectedAutocompleteDatas.push(curappid);
    }
    else {
        selectedAutocompleteDatas.splice($.inArray(curappid, selectedAutocompleteDatas), 1);//移除当前选中游戏
    }
    debugger;
}

function Save() {
    $.post("EditGameAuthority",
        { userid: userid, authority: JSON.stringify(selectedDatas), autocomplete: JSON.stringify(selectedAutocompleteDatas) },
        function (result) {
            alert(result.Message);
            if (result.Result == true) {
                $('#dialog').modal('hide');
                $("#searchbtn").click();
            }
        }, "json");
}



//全选操作
function SelectAllGames(thisOjb) {
    var isCheckedAll = $(thisOjb).attr("IsAllSelected") == "true";
    $("#divGames input").prop("checked", isCheckedAll);
    $("#divGames input").click();
    $(thisOjb).attr("IsAllSelected", !isCheckedAll);
}
function SelectAllAppids(thisOjb) {
    var isCheckedAll = $(thisOjb).attr("IsAllSelected") == "true";
    $("#divAppids input").prop("checked", isCheckedAll);
    $("#divAppids input").click();
    $(thisOjb).attr("IsAllSelected", !isCheckedAll);
}
function SelectAllGroups(thisOjb) {
    var isCheckedAll = $(thisOjb).attr("IsAllSelected") == "true";
    $("#divGroups input").prop("checked", isCheckedAll);
    $("#divGroups input").click();
    $(thisOjb).attr("IsAllSelected", !isCheckedAll);
    //if (isCheckedAll && $(".autocompleteinput:visible").length > 0) {
    //    $($(".autocompleteinput:visible")[0]).prop("checked", false);
    //    UpdateAutoCompleteAppId($($(".autocompleteinput:visible")[0]));
    //    //$("#autocomplete").prop("checked", false);
    //}
}
function AutoCompleteGroups(thisOjb) {
    debugger;
    isautocomplete = true;
    if ($(thisOjb).is(':checked')) {
        $("#chkallgroup").prop("checked", true);
        $("#chkallgroup").attr("IsAllSelected", false);
        SelectAllGroups($("#chkallgroup"));
    }
    UpdateAutoCompleteAppId(thisOjb);
}



$(function () {
    FillGames();
});
