/*
SQLyog Ultimate v12.08 (64 bit)
MySQL - 8.0.11 : Database - swimming
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
/*Table structure for table `appointment` */

DROP TABLE IF EXISTS `appointment`;

CREATE TABLE `appointment` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `student_id` int(11) NOT NULL COMMENT '学员id',
  `course_id` int(11) NOT NULL COMMENT '课程id',
  `venue_id` int(11) NOT NULL COMMENT '场馆id',
  `start_time` datetime DEFAULT NULL COMMENT '开始时间',
  `end_time` datetime DEFAULT NULL COMMENT '结束时间',
  `state` tinyint(4) DEFAULT '0' COMMENT '状态0无教练接单 1有教练接单',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='预约请求';

/*Table structure for table `campus` */

DROP TABLE IF EXISTS `campus`;

CREATE TABLE `campus` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(20) DEFAULT NULL COMMENT '区域名称',
  `state` tinyint(4) DEFAULT NULL COMMENT '0正常 1禁用',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='校区';

/*Table structure for table `category` */

DROP TABLE IF EXISTS `category`;

CREATE TABLE `category` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL COMMENT '名称',
  `parent` int(11) NOT NULL DEFAULT '0' COMMENT '父级id',
  `type` tinyint(4) NOT NULL DEFAULT '0' COMMENT '类型0课程 1通用商品',
  `state` tinyint(4) NOT NULL DEFAULT '0' COMMENT '状态0正常 1禁用',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COMMENT='商品分类';

/*Table structure for table `class` */

DROP TABLE IF EXISTS `class`;

CREATE TABLE `class` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `course_id` int(11) NOT NULL COMMENT '课程id',
  `coach_id` int(11) NOT NULL COMMENT '教练id',
  `venue_id` int(11) NOT NULL COMMENT '场馆id',
  `start_time` datetime DEFAULT NULL COMMENT '开始时间',
  `end_time` datetime DEFAULT NULL COMMENT '结束时间',
  `state` tinyint(4) DEFAULT '0' COMMENT '0未上课 1已上课',
  `rate` float DEFAULT '0' COMMENT '教练分成比例',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COMMENT='具体每次上课信息';

/*Table structure for table `class_comment` */

DROP TABLE IF EXISTS `class_comment`;

CREATE TABLE `class_comment` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `class_id` int(11) NOT NULL COMMENT '上课id',
  `comment` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '教练评论的文字内容',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COMMENT='课程结束后教练评论的文字内容';

/*Table structure for table `class_comment_url` */

DROP TABLE IF EXISTS `class_comment_url`;

CREATE TABLE `class_comment_url` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `class_id` int(11) NOT NULL COMMENT '上课id',
  `url` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '教练上传的图片或视频地址',
  `type` tinyint(4) DEFAULT '0' COMMENT '类型0图片 1视频',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COMMENT='课程结束后教练上传的图片或视频信息';

/*Table structure for table `class_student` */

DROP TABLE IF EXISTS `class_student`;

