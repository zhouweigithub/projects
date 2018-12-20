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


function getRandomString(len) {　　
  len = len || 32;　　
  //ABCDEFGHJKMNPQRSTWXYZabcdefhijkmnprstwxyz2345678
  var $chars = 'ABCDEFGHJKMNPQRSTWXYZ2345678'; /****默认去掉了容易混淆的字符oOLl,9gq,Vv,Uu,I1****/ 　　
  var maxPos = $chars.length;　　
  var pwd = '';　　
  for (var i = 0; i < len; i++) {　　　　
    pwd += $chars.charAt(Math.floor(Math.random() * maxPos));　　
  }　　
  return pwd;
}

module.exports = {
  getRandomString: getRandomString,
  resetSummary: resetSummary,
  resetSummaryList: resetSummaryList,
}