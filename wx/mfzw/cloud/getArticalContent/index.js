// 云函数入口文件
const cloud = require('wx-server-sdk')

cloud.init({
  env: "mycloud-ylmm4"
})

// 云函数入口函数
exports.main = async (event, context) => {
  return cloud.database().collection("artical").field({
    content: true,
    }).where({
    _id: event._id,
  }).get()
}