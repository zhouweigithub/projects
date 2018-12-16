import commonData from 'commonData.js';

function addArticalData(obj) {
  var allArticals = wx.getStorageSync(commonData.allArticalsCacheName);
  if (allArticals == '') {
    allArticals = {};
    allArticals[obj.id] = obj;
  } else {
    if (allArticals[obj.id] == null)
      allArticals[obj.id] = obj;
  }
  wx.setStorageSync(commonData.allArticalsCacheName, allArticals);
}

function isExist(id) {
  var allArticals = wx.getStorageSync(commonData.allArticalsCacheName);
  if (allArticals == '') {
    return false;
  } else {
    if (allArticals[obj.id] == null)
      return false;
    else
      return true;
  }
}

function getArticals(idArray) {
  var allArticals = getArticalsArray(idArray);
  return allArticals;
}

function getArticalsArray(idArray) {
  var allArticals = wx.getStorageSync(commonData.allArticalsCacheName);
  if (allArticals == '') {
    return [];
  } else {
    var result = [];
    for (var i = 0; i < idArray.length; i++) {
      result.push(allArticals[idArray[i]]);
    }
    return result;
  }

}

module.exports = {
  addArticalData: addArticalData,
  isExist: isExist,
  getArticals: getArticals
}