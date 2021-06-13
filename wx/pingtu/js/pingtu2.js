import common from 'common.js';

export default class pinggu {

  constructor(screenWidth, containerWidthRate, marginTop) {
    this.initPieceCount();
    this.containerWidth = (screenWidth * containerWidthRate);
    this.containerMarginRate = (1 - containerWidthRate) / 2;
    this.pieceWidth = (this.containerWidth / this.pieceCountX);
    this.pieceHeight = (this.containerWidth / this.pieceCountY);
    this.containerPosition = this.getContainerPosition(screenWidth, containerWidthRate, marginTop);
    this.imgSrcList = [
      "http://images.ullfly.com/imgs/1.jpeg",
      "http://images.ullfly.com/imgs/2.jpg",
      "http://images.ullfly.com/imgs/3.jpg",
      "http://images.ullfly.com/imgs/4.jpeg",
      "http://images.ullfly.com/imgs/5.jpg",
      "http://images.ullfly.com/imgs/6.jpeg",
      "http://images.ullfly.com/imgs/7.jpeg",
      "http://images.ullfly.com/imgs/8.jpeg",
      "http://images.ullfly.com/imgs/9.jpeg",
      "http://images.ullfly.com/imgs/10.jpg",
      "http://images.ullfly.com/imgs/11.jpg",
      "http://images.ullfly.com/imgs/12.jpg",
      "http://images.ullfly.com/imgs/13.jpg",
      "http://images.ullfly.com/imgs/14.jpg",
      "http://images.ullfly.com/imgs/15.jpg",
      "http://images.ullfly.com/imgs/16.jpg",
    ];
  }

  getImgSrcList() {
    return this.imgSrcList;
  }

  getPiecePosition() {
    this.positions = [];
    for (var i = 0; i < this.pieceCountX; i++) {
      for (var j = 0; j < this.pieceCountY; j++) {
        this.positions.push({
          origx: i,
          origy: j
        });
      }
    }
    this.getRandomPiecePosition()
  }

  getRandomPiecePosition() {
    var tmp = [];
    for (var i = 0; i < this.pieceCountX; i++) {
      for (var j = 0; j < this.pieceCountY; j++) {
        tmp.push({
          x: i,
          y: j
        });
      }
    }

    var tmpCount = tmp.length;
    for (var i = 0; i < tmpCount; i++) {
      var ranNum = common.getRandomNum(0, tmp.length - 1);
      this.positions[i].curx = tmp[ranNum].x;
      this.positions[i].cury = tmp[ranNum].y;
      tmp.splice(ranNum, 1);
    }
  }

  getParas() {
    return {
      containerWidth: this.containerWidth,
      containerMarginRate: this.containerMarginRate,
      pieceWidth: this.pieceWidth,
      pieceHeight: this.pieceHeight,
      containerPosition: this.containerPosition,
      positions: this.positions,
    }
  }

  init() {
    //this.createPositionArray();
    this.getPiecePosition();
    this.getPieceAbsolutePosition();
    this.getPieceImgPosition();
  }

  getContainerPosition(screenWidth, marginRate, marginTop) {
    var n = (screenWidth * this.containerMarginRate);
    var marginToTop = marginTop ? marginTop : ((screenWidth * marginRate) / 3) + 15;
    return {
      x: marginToTop,
      y: n
    };
  }

  getPiece(point) {
    for (var k = 0; k < this.positions.length; k++) {
      if (this.positions[k].curx == point.curx && this.positions[k].cury == point.cury) {
        return this.positions[k];
      }
    }
  }

  getPieceAbsolutePosition() {
    for (var i = 0; i < this.pieceCountX; i++) {
      for (var j = 0; j < this.pieceCountY; j++) {
        for (var k = 0; k < this.positions.length; k++) {
          if (this.positions[k].curx == i && this.positions[k].cury == j) {
            this.positions[k].top = this.containerPosition.x + this.pieceHeight * i;
            this.positions[k].left = this.containerPosition.y + this.pieceWidth * j;
          }
        }
      }
    }
  }

  getPieceImgPosition() {
    for (var i = 0; i < this.pieceCountX; i++) {
      for (var j = 0; j < this.pieceCountY; j++) {
        for (var k = 0; k < this.positions.length; k++) {
          if (this.positions[k].origx == i && this.positions[k].origy == j) {
            this.positions[k].imgx = -this.pieceWidth * j;
            this.positions[k].imgy = -this.pieceHeight * i;
          }
        }
      }
    }
  }

