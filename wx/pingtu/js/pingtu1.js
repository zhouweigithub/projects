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
      //"../../images/food.jpg",
      "http://ullfly.com/images/wx/puzzle-middle.jpg",
      "http://ullfly.com/images/wx/img1.jpg",
      "http://ullfly.com/images/wx/img2.jpg",
      "http://ullfly.com/images/wx/img3.jpg",
      "http://ullfly.com/images/wx/img4.jpg",
      "http://ullfly.com/images/wx/img5.jpg",
      "http://ullfly.com/images/wx/img6.jpg",
      "http://ullfly.com/images/wx/img7.jpg",
      "http://ullfly.com/images/wx/img8.jpg",
      "http://ullfly.com/images/wx/img9.jpg",
      "http://ullfly.com/images/wx/img10.jpg",
      "http://ullfly.com/images/wx/img11.jpg",
      "http://ullfly.com/images/wx/img12.jpg",
      "http://ullfly.com/images/wx/img13.jpg",
      "http://ullfly.com/images/wx/img14.jpg",
      "http://ullfly.com/images/wx/img15.jpg",
    ];
  }

  getImgSrcList() {
    return this.imgSrcList;
  }

  getPiecePosition() {
    this.positions = [];
    for (var i = 0; i < this.pieceCountX; i++) {
      for (var j = 0; j < this.pieceCountY; j++) {
        if (i * j != (this.pieceCountX - 1) * (this.pieceCountY - 1)) {
          this.positions.push({
            origx: i,
            origy: j
          });
        }
      }
    }
    this.getRandomPiecePosition()
  }

  getRandomPiecePosition() {
    var tmp = [];
    for (var i = 0; i < this.pieceCountX; i++) {
      for (var j = 0; j < this.pieceCountY; j++) {
        if (i * j != (this.pieceCountX - 1) * (this.pieceCountY - 1)) {
          tmp.push({
            x: i,
            y: j
          });
        }
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
    this.createPositionArray();
    this.getPiecePosition();
    this.getPieceAbsolutePosition();
    this.getPieceImgPosition();
  }

  createPositionArray() {
    this.positionArray = new Array();
    for (var i = 0; i < this.pieceCountX; i++) {
      this.positionArray[i] = new Array();
      for (var j = 0; j < this.pieceCountY; j++) {
        if (i * j != (this.pieceCountX - 1) * (this.pieceCountY - 1)) {
          this.positionArray[i][j] = 1;
        } else {
          this.positionArray[i][j] = 0;
        }
      }
    }
  }

  getContainerPosition(screenWidth, marginRate, marginTop) {
    var n = (screenWidth * this.containerMarginRate);
    var marginToTop = marginTop ? marginTop : n + ((screenWidth * marginRate) / 2);
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

  getPointNearEmptyPoint(direct) {
    for (var i = 0; i < this.pieceCountX; i++) {
      for (var j = 0; j < this.pieceCountY; j++) {
        if (this.positionArray[i][j] == 0) {
          return {
            curx: i,
            cury: j
          };
        }
      }
    }
  }

  //获取向各个方向滑动时，需要被移动的格子的坐标
  getNeedMovedPosition(direct) {
    var result;
    var emptyPoint = this.getPointNearEmptyPoint(direct);
    switch (direct) {
      case "up":
        if (emptyPoint.curx < this.pieceCountY - 1)
          result = {
            curx: emptyPoint.curx + 1,
            cury: emptyPoint.cury
          };
        break;
      case "down":
        if (emptyPoint.curx > 0)
          result = {
            curx: emptyPoint.curx - 1,
            cury: emptyPoint.cury
          };
        break;
      case "left":
        if (emptyPoint.cury < this.pieceCountX - 1)
          result = {
            curx: emptyPoint.curx,
            cury: emptyPoint.cury + 1
          };
        break;
      case "right":
        if (emptyPoint.cury > 0)
          result = {
            curx: emptyPoint.curx,
            cury: emptyPoint.cury - 1
          };
        break;
    }

    return result;
  }

  getDirect(startPoint, endPoint) {
    if (startPoint && endPoint) {
      var xlength = startPoint.clientX - endPoint.clientX;
      var ylength = startPoint.clientY - endPoint.clientY;

      if (Math.abs(xlength) >= Math.abs(ylength)) {
        //水平移动
        if (startPoint.clientX > endPoint.clientX) {
          return "left";
        } else {
          return "right";
        }
      } else {
        //竖直移动
        if (startPoint.clientY > endPoint.clientY) {
          return "up";
        } else {
          return "down";
        }
      }
    }
  }

  movePosition(startChangedTouches, endChangedTouches) {
    var direct = this.getDirect(startChangedTouches, endChangedTouches);
    var curPoint = this.getNeedMovedPosition(direct); //需要被动移动的格子
    if (!curPoint)
      return;

    var curPosition = this.getPiece(curPoint);

    switch (direct) {
      case "up":
        curPosition.top = curPosition.top - this.pieceHeight //上
        curPosition.curx = curPoint.curx - 1;
        this.positionArray[curPoint.curx - 1][curPoint.cury] = 1;
        this.positionArray[curPoint.curx][curPoint.cury] = 0;
        break;
      case "down":
        curPosition.top = curPosition.top + this.pieceHeight //下
        curPosition.curx = curPoint.curx + 1;
        this.positionArray[curPoint.curx + 1][curPoint.cury] = 1;
        this.positionArray[curPoint.curx][curPoint.cury] = 0;
        break;
      case "left":
        curPosition.left = curPosition.left - this.pieceWidth //左
        curPosition.cury = curPoint.cury - 1;
        this.positionArray[curPoint.curx][curPoint.cury - 1] = 1;
        this.positionArray[curPoint.curx][curPoint.cury] = 0;
        break;
      case "right":
        curPosition.left = curPosition.left + this.pieceWidth //右
        curPosition.cury = curPoint.cury + 1;
        this.positionArray[curPoint.curx][curPoint.cury + 1] = 1;
        this.positionArray[curPoint.curx][curPoint.cury] = 0;
        break;
    }
  }


  getPieceAbsolutePosition() {
    for (var i = 0; i < this.pieceCountX; i++) {
      for (var j = 0; j < this.pieceCountY; j++) {
        if (i * j != (this.pieceCountX - 1) * (this.pieceCountY - 1)) {
          for (var k = 0; k < this.positions.length; k++) {
            if (this.positions[k].curx == i && this.positions[k].cury == j) {
              this.positions[k].top = this.containerPosition.x + this.pieceHeight * i;
              this.positions[k].left = this.containerPosition.y + this.pieceWidth * j;
            }
          }
        }
      }
    }
  }

  getPieceImgPosition() {
    for (var i = 0; i < this.pieceCountX; i++) {
      for (var j = 0; j < this.pieceCountY; j++) {
        if (i * j != (this.pieceCountX - 1) * (this.pieceCountY - 1)) {
          for (var k = 0; k < this.positions.length; k++) {
            if (this.positions[k].origx == i && this.positions[k].origy == j) {
              this.positions[k].imgx = -this.pieceWidth * j;
              this.positions[k].imgy = -this.pieceHeight * i;
            }
          }
        }
      }
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