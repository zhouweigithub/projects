import common from 'common.js';

export default class pinggu {

  constructor() {
    this.gateInfoList = this.initGateInfo();
  }

  initGateInfo() {
      //http://images.ullfly.com/imgs/1.jpeg
    return {
      1: { isfree: true, difficulty: 1, limitTime: 180, img: 'http://images.ullfly.com/imgs/17.jpg' },
      2: { isfree: true, difficulty: 1, limitTime: 120, img: 'http://images.ullfly.com/imgs/18.jpg' },
      3: { isfree: true, difficulty: 1, limitTime: 90, img: 'http://images.ullfly.com/imgs/19.jpg' },
      4: { isfree: true, difficulty: 1, limitTime: 60, img: 'http://images.ullfly.com/imgs/20.jpg' },
      5: { isfree: true, difficulty: 1, limitTime: 50, img: 'http://images.ullfly.com/imgs/21.jpg' },
      6: { isfree: true, difficulty: 1, limitTime: 40, img: 'http://images.ullfly.com/imgs/22.jpg' },
      7: { isfree: true, difficulty: 1, limitTime: 30, img: 'http://images.ullfly.com/imgs/23.jpg' },
      8: { isfree: true, difficulty: 1, limitTime: 20, img: 'http://images.ullfly.com/imgs/24.jpg' },
      9: { isfree: true, difficulty: 1, limitTime: 15, img: 'http://images.ullfly.com/imgs/25.jpeg' },
      10: { isfree: true, difficulty: 1, limitTime: 10, img: 'http://images.ullfly.com/imgs/26.jpg' },
      11: { isfree: true, difficulty: 2, limitTime: 240, img: 'http://images.ullfly.com/imgs/27.jpg' },
      12: { isfree: true, difficulty: 2, limitTime: 180, img: 'http://images.ullfly.com/imgs/28.jpg' },
      13: { isfree: true, difficulty: 2, limitTime: 150, img: 'http://images.ullfly.com/imgs/29.jpeg' },
      14: { isfree: true, difficulty: 2, limitTime: 120, img: 'http://images.ullfly.com/imgs/30.jpg' },
      15: { isfree: true, difficulty: 2, limitTime: 90, img: 'http://images.ullfly.com/imgs/31.jpg' },
      16: { isfree: true, difficulty: 2, limitTime: 60, img: 'http://images.ullfly.com/imgs/32.jpg' },
      17: { isfree: true, difficulty: 3, limitTime: 240, img: 'http://images.ullfly.com/imgs/33.jpg' },
      18: { isfree: true, difficulty: 3, limitTime: 180, img: 'http://images.ullfly.com/imgs/34.jpg' },
      19: { isfree: true, difficulty: 3, limitTime: 150, img: 'http://images.ullfly.com/imgs/35.jpg' },
      20: { isfree: false, difficulty: 1, limitTime: 600, img: 'http://images.ullfly.com/imgs/36.jpg' },
      21: { isfree: false, difficulty: 1, limitTime: 480, img: 'http://images.ullfly.com/imgs/37.jpg' },
      22: { isfree: false, difficulty: 1, limitTime: 360, img: 'http://images.ullfly.com/imgs/38.jpg' },
      23: { isfree: false, difficulty: 1, limitTime: 300, img: 'http://images.ullfly.com/imgs/39.jpg' },
      24: { isfree: false, difficulty: 1, limitTime: 300, img: 'http://images.ullfly.com/imgs/40.jpg' },
    };

  }

  //更新已通关信息
  updateSuccessInfo(gate, userTime) {
    var intGate = parseInt(gate);
    //从缓存读取已通过关卡的时间
    var userTimeList = wx.getStorageSync(common.userTime) || {};
    if (userTimeList[intGate] == null || userTimeList[intGate] == 0 || userTimeList[intGate] > userTime) {
      userTimeList[intGate] = userTime; //更改当前时间
    }
    if (userTimeList[intGate + 1] == null) {
      userTimeList[intGate + 1] = 0; //解锁下一关
    }
    wx.setStorageSync(common.userTime, userTimeList);
  }


  getGateInfo(gate) {
    if (gate && this.gateInfoList[gate])
      return this.gateInfoList[gate];
    else
      return {};
  }

  getRandomImgSrc() {
    var ranNum = common.getRandomNum(0, Object.keys(this.gateInfoList).length - 1);
    return this.gateInfoList[ranNum].img;
  }

  //关卡是否为锁定状态
  isGateLocked(gate) {
    var unlockedGates = [];
    var userTimeList = wx.getStorageSync(common.userTime) || {};
    for (var a in userTimeList) {
      unlockedGates.push(a);
    }
    var unlockedMaxGate = Math.max(unlockedGates);
    var nextGate = unlockedMaxGate + 1;
    unlockedGates.push(nextGate);
    return unlockedGates.indexOf(gate) == -1;
  }

  getMaxUnlockedGate() {
    var userTimeList = wx.getStorageSync(common.userTime) || {
      1: 0
    };
    var keys = Object.keys(userTimeList);
    return parseInt(keys[keys.length - 1]);
  }

  getAllGatesArray() {
    var result = [];
    var userTimeList = wx.getStorageSync(common.userTime) || {
      1: 0
    };
    for (var a in this.gateInfoList) {
      result.push({
        gate: a,
        passTime: userTimeList[a], //通关时间，未通关的为null，刚解锁的这关为0
      });
    }

    return result;
  }

  getShareMessage(gate, state) {
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
    var successMsgList = [
      "我已经成功通过了第 " + gate + " 关，你也来试试吧！",
      "又通关了，好高兴啊！",
      "终于通过了第 " + gate + " 关，哈哈",
      "第 " + gate + " 关通关成功，哈哈",
      "第 " + gate + " 关已过，就问还有谁？还有谁？",
    ];
    var failMsgList = [
      "第 " + gate + " 关过不去啦，好着急啊！",
      "卡在第 " + gate + " 关了，能帮帮我吗？",
      "这关好难，脑瓜疼，脑瓜疼",
    ];

    var list;
    if (state && gate) {
      if (state == "success")
        list = successMsgList;
      else
        list = failMsgList;
    } else {
      list = msgList;
    }

    var ranNum = common.getRandomNum(0, list.length - 1);
    return list[ranNum];
  }

}