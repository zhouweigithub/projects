import favoriteBLL from '../../js/favoriteBLL.js';
import historyBLL from '../../js/historyBLL.js';
import allArticalsBLL from '../../js/allArticalsBLL.js';
import requestBLL from '../../js/requestBLL.js';
import commonBLL from '../../js/commonBLL.js';
import commonData from '../../js/commonData.js';

var WxParse = require('../wxParse/wxParse.js');
var currentid = 0;

Page({

  /**
   * 页面的初始数据
   */
  data: {
    isloading: true,
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function(options) {
    if (options.id) {
      currentid = parseInt(options.id);
      var cachedData = allArticalsBLL.getArticalById(currentid);
      if (cachedData != null) {
        this.bindArtical(cachedData);
      } else {
        var url = "https://wx.ullfly.com/artidata/getartical";
        var data = {
          id: currentid,
        };
        requestBLL.getDataFromServer(url, data, this.bindArtical);
      }
      //获取推荐信息
      var url = "https://wx.ullfly.com/artidata/getarticallistbyrandom";
      requestBLL.getDataFromServer(url, {}, this.bindMore);

      //增加文章点击量
      var url = "https://wx.ullfly.com/artidata/addarticalclicklog";
      var data = {
        DeviceToken: wx.getStorageSync(commonData.deviceTokenCacheName),
        Articalid: currentid,
      };
      requestBLL.postDataToServer(url, data);
    }

    //增加页面点击量
    var url = "https://wx.ullfly.com/artidata/addpageclicklog";
    var data = {
      DeviceToken: wx.getStorageSync(commonData.deviceTokenCacheName),
      Page: "artial",
    };
    requestBLL.postDataToServer(url, data);
  },
  bindArtical: function(data) {
    commonBLL.resetSummary(data);
    WxParse.wxParse('article', 'html', data.Content, this, 5);
    var isFavorited = favoriteBLL.isFavorited(currentid);
    historyBLL.addHistory(currentid);
    // data.CrTime = new Date();
    allArticalsBLL.addArticalData(data);
    this.setData({
      data: data,
      isFavorited: isFavorited,
      isloading: false,
    });
  },
  bindMore: function(datas) {
    commonBLL.resetSummaryList(datas);
    this.setData({
      moreData: datas,
    });
  },
  onUserTap: function() {
    wx.redirectTo({
      url: '../my/my'
    });
  },
  onHomeTap: function() {
    wx.redirectTo({
      url: '../index/index'
    });
  },
  onFavoriteTap: function() {
    //收藏
    var isFavorited = favoriteBLL.switchFavorite(currentid);
    var alertMsg = isFavorited ? "收藏成功" : "删除收藏成功";
    this.setData({
      isFavorited: isFavorited,
      showAlertText: true,
      alerttext: alertMsg,
    });
    setTimeout(this.hideAlertVewAuto, 1000);
  },
  onShareAppMessage: (res) => {
    if (res.from === 'button') {} else {}
    return {
      title: "高考满分作文，请你欣赏！",
      path: '/pages/artical/artical',
      // imageUrl: pt.getRandomImgSrc(),
      success: (res) => {},
      fail: (res) => {}
    }
  },
  hideAlertVewAuto() {
    this.setData({ //选择了未解锁的关卡
      showAlertText: false,
      alerttext: "",
    });
  },
  itemClick: function(evt) {
    wx.redirectTo({
      url: '../artical/artical?id=' + evt.currentTarget.dataset.id
    });
  },
  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function() {

  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function() {

  },

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function() {

  },

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function() {

  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function() {

  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function() {

  },
})