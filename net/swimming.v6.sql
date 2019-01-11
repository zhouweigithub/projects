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
/*Table structure for table `banner` */

DROP TABLE IF EXISTS `banner`;

CREATE TABLE `banner` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `url` varchar(500) NOT NULL COMMENT '图片地址',
  `state` tinyint(4) DEFAULT '0' COMMENT '0正常 1禁用',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='首页顶部滚动图片';

/*Data for the table `banner` */

/*Table structure for table `booking_coach_queue` */

DROP TABLE IF EXISTS `booking_coach_queue`;

CREATE TABLE `booking_coach_queue` (
  `class_id` int(11) NOT NULL,
  `coach_id` int(11) NOT NULL,
  `end_time` datetime DEFAULT NULL,
  PRIMARY KEY (`class_id`,`coach_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='接单时教练限定';

/*Data for the table `booking_coach_queue` */

/*Table structure for table `campus` */

DROP TABLE IF EXISTS `campus`;

CREATE TABLE `campus` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(20) DEFAULT NULL COMMENT '区域名称',
  `state` tinyint(4) DEFAULT NULL COMMENT '0正常 1禁用',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='校区';

/*Data for the table `campus` */

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

/*Data for the table `category` */

insert  into `category`(`id`,`name`,`parent`,`type`,`state`,`crtime`) values (1,'游泳',0,0,0,'2019-01-07 15:56:42');

/*Table structure for table `class` */

DROP TABLE IF EXISTS `class`;

CREATE TABLE `class` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `product_id` int(11) NOT NULL COMMENT '课程id',
  `coach_id` int(11) NOT NULL COMMENT '教练id',
  `venue_id` int(11) NOT NULL COMMENT '场馆id',
  `hour` int(11) DEFAULT '0' COMMENT '第几课时',
  `max_count` int(11) DEFAULT '0' COMMENT '最大教学人数',
  `start_time` datetime DEFAULT NULL COMMENT '开始时间',
  `end_time` datetime DEFAULT NULL COMMENT '结束时间',
  `state` tinyint(4) DEFAULT '0' COMMENT '0未上课 1已上课 2教练已评价',
  `rate` float DEFAULT '0' COMMENT '教练分成比例',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COMMENT='具体每次预约及后续上课信息';

/*Data for the table `class` */

insert  into `class`(`id`,`product_id`,`coach_id`,`venue_id`,`hour`,`max_count`,`start_time`,`end_time`,`state`,`rate`,`crtime`) values (1,1,0,1,5,2,'2019-01-12 11:23:35','2019-01-12 11:23:39',0,0,'2019-01-08 11:23:43'),(2,1,0,1,4,1,'2019-01-13 16:28:35','2019-01-13 16:28:42',0,0,'2019-01-11 16:28:45');

/*Table structure for table `class_comment` */

DROP TABLE IF EXISTS `class_comment`;

CREATE TABLE `class_comment` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `class_id` int(11) NOT NULL COMMENT '上课id',
  `comment` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '教练评论的文字内容',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COMMENT='课程结束后教练评论的文字内容';

/*Data for the table `class_comment` */

insert  into `class_comment`(`id`,`class_id`,`comment`,`crtime`) values (1,1,'学员们非常不错哦','2019-01-08 11:25:46');

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

/*Data for the table `class_comment_url` */

insert  into `class_comment_url`(`id`,`class_id`,`url`,`type`,`crtime`) values (1,1,'http://pic-bucket.ws.126.net/photo/0005/2019-01-07/E4ULVC640B4C0005NOS.jpg',0,'2019-01-08 11:26:00'),(2,1,'http://h.hiphotos.baidu.com/image/h%3D300/sign=f2db86688ccb39dbdec06156e01709a7/2f738bd4b31c87018e9450642a7f9e2f0708ff16.jpg',1,'2019-01-08 14:20:15');

/*Table structure for table `class_student` */

DROP TABLE IF EXISTS `class_student`;

CREATE TABLE `class_student` (
  `class_id` int(11) NOT NULL,
  `student_id` int(11) NOT NULL,
  `state` tinyint(4) DEFAULT '0' COMMENT '0预约中 1预约成功 2已上课 3已评价',
  `marking` float DEFAULT '-1' COMMENT '学员给本次上课打分',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`class_id`,`student_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='上课的学生';

/*Data for the table `class_student` */

insert  into `class_student`(`class_id`,`student_id`,`state`,`marking`,`crtime`) values (1,1,0,-1,'2019-01-11 16:59:28'),(1,2,0,-1,'2019-01-11 17:11:27'),(1,3,0,-1,'2019-01-11 17:13:54'),(2,4,0,-1,'2019-01-11 17:38:53');

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
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 COMMENT='注册教练信息表';

/*Data for the table `coach` */

insert  into `coach`(`id`,`username`,`password`,`name`,`sex`,`phone`,`headimg`,`self_img`,`idcard_img`,`coaching_card_img`,`cash`,`state`,`crtime`) values (1,'xxx','fff','李咏',0,'125412541','http://cms-bucket.nosdn.127.net/2355aab3ab86470593a7fe33239a09cb20170105080207.jpg','http://cms-bucket.nosdn.127.net/2355aab3ab86470593a7fe33239a09cb20170105080207.jpg','http://cms-bucket.nosdn.127.net/2355aab3ab86470593a7fe33239a09cb20170105080207.jpg','http://cms-bucket.nosdn.127.net/2355aab3ab86470593a7fe33239a09cb20170105080207.jpg',0,0,'2019-01-08 11:23:10'),(2,'ttt','ss','李白',1,'3456798765',NULL,NULL,NULL,NULL,0,0,'2019-01-11 16:16:47'),(3,'eee','ggg','陆游',0,'33333333333',NULL,NULL,NULL,NULL,0,0,'2019-01-11 16:17:45'),(4,'77','88','陈坤',0,NULL,NULL,NULL,NULL,NULL,0,0,'2019-01-11 16:30:57');

/*Table structure for table `coach_caption` */

DROP TABLE IF EXISTS `coach_caption`;

CREATE TABLE `coach_caption` (
  `coach_id` int(11) NOT NULL COMMENT '教练id',
  `caption_id` int(11) DEFAULT NULL COMMENT '教练队长id',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`coach_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='教练跟教练队长的关系';

/*Data for the table `coach_caption` */

insert  into `coach_caption`(`coach_id`,`caption_id`,`crtime`) values (1,2,'2019-01-11 16:16:16'),(2,2,'2019-01-11 16:30:31'),(3,2,'2019-01-11 16:17:53'),(4,4,'2019-01-11 16:31:10');

/*Table structure for table `coachcaption_venue` */

DROP TABLE IF EXISTS `coachcaption_venue`;

CREATE TABLE `coachcaption_venue` (
  `coach_id` int(11) NOT NULL COMMENT '教练队长id',
  `venue_id` int(11) NOT NULL COMMENT '场馆id',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`coach_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='教练队长与场馆的关系';

/*Data for the table `coachcaption_venue` */

insert  into `coachcaption_venue`(`coach_id`,`venue_id`,`crtime`) values (2,1,'2019-01-11 16:17:05');

/*Table structure for table `coupon` */

DROP TABLE IF EXISTS `coupon`;

CREATE TABLE `coupon` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '优惠券名称',
  `money` int(11) NOT NULL DEFAULT '0' COMMENT '金额',
  `category_id` int(11) NOT NULL DEFAULT '0' COMMENT '商品分类限定 0为不限定',
  `multiple` tinyint(4) DEFAULT '0' COMMENT '是否可以叠加使用0可以 1不可以',
  `start_time` datetime DEFAULT NULL COMMENT '开始时间',
  `end_time` datetime DEFAULT NULL COMMENT '结束时间',
  `state` tinyint(4) DEFAULT '0' COMMENT '状态0正常1不可用',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COMMENT='优惠券基本信息';

/*Data for the table `coupon` */

insert  into `coupon`(`id`,`name`,`money`,`category_id`,`multiple`,`start_time`,`end_time`,`state`,`crtime`) values (1,'50元优惠券',50,1,0,'2019-01-01 17:21:20','2019-01-30 17:21:27',0,'2019-01-08 17:21:30'),(2,'100元优惠券',100,2,0,'2019-01-01 17:21:20','2019-01-07 17:21:27',0,'2019-01-08 17:21:42');

/*Table structure for table `course` */

DROP TABLE IF EXISTS `course`;

CREATE TABLE `course` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `student_id` int(11) NOT NULL COMMENT '学生id',
  `product_id` int(11) NOT NULL COMMENT '课程商品id',
  `venue_id` int(11) NOT NULL COMMENT '场馆id',
  `order_id` varchar(64) NOT NULL COMMENT '订单id',
  `max_count` int(11) NOT NULL DEFAULT '0' COMMENT '最大教学人数',
  `hour` int(11) DEFAULT '0' COMMENT '总学时',
  `over_hour` int(11) DEFAULT '0' COMMENT '已完成学时',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `IX_STUDENTID_PRODUCTID` (`student_id`,`product_id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 COMMENT='购买成功的课程表';

/*Data for the table `course` */

insert  into `course`(`id`,`student_id`,`product_id`,`venue_id`,`order_id`,`max_count`,`hour`,`over_hour`,`crtime`) values (1,1,1,1,'ddd',0,0,0,'2019-01-11 17:44:49'),(2,2,1,1,'dd',0,0,0,'2019-01-11 17:48:06'),(3,3,1,2,'gg',0,0,0,'2019-01-11 17:48:23');

/*Table structure for table `course_type` */

DROP TABLE IF EXISTS `course_type`;

CREATE TABLE `course_type` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(20) NOT NULL COMMENT '名称',
  `max_people` int(11) NOT NULL DEFAULT '0' COMMENT '最大上课人数',
  `state` tinyint(4) DEFAULT '0' COMMENT '0可用 1禁用',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 COMMENT='游泳课类型';

/*Data for the table `course_type` */

insert  into `course_type`(`id`,`name`,`max_people`,`state`,`crtime`) values (1,'1v1',1,0,'2019-01-10 20:17:08'),(2,'1v2',2,0,'2019-01-10 20:17:19'),(3,'1v3',3,0,'2019-01-10 20:17:26');

/*Table structure for table `deliver` */

DROP TABLE IF EXISTS `deliver`;

CREATE TABLE `deliver` (
  `id` varchar(10) NOT NULL,
  `name` varchar(10) DEFAULT NULL,
  `state` tinyint(4) DEFAULT '0' COMMENT '0正常 1禁用',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='配送方式';

/*Data for the table `deliver` */

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

/*Data for the table `discount` */

insert  into `discount`(`id`,`name`,`summary`,`money`,`state`,`crtime`) values (1,'新人立减','新用户立即减去','50.00',0,'2019-01-08 10:46:25'),(2,'开业大酬宾','开业才有','100.00',0,'2019-01-08 10:46:41');

/*Table structure for table `hours` */

DROP TABLE IF EXISTS `hours`;

CREATE TABLE `hours` (
  `product_id` int(11) NOT NULL COMMENT '课程id',
  `hour` int(11) NOT NULL DEFAULT '0' COMMENT '课时',
  PRIMARY KEY (`product_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='课时对应的课程的基本信息';

/*Data for the table `hours` */

/*Table structure for table `order` */

DROP TABLE IF EXISTS `order`;

CREATE TABLE `order` (
  `order_id` varchar(64) NOT NULL COMMENT '订单号',
  `userid` int(11) DEFAULT NULL COMMENT '用户id',
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

/*Data for the table `order` */

/*Table structure for table `order_coupon` */

DROP TABLE IF EXISTS `order_coupon`;

CREATE TABLE `order_coupon` (
  `order_id` varchar(64) NOT NULL COMMENT '订单id',
  `coupon_id` int(11) NOT NULL COMMENT '优惠券id',
  `name` varchar(50) NOT NULL COMMENT '优惠券名称',
  `price` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '优惠券金额',
  `count` int(11) NOT NULL COMMENT '优惠券数量',
  `money` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '优惠总金额',
  PRIMARY KEY (`order_id`,`coupon_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='订单优惠券信息';

/*Data for the table `order_coupon` */

/*Table structure for table `order_discount` */

DROP TABLE IF EXISTS `order_discount`;

CREATE TABLE `order_discount` (
  `order_id` varchar(64) NOT NULL COMMENT '订单id',
  `product_id` int(11) NOT NULL COMMENT '商品id',
  `discount_id` int(11) NOT NULL COMMENT '折扣id',
  `name` varchar(50) NOT NULL COMMENT '折扣名称',
  `money` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '立减金额',
  PRIMARY KEY (`order_id`,`product_id`,`discount_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='订单折扣信息';

/*Data for the table `order_discount` */

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

/*Data for the table `order_product` */

/*Table structure for table `order_product_specification_detail` */

DROP TABLE IF EXISTS `order_product_specification_detail`;

CREATE TABLE `order_product_specification_detail` (
  `order_id` varchar(64) NOT NULL COMMENT '订单id',
  `product_id` int(11) NOT NULL DEFAULT '0' COMMENT '商品id',
  `plan_code` varchar(4) NOT NULL DEFAULT '0' COMMENT '规格组合id',
  `price` decimal(10,2) DEFAULT '0.00' COMMENT '价格',
  PRIMARY KEY (`order_id`,`product_id`,`plan_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='订单商品的规格与价格表';

/*Data for the table `order_product_specification_detail` */

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

/*Data for the table `product` */

insert  into `product`(`id`,`name`,`code`,`category_id`,`summary`,`detail_id`,`stock`,`min_buy_count`,`max_buy_count`,`main_img`,`state`,`crtime`) values (1,'自由游','ZYY',1,'哈哈，就是随便游啦',1,9999,1,9999,NULL,0,'2019-01-07 15:56:00'),(2,'仰泳','yy',1,'就是低头游啦',2,9999,1,9999,NULL,0,'2019-01-08 10:35:14');

/*Table structure for table `product_detail` */

DROP TABLE IF EXISTS `product_detail`;

CREATE TABLE `product_detail` (
  `product_id` int(11) NOT NULL,
  `detail` mediumtext CHARACTER SET utf8 COLLATE utf8_general_ci COMMENT '商品详情',
  PRIMARY KEY (`product_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='商品详细描述';

/*Data for the table `product_detail` */

insert  into `product_detail`(`product_id`,`detail`) values (1,'ffffffff'),(2,'dddddddddddd');

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

/*Data for the table `product_discount` */

insert  into `product_discount`(`id`,`product_id`,`discount_id`,`start_time`,`end_time`,`state`,`crttime`) values (1,1,1,'2019-01-01 10:46:55','2019-01-16 10:47:01',0,'2019-01-08 10:47:05');

/*Table structure for table `product_headimg` */

DROP TABLE IF EXISTS `product_headimg`;

CREATE TABLE `product_headimg` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `product_id` int(11) NOT NULL COMMENT '商品id',
  `headimg_url` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '头部图片地址',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COMMENT='商品的头部图片';

/*Data for the table `product_headimg` */

insert  into `product_headimg`(`id`,`product_id`,`headimg_url`,`crtime`) values (1,1,'http://h.hiphotos.baidu.com/image/h%3D300/sign=f52899ade6c4b7452b94b116fffd1e78/58ee3d6d55fbb2fbbc6b4796424a20a44723dcf6.jpg','2019-01-08 14:42:22');

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

/*Data for the table `product_specification_detail` */

insert  into `product_specification_detail`(`product_id`,`plan_code`,`specification_detail_id`,`state`,`crtime`) values (1,'p1',2,0,'2019-01-07 15:59:48'),(1,'p1',3,0,'2019-01-07 15:59:48'),(1,'p2',1,0,'2019-01-07 16:26:26'),(1,'p2',4,0,'2019-01-07 16:26:33');

/*Table structure for table `product_specification_detail_price` */

DROP TABLE IF EXISTS `product_specification_detail_price`;

CREATE TABLE `product_specification_detail_price` (
  `product_id` int(11) NOT NULL COMMENT '商品id',
  `plan_code` varchar(4) NOT NULL COMMENT '规格组合方案代码，用来区分不同种规格组合',
  `price` decimal(4,0) NOT NULL DEFAULT '0' COMMENT '该规格组合对应的价格',
  PRIMARY KEY (`product_id`,`plan_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='商品各规格组合与对应价格表';

/*Data for the table `product_specification_detail_price` */

insert  into `product_specification_detail_price`(`product_id`,`plan_code`,`price`) values (1,'p1','188'),(1,'p2','299');

/*Table structure for table `receiver_info` */

DROP TABLE IF EXISTS `receiver_info`;

CREATE TABLE `receiver_info` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL COMMENT '用户id',
  `name` varchar(20) NOT NULL COMMENT '收货人姓名',
  `phone` varchar(11) DEFAULT NULL COMMENT '收货电话',
  `address` varchar(200) NOT NULL COMMENT '收货地址',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='收货地址';

/*Data for the table `receiver_info` */

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

/*Data for the table `settlement` */

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

/*Data for the table `specification` */

insert  into `specification`(`id`,`name`,`summary`,`state`,`crtime`) values (1,'人数','游戏的人数',0,'2019-01-07 15:58:24'),(2,'大小','大小多少',0,'2019-01-07 16:24:43');

/*Table structure for table `specification_detail` */

DROP TABLE IF EXISTS `specification_detail`;

CREATE TABLE `specification_detail` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `specification_id` int(11) NOT NULL COMMENT '规格类型id',
  `name` varchar(50) NOT NULL COMMENT '名称',
  `summary` varchar(500) DEFAULT NULL COMMENT '描述',
  `value` int(11) DEFAULT '0' COMMENT '规格附加的值,目前存储最大上课人数',
  `state` tinyint(4) DEFAULT '0' COMMENT '0正常 1禁用',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 COMMENT='规格详情';

/*Data for the table `specification_detail` */

insert  into `specification_detail`(`id`,`specification_id`,`name`,`summary`,`value`,`state`,`crtime`) values (1,1,'1v1','就是一个对一个啦',1,0,'2019-01-07 15:58:44'),(2,1,'1v2','就是一个对两个啦',2,0,'2019-01-07 15:58:53'),(3,2,'大','大的',0,0,'2019-01-07 16:25:08'),(4,2,'小','小的',0,0,'2019-01-07 16:25:20');

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

/*Data for the table `student` */

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

/*Data for the table `student_coupon` */

insert  into `student_coupon`(`id`,`student_id`,`coupon_id`,`state`,`crtime`) values (1,1,1,0,'2019-01-08 17:22:05'),(2,1,1,1,'2019-01-08 17:22:09'),(3,1,2,0,'2019-01-08 17:22:10'),(4,1,2,1,'2019-01-08 17:22:13'),(5,1,2,0,'2019-01-08 17:22:14');

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

/*Data for the table `venue` */

insert  into `venue`(`id`,`name`,`code`,`campus_id`,`address`,`summary`,`state`,`crtime`) values (1,'世纪城洗浴中心','SJC',NULL,'世纪城南路999号','免费',0,'2019-01-08 11:24:15');

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

/*Data for the table `withdrawal` */

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