CREATE TABLE `class_student` (
  `class_id` int(11) NOT NULL,
  `student_id` int(11) NOT NULL,
  `state` tinyint(4) DEFAULT '0' COMMENT '0正常上课 1未上课',
  `marking` float DEFAULT '-1' COMMENT '学员给本次上课打分',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`class_id`,`student_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='上课的学生';

/*Table structure for table `coach` */

DROP TABLE IF EXISTS `coach`;

CREATE TABLE `coach` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '自增',
  `username` varchar(20) NOT NULL COMMENT '用户名',
  `password` varchar(64) NOT NULL COMMENT '密码',
  `name` varchar(20) DEFAULT NULL COMMENT '姓名',
  `sex` tinyint(4) DEFAULT '0' COMMENT '性别0男 1女',
  `phone` varchar(11) DEFAULT NULL COMMENT '电话号码',
  `headimg` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '头像图片地址',
  `self_img` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '自身照片',
  `idcard_img` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '身份证照片',
  `coaching_card_img` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '教练证照片',
  `cash` int(11) NOT NULL DEFAULT '0' COMMENT '账户余额',
  `state` tinyint(4) NOT NULL DEFAULT '0' COMMENT '0正常 1受限',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`),
  UNIQUE KEY `IX_USERNAME` (`username`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COMMENT='注册教练信息表';

/*Table structure for table `coach_caption` */

DROP TABLE IF EXISTS `coach_caption`;

CREATE TABLE `coach_caption` (
  `coach_id` int(11) NOT NULL COMMENT '教练id',
  `caption_id` int(11) DEFAULT NULL COMMENT '教练队长id',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`coach_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='教练跟教练队长的关系';

/*Table structure for table `coachcaption_venue` */

DROP TABLE IF EXISTS `coachcaption_venue`;

CREATE TABLE `coachcaption_venue` (
  `coach_id` int(11) NOT NULL COMMENT '教练队长id',
  `venue_id` int(11) NOT NULL COMMENT '场馆id',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`coach_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='教练队长与场馆的关系';

/*Table structure for table `coupon` */

DROP TABLE IF EXISTS `coupon`;

CREATE TABLE `coupon` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '优惠券名称',
  `money` int(11) NOT NULL DEFAULT '0' COMMENT '金额',
  `category_id` int(11) NOT NULL DEFAULT '0' COMMENT '商品分类限定 0为不限定',
  `start_time` datetime DEFAULT NULL COMMENT '开始时间',
  `end_time` datetime DEFAULT NULL COMMENT '结束时间',
  `state` tinyint(4) DEFAULT '0' COMMENT '状态0正常1不可用',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COMMENT='优惠券基本信息';

/*Table structure for table `deliver` */

DROP TABLE IF EXISTS `deliver`;

CREATE TABLE `deliver` (
  `id` varchar(10) NOT NULL,
  `name` varchar(10) DEFAULT NULL,
  `state` tinyint(4) DEFAULT '0' COMMENT '0正常 1禁用',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='配送方式';

/*Table structure for table `discount` */

DROP TABLE IF EXISTS `discount`;

CREATE TABLE `discount` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL COMMENT '折扣名称',
  `summary` varchar(200) DEFAULT NULL COMMENT '简介',
  `money` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '立减金额',
  `state` tinyint(4) DEFAULT '0' COMMENT '状态0正常 1禁用',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COMMENT='折扣基本信息';

/*Table structure for table `order` */

DROP TABLE IF EXISTS `order`;

CREATE TABLE `order` (
  `order_id` varchar(64) NOT NULL COMMENT '订单号',
  `money` decimal(10,2) DEFAULT '0.00' COMMENT '实际支付金额',
  `discount_money` decimal(10,2) DEFAULT '0.00' COMMENT '商品折扣金额',
  `coupon_money` decimal(10,2) DEFAULT '0.00' COMMENT '优惠券抵扣金额',
  `receiver` varchar(20) DEFAULT NULL COMMENT '收货人',
  `phone` varchar(11) DEFAULT NULL COMMENT '电话号码',
  `address` varchar(200) DEFAULT NULL COMMENT '收货地址',
  `message` varchar(200) DEFAULT NULL COMMENT '买家留言',
  `deliver_id` varchar(10) DEFAULT NULL COMMENT '配送方式',
  `freight` decimal(10,2) DEFAULT '0.00' COMMENT '运费',
  `state` tinyint(4) DEFAULT '0' COMMENT '状态0未支付1已支付2已发货3已收货',
  `crdate` date DEFAULT NULL COMMENT '日期',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '时间',
  PRIMARY KEY (`order_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='购物订单信息';

/*Table structure for table `order_coupon` */

DROP TABLE IF EXISTS `order_coupon`;

CREATE TABLE `order_coupon` (
  `order_id` int(11) NOT NULL COMMENT '订单id',
  `product_id` int(11) NOT NULL COMMENT '商品id',
  `coupon_id` int(11) NOT NULL COMMENT '优惠券id',
  `name` varchar(50) NOT NULL COMMENT '优惠券名称',
  `price` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '优惠券金额',
  `count` int(11) NOT NULL COMMENT '优惠券数量',
  `money` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '优惠总金额',
  PRIMARY KEY (`order_id`,`product_id`,`coupon_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='订单优惠券信息';

/*Table structure for table `order_discount` */

DROP TABLE IF EXISTS `order_discount`;

CREATE TABLE `order_discount` (
  `order_id` int(11) NOT NULL COMMENT '订单id',
  `product_id` int(11) NOT NULL COMMENT '商品id',
  `discount_id` int(11) NOT NULL COMMENT '折扣id',
  `name` varchar(50) NOT NULL COMMENT '折扣名称',
  `money` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '立减金额',
  PRIMARY KEY (`order_id`,`product_id`,`discount_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='订单折扣信息';

/*Table structure for table `order_product` */

DROP TABLE IF EXISTS `order_product`;

CREATE TABLE `order_product` (
  `order_id` varchar(64) NOT NULL COMMENT '订单编号',
  `product_id` int(11) NOT NULL COMMENT '商品id',
  `product_name` varchar(100) DEFAULT NULL COMMENT '商品名称',
  `count` int(11) DEFAULT '0' COMMENT '数量',
  `price` decimal(10,2) DEFAULT '0.00' COMMENT '单价',
  `total_amount` decimal(10,2) DEFAULT '0.00' COMMENT '总金额',
  `discount_amount` decimal(10,2) DEFAULT '0.00' COMMENT '折扣金额',
  `coupon_amount` decimal(10,2) DEFAULT '0.00' COMMENT '优惠券抵扣金额',
  PRIMARY KEY (`order_id`,`product_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='订单中的商品信息';

/*Table structure for table `order_product_specification_detail` */

DROP TABLE IF EXISTS `order_product_specification_detail`;

CREATE TABLE `order_product_specification_detail` (
  `order_id` varchar(64) NOT NULL COMMENT '订单id',
  `product_id` int(11) NOT NULL DEFAULT '0' COMMENT '商品id',
  `plan_code` int(11) NOT NULL DEFAULT '0' COMMENT '规格组合id',
  `price` decimal(10,2) DEFAULT '0.00' COMMENT '价格',
  PRIMARY KEY (`order_id`,`product_id`,`plan_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='订单商品的规格与价格表';

/*Table structure for table `product` */

DROP TABLE IF EXISTS `product`;

CREATE TABLE `product` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '自增主键',
  `name` varchar(100) NOT NULL COMMENT '商品名称',
  `code` varchar(20) NOT NULL COMMENT '商品编码',
  `category_id` int(11) NOT NULL DEFAULT '0' COMMENT '类别',
  `summary` varchar(200) DEFAULT NULL COMMENT '简介',
  `detail_id` int(11) DEFAULT '0' COMMENT '详细描述',
  `stock` int(11) DEFAULT '9999' COMMENT '库存数量',
  `min_buy_count` int(4) DEFAULT '1' COMMENT '单次最少购买量',
  `max_buy_count` int(4) DEFAULT '9999' COMMENT '单次最大购买量',
  `main_img` varchar(100) DEFAULT NULL COMMENT '主图',
  `state` tinyint(4) DEFAULT '0' COMMENT '状态0正常 1禁用',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COMMENT='商品信息';

/*Table structure for table `product_detail` */

DROP TABLE IF EXISTS `product_detail`;

CREATE TABLE `product_detail` (
  `product_id` int(11) NOT NULL,
  `detail` mediumtext CHARACTER SET utf8 COLLATE utf8_general_ci COMMENT '商品详情',
  PRIMARY KEY (`product_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='商品详细描述';

/*Table structure for table `product_discount` */

DROP TABLE IF EXISTS `product_discount`;

CREATE TABLE `product_discount` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `product_id` int(11) NOT NULL COMMENT '商品id',
  `discount_id` int(11) NOT NULL COMMENT '折扣信息id',
  `start_time` datetime DEFAULT NULL COMMENT '折扣开始时间',
  `end_time` datetime DEFAULT NULL COMMENT '折扣结束时间',
  `state` tinyint(4) DEFAULT '0' COMMENT '状态0正常 1禁用',
  `crttime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COMMENT='商品折扣信息';

/*Table structure for table `product_headimg` */

DROP TABLE IF EXISTS `product_headimg`;

CREATE TABLE `product_headimg` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `product_id` int(11) NOT NULL COMMENT '商品id',
  `headimg_url` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '头部图片地址',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COMMENT='商品的头部图片';

/*Table structure for table `product_specification_detail` */

DROP TABLE IF EXISTS `product_specification_detail`;

CREATE TABLE `product_specification_detail` (
  `product_id` int(11) NOT NULL COMMENT '商品id',
  `plan_code` varchar(4) NOT NULL COMMENT '规格组合方案代码，用来区分不同种规格组合',
  `specification_detail_id` int(11) NOT NULL COMMENT '规格详情id',
  `state` tinyint(4) DEFAULT '0' COMMENT '0正常 1禁用',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`product_id`,`plan_code`,`specification_detail_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='商品各规格与价格表';

/*Table structure for table `product_specification_detail_price` */

DROP TABLE IF EXISTS `product_specification_detail_price`;

CREATE TABLE `product_specification_detail_price` (
  `product_id` int(11) NOT NULL COMMENT '商品id',
  `plan_code` varchar(4) NOT NULL COMMENT '规格组合方案代码，用来区分不同种规格组合',
  `price` decimal(4,0) NOT NULL DEFAULT '0' COMMENT '该规格组合对应的价格',
  PRIMARY KEY (`product_id`,`plan_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='商品各规格组合与对应价格表';

/*Table structure for table `receiver_info` */

DROP TABLE IF EXISTS `receiver_info`;

CREATE TABLE `receiver_info` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL COMMENT '用户id',
  `phone` varchar(11) DEFAULT NULL COMMENT '收货电话',
  `address` varchar(200) NOT NULL COMMENT '收货地址',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='收货地址';

/*Table structure for table `settlement` */

DROP TABLE IF EXISTS `settlement`;

CREATE TABLE `settlement` (
  `class_id` int(11) NOT NULL,
  `total_money` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '本次课时总费用',
  `coach_money` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '教练所得费用',
  `platform_money` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '平台所得费用',
  `rate` float NOT NULL DEFAULT '0' COMMENT '教练分成比例',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`class_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='教练上课后资金结算';

/*Table structure for table `specification` */

DROP TABLE IF EXISTS `specification`;

CREATE TABLE `specification` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL COMMENT '名称',
  `summary` varchar(500) DEFAULT NULL COMMENT '简介',
  `state` tinyint(4) DEFAULT '0' COMMENT '0正常 1禁用',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COMMENT='规格分类';

/*Table structure for table `specification_detail` */

DROP TABLE IF EXISTS `specification_detail`;

CREATE TABLE `specification_detail` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `specification_id` int(11) NOT NULL COMMENT '规格类型id',
  `name` varchar(50) NOT NULL COMMENT '名称',
  `summary` varchar(500) DEFAULT NULL COMMENT '描述',
  `value` int(11) DEFAULT '0' COMMENT '规格附加的值',
  `state` tinyint(4) DEFAULT '0' COMMENT '0正常 1禁用',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 COMMENT='规格详情';

/*Table structure for table `student` */

DROP TABLE IF EXISTS `student`;

CREATE TABLE `student` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '自增',
  `username` varchar(20) NOT NULL COMMENT '用户名',
  `password` varchar(64) NOT NULL COMMENT '密码',
  `name` varchar(20) DEFAULT NULL COMMENT '姓名',
  `sex` tinyint(4) DEFAULT '0' COMMENT '性别0男 1女',
  `phone` varchar(11) DEFAULT NULL COMMENT '电话号码',
  `birthday` date DEFAULT NULL COMMENT '生日',
  `headimg` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '头像图片地址',
  `state` tinyint(4) NOT NULL DEFAULT '0' COMMENT '0正常 1受限',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`),
  UNIQUE KEY `IX_USERNAME` (`username`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8 COMMENT='注册用户信息表';

/*Table structure for table `student_coupon` */

DROP TABLE IF EXISTS `student_coupon`;

CREATE TABLE `student_coupon` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `student_id` int(11) NOT NULL COMMENT '学员id',
  `coupon_id` int(11) NOT NULL COMMENT '优惠券id',
  `state` tinyint(4) DEFAULT '0' COMMENT '状态0未使用 1已使用',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8 COMMENT='学员的优惠券信息';

/*Table structure for table `student_course` */

DROP TABLE IF EXISTS `student_course`;

CREATE TABLE `student_course` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `student_id` int(11) NOT NULL COMMENT '学员id',
  `course_id` int(11) NOT NULL COMMENT '课程id',
  `venue_id` int(11) NOT NULL DEFAULT '0' COMMENT '挂靠的场馆id',
  `money` int(11) DEFAULT '0' COMMENT '课程金额',
  `state` tinyint(4) NOT NULL DEFAULT '0' COMMENT '0未支付 1已支付',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP COMMENT '下单时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='学生预约课程情况';

/*Table structure for table `venue` */

DROP TABLE IF EXISTS `venue`;

CREATE TABLE `venue` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(64) NOT NULL COMMENT '名称',
  `code` varchar(16) NOT NULL COMMENT '编码',
  `campus_id` int(11) DEFAULT NULL COMMENT '区域id',
  `address` varchar(100) DEFAULT NULL COMMENT '地址',
  `summary` varchar(1000) DEFAULT NULL COMMENT '简介',
  `state` tinyint(4) NOT NULL DEFAULT '0' COMMENT '状态 0正常 1受限',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COMMENT='场馆信息';

/*Table structure for table `withdrawal` */

DROP TABLE IF EXISTS `withdrawal`;

CREATE TABLE `withdrawal` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `coach_id` int(11) NOT NULL COMMENT '教练id',
  `money` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '提现金额',
  `state` tinyint(4) NOT NULL DEFAULT '0' COMMENT '状态0成功 1失败',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='提现记录';

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
