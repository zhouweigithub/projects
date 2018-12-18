import commonData from 'commonData.js';


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