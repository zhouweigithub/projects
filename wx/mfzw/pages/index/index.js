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
    var headUrl = '/images/unknown.png';
    var userInfo = wx.getStorageSync(commonData.userInfoCacheName);
    if (userInfo != "") {
      headUrl = userInfo.avatarUrl;
    }
    this.setData({
      avatarUrl: headUrl,
    });

    //增加页面点击量
    var url = "https://wx.ullfly.com/artidata/addpageclicklog";
    var data = {
      DeviceToken: wx.getStorageSync(commonData.deviceTokenCacheName),
      Page: "index",
    };
    requestBLL.postDataToServer(url, data);
  },
  itemClick: function(option) {
    wx.navigateTo({
      url: '../search/search?year=' + option.currentTarget.dataset.year,
    });
  },
  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function() {

  },
  onUserTap: function() {
    wx.navigateTo({
      url: '../my/my'
    });
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