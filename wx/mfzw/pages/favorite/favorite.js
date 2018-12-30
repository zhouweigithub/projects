import allArticalsBLL from '../../js/allArticalsBLL.js';
import favoriteBLL from '../../js/favoriteBLL.js';
import commonBLL from '../../js/commonBLL.js';
import commonData from '../../js/commonData.js';
import requestBLL from '../../js/requestBLL.js';

var selectedItemms;

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
    selectedItemms = [];
    this.initDatas();

    //增加页面点击量
    var url = "https://wx.ullfly.com/artidata/addpageclicklog";
    var data = {
      DeviceToken: wx.getStorageSync(commonData.deviceTokenCacheName),
      Page: "favorite",
    };
    requestBLL.postDataToServer(url, data);
  },
  deleteTap: function(evt) {
    favoriteBLL.deleteFavorites(selectedItemms);
    this.initDatas();
  },
  checkboxChange: function(evt) {
    selectedItemms = evt.detail.value;
  },
  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function() {

  },
  itemClick: function(option) {
    wx.navigateTo({
      url: '../artical/artical?id=' + option.currentTarget.dataset.id
    });
  },
  initDatas: function() {
    var favortedIds = favoriteBLL.getAllFavoriteIds();
    var allArticals = allArticalsBLL.getArticals(favortedIds);
    console.log(allArticals);
    commonBLL.resetSummaryList(allArticals);
    allArticals.sort((a, b) => {
      return a.CrTime > b.CrTime;
    });
    for (var i = 0; i < allArticals.length; i++) {
      allArticals[i].checked = false;
    }
    this.setData({
      articals: allArticals
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