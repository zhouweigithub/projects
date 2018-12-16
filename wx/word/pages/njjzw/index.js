import common from "../../js/common.js";
import njjzw from "../../js/njjzw.js";

var bll = new njjzw();
var currentIndex = 0; //当前第几题
var currentQuestionInfo;

Page({

  /**
   * 页面的初始数据
   */
  data: {
    showAnswer: false,
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function(options) {
    if (options.index) {
      //直接跳转到第几题
      currentIndex = parseInt(options.index);
    } else {
      //获取当前看到第几题了
      var storageIndex = wx.getStorageSync(common.currentIndex);
      if (storageIndex != "")
        currentIndex = parseInt(storageIndex);
      else {
        //随机初始化第一条数据
        var maxCount = bll.getQuestionCount();
        var ranNum = common.getRandomNum(0, maxCount - 1);
        currentIndex = ranNum;
      }
    }
    this.InitQuestion();
  },
  pretap: function() {
    if (currentIndex > 0) {
      currentIndex--;
    } else {
      //第一条的上一条取最后一条
      currentIndex = bll.getQuestionCount() - 1;
    }
    wx.setStorageSync(common.currentIndex, currentIndex);
    this.InitQuestion();
  },
  nexttap: function() {
    if (currentIndex < bll.getQuestionCount() - 1) {
      currentIndex++;
    } else {
      //最后一条的上一条取第一条
      currentIndex = 0;
    }
    wx.setStorageSync(common.currentIndex, currentIndex);
    this.InitQuestion();
  },
  showanswertap: function() {
    this.setData({
      showAnswer: true,
    });
  },
  InitQuestion() {
    //重置数据
    currentQuestionInfo = bll.getQuestion(currentIndex);
    this.setData({
      // currentIndex: currentIndex + 1,
      showAnswer: false,
      question: currentQuestionInfo.question,
      answer: currentQuestionInfo.answer,
    });
  },
  cleardatatap: function() { //清空数据
    wx.clearStorageSync();
    this.InitQuestion();
  },
  redirectToTap: function() {
    wx.redirectTo({
      url: "../cmy/index"
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
    return {
      title: bll.getShareMessage(),
      path: '/pages/njjzw/index?index=' + currentIndex,
      imageUrl: bll.getRandomImgSrc(),
      success: (res) => {},
      fail: (res) => {}
    }
  }
})