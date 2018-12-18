import commonData from 'commonData.js';

function addArticalData(obj) {
  var allArticals = wx.getStorageSync(commonData.allArticalsCacheName);
  if (allArticals == '') {
    allArticals = {};
    allArticals[obj.Id] = obj;
  } else {
    if (allArticals[obj.Id] == null)
      allArticals[obj.Id] = obj;
  }
  wx.setStorageSync(commonData.allArticalsCacheName, allArticals);
}

function getArticalById(id) {
  var allArticals = wx.getStorageSync(commonData.allArticalsCacheName);
  if (allArticals == '') {
    return null;
  } else {
    if (allArticals[id] != null)
      return allArticals[id];
    else
      return null;
  }
}

function isExist(id) {
  var allArticals = wx.getStorageSync(commonData.allArticalsCacheName);
  if (allArticals == '') {
    return false;
  } else {
    if (allArticals[obj.Id] == null)
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
  getArticals: getArticals,
  getArticalById: getArticalById,
}