
function resetSummaryList(dataList) {
  if (!dataList)
    return;

  for (var i = 0; i < dataList.length; i++) {
    var item = dataList[i];
    resetSummary(item)
  }
}

function resetSummary(data) {
  data.Summary = "";
  if (data.Year > 0)
    data.Summary += (data.Year + "年-");
  if (data.Area && data.Area != "")
    data.Summary += (data.Area + "卷-");
  data.Summary += "满分作文";
}


module.exports = {
  resetSummary: resetSummary,
  resetSummaryList: resetSummaryList,
}