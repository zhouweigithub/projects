import commonData from '../../js/commonData.js';
import requestBLL from '../../js/requestBLL.js';

Page({

  /**
   * 页面的初始数据
   */
  data: {
    inputValue: '',
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function(options) {
    //增加页面点击量
    var url = "https://wx.ullfly.com/artidata/addpageclicklog";
    var data = {
      DeviceToken: wx.getStorageSync(commonData.deviceTokenCacheName),
      Page: "feedback",
    };
    requestBLL.postDataToServer(url, data);
  },
  bindFormSubmit(e) {
    var text = e.detail.value.feedbackvalue.trim();
    if (text == '') {
      this.setData({
        errmsg: "内容不能为空",
      });
    } else {
      var url = "https://wx.ullfly.com/artidata/AddFeedback";
      var data = {
        DeviceToken: wx.getStorageSync(commonData.deviceTokenCacheName),
        Content: text,
      };
      var that = this;
      requestBLL.postDataToServer(url, data, function(resp) {
        that.setData({
          showAlertText: true,
          alerttext: "提交成功",
          errmsg: '',
        });
        setTimeout(that.hideAlertVewAuto, 1000);
      });
    }
  },
  hideAlertVewAuto() {
    this.setData({ //选择了未解锁的关卡
      showAlertText: false,
      alerttext: "",
      inputValue: '',
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