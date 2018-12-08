//拼图
const difficultyKey = "settingDifficulty"; //当前使用的难度
const imgKey = "settingImg"; //当前使用的图片
const recordKeyClassic = "maxRecordClassic"; //经典模式最高纪录
const recordKeyFree = "maxRecordFree"; //自由模式最高纪录
const arrivedGate = "arrivedGate"; //通过的关卡
const userTime = "userTime"; //通过各关所用的时间


function getRandomNum(minNum, maxNum) {
  switch (arguments.length) {
    case 1:
      return parseInt(Math.random() * minNum + 1, 10);
      break;
    case 2:
      return parseInt(Math.random() * (maxNum - minNum + 1) + minNum, 10);
      break;
    default:
      return 0;
      break;
  }
}

function copyProperty(obj) {
  if (obj) {
    var result = {};
    for (let key in obj) {
      result[key] = obj[key];
    }
    return result;
  } else {
    return null;
  }
}

module.exports = {
  imgKey: imgKey,
  recordKeyClassic: recordKeyClassic,
  recordKeyFree: recordKeyFree,
  difficultyKey: difficultyKey,
  getRandomNum: getRandomNum,
  copyProperty: copyProperty,
  arrivedGate: arrivedGate,
  userTime: userTime,
}