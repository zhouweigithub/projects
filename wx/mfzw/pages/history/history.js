import allArticalsBLL from '../../js/allArticalsBLL.js';
import historyBLL from '../../js/historyBLL.js';
import commonBLL from '../../js/commonBLL.js';
import commonData from '../../js/commonData.js';
import requestBLL from '../../js/requestBLL.js';

Page({

  /**
   * 页面的初始数据
   */
  data: {

  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function(options) {
    var historyIds = historyBLL.getAllHistoryIds();
    historyIds.reverse();
    var allArticals = allArticalsBLL.getArticals(historyIds);
    commonBLL.resetSummaryList(allArticals);
    this.setData({
      articals: allArticals
    });

    //增加页面点击量
    var url = "https://wx.ullfly.com/artidata/addpageclicklog";
    var data = {
      DeviceToken: wx.getStorageSync(commonData.deviceTokenCacheName),
      Page: "history",
    };
    requestBLL.postDataToServer(url, data);
  },
  itemClick: function(option) {
    wx.navigateTo({
      url: '../artical/artical?id=' + option.currentTarget.dataset.id
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

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function() {

  }
})