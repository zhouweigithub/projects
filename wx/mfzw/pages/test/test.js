// pages/test/test.js
Page({

  /**
   * 页面的初始数据
   */
  data: {

  },
  qiuhe() {
    wx.cloud.callFunction({
      name: "getArticalInfo",
      success(res) {
        console.log(res);
      },
      fail(err) {
        console.log(err);
      }
    })

    wx.cloud.callFunction({
      name: "getArticalContent",
      data: {
        _id: "17b0c7775e9a9eb9005b8f466b9614db"
      },
      success(res) {
        console.log(res);
      },
      fail(err) {
        console.log(err);
      }
    })

  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    this.qiuhe()
  },

  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function () {

  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {

  },

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function () {

  },

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function () {

  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function () {

  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function () {

  },

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function () {

  }
})