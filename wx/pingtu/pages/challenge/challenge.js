//获取应用实例
const app = getApp()
//var pingtu = require("../../js/pingtu.js");
import common from "../../js/common.js";
import time from "../../js/time.js";
import pingtu1 from "../../js/pingtu1.js";
import pingtu2 from "../../js/pingtu2.js";
import pingtu3 from "../../js/pingtu3.js";
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
var touchStartEvent = {};
var thisInterval; //setInterval实例
var pt3 = new pingtu3();
var currentGate = 1; //当前第几关
var currentGateInfo; //当前关卡信息

Page({
  data: {
    leftTimeString: "00:00",
  },
  onLoad: function() {
    var maxUnlockedGate = pt3.getMaxUnlockedGate();
    this.initGame(maxUnlockedGate);
  },
  setTime: function() {
    var seconds = time.GetPlaySeconds();
    var left = currentGateInfo.limitTime - seconds;
    if (left >= 0) {
      //还有剩余时间
      this.setData({
        leftTimeString: time.formatTime(left)
      });
    }
    if (left <= 0) {
      //时间已用完
      isPlaying = false;
      time.StopSeconds();
      time.ClearPlaySeconds();
      clearInterval(thisInterval);

      this.setData({
        showGateSelect: false,
        showWinDialog: true,
        showFullScreemMask: true,
        allGates: pt3.getAllGatesArray(),
        successMsg: "很遗憾，挑战失败",
        iswin: false,
      });
    }
  },
  resetTime: function() {
    clearInterval(thisInterval);
    time.StopSeconds();
    time.ClearPlaySeconds();
    time.StartSeconds();
    thisInterval = setInterval(this.setTime, 1000);
  },
  touchstart: function(event) {
    touchStartEvent = common.copyProperty(event);
  },
  touchend: function(event) {
    if (!isPlaying)
      return;

    if (currentGateInfo.isfree) {
      pt.exchangePosition(event);
    } else {
      var startPoint = touchStartEvent.changedTouches[0];
      var endPoint = event.changedTouches[0];
      //移动不足10个像素不处理
      if (Math.abs(startPoint.clientX - endPoint.clientX) < 10 && Math.abs(startPoint.clientY - endPoint.clientY) < 10)
        return;

      pt.movePosition(startPoint, endPoint);
    }

    this.setData({
      arrary: pt.getParas().positions
    });

    if (pt.isWin()) {
      isPlaying = false;
      var usedTime = time.GetPlaySeconds();
      time.StopSeconds();
      clearInterval(thisInterval);
      pt3.updateSuccessInfo(currentGate, usedTime);

      this.setData({
        showWinDialog: true,
        showGateSelect: false,
        showFullScreemMask: true,
        allGates: pt3.getAllGatesArray(),
        successMsg: "恭喜，挑战成功",
        iswin: true,
      });
    }
  },
  selectgatetap: function() { //选择关卡
    this.setData({
      showGateSelect: true,
      showFullScreemMask: true,
    });
  },
  selectmodeltap: function() { //选择模式
    this.setData({
      showModelSelect: true,
      showFullScreemMask: true,
    });
  },
  closewintap: function() { //关闭完成窗口
    this.setData({
      showWinDialog: false,
      showFullScreemMask: false,
    });
  },
  closegatetap: function() { //关闭选择关卡窗口
    this.setData({
      showWinDialog: false,
      showGateSelect: false,
      showFullScreemMask: false,
    });
  },
  nexttap: function() { //下一关
    currentGate += 1;
    this.initGame(currentGate);
  },
  restarttap: function() { //重新开始
    this.initGame(currentGate);
  },
  cleardatatap: function() { //清空数据
    wx.clearStorageSync();
    this.initGame(1);
  },
  gatetap: function(event) {
    //选择关卡
    if (event.currentTarget.dataset.islocked == "true") {
      this.setData({ //选择了未解锁的关卡
        showAlertText: true,
        alerttext: "尚未解锁",
      });
      setTimeout(this.hideAlertVewAuto, 1000);
    } else {
      currentGate = parseInt(event.currentTarget.dataset.gate);
      this.initGame(currentGate);
    }
  },
  hideAlertVewAuto() {
    this.setData({ //选择了未解锁的关卡
      showAlertText: false,
      alerttext: "",
    });
  },
  freemodeltap: function(event) { //切换到自由模式
    wx.redirectTo({
      url: "../slide/slide"
    });
  },
  classicmodeltap: function(event) { //切换到经典模式
    wx.redirectTo({
      url: "../main/main"
    });
  },
  hallengemodeltap: function(event) { //切换到挑战模式
    this.setData({
      showModelSelect: false,
      showFullScreemMask: false,
    });
  },
  onShareAppMessage: (res) => {
    var state = res.target.dataset.state;
    var msg = pt3.getShareMessage(currentGate, state);

    //if (res.from === 'button') {} else {}
    return {
      title: msg,
      path: '/pages/challenge/challenge',
      imageUrl: pt3.getRandomImgSrc(),
      success: (res) => {},
      fail: (res) => {}
    }
  },
  onShow: function(res) {},
  onHide: function(res) {},
  initGame: function(gate) {
    var data = this.initPage(gate);
    this.setData(data);
    this.resetTime();
  },
  initPage: function(gate = 1) {
    currentGate = gate;
    currentGateInfo = pt3.getGateInfo(gate);
    //设置拼图的格子数量 1:3x3 2:4x4 3:5x5
    wx.setStorageSync(common.difficultyKey, currentGateInfo.difficulty);

    var marginTop = 186; //用于拼图绝对定位时计算距离顶部的距离
    if (currentGateInfo.isfree)
      pt = new pingtu2(windowWidth, containerWidthRate, marginTop);
    else
      pt = new pingtu1(windowWidth, containerWidthRate, marginTop);

    pt.init();
    paras = pt.getParas();
    containerWidth = paras.containerWidth;
    pieceWidth = paras.pieceWidth;
    pieceHeight = paras.pieceHeight;
    containerMarginRate = paras.containerMarginRate;
    isPlaying = true;

    return {
      showWinDialog: false,
      showGateSelect: false,
      showFullScreemMask: false,
      showAlertText: false,
      showModelSelect: false,
      windowWidth: windowWidth,
      windowHeight: windowHeight,
      containerWidth: containerWidth,
      imgSrc: currentGateInfo.img,
      pieceWidth: pieceWidth,
      pieceHeight: pieceHeight,
      containerWidthRate: containerWidth + "px",
      containerMarginRate: windowWidth * containerMarginRate + "px",
      arrary: paras.positions,
      leftTimeString: time.formatTime(currentGateInfo.limitTime),
      isfree: currentGateInfo.isfree,
      gate: gate,
      allGates: pt3.getAllGatesArray(),
    };
  }
});