  //根据页面坐标获取格子坐标
  getPositionByClientPosition(top, left) {
    var result = {};
    result.curx = parseInt((top - this.containerPosition.x) / this.pieceWidth);
    result.cury = parseInt((left - this.containerPosition.y) / this.pieceHeight);
    return result;
  }

  //根据页面坐标获取格子数据
  getPieceByClientPosition(top, left) {
    var p = this.getPositionByClientPosition(top, left);
    return this.getPiece(p);
  }

  //滑动时交换两个坐标的数据
  exchangePosition(event) {
    var piece1 = this.getPiece({
      curx: event.target.dataset.curx,
      cury: event.target.dataset.cury
    });
    var piece2 = this.getPieceByClientPosition(event.changedTouches[0].clientY, event.changedTouches[0].clientX);
    //是否相临
    var isNear = Math.abs(piece1.curx - piece2.curx) + Math.abs(piece1.cury - piece2.cury) == 1;
    if (piece1 && piece2 && piece1 != piece2 && isNear) {
      var piece1Copy = common.copyProperty(piece1);

      piece1.curx = piece2.curx;
      piece1.cury = piece2.cury;
      piece1.top = piece2.top;
      piece1.left = piece2.left;

      piece2.curx = piece1Copy.curx;
      piece2.cury = piece1Copy.cury;
      piece2.top = piece1Copy.top;
      piece2.left = piece1Copy.left;
    }
  }

  isWin() {
    for (var i = 0; i < this.positions.length; i++) {
      var piece = this.positions[i];
      if (piece.curx != piece.origx || piece.cury != piece.origy)
        return false;
    }
    return true;
  }

  initPieceCount() {
    var difficulty = wx.getStorageSync(common.difficultyKey)
    switch (difficulty) {
      case "":
      case 1:
        this.pieceCountX = 3;
        this.pieceCountY = 3;
        break;
      case 2:
        this.pieceCountX = 4;
        this.pieceCountY = 4;
        break;
      case 3:
        this.pieceCountX = 5;
        this.pieceCountY = 5;
        break;
    }
  }

  getImgSrc() {
    var imgIndex = wx.getStorageSync(common.imgKey);
    if (imgIndex == "")
      imgIndex = 0;
    return this.imgSrcList[imgIndex];
  }

  getRandomImgSrc() {
    var ranNum = common.getRandomNum(0, this.imgSrcList.length - 1);
    return this.imgSrcList[ranNum];
  }


  getShareMessage(record) {
    var msgList = [
      "快来和我一起玩拼图，等你哟！",
      "一款简单有趣的拼图小游戏",
      "休闲拼图小游戏",
      "赶紧试试你用多长时间能拼出来",
      "百变生活，自由拼图",
      "终于拼出来了，哈哈",
      "你能拼好这图吗？",
      "拼图？试试？",
      "没有最快，只有更快，你能破我的记录吗？",
      "我的拼图，我的快乐",
      "今天你拼图了吗？",
      "今天你拼成功一次了吗？",
      "拼图游戏，敢挑战吗？",
      "吃鸡是不可能吃鸡的，只能玩玩小游戏这样子的",
      "你能拼出来算我输",
      "能拼出来的都是人才",
      "我又破纪录了，你呢？",
      "不是每种牛奶都叫特能输",
      "今天不拼出来我就不睡觉了",
      "闲暇时光，其乐无穷",
      "说简单不简单，说难不难，你准备好了吗？",
      "我叫你，你敢应吗？",
      "始终拼不上，能帮我想想办法吗？",
      "呵呵，有点意思",
      "不玩了不玩了，太磨人了",
      "简简单单才是真",
      "无聊？试试？",
      "再不理我，我可要生气了哟",
      "走过路过千万不要错过",
      "再不试试就没机会了",
      "饭前来一把，快活似神仙",
      "就问你服不服",
      "拼图小游戏",
      "你的闲暇时光由我来陪",
      "放开那个女孩，让我来，还是你来吧",
      "再不拼图就老了",
      "爱我就不要离开我",
      "这破游戏，我才不玩呢",
      "一点都不好玩",
      "贾君鹏，你妈叫你回家吃饭呢",
      "不要失去了才后悔莫急",
      "相见恨晚",
      "是兄弟就跟我一起玩",
    ];
    if (record && record != '') {
      msgList.push("我只用了 " + record + " 秒就完成了，你也来试试吧！");
    }
    var ranNum = common.getRandomNum(0, msgList.length - 1);
    return msgList[ranNum];
  }

}