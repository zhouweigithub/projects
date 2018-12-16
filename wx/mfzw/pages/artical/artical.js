import favoriteBLL from '../../js/favoriteBLL.js';
import historyBLL from '../../js/historyBLL.js';
import allArticalsBLL from '../../js/allArticalsBLL.js';

var WxParse = require('../wxParse/wxParse.js');
var currentid = 0;

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
    var content = '<p>　　“每一只蝴蝶都是一朵花的轮回。”甲虫微笑着对我说：“有一天，你也会有一双斑斓的翅膀，在天空中蹁跹舞动。”阳光安静地淌过我每一寸藏青的肌肤，我听到山风掠过树梢，如同一支深远的骊歌。</p><p>　　“啊，你！”小甲虫尖锐的叫声在我耳边响起，我拖着湿漉漉的双翼，睁开疲惫的眼睛——浅黄的粉翅不幸地覆在地上，我从一只毛毛虫变成一只小飞蛾，我笑了。阳光明媚得有些奢侈，虽然好朋友小甲虫一直期待我成为一只最美丽的蝴蝶，轻盈地在花丛中飞舞，但我无恨亦无悔，如果注定我只是一只小飞蛾，经历了茧里的痛苦与挣扎，我也早已脱了胎，换了骨，虽然我没有斑斓的翅膀，我依然有破茧而出那一瞬的动人，我依然会用并不曼妙的舞姿歌颂这个美丽的世界，请祝福我吧，我是一只可爱的飞蛾。</p><p>　　“每只鹰的尽头都是苍穹”，妈妈的目光中充满期待，“有一天，你也会有一双坚强的翅膀，在天空中铺展你的辉煌”，轻风温柔地吹动我茸黄的毛，我听到燕语呢喃，讲述着一些古老的故事。</p><p>　　“啊，你！”妈妈沉重的叹息重重地敲打在我的心上，我当然明白，我只是一只平凡的鸡，天空只是一个太过遥远的梦，而妈妈收养我这个孤儿，一心盼望我会是天空最骄傲的一只，对不起，妈妈，我只是一只鸡，所以蓝天不是我的选择，江南三月，草长莺飞，我会在那里尽情地跑动，如一朵黄色的云掠过碧绿的天际，那里才是承载我快乐的天国，我不会把鹰的期望强锁上眉头，碾碎一个春花秋月的日子，在一片放逐理想的草原，我会和鹰一样快乐，妈妈，请祝福我吧，我是一只幸福的鸡。</p><p>　　“每一只天鹅都是天使的一次微笑”，爸爸慈祥地凝视着我，“有一天，你也会有一双洁白的羽翼，在天空中书写你高贵的美丽。”水珠轻轻地顺着我的羽毛淌下，我似乎听到雨打芭蕉，点滴着一些天荒地老的忧伤。</p><p>　　“啊，你！”爸爸失望的目光黯淡如被浇息的火炬，我天生有残疾，永远不能圆轻盈飞翔的梦想，我没有落泪，我想，有一些飞翔是不要翅膀的，有一些美丽是不需要书写的，我依然洁白如玉，在水间漫步，有我的快乐在流淌，我会微笑着，祝福那些空中的同伴。天使的姿态，不如没有翅膀的飞翔更接近天堂，祝福我吧，我是一只快乐的天鹅。</p></p><p>　　■阅卷老师点评</p><p>　　本文立意深刻，结构严谨，语言优美，极具想象力，富有哲理性。选取飞蛾、鸡、天鹅作为陈述主体，分别面对他人（“朋友”“妈妈”“爸爸”）的期望审视自我，形象地阐明“自我认识与他人期望”的关系，生动逼真，引人深思，耐人寻味。并且结构严谨，文笔不凡，行文流畅，干净利落，显示了考生驾驭语言的功夫。 </p>';
    WxParse.wxParse('article', 'html', content, this, 5);

    var id = 3;
    var title = "没有翅膀的飞翔";
    var year = "2005";
    var area = "重庆";

    allArticalsBLL.addArticalData({
      id: id,
      title: title,
      year: year,
      area: area,
      content: content,
      crtime: new Date(),
    });
    historyBLL.addHistory(id);
    currentid = id;

    var summary = "";
    if (year != '')
      summary += (year + "年　");
    if (area != '')
      summary += (area + "高考　");

    summary += "满分作文";

    var isFavorited = favoriteBLL.isFavorited(currentid);

    this.setData({
      title: title,
      summary: summary,
      isFavorited: isFavorited,
    });
  },
  onUserTap: function() {
    wx.redirectTo({
      url: '../my/my'
    });
  },
  onHomeTap: function() {
    wx.redirectTo({
      url: '../index/index'
    });
  },
  onFavoriteTap: function() {
    //收藏
    var isFavorited = favoriteBLL.switchFavorite(currentid);
    var alertMsg = isFavorited ? "收藏成功" : "删除收藏成功";
    this.setData({
      isFavorited: isFavorited,
      showAlertText: true,
      alerttext: alertMsg,
    });
    setTimeout(this.hideAlertVewAuto, 1000);
  },
  onShareAppMessage: (res) => {
    if (res.from === 'button') {} else {}
    return {
      title: msg,
      path: '/pages/artical/artical',
      // imageUrl: pt.getRandomImgSrc(),
      success: (res) => {},
      fail: (res) => {}
    }
  },
  hideAlertVewAuto() {
    this.setData({ //选择了未解锁的关卡
      showAlertText: false,
      alerttext: "",
    });
  },
  itemClick: function() {
    wx.navigateTo({
      url: '../artical/artical'
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
})