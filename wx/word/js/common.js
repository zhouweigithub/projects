//脑筋急转弯
const currentIndex = "currentIndex"; //当前第几题
//猜谜语
const currentMyIndex = "currentMyIndex"; //当前第几题


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
  getRandomNum: getRandomNum,
  copyProperty: copyProperty,
  currentIndex: currentIndex,
  currentMyIndex: currentMyIndex,
}