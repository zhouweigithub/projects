//index.js
//获取应用实例
const app = getApp()
import common from "../../js/common.js";
import pingtu from "../../js/pingtu1.js";
const marginWidth = 10;
var windowWidth = wx.getSystemInfoSync().windowWidth;
var windowHeight = wx.getSystemInfoSync().windowHeight;
var imgWidthHeight = (windowWidth - 36) / 2;
var pt = new pingtu(windowWidth, 0.8);

Page({
  data: {
    imgSrcList: pt.getImgSrcList(),
    marginWidth: marginWidth,
    imgWidthHeight: imgWidthHeight,
  },
  onLoad: function() {
    var difficulty = wx.getStorageSync(common.difficultyKey)
    var img = wx.getStorageSync(common.imgKey)
    this.setData({
      difficultySelected: difficulty == "" ? 1 : difficulty,
      imgSelected: (img == "" ? 0 : img),
    });
  },
  difficultyclicktap: function(event) {
    var data = event.target;
    wx.setStorageSync(common.difficultyKey, parseInt(data.dataset.difficulty));
    this.setData({
      difficultySelected: data.dataset.difficulty
    });
  },
  imgclicktap: function(event) {
    var data = event.target;
    wx.setStorageSync(common.imgKey, data.dataset.img);
    this.setData({
      imgSelected: data.dataset.img
    });
  }

});