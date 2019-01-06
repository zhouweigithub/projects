//$('#from_add').ajaxForm({
//    beforeSend: function () { return IsValid() },
//    uploadProgress: function (event, position, total, percentComplete) {
//    },
//    success: onDataMidff
//});

function IsValid() {
    return true;
}



function ExportExcel() {
    if (IsValid() && confirm("是否导出当前数据？")) {
        window.location.target = "_blank";

        if (getUrlParam("titlePara", true) == null)
            window.location.href = "ArticalExport";
        else
            window.location.href = "ArticalExport?titlePara=" + getUrlParam("titlePara") + "&yearPara=" + getUrlParam("yearPara") + "&areaPara=" + getUrlParam("areaPara");
    }
}



$(function () {

    $('.pager').pager({
        page: page,
        recPerPage: pagesize,
        recTotal: totalTotalRecordCount,
        pageSizeOptions: [20, 50, 100],
        elements: ['first_icon', 'prev_icon', 'pages', 'next_icon', 'last_icon', 'size_menu', 'goto', 'page_of_total_text', 'items_range_text', 'total_text'],
        onPageChange: function (state, oldState) {
            if (Object.getOwnPropertyNames(oldState).length > 0 && (state.page != oldState.page || state.recPerPage != oldState.recPerPage)) {
                window.location.href = "Index?titlePara=" + getUrlParam("titlePara") + "&yearPara=" + getUrlParam("yearPara")
                    + "&areaPara=" + getUrlParam("areaPara") + "&page=" + state.page + "&pagesize=" + state.recPerPage;
            }
        }
    });

});
