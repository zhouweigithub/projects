import commonData from 'commonData.js';
import requestBLL from 'requestBLL.js';


function addFavorite(id) {
  var favoriteArray = wx.getStorageSync(commonData.favoriteCacheName);
  if (favoriteArray == '') {
    favoriteArray = [id];
  } else {
    if (favoriteArray.indexOf(id) < 0)
      favoriteArray.push(id);
  }
  wx.setStorageSync(commonData.favoriteCacheName, favoriteArray);

  //post to server
  var url = "https://wx.ullfly.com/artidata/addfavorite";
  var data = {
    DeviceToken: wx.getStorageSync(commonData.deviceTokenCacheName),
    Articalid: id,
    Openid: ''
  };
  requestBLL.postDataToServer(url, data);
}

function deleteFavorite(id) {
  var intId = parseInt(id);
  var favoriteArray = wx.getStorageSync(commonData.favoriteCacheName);
  if (favoriteArray == '') {
    favoriteArray = [];
  } else {
    var index = favoriteArray.indexOf(intId);
    if (index > -1) {
      favoriteArray.splice(index, 1);
      wx.setStorageSync(commonData.favoriteCacheName, favoriteArray);
    }
  }
}

function deleteFavorites(idArray) {
  for (var i = 0; i < idArray.length; i++) {
    deleteFavorite(idArray[i]);
  }
}

function isFavorited(id) {
  var favoriteArray = wx.getStorageSync(commonData.favoriteCacheName);
  if (favoriteArray == '') {
    return false;
  } else {
    return favoriteArray.indexOf(id) > -1;
  }
}

function switchFavorite(id) {
  if (isFavorited(id))
    deleteFavorite(id);
  else
    addFavorite(id);

  return isFavorited(id);
}

function getAllFavoriteIds() {
  var favoriteArray = wx.getStorageSync(commonData.favoriteCacheName);
  if (favoriteArray == '') {
    return [];
  } else {
    return favoriteArray;
  }
}

module.exports = {
  deleteFavorites: deleteFavorites,
  switchFavorite: switchFavorite,
  isFavorited: isFavorited,
  getAllFavoriteIds: getAllFavoriteIds,
}