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
  `type` tinyint(4) DEFAULT '0' COMMENT '类型0首页顶部图片1电话处图片2介绍处图片3介绍内容图片4商城顶部图片',
  `state` tinyint(4) DEFAULT '0' COMMENT '0正常 1禁用',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8 COMMENT='站内各处的图片';

/*Data for the table `banner` */

insert  into `banner`(`id`,`url`,`type`,`state`,`crtime`) values (1,'http://img5.duitang.com/uploads/item/201411/07/20141107164412_v284V.jpeg',0,0,'2019-01-14 14:36:04'),(2,'http://www.pptbz.com/pptpic/UploadFiles_6909/201211/2012111719294197.jpg',0,0,'2019-01-14 14:40:43'),(3,'http://www.pptbz.com/pptpic/UploadFiles_6909/201203/2012031220134655.jpg',1,0,'2019-01-14 14:41:00'),(4,'https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1547547403763&di=bfada9702b66aee4a608627298966b5e&imgtype=0&src=http%3A%2F%2Fpic5.photophoto.cn%2F20071006%2F0034034447148990_b.jpg',1,0,'2019-01-15 15:28:58'),(5,'https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1547547403762&di=0b6df39c11ab9c379bc4e0d4476ba08e&imgtype=0&src=http%3A%2F%2Fpic1.win4000.com%2Fwallpaper%2F6%2F59a3d9026f621.jpg',1,0,'2019-01-15 15:29:07'),(6,'https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1547547403761&di=64a2bc902a59a3574fe72c0fed1c6a40&imgtype=0&src=http%3A%2F%2Fpic1.win4000.com%2Fwallpaper%2F0%2F587c7e37e50ab.jpg',2,0,'2019-01-15 15:29:15'),(7,'https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1547547403760&di=d28935b1b05dceee698e1e741da5e4b7&imgtype=0&src=http%3A%2F%2Fimg17.3lian.com%2Fd%2Ffile%2F201702%2F16%2Fe177fd9962cb2dc0a4e2222338ad04fd.jpg',3,0,'2019-01-15 15:29:23'),(8,'https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1547547403760&di=b2aad45496ad9a613a36076c4f655b59&imgtype=0&src=http%3A%2F%2Fpic40.nipic.com%2F20140329%2F8952533_184915360000_2.jpg',3,0,'2019-01-15 15:29:31'),(9,'https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1547547403759&di=e4d5547b7904d86ad1e9ac65ee3cb069&imgtype=0&src=http%3A%2F%2Fpic1.win4000.com%2Fwallpaper%2F2017-12-08%2F5a2a02df312c7.jpg',3,0,'2019-01-15 15:29:42'),(10,'https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1547547403755&di=de1a5ed672343ca0e701d90081ec7555&imgtype=0&src=http%3A%2F%2Fpic1.win4000.com%2Fwallpaper%2Fa%2F5513a40021e54.jpg',3,0,'2019-01-15 15:29:43');

/*Table structure for table `booking_coach_queue` */

DROP TABLE IF EXISTS `booking_coach_queue`;

