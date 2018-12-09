var WxParse = require('../wxParse/wxParse.js');

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
    var content = '<p>春天来了。<br />      <br />　　雨后的空气，是湿润了很多的，但仍然无异地夹着冰粒的西北风。春水淙淙的馨香，草木复苏的清香，桃李芬芳的芳香，我在这个城市森林里，是无从寻找的。<br /></p><p><br />  夜色已深。窗外是一片黑暗，灯光已不多，我的脸被时而的凉风掠过，有几分寒意。现在却正是春天。绿色是生命的象征，正是春天的色彩。不，还不只绿色，还有红的花，黄的阳光，蓝的湖水，我应该打开画册欣赏的。我在干什么呢？什么都让我觉得莫名其妙，原来春天的主打应该在自己手中吗？我就在这个城市里，我是不是该知足呢？<br /></p>';
    WxParse.wxParse('article', 'html', content, this, 5);
  },
  onHomeTap: function() {
    wx.redirectTo({
      url: '../index/index'
    });
  },
  onShareAppMessage: (res) => {
    if (res.from === 'button') { } else { }
    return {
      title: msg,
      path: '/pages/artical/artical',
      // imageUrl: pt.getRandomImgSrc(),
      success: (res) => { },
      fail: (res) => { }
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
})