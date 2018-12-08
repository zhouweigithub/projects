//获取应用实例
const app = getApp()
//var pingtu = require("../../js/pingtu.js");
import common from "../../js/common.js";
import time from "../../js/time.js";
import pingtu from "../../js/pingtu2.js";
var isPlaying = false;
var containerWidthRate = 0.8;
var windowWidth = wx.getSystemInfoSync().windowWidth;
var windowHeight = wx.getSystemInfoSync().windowHeight;

var pt;
var paras;
var containerWidth;
var pieceWidth;
var pieceHeight;
var containerMarginRate;
var isRedirectToSettingPage = false;
var thisInterval; //setInterval实例

Page({
  data: {
    usingTime: "00:00",
  },
  onLoad: function() {
    var data = init();
    this.setData(data);
    this.resetTime();
  },
  setTime: function() {
    var seconds = time.GetPlaySecondsFormatString();
    this.setData({
      usingTime: seconds
    });
  },
  resetTime: function() {
    clearInterval(thisInterval);
    time.StopSeconds();
    time.ClearPlaySeconds();
    time.StartSeconds();
    thisInterval = setInterval(this.setTime, 1000);
  },
  touchstart: function(event) {

  },
  touchend: function(event) {
    //没开始游戏则不处理
    if (!isPlaying)
      return;

    pt.exchangePosition(event);
    this.setData({
      arrary: pt.getParas().positions
    });


    if (pt.isWin()) {
      isPlaying = false;
      clearInterval(thisInterval);
      time.StopSeconds();
      var msg = '';
      var curentTime = time.GetPlaySeconds();
      var record = wx.getStorageSync(common.recordKeyFree);
      if (record == "")
        record = 0;
      if (record == 0 || curentTime < record) {
        //刷新了纪录
        record = curentTime;
        msg = "新纪录 " + time.GetPlaySecondsFormatString();
        wx.setStorageSync(common.recordKeyFree, curentTime);
      } else {
        msg = "恭喜完成";
      }
      this.setData({
        iswin: true,
        showFullScreemMask: true,
        successMsg: msg,
      });
    }
  },
  restarttap: function() {
    isPlaying = true;
    pt.init();
    this.resetTime();
    this.setData({
      arrary: pt.getParas().positions,
      iswin: false,
      showFullScreemMask: false,
    });
  },
  closettap: function() {
    this.setData({
      iswin: false,
      showFullScreemMask: false,
    });
  },
  settingtap: function() {
    isRedirectToSettingPage = true;
    wx.navigateTo({
      url: "../picture/picture"
    });
  },
  selectmodeltap: function() { //选择模式
    this.setData({
      showModelSelect: true,
      showFullScreemMask: true,
    });
  },
  freemodeltap: function(event) { //切换到自由模式
    this.setData({
      showModelSelect: false,
      showFullScreemMask: false,
    });
  },
  classicmodeltap: function(event) { //切换到经典模式
    wx.redirectTo({
      url: "../main/main"
    });
  },
  hallengemodeltap: function(event) { //切换到挑战模式
    wx.redirectTo({
      url: "../challenge/challenge"
    });
  },
  onShareAppMessage: (res) => {
    var record = wx.getStorageSync(common.recordKeyFree);
    var msg = pt.getShareMessage(record);

    if (res.from === 'button') {} else {}
    return {
      title: msg,
      path: '/pages/slide/slide',
      imageUrl: pt.getRandomImgSrc(),
      success: (res) => {},
      fail: (res) => {}
    }
  },
  onShow: function(res) {
    if (isRedirectToSettingPage) {
      var data = init();
      this.setData(data);
      this.resetTime();
    }
    isRedirectToSettingPage = false;
  },
  onHide: function(res) {},
});

function init() {
  pt = new pingtu(windowWidth, containerWidthRate);
  pt.init();
  paras = pt.getParas();
  containerWidth = paras.containerWidth;
  pieceWidth = paras.pieceWidth;
  pieceHeight = paras.pieceHeight;
  containerMarginRate = paras.containerMarginRate;
  isPlaying = true;

  return {
    windowWidth: windowWidth,
    windowHeight: windowHeight,
    containerWidth: containerWidth,
    imgSrc: pt.getImgSrc(),
    pieceWidth: pieceWidth,
    pieceHeight: pieceHeight,
    containerWidthRate: containerWidth + "px",
    containerMarginRate: windowWidth * containerMarginRate + "px",
    arrary: paras.positions,
    usingTime: "00:00",
  };
}