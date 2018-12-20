import commonData from 'commonData.js';


// function getDataFromServer(year, callback) {

//   wx.request({
//     url: "https://wx.ullfly.com/artidata/getarticallistbyyear",
//     data: {
//       year: year
//     },
//     method: "post",
//     dataType: "json",
//     success: function(resp) {
//       if (callback) {
//         callback(resp.data);
//       }
//     },
//     fail: function(err) {
//       console.log(err);
//     },
//     complete: function(obj) {},
//   });

// }

function getDataFromServer(url, data, callback) {
  wx.request({
    url: url,
    data: data,
    method: "get",
    dataType: "json",
    success: function(resp) {
      if (callback) {
        callback(resp.data);
      }
    },
    fail: function(err) {
      console.log(err);
    },
    complete: function(obj) {},
  });
}

function postDataToServer(url, data, callback) {
  wx.request({
    url: url,
    data: data,
    method: "post",
    dataType: "json",
    success: function (resp) {
      if (callback) {
        callback(resp.data);
      }
    },
    fail: function (err) {
      console.log(err);
    },
    complete: function (obj) { },
  });
}

module.exports = {
  getDataFromServer: getDataFromServer,
  postDataToServer: postDataToServer,
}