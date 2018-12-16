import commonData from '../../js/commonData.js';
const app = getApp()

Page({

  /**
   * 页面的初始数据
   */
  data: {
    userInfo: {},
    hasUserInfo: false,
    canIUse: wx.canIUse('button.open-type.getUserInfo')
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function(options) {
    var userInfo = wx.getStorageSync(commonData.userInfoCacheName);
    if (userInfo != "") {
      this.setData({
        userInfo: userInfo,
        hasUserInfo: true
      })
    }
  },
  favoriteTap: function(obj) {
    wx.navigateTo({
      url: '../favorite/favorite'
    });
  },
  historyTap: function(obj) {
    wx.navigateTo({
      url: '../history/history'
    });
  },
  feedbackTap: function(obj) {
    wx.navigateTo({
      url: '../feedback/feedback'
    });
  },
  getUserInfo: function(e) {
    if (e.detail.userInfo) {
      wx.setStorageSync(commonData.userInfoCacheName, e.detail.userInfo)
      this.setData({
        userInfo: e.detail.userInfo,
        hasUserInfo: true
      });
    }
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