CREATE TABLE `booking_coach_queue` (
  `course_id` int(11) NOT NULL,
  `coach_id` int(11) NOT NULL,
  `end_time` datetime DEFAULT NULL,
  PRIMARY KEY (`course_id`,`coach_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='接单时教练限定';

/*Data for the table `booking_coach_queue` */

/*Table structure for table `booking_student` */

DROP TABLE IF EXISTS `booking_student`;

CREATE TABLE `booking_student` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `course_id` int(11) NOT NULL COMMENT 'course.id',
  `start_time` datetime DEFAULT NULL COMMENT '预订上课开始时间',
  `end_time` datetime DEFAULT NULL COMMENT '预订上课结束时间',
  `state` tinyint(4) DEFAULT '0' COMMENT '状态0正常 1无效',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COMMENT='用户下的预订信息';

/*Data for the table `booking_student` */

insert  into `booking_student`(`id`,`course_id`,`start_time`,`end_time`,`state`,`crtime`) values (1,1,'2019-01-19 18:59:38','2019-01-19 18:59:38',0,'2019-01-11 19:01:05');

/*Table structure for table `campus` */

DROP TABLE IF EXISTS `campus`;

CREATE TABLE `campus` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '区域名称',
  `state` tinyint(4) DEFAULT '0' COMMENT '0正常 1禁用',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 COMMENT='校区';

/*Data for the table `campus` */

insert  into `campus`(`id`,`name`,`state`,`crtime`) values (1,'成都校区',0,'2019-01-14 14:41:49'),(2,'广州校区',0,'2019-01-14 14:42:28'),(3,'北京校区',0,'2019-01-14 14:42:46');

/*Table structure for table `cashback_history` */

DROP TABLE IF EXISTS `cashback_history`;

CREATE TABLE `cashback_history` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `student_id` int(11) NOT NULL COMMENT '学员id',
  `from_student_id` int(11) NOT NULL COMMENT '触发返现的学员id',
  `money` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '返现金额',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '返现时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='学员邀请返现记录';

/*Data for the table `cashback_history` */

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
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COMMENT='商品分类';

/*Data for the table `category` */

insert  into `category`(`id`,`name`,`parent`,`type`,`state`,`crtime`) values (1,'游泳',0,0,0,'2019-01-07 15:56:42'),(2,'画画',0,1,0,'2019-01-14 14:34:58');

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
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 COMMENT='预约成功后的上课信息';

/*Data for the table `class` */

insert  into `class`(`id`,`product_id`,`coach_id`,`venue_id`,`hour`,`max_count`,`start_time`,`end_time`,`state`,`rate`,`crtime`) values (1,1,1,1,5,2,'2019-01-12 11:23:35','2019-01-12 11:23:39',0,0,'2019-01-08 11:23:43'),(2,1,1,1,4,1,'2019-01-13 16:28:35','2019-01-13 16:28:42',0,0,'2019-01-11 16:28:45'),(3,1,1,1,6,1,'2019-01-19 18:59:38','2019-01-19 19:31:23',0,0,'2019-01-11 19:30:59');

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
  `state` tinyint(4) DEFAULT '0' COMMENT '0未上课 1已上课 2已评价',
  `marking` float DEFAULT '-1' COMMENT '学员给本次上课打分',
  `comment` varchar(100) DEFAULT NULL COMMENT '评价内容',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`class_id`,`student_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='上课的学生';

/*Data for the table `class_student` */

insert  into `class_student`(`class_id`,`student_id`,`state`,`marking`,`comment`,`crtime`) values (1,1,0,-1,NULL,'2019-01-11 16:59:28'),(1,2,0,-1,NULL,'2019-01-11 17:11:27'),(1,3,0,-1,NULL,'2019-01-11 17:13:54'),(2,4,0,-1,NULL,'2019-01-11 17:38:53'),(3,5,0,-1,NULL,'2019-01-11 19:31:41');

/*Table structure for table `coach` */

DROP TABLE IF EXISTS `coach`;

CREATE TABLE `coach` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '自增',
  `username` varchar(20) NOT NULL COMMENT '用户名',
  `password` varchar(64) NOT NULL COMMENT '密码',
  `name` varchar(20) DEFAULT NULL COMMENT '姓名',
  `sex` tinyint(4) DEFAULT '0' COMMENT '性别0男 1女',
  `phone` varchar(11) DEFAULT NULL COMMENT '电话号码',
  `type` tinyint(4) DEFAULT '0' COMMENT '类型0游泳1画画2弹琴3...',
  `level` tinyint(4) DEFAULT '0' COMMENT '教练评级',
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

insert  into `coach`(`id`,`username`,`password`,`name`,`sex`,`phone`,`type`,`level`,`headimg`,`self_img`,`idcard_img`,`coaching_card_img`,`cash`,`state`,`crtime`) values (1,'xxx','fff','李咏',0,'125412541',0,0,'http://cms-bucket.nosdn.127.net/2355aab3ab86470593a7fe33239a09cb20170105080207.jpg','http://cms-bucket.nosdn.127.net/2355aab3ab86470593a7fe33239a09cb20170105080207.jpg','http://cms-bucket.nosdn.127.net/2355aab3ab86470593a7fe33239a09cb20170105080207.jpg','http://cms-bucket.nosdn.127.net/2355aab3ab86470593a7fe33239a09cb20170105080207.jpg',0,0,'2019-01-08 11:23:10'),(2,'ttt','ss','李白',1,'3456798765',0,0,'http://cms-bucket.nosdn.127.net/2355aab3ab86470593a7fe33239a09cb20170105080207.jpg','http://cms-bucket.nosdn.127.net/2355aab3ab86470593a7fe33239a09cb20170105080207.jpg','http://cms-bucket.nosdn.127.net/2355aab3ab86470593a7fe33239a09cb20170105080207.jpg','http://cms-bucket.nosdn.127.net/2355aab3ab86470593a7fe33239a09cb20170105080207.jpg',0,0,'2019-01-11 16:16:47'),(3,'eee','ggg','陆游',0,'33333333333',0,0,'http://cms-bucket.nosdn.127.net/2355aab3ab86470593a7fe33239a09cb20170105080207.jpg','http://cms-bucket.nosdn.127.net/2355aab3ab86470593a7fe33239a09cb20170105080207.jpg','http://cms-bucket.nosdn.127.net/2355aab3ab86470593a7fe33239a09cb20170105080207.jpg','http://cms-bucket.nosdn.127.net/2355aab3ab86470593a7fe33239a09cb20170105080207.jpg',0,0,'2019-01-11 16:17:45'),(4,'77','88','陈坤',0,'13266945548',0,0,'http://cms-bucket.nosdn.127.net/2355aab3ab86470593a7fe33239a09cb20170105080207.jpg','http://cms-bucket.nosdn.127.net/2355aab3ab86470593a7fe33239a09cb20170105080207.jpg','http://cms-bucket.nosdn.127.net/2355aab3ab86470593a7fe33239a09cb20170105080207.jpg','http://cms-bucket.nosdn.127.net/2355aab3ab86470593a7fe33239a09cb20170105080207.jpg',0,0,'2019-01-11 16:30:57');

/*Table structure for table `coach_caption` */

DROP TABLE IF EXISTS `coach_caption`;

CREATE TABLE `coach_caption` (
  `coach_id` int(11) NOT NULL COMMENT '教练id',
  `caption_id` int(11) NOT NULL COMMENT '教练队长id',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`coach_id`,`caption_id`)
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

insert  into `coachcaption_venue`(`coach_id`,`venue_id`,`crtime`) values (2,1,'2019-01-11 16:17:05'),(4,2,'2019-01-14 14:44:04');

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
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 COMMENT='优惠券基本信息';

/*Data for the table `coupon` */

insert  into `coupon`(`id`,`name`,`money`,`category_id`,`multiple`,`start_time`,`end_time`,`state`,`crtime`) values (1,'50元优惠券',50,1,0,'2019-01-01 17:21:20','2019-01-30 17:21:27',0,'2019-01-08 17:21:30'),(2,'100元优惠券',100,2,0,'2019-01-01 17:21:20','2019-01-07 17:21:27',0,'2019-01-08 17:21:42'),(3,'500元优惠券',500,1,0,'2019-01-15 14:45:51','2019-01-18 14:45:56',0,'2019-01-14 14:45:47');

/*Table structure for table `coupon_history` */

DROP TABLE IF EXISTS `coupon_history`;

CREATE TABLE `coupon_history` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `student_id` int(11) NOT NULL COMMENT '学员id',
  `coupon_id` int(11) NOT NULL COMMENT '优惠券id',
  `count` int(11) NOT NULL DEFAULT '1' COMMENT '数量',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '获得时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='优惠券获得历史记录';

/*Data for the table `coupon_history` */

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
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `state` tinyint(4) DEFAULT '0' COMMENT '0正常 1禁用',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8 COMMENT='配送方式';

/*Data for the table `deliver` */

insert  into `deliver`(`id`,`name`,`state`,`crtime`) values (1,'免配送',0,'2019-01-14 16:34:56'),(2,'中通快递',0,'2019-01-14 16:34:56'),(3,'圆通快递',0,'2019-01-14 16:34:56'),(4,'百世快递',0,'2019-01-14 16:34:56'),(5,'申通快递',0,'2019-01-14 16:34:56'),(6,'顺风快递',0,'2019-01-14 16:34:56'),(7,'邮政EMS',0,'2019-01-14 16:34:56');

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

/*Table structure for table `hot_course` */

DROP TABLE IF EXISTS `hot_course`;

CREATE TABLE `hot_course` (
  `product_id` int(11) NOT NULL COMMENT '商品id',
  `type` tinyint(4) NOT NULL DEFAULT '0' COMMENT '类型0热门课程1精品课程',
  `sort_order` int(11) DEFAULT '0' COMMENT '排列序号',
  PRIMARY KEY (`product_id`,`type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='热门课程';

/*Data for the table `hot_course` */

insert  into `hot_course`(`product_id`,`type`,`sort_order`) values (1,0,1),(1,1,2),(2,0,2),(2,1,1),(4,0,3);

/*Table structure for table `hours` */

DROP TABLE IF EXISTS `hours`;

CREATE TABLE `hours` (
  `product_id` int(11) NOT NULL COMMENT '课程id',
  `hour` int(11) NOT NULL DEFAULT '0' COMMENT '课时',
  PRIMARY KEY (`product_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='课时对应的课程的基本信息';

/*Data for the table `hours` */

insert  into `hours`(`product_id`,`hour`) values (1,10),(2,20),(3,15);

/*Table structure for table `invitation` */

DROP TABLE IF EXISTS `invitation`;

CREATE TABLE `invitation` (
  `student_id` int(11) NOT NULL COMMENT '用户id',
  `from_student_id` int(11) NOT NULL COMMENT '邀请人id',
  `state` tinyint(4) NOT NULL DEFAULT '0' COMMENT '0未发放奖励 1已发放奖励',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`student_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='邀请关系';

/*Data for the table `invitation` */

insert  into `invitation`(`student_id`,`from_student_id`,`state`,`crtime`) values (2,1,0,'2019-01-16 15:53:10'),(3,2,0,'2019-01-16 15:53:07'),(4,3,0,'2019-01-16 15:53:01');

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
  `state` tinyint(4) DEFAULT '0' COMMENT '状态0未支付1已支付2已发货3已收货4已评价',
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
  `count` int(11) NOT NULL COMMENT '优惠券数量',
  `money` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '优惠券金额',
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
  `plan_code` varchar(4) DEFAULT NULL COMMENT '规格',
  `count` int(11) DEFAULT '0' COMMENT '数量',
  `price` decimal(10,2) DEFAULT '0.00' COMMENT '折扣后价格',
  `orig_price` decimal(10,2) DEFAULT '0.00' COMMENT '原价',
  `total_amount` decimal(10,2) DEFAULT '0.00' COMMENT '去除折扣后的总金额',
  `discount_amount` decimal(10,2) DEFAULT '0.00' COMMENT '折扣金额',
  `venueid` int(11) DEFAULT NULL COMMENT '课程场馆id',
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

/*Table structure for table `pay_record` */

DROP TABLE IF EXISTS `pay_record`;

CREATE TABLE `pay_record` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL COMMENT '用户id',
  `user_type` tinyint(4) DEFAULT '0' COMMENT '0学员1教练',
  `order_id` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '购物订单号',
  `money` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '金额(正值为收入，负值为支出)',
  `type` tinyint(4) DEFAULT '0' COMMENT '类型0购买商品1返现2提现3上课结算收入4充值',
  `comment` varchar(100) NOT NULL COMMENT '备注',
  `balance` decimal(10,0) DEFAULT '0' COMMENT '余额',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='用户资金变动记录';

/*Data for the table `pay_record` */

/*Table structure for table `product` */

DROP TABLE IF EXISTS `product`;

CREATE TABLE `product` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '自增主键',
  `name` varchar(100) NOT NULL COMMENT '商品名称',
  `code` varchar(20) NOT NULL COMMENT '商品编码',
  `category_id` int(11) NOT NULL DEFAULT '0' COMMENT '类别',
  `summary` varchar(200) DEFAULT NULL COMMENT '简介',
  `detail_id` int(11) DEFAULT '0' COMMENT '详细描述',
  `sales` int(11) DEFAULT '0' COMMENT '销量',
  `stock` int(11) DEFAULT '9999' COMMENT '库存数量',
  `min_buy_count` int(4) DEFAULT '1' COMMENT '单次最少购买量',
  `max_buy_count` int(4) DEFAULT '9999' COMMENT '单次最大购买量',
  `main_img` varchar(100) DEFAULT NULL COMMENT '主图',
  `state` tinyint(4) DEFAULT '0' COMMENT '状态0正常 1禁用',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8 COMMENT='商品信息';

/*Data for the table `product` */

insert  into `product`(`id`,`name`,`code`,`category_id`,`summary`,`detail_id`,`sales`,`stock`,`min_buy_count`,`max_buy_count`,`main_img`,`state`,`crtime`) values (1,'自由泳','ZYY',1,'哈哈，就是随便游啦',1,0,9999,1,9999,'http://img.zcool.cn/community/01b82e56655b2532f8754573e34730.jpg@1280w_1l_2o_100sh.jpg',0,'2019-01-07 15:56:00'),(2,'仰泳','yy',1,'就是低头游啦',2,0,9999,1,9999,'http://p.ananas.chaoxing.com/star3/origin/2069697acd73f47b875d392d7c7be876.jpg',0,'2019-01-08 10:35:14'),(3,'素描基础讲解','SMJC',2,'素描基础知识',3,0,9999,1,9999,'http://pic17.nipic.com/20111127/8831577_105726414163_2.jpg',0,'2019-01-14 14:50:35'),(4,'蛙泳','WY',1,'蛙泳基础知识',0,0,9999,1,9999,'http://pic17.nipic.com/20111127/8831577_105726414163_2.jpg',0,'2019-01-14 15:13:58'),(5,'蝶泳','DY',1,'蝶泳基础知识',0,0,9999,1,9999,'http://pic17.nipic.com/20111127/8831577_105726414163_2.jpg',0,'2019-01-14 15:14:09'),(6,'潜水','QS',1,'潜水基础知识',0,0,9999,1,9999,'http://pic17.nipic.com/20111127/8831577_105726414163_2.jpg',0,'2019-01-14 15:14:58'),(7,'狗刨','GP',1,'狗刨基础知识',0,0,9999,1,9999,'http://pic17.nipic.com/20111127/8831577_105726414163_2.jpg',0,'2019-01-14 15:15:34');

/*Table structure for table `product_detail` */

DROP TABLE IF EXISTS `product_detail`;

CREATE TABLE `product_detail` (
  `product_id` int(11) NOT NULL,
  `detail` mediumtext CHARACTER SET utf8 COLLATE utf8_general_ci COMMENT '商品详情',
  PRIMARY KEY (`product_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='商品详细描述';

/*Data for the table `product_detail` */

insert  into `product_detail`(`product_id`,`detail`) values (1,'http://pic15.nipic.com/20110715/7935001_120846634125_2.jpg'),(2,'http://pic21.nipic.com/20120519/5454342_154115399000_2.jpg'),(3,'http://pic22.nipic.com/20120625/8534374_064230576000_2.jpg');

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
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COMMENT='商品折扣信息';

/*Data for the table `product_discount` */

insert  into `product_discount`(`id`,`product_id`,`discount_id`,`start_time`,`end_time`,`state`,`crttime`) values (1,1,1,'2019-01-01 10:46:55','2019-01-16 10:47:01',0,'2019-01-08 10:47:05'),(2,2,2,'2019-01-08 14:52:30','2019-01-23 14:52:34',0,'2019-01-14 14:52:29');

/*Table structure for table `product_headimg` */

DROP TABLE IF EXISTS `product_headimg`;

CREATE TABLE `product_headimg` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `product_id` int(11) NOT NULL COMMENT '商品id',
  `headimg_url` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '头部图片地址',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 COMMENT='商品的头部图片';

/*Data for the table `product_headimg` */

insert  into `product_headimg`(`id`,`product_id`,`headimg_url`,`crtime`) values (1,1,'http://h.hiphotos.baidu.com/image/h%3D300/sign=f52899ade6c4b7452b94b116fffd1e78/58ee3d6d55fbb2fbbc6b4796424a20a44723dcf6.jpg','2019-01-08 14:42:22'),(2,2,'http://pic26.nipic.com/20121221/2531170_194321033000_2.jpg','2019-01-14 14:52:53'),(3,3,'http://pic1.nipic.com/2008-11-20/20081120145730349_2.jpg','2019-01-14 14:53:07');

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

insert  into `product_specification_detail`(`product_id`,`plan_code`,`specification_detail_id`,`state`,`crtime`) values (1,'p1',2,0,'2019-01-07 15:59:48'),(1,'p1',3,0,'2019-01-07 15:59:48'),(1,'p2',1,0,'2019-01-07 16:26:26'),(1,'p2',4,0,'2019-01-07 16:26:33'),(2,'p3',1,0,'2019-01-14 15:07:38'),(2,'p3',2,0,'2019-01-14 15:07:46'),(3,'p4',1,0,'2019-01-14 15:08:12');

/*Table structure for table `product_specification_detail_price` */

DROP TABLE IF EXISTS `product_specification_detail_price`;

CREATE TABLE `product_specification_detail_price` (
  `product_id` int(11) NOT NULL COMMENT '商品id',
  `plan_code` varchar(4) NOT NULL COMMENT '规格组合方案代码，用来区分不同种规格组合',
  `price` decimal(4,0) NOT NULL DEFAULT '0' COMMENT '该规格组合对应的价格',
  PRIMARY KEY (`product_id`,`plan_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='商品各规格组合与对应价格表';

/*Data for the table `product_specification_detail_price` */

insert  into `product_specification_detail_price`(`product_id`,`plan_code`,`price`) values (1,'p1','188'),(1,'p2','299'),(2,'p3','400'),(3,'p4','255');

/*Table structure for table `receiver_info` */

DROP TABLE IF EXISTS `receiver_info`;

CREATE TABLE `receiver_info` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `student_id` int(11) NOT NULL COMMENT '用户id',
  `name` varchar(20) NOT NULL COMMENT '收货人姓名',
  `phone` varchar(11) DEFAULT NULL COMMENT '收货电话',
  `address` varchar(200) NOT NULL COMMENT '收货地址',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COMMENT='收货地址';

/*Data for the table `receiver_info` */

insert  into `receiver_info`(`id`,`student_id`,`name`,`phone`,`address`,`crtime`) values (1,1,'小K','16599845762','北京市海淀区清华大学蒙民伟科技大楼南楼801、803、805室','2019-01-14 14:54:17'),(2,1,'钟慧','18874586215','北京市海淀区清华大学蒙民伟科技大楼南楼801、803、805室','2019-01-14 14:55:15');

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

/*Table structure for table `site_info` */

DROP TABLE IF EXISTS `site_info`;

CREATE TABLE `site_info` (
  `key` varchar(50) NOT NULL COMMENT '关键字',
  `value` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '值',
  `uptime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`key`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='站点基本信息';

/*Data for the table `site_info` */

insert  into `site_info`(`key`,`value`,`uptime`) values ('客服电话','4008221542','2019-01-19 11:17:39'),('用户注册返现金额','20','2019-01-19 11:17:39'),('站点名称','sunny baby','2019-01-19 11:17:39'),('邀请返现金额','30','2019-01-19 11:17:39');

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

insert  into `specification`(`id`,`name`,`summary`,`state`,`crtime`) values (1,'教学人数','每堂课的上课最大人数',0,'2019-01-07 15:58:24'),(2,'大小','大小',0,'2019-01-07 16:24:43');

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
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8 COMMENT='规格详情';

/*Data for the table `specification_detail` */

insert  into `specification_detail`(`id`,`specification_id`,`name`,`summary`,`value`,`state`,`crtime`) values (1,1,'1v1','就是一个对一个啦',0,0,'2019-01-07 15:58:44'),(2,1,'1v2','就是一个对两个啦',0,0,'2019-01-07 15:58:53'),(3,2,'大','大号',0,0,'2019-01-07 16:25:08'),(4,2,'小','中号',0,0,'2019-01-07 16:25:20'),(5,3,'小','小号',0,0,'2019-01-14 14:56:52');

/*Table structure for table `student` */

DROP TABLE IF EXISTS `student`;

CREATE TABLE `student` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '自增',
  `username` varchar(20) NOT NULL COMMENT '用户名',
  `password` varchar(64) NOT NULL COMMENT '密码',
  `name` varchar(20) DEFAULT NULL COMMENT '姓名',
  `sex` tinyint(4) DEFAULT '0' COMMENT '性别0男 1女',
  `phone` varchar(11) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '电话号码',
  `birthday` date DEFAULT NULL COMMENT '生日',
  `headimg` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '头像图片地址',
  `cash` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '账户余额',
  `state` tinyint(4) NOT NULL DEFAULT '0' COMMENT '0正常 1受限',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`),
  UNIQUE KEY `IX_USERNAME` (`username`),
  UNIQUE KEY `IX_PHONE` (`phone`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8 COMMENT='注册用户信息表';

/*Data for the table `student` */

insert  into `student`(`id`,`username`,`password`,`name`,`sex`,`phone`,`birthday`,`headimg`,`cash`,`state`,`crtime`) values (1,'xyz','e10adc3949ba59abbe56e057f20f883e','陈乔朋',0,'15271459965','2019-06-27','http://img3.redocn.com/tupian/20150318/shengdanquanbiankuangsucai_4020876.jpg','112.00',0,'2019-01-14 14:58:12'),(2,'space','54ea1cafa9e86fe45877ef16e38a8774','庄仕华',0,'17033215468','2019-01-01','http://img1.imgtn.bdimg.com/it/u=93745383,492158812&fm=26&gp=0.jpg','0.00',0,'2019-01-14 15:00:26');

/*Table structure for table `student_coupon` */

DROP TABLE IF EXISTS `student_coupon`;

CREATE TABLE `student_coupon` (
  `student_id` int(11) NOT NULL COMMENT '学员id',
  `coupon_id` int(11) NOT NULL COMMENT '优惠券id',
  `count` int(11) DEFAULT '1' COMMENT '数量',
  `state` tinyint(4) DEFAULT '0' COMMENT '状态0正常 1禁用',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`student_id`,`coupon_id`),
  UNIQUE KEY `IX_STUDENTID_COUPONID` (`student_id`,`coupon_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='学员的优惠券信息';

/*Data for the table `student_coupon` */

insert  into `student_coupon`(`student_id`,`coupon_id`,`count`,`state`,`crtime`) values (1,1,1,0,'2019-01-08 17:22:05');

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
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COMMENT='场馆信息';

/*Data for the table `venue` */

insert  into `venue`(`id`,`name`,`code`,`campus_id`,`address`,`summary`,`state`,`crtime`) values (1,'世纪城洗浴中心','SJC',1,'世纪城南路999号','免费',0,'2019-01-08 11:24:15'),(2,'清华园大澡堂','QYH',2,'清华大学第三食堂旁','1块钱个小时',0,'2019-01-14 14:44:34');

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
