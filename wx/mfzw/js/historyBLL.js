import commonData from 'commonData.js';
import requestBLL from 'requestBLL.js';


function addHistory(id) {
  var historyArray = wx.getStorageSync(commonData.historyCacheName);
  if (historyArray == '') {
    historyArray = [id];
  } else {
    var index = historyArray.indexOf(id);
    if (index > -1) {
      historyArray.splice(index, 1);
    }
    historyArray.push(id);

    //只保留50个历史记录
    var maxCout = 50;
    if (historyArray.length > maxCout) {
      var deleteCount = historyArray.length - maxCout;
      historyArray.splice(0, deleteCount);
    }

    //post to server
    var url = "https://wx.ullfly.com/artidata/addhistory";
    var data = {
      DeviceToken: wx.getStorageSync(commonData.deviceTokenCacheName),
      Articalid: id,
      Openid: ''
    };
    requestBLL.postDataToServer(url, data);
  }
  wx.setStorageSync(commonData.historyCacheName, historyArray);
}

function deleteHistory(id) {
  var historyArray = wx.getStorageSync(commonData.historyCacheName);
  if (historyArray == '') {
    historyArray = [];
  } else {
    var index = historyArray.indexOf(id);
    if (index > -1) {
      historyArray.splice(index, 1);
      wx.setStorageSync(commonData.historyCacheName, historyArray);
    }
  }
}

function getAllHistoryIds() {
  var historyArray = wx.getStorageSync(commonData.historyCacheName);
  if (historyArray == '') {
    return [];
  } else {
    return historyArray;
  }
}

module.exports = {
  addHistory: addHistory,
  deleteHistory: deleteHistory,
  getAllHistoryIds: getAllHistoryIds,
}