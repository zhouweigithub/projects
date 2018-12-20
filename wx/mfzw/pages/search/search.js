import requestBLL from '../../js/requestBLL.js';
import commonBLL from '../../js/commonBLL.js';
import commonData from '../../js/commonData.js';


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
    if (options.year) {
      var url = "https://wx.ullfly.com/artidata/getarticallistbyyear";
      var data = {
        year: options.year,
      };
      requestBLL.getDataFromServer(url, data, this.bindData);
    }

    //增加页面点击量
    var url = "https://wx.ullfly.com/artidata/addpageclicklog";
    var data = {
      DeviceToken: wx.getStorageSync(commonData.deviceTokenCacheName),
      Page: "search",
    };
    requestBLL.postDataToServer(url, data); },
  bindData: function(datas) {
    commonBLL.resetSummaryList(datas);
    this.setData({
      datalist: datas,
      isloading: false,
    });
  },
  itemClick: function(option) {
    wx.navigateTo({
      url: '../artical/artical?id=' + option.currentTarget.dataset.id
    });
  },
  confirmTap: function(evt) {
    this.setData({
      datalist: [],
      isloading: true,
    });
    var url = "https://wx.ullfly.com/artidata/getarticallistbykeyword";
    var data = {
      keyword: evt.detail.value,
    };
    requestBLL.getDataFromServer(url, data, this.bindData);
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