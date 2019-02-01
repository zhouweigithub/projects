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

insert  into `banner`(`id`,`url`,`type`,`state`,`crtime`) values (1,'https://img10.360buyimg.com/n1/s450x450_jfs/t18448/200/2532654839/268503/b46a717e/5afe4d0cN10f96d55.jpg',0,0,'2019-01-14 14:36:04'),(2,'http://www.pptbz.com/pptpic/UploadFiles_6909/201211/2012111719294197.jpg',0,0,'2019-01-14 14:40:43'),(3,'http://www.pptbz.com/pptpic/UploadFiles_6909/201203/2012031220134655.jpg',1,0,'2019-01-14 14:41:00'),(4,'https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1547547403763&di=bfada9702b66aee4a608627298966b5e&imgtype=0&src=http%3A%2F%2Fpic5.photophoto.cn%2F20071006%2F0034034447148990_b.jpg',1,0,'2019-01-15 15:28:58'),(5,'https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1547547403762&di=0b6df39c11ab9c379bc4e0d4476ba08e&imgtype=0&src=http%3A%2F%2Fpic1.win4000.com%2Fwallpaper%2F6%2F59a3d9026f621.jpg',1,0,'2019-01-15 15:29:07'),(6,'https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1547547403761&di=64a2bc902a59a3574fe72c0fed1c6a40&imgtype=0&src=http%3A%2F%2Fpic1.win4000.com%2Fwallpaper%2F0%2F587c7e37e50ab.jpg',2,0,'2019-01-15 15:29:15'),(7,'https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1547547403760&di=d28935b1b05dceee698e1e741da5e4b7&imgtype=0&src=http%3A%2F%2Fimg17.3lian.com%2Fd%2Ffile%2F201702%2F16%2Fe177fd9962cb2dc0a4e2222338ad04fd.jpg',3,0,'2019-01-15 15:29:23'),(8,'https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1547547403760&di=b2aad45496ad9a613a36076c4f655b59&imgtype=0&src=http%3A%2F%2Fpic40.nipic.com%2F20140329%2F8952533_184915360000_2.jpg',3,0,'2019-01-15 15:29:31'),(9,'https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1547547403759&di=e4d5547b7904d86ad1e9ac65ee3cb069&imgtype=0&src=http%3A%2F%2Fpic1.win4000.com%2Fwallpaper%2F2017-12-08%2F5a2a02df312c7.jpg',3,0,'2019-01-15 15:29:42'),(10,'https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1547547403755&di=de1a5ed672343ca0e701d90081ec7555&imgtype=0&src=http%3A%2F%2Fpic1.win4000.com%2Fwallpaper%2Fa%2F5513a40021e54.jpg',3,0,'2019-01-15 15:29:43');

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
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8 COMMENT='用户下的预订信息';

/*Data for the table `booking_student` */

insert  into `booking_student`(`id`,`course_id`,`start_time`,`end_time`,`state`,`crtime`) values (1,1,'2019-01-28 18:59:38','2019-01-29 18:59:38',0,'2019-01-11 19:01:05'),(2,0,'0001-01-01 00:00:00','0001-01-01 00:00:00',0,'2019-01-29 20:17:19'),(3,3,'2019-02-02 16:00:00','2019-02-02 17:00:00',0,'2019-01-29 20:24:03'),(4,3,'2019-02-02 16:00:00','2019-02-02 17:00:00',0,'2019-01-29 20:25:06'),(5,3,'2019-02-02 15:00:00','2019-02-02 16:00:00',0,'2019-01-29 20:43:41'),(6,2,'2019-02-02 14:00:00','2019-02-02 15:00:00',0,'2019-01-29 21:32:36'),(7,5,'2019-02-01 14:00:00','2019-02-01 15:00:00',1,'2019-01-30 10:53:26');

/*Table structure for table `campus` */

DROP TABLE IF EXISTS `campus`;

CREATE TABLE `campus` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(20) NOT NULL COMMENT '区域名称',
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
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 COMMENT='学员邀请返现记录';

/*Data for the table `cashback_history` */

insert  into `cashback_history`(`id`,`student_id`,`from_student_id`,`money`,`crtime`) values (1,12,1,'10.00','2019-01-25 21:10:23'),(2,12,2,'5.00','2019-01-25 21:10:35'),(3,13,1,'8.00','2019-01-30 13:57:16');

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
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8 COMMENT='商品分类';

/*Data for the table `category` */

insert  into `category`(`id`,`name`,`parent`,`type`,`state`,`crtime`) values (1,'游泳',0,0,0,'2019-01-07 15:56:42'),(2,'画画',0,0,0,'2019-01-14 14:34:58'),(3,'弹琴',0,0,0,'2019-01-25 20:05:26'),(4,'炒菜',0,0,0,'2019-01-25 20:05:36'),(5,'泳衣',0,1,0,'2019-01-25 20:05:45'),(6,'护具',0,1,0,'2019-01-25 20:06:04');

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
  PRIMARY KEY (`id`),
  UNIQUE KEY `IX_UNIQUE` (`coach_id`,`start_time`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 COMMENT='预约成功后的上课信息';

/*Data for the table `class` */

insert  into `class`(`id`,`product_id`,`coach_id`,`venue_id`,`hour`,`max_count`,`start_time`,`end_time`,`state`,`rate`,`crtime`) values (1,1,1,1,5,2,'2019-02-12 11:00:00','2019-01-12 11:23:39',0,0,'2019-01-08 11:23:43'),(2,1,1,1,4,1,'2019-01-13 16:28:35','2019-01-13 16:28:42',0,0,'2019-01-11 16:28:45'),(3,1,1,1,6,1,'2019-01-19 18:59:38','2019-01-19 19:31:23',0,0,'2019-01-11 19:30:59'),(4,2,1,2,2,1,'2019-01-31 11:04:13','2019-01-31 11:04:16',0,0,'2019-01-30 11:04:20');

/*Table structure for table `class_comment` */

DROP TABLE IF EXISTS `class_comment`;

CREATE TABLE `class_comment` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `class_id` int(11) NOT NULL COMMENT '上课id',
  `comment` varchar(500) NOT NULL COMMENT '教练评论的文字内容',
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
  `url` varchar(500) NOT NULL COMMENT '教练上传的图片或视频地址',
  `type` tinyint(4) DEFAULT '0' COMMENT '类型0图片 1视频',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 COMMENT='课程结束后教练上传的图片或视频信息';

/*Data for the table `class_comment_url` */

insert  into `class_comment_url`(`id`,`class_id`,`url`,`type`,`crtime`) values (1,1,'http://pic-bucket.ws.126.net/photo/0005/2019-01-07/E4ULVC640B4C0005NOS.jpg',0,'2019-01-08 11:26:00'),(2,1,'http://h.hiphotos.baidu.com/image/h%3D300/sign=f2db86688ccb39dbdec06156e01709a7/2f738bd4b31c87018e9450642a7f9e2f0708ff16.jpg',0,'2019-01-08 14:20:15'),(3,1,'https://sunny.ullfly.com/video/a_yHP6ky9a9UTf1548249633_10s.mp4',1,'2019-01-27 10:15:15'),(4,1,'https://sunny.ullfly.com/video/8ab9480b7e864bb088bc9bf23b4bc61d_34.mp4',1,'2019-01-27 10:15:50');

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

insert  into `class_student`(`class_id`,`student_id`,`state`,`marking`,`comment`,`crtime`) values (1,2,0,2,'fff','2019-01-11 17:11:27'),(1,3,0,-1,NULL,'2019-01-11 17:13:54'),(1,12,0,1,'eeee','2019-01-11 16:59:28'),(2,12,0,4,'rrr','2019-01-11 17:38:53'),(3,12,0,-1,NULL,'2019-01-11 19:31:41'),(4,13,0,-1,NULL,'2019-01-30 11:05:02');

/*Table structure for table `coach` */

DROP TABLE IF EXISTS `coach`;

CREATE TABLE `coach` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '自增',
  `username` varchar(64) NOT NULL COMMENT '用户名',
  `password` varchar(64) DEFAULT NULL COMMENT '密码',
  `name` varchar(20) DEFAULT NULL COMMENT '姓名',
  `sex` tinyint(4) DEFAULT '0' COMMENT '性别0男 1女',
  `phone` varchar(11) NOT NULL COMMENT '电话号码',
  `type` tinyint(4) DEFAULT '0' COMMENT '类型0游泳1画画2弹琴3...',
  `level` tinyint(4) DEFAULT '0' COMMENT '教练评级',
  `headimg` varchar(500) DEFAULT NULL COMMENT '头像图片地址',
  `cash` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '账户余额',
  `state` tinyint(4) NOT NULL DEFAULT '0' COMMENT '0正常 1受限',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`),
  UNIQUE KEY `IX_USERNAME` (`username`),
  UNIQUE KEY `IX_PHONE` (`phone`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8 COMMENT='注册教练信息表';

/*Data for the table `coach` */

insert  into `coach`(`id`,`username`,`password`,`name`,`sex`,`phone`,`type`,`level`,`headimg`,`cash`,`state`,`crtime`) values (1,'xxx','fff','李咏',0,'13425645987',0,0,'http://cms-bucket.nosdn.127.net/2355aab3ab86470593a7fe33239a09cb20170105080207.jpg','0.00',0,'2019-01-08 11:23:10'),(2,'ttt','ss','李白',1,'3456798765',0,0,'http://cms-bucket.nosdn.127.net/2355aab3ab86470593a7fe33239a09cb20170105080207.jpg','0.00',0,'2019-01-11 16:16:47'),(3,'eee','ggg','陆游',0,'33333333333',0,0,'http://cms-bucket.nosdn.127.net/2355aab3ab86470593a7fe33239a09cb20170105080207.jpg','0.00',0,'2019-01-11 16:17:45'),(4,'77','88','陈坤',0,'13266945548',0,0,'http://cms-bucket.nosdn.127.net/2355aab3ab86470593a7fe33239a09cb20170105080207.jpg','0.00',0,'2019-01-11 16:30:57'),(5,'oqol35CUc3zO2cPEmXmDnC5YNL1k_0',NULL,NULL,0,'13800000000',0,0,NULL,'0.00',0,'2019-02-01 10:31:06'),(6,'ddd','',NULL,0,'13548798487',0,0,NULL,'0.00',0,'2019-02-01 22:00:11');

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

/*Table structure for table `coach_img` */

DROP TABLE IF EXISTS `coach_img`;

CREATE TABLE `coach_img` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `coach_id` int(11) NOT NULL COMMENT '教练id',
  `url` varchar(500) NOT NULL COMMENT '图片地址',
  `comment` varchar(50) DEFAULT NULL COMMENT '备注',
  `type` tinyint(4) NOT NULL DEFAULT '0' COMMENT '类型0身份证照1自身照2教练证照',
  `state` tinyint(4) NOT NULL DEFAULT '0' COMMENT '状态0正常1禁用',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 COMMENT='教练上传的自身相关证件照片';

/*Data for the table `coach_img` */

insert  into `coach_img`(`id`,`coach_id`,`url`,`comment`,`type`,`state`,`crtime`) values (1,1,'http://pic1.nipic.com/2008-11-20/20081120145730349_2.jpg','rrr',0,0,'2019-01-25 21:09:33'),(2,1,'http://pic1.nipic.com/2008-11-20/20081120145730349_2.jpg','rrr',1,0,'2019-01-25 21:09:53'),(3,1,'http://pic1.nipic.com/2008-11-20/20081120145730349_2.jpg','rrr',2,0,'2019-01-25 21:09:58');

/*Table structure for table `coachcaption_venue` */

DROP TABLE IF EXISTS `coachcaption_venue`;

CREATE TABLE `coachcaption_venue` (
  `coach_id` int(11) NOT NULL COMMENT '教练队长id',
  `venue_id` int(11) NOT NULL COMMENT '场馆id',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`coach_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='教练队长与场馆的关系';

/*Data for the table `coachcaption_venue` */

insert  into `coachcaption_venue`(`coach_id`,`venue_id`,`crtime`) values (1,1,'2019-01-31 23:12:41'),(2,2,'2019-01-11 16:17:05'),(4,2,'2019-01-14 14:44:04');

/*Table structure for table `coupon` */

DROP TABLE IF EXISTS `coupon`;

CREATE TABLE `coupon` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(64) NOT NULL COMMENT '优惠券名称',
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

insert  into `coupon`(`id`,`name`,`money`,`category_id`,`multiple`,`start_time`,`end_time`,`state`,`crtime`) values (1,'50元优惠券',50,1,0,'2019-01-01 17:21:20','2019-02-07 17:21:27',0,'2019-01-08 17:21:30'),(2,'100元优惠券',100,2,0,'2019-01-01 17:21:20','2019-02-09 17:21:27',0,'2019-01-08 17:21:42'),(3,'500元优惠券',500,1,0,'2019-01-15 14:45:51','2019-01-18 14:45:56',0,'2019-01-14 14:45:47');

/*Table structure for table `coupon_history` */

DROP TABLE IF EXISTS `coupon_history`;

CREATE TABLE `coupon_history` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `student_id` int(11) NOT NULL COMMENT '学员id',
  `coupon_id` int(11) NOT NULL COMMENT '优惠券id',
  `count` int(11) NOT NULL DEFAULT '1' COMMENT '数量',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '获得时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8 COMMENT='优惠券获得历史记录';

/*Data for the table `coupon_history` */

insert  into `coupon_history`(`id`,`student_id`,`coupon_id`,`count`,`crtime`) values (1,12,1,1,'2019-01-25 21:07:58'),(2,12,2,2,'2019-01-25 21:08:04'),(3,12,1,1,'2019-01-25 21:08:09'),(4,12,2,1,'2019-01-25 21:08:12'),(5,1,1,1,'2019-01-25 21:08:15');

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
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COMMENT='购买成功的课程表';

/*Data for the table `course` */

insert  into `course`(`id`,`student_id`,`product_id`,`venue_id`,`order_id`,`max_count`,`hour`,`over_hour`,`crtime`) values (1,12,1,2,'1000012_2019013122284880142203',1,10,0,'2019-01-31 22:28:48'),(2,12,1,1,'1000012_2019013122385635224832',1,10,0,'2019-01-31 22:38:56');

/*Table structure for table `course_price` */

DROP TABLE IF EXISTS `course_price`;

CREATE TABLE `course_price` (
  `product_id` int(11) NOT NULL COMMENT '课程商品id',
  `venue_id` int(11) NOT NULL COMMENT '场馆id',
  `type_id` int(11) NOT NULL COMMENT '上课人数类型course_type.id',
  `price` decimal(10,0) NOT NULL DEFAULT '0' COMMENT '价格',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`product_id`,`venue_id`,`type_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='课程商品的价格';

/*Data for the table `course_price` */

insert  into `course_price`(`product_id`,`venue_id`,`type_id`,`price`,`crtime`) values (1,1,1,'600','2019-01-25 20:44:46'),(1,2,1,'200','2019-01-26 22:14:18');

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
  `name` varchar(10) NOT NULL,
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

insert  into `hot_course`(`product_id`,`type`,`sort_order`) values (1,0,1),(1,1,2),(2,0,2),(2,1,1),(4,0,3),(5,0,0),(6,1,0);

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

insert  into `invitation`(`student_id`,`from_student_id`,`state`,`crtime`) values (12,1,0,'2019-01-26 14:32:03'),(13,12,0,'2019-01-30 10:49:05');

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
  `state` tinyint(4) DEFAULT '0' COMMENT '状态0未支付1已支付2已发货3已收货4已评价5已取消',
  `crdate` date DEFAULT NULL COMMENT '日期',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '时间',
  PRIMARY KEY (`order_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='购物订单信息';

/*Data for the table `order` */

insert  into `order`(`order_id`,`userid`,`money`,`discount_money`,`coupon_money`,`receiver`,`phone`,`address`,`message`,`deliver_id`,`freight`,`state`,`crdate`,`crtime`) values ('1000012_2019013122255290976702',12,'600.00','0.00','0.00',NULL,NULL,NULL,'请尽快发货','0','0.00',0,'2019-01-31','2019-01-31 22:25:52'),('1000012_2019013122284880142203',12,'200.00','0.00','0.00',NULL,NULL,NULL,'请尽快发货','0','0.00',0,'2019-01-31','2019-01-31 22:28:48'),('1000012_2019013122385635224832',12,'600.00','0.00','0.00',NULL,NULL,NULL,'请尽快发货','0','0.00',0,'2019-01-31','2019-01-31 22:38:56');

/*Table structure for table `order_callback_pay` */

DROP TABLE IF EXISTS `order_callback_pay`;

CREATE TABLE `order_callback_pay` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `appid` varchar(32) DEFAULT NULL COMMENT '小程序ID',
  `mch_id` varchar(32) DEFAULT NULL COMMENT '商户号',
  `device_info` varchar(32) DEFAULT NULL COMMENT '设备号',
  `nonce_str` varchar(32) DEFAULT NULL COMMENT '随机字符串',
  `sign` varchar(32) DEFAULT NULL COMMENT '签名',
  `sign_type` varchar(32) DEFAULT NULL COMMENT '签名类型',
  `result_code` varchar(16) DEFAULT NULL COMMENT '业务结果',
  `err_code` varchar(32) DEFAULT NULL COMMENT '错误代码',
  `err_code_des` varchar(128) DEFAULT NULL COMMENT '错误代码描述',
  `openid` varchar(128) DEFAULT NULL COMMENT '用户标识',
  `is_subscribe` varchar(1) DEFAULT NULL COMMENT '是否关注公众账号',
  `trade_type` varchar(16) DEFAULT NULL COMMENT '交易类型',
  `bank_type` varchar(16) DEFAULT NULL COMMENT '付款银行',
  `total_fee` int(11) DEFAULT '0' COMMENT '订单金额',
  `settlement_total_fee` int(11) DEFAULT NULL COMMENT '应结订单金额',
  `fee_type` varchar(8) DEFAULT NULL COMMENT '货币种类',
  `cash_fee` int(11) DEFAULT '0' COMMENT '现金支付金额',
  `cash_fee_type` varchar(16) DEFAULT NULL COMMENT '现金支付货币类型',
  `coupon_fee` int(11) DEFAULT '0' COMMENT '总代金券金额',
  `coupon_count` int(11) DEFAULT '0' COMMENT '代金券使用数量',
  `transaction_id` varchar(32) DEFAULT NULL COMMENT '微信支付订单号',
  `out_trade_no` varchar(32) DEFAULT NULL COMMENT '商户订单号',
  `attach` varchar(128) DEFAULT NULL COMMENT '商家数据包',
  `time_end` varchar(14) DEFAULT NULL COMMENT '支付完成时间',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='支付回调';

/*Data for the table `order_callback_pay` */

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

insert  into `order_product`(`order_id`,`product_id`,`product_name`,`plan_code`,`count`,`price`,`orig_price`,`total_amount`,`discount_amount`,`venueid`) values ('1000012_2019013122284880142203',1,'富贵鸟棉衣男外套2018冬季新款棉服男士修身韩版连帽',NULL,1,'200.00','200.00','200.00','0.00',2),('1000012_2019013122385635224832',1,'富贵鸟棉衣男外套2018冬季新款棉服男士修身韩版连帽',NULL,1,'600.00','600.00','600.00','0.00',1);

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

insert  into `order_product_specification_detail`(`order_id`,`product_id`,`plan_code`,`price`) values ('1000012_2019013122154621835553',1,'0','600.00'),('1000012_2019013122255290976702',1,'0','600.00'),('1000012_2019013122284880142203',1,'0','200.00'),('1000012_2019013122385635224832',1,'0','600.00'),('aaa',1,'ddd','50.00');

/*Table structure for table `pay_record` */

DROP TABLE IF EXISTS `pay_record`;

CREATE TABLE `pay_record` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL COMMENT '用户id',
  `user_type` tinyint(4) DEFAULT '0' COMMENT '0学员1教练',
  `order_id` varchar(64) DEFAULT NULL COMMENT '购物订单号',
  `money` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '金额(正值为收入，负值为支出)',
  `type` tinyint(4) DEFAULT '0' COMMENT '类型0购买商品1返现2提现3上课结算收入4充值',
  `comment` varchar(100) NOT NULL COMMENT '备注',
  `balance` decimal(10,0) DEFAULT '0' COMMENT '余额',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8 COMMENT='用户资金变动记录';

/*Data for the table `pay_record` */

insert  into `pay_record`(`id`,`user_id`,`user_type`,`order_id`,`money`,`type`,`comment`,`balance`,`crtime`) values (1,12,0,'aaa','23.00',1,'fff','20','2019-01-26 22:17:58'),(2,12,0,'aaa','45.00',3,'ee','55','2019-01-26 22:18:31'),(3,13,0,'vvv','-98.00',0,'交保护费','24','2019-01-30 13:58:29'),(4,13,0,NULL,'33.00',0,'fff','1','2019-01-30 13:58:41'),(5,12,0,'1000012_2019013121263377531538','-550.00',0,'购买商品','1105','2019-01-31 21:26:33'),(6,12,0,'1000012_2019013121372061906707','-600.00',0,'购买商品','505','2019-01-31 21:37:20'),(7,12,0,'1000012_2019013121393832226437','-600.00',0,'购买商品','-95','2019-01-31 21:39:38'),(8,12,0,'1000012_2019013121462421286731','-600.00',0,'购买商品','-695','2019-01-31 21:46:24'),(9,12,0,'1000012_2019013122015888491177','-600.00',0,'购买商品','-1295','2019-01-31 22:01:58'),(10,12,0,'1000012_2019013122034696373033','-200.00',0,'购买商品','-1495','2019-01-31 22:03:46'),(11,12,0,'1000012_2019013122054276137172','-600.00',0,'购买商品','-2095','2019-01-31 22:05:42'),(12,12,0,'1000012_2019013122094184108541','-600.00',0,'购买商品','-2695','2019-01-31 22:09:41'),(13,12,0,'1000012_2019013122131867058174','-600.00',0,'购买商品','-3295','2019-01-31 22:13:18'),(14,12,0,'1000012_2019013122154621835553','-600.00',0,'购买商品','-3895','2019-01-31 22:15:46'),(15,12,0,'1000012_2019013122255290976702','-600.00',0,'购买商品','-4495','2019-01-31 22:25:52'),(16,12,0,'1000012_2019013122284880142203','-200.00',0,'购买商品','-4695','2019-01-31 22:28:48'),(17,12,0,'1000012_2019013122385635224832','-600.00',0,'购买商品','-5295','2019-01-31 22:38:56');

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
  `main_img` varchar(500) DEFAULT NULL COMMENT '主图',
  `state` tinyint(4) DEFAULT '0' COMMENT '状态0正常 1禁用',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8 COMMENT='商品信息';

/*Data for the table `product` */

insert  into `product`(`id`,`name`,`code`,`category_id`,`summary`,`detail_id`,`sales`,`stock`,`min_buy_count`,`max_buy_count`,`main_img`,`state`,`crtime`) values (1,'富贵鸟棉衣男外套2018冬季新款棉服男士修身韩版连帽','ZYY',1,'哈哈，就是随便游啦',1,22,9999,1,9999,'http://img.zcool.cn/community/01b82e56655b2532f8754573e34730.jpg@1280w_1l_2o_100sh.jpg',0,'2019-01-07 15:56:00'),(2,'七匹狼夹克男秋冬加厚加绒保暖外套男士立领休闲青中年','yy',1,'就是低头游啦',2,33,9999,1,9999,'http://p.ananas.chaoxing.com/star3/origin/2069697acd73f47b875d392d7c7be876.jpg',0,'2019-01-08 10:35:14'),(3,'臻杰龙牛仔外套男装夹克修身短外套秋冬季加绒加厚休闲男生','SMJC',1,'素描基础知识',3,11,9999,1,9999,'http://pic17.nipic.com/20111127/8831577_105726414163_2.jpg',0,'2019-01-14 14:50:35'),(4,'蛙泳基础蛙泳基础蛙泳基础','WY',1,'蛙泳基础知识',1,34,9999,1,9999,'http://pic17.nipic.com/20111127/8831577_105726414163_2.jpg',0,'2019-01-14 15:13:58'),(5,'蝶泳基础蝶泳基础蝶泳基础','DY',1,'蝶泳基础知识',2,56,9999,1,9999,'http://pic17.nipic.com/20111127/8831577_105726414163_2.jpg',0,'2019-01-14 15:14:09'),(6,'潜水基础潜水基础潜水基础','QS',1,'潜水基础知识',3,78,9999,1,9999,'http://pic17.nipic.com/20111127/8831577_105726414163_2.jpg',0,'2019-01-14 15:14:58'),(7,'狗刨基础狗刨基础狗刨基础','GP',1,'狗刨基础知识',1,99,9999,1,9999,'http://pic17.nipic.com/20111127/8831577_105726414163_2.jpg',0,'2019-01-14 15:15:34'),(8,'英特尔（Intel） i7 8700K 酷睿六核 盒装CPU处理器','ff',5,'xxx',3,13,9999,1,9999,'https://img10.360buyimg.com/n1/s450x450_jfs/t18448/200/2532654839/268503/b46a717e/5afe4d0cN10f96d55.jpg',0,'2019-01-26 22:02:49'),(9,'金士顿(Kingston)HyperX Fury系列 雷电 480G SATA3 RGB SSD固态硬盘','yp',6,'ffff',2,1,9999,1,9999,'https://img10.360buyimg.com/n1/s450x450_jfs/t18448/200/2532654839/268503/b46a717e/5afe4d0cN10f96d55.jpg',0,'2019-01-26 22:04:52');

/*Table structure for table `product_detail` */

DROP TABLE IF EXISTS `product_detail`;

CREATE TABLE `product_detail` (
  `product_id` int(11) NOT NULL,
  `detail` mediumtext COMMENT '商品详情',
  PRIMARY KEY (`product_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='商品详细描述';

/*Data for the table `product_detail` */

insert  into `product_detail`(`product_id`,`detail`) values (1,'http://pic15.nipic.com/20110715/7935001_120846634125_2.jpg,http://pic21.nipic.com/20120519/5454342_154115399000_2.jpg,http://pic15.nipic.com/20110715/7935001_120846634125_2.jpg,http://pic21.nipic.com/20120519/5454342_154115399000_2.jpg'),(2,'http://pic15.nipic.com/20110715/7935001_120846634125_2.jpg,http://pic21.nipic.com/20120519/5454342_154115399000_2.jpg'),(3,'http://pic15.nipic.com/20110715/7935001_120846634125_2.jpg,http://pic21.nipic.com/20120519/5454342_154115399000_2.jpg,http://pic15.nipic.com/20110715/7935001_120846634125_2.jpg,http://pic21.nipic.com/20120519/5454342_154115399000_2.jpg');

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
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 COMMENT='商品折扣信息';

/*Data for the table `product_discount` */

insert  into `product_discount`(`id`,`product_id`,`discount_id`,`start_time`,`end_time`,`state`,`crttime`) values (1,1,1,'2019-01-25 10:46:55','2019-01-28 10:47:01',0,'2019-01-08 10:47:05'),(2,2,2,'2019-01-08 14:52:30','2019-01-23 14:52:34',0,'2019-01-14 14:52:29'),(3,3,2,'2019-01-25 10:46:55','2019-01-27 10:47:01',0,'2019-01-08 10:47:05');

/*Table structure for table `product_headimg` */

DROP TABLE IF EXISTS `product_headimg`;

CREATE TABLE `product_headimg` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `product_id` int(11) NOT NULL COMMENT '商品id',
  `headimg_url` varchar(500) NOT NULL COMMENT '头部图片地址',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8 COMMENT='商品的头部图片';

/*Data for the table `product_headimg` */

insert  into `product_headimg`(`id`,`product_id`,`headimg_url`,`crtime`) values (1,1,'http://h.hiphotos.baidu.com/image/h%3D300/sign=f52899ade6c4b7452b94b116fffd1e78/58ee3d6d55fbb2fbbc6b4796424a20a44723dcf6.jpg','2019-01-08 14:42:22'),(2,2,'http://pic26.nipic.com/20121221/2531170_194321033000_2.jpg','2019-01-14 14:52:53'),(3,3,'http://pic1.nipic.com/2008-11-20/20081120145730349_2.jpg','2019-01-14 14:53:07'),(4,4,'https://ss2.bdstatic.com/70cFvnSh_Q1YnxGkpoWK1HF6hhy/it/u=3123422028,2515050930&fm=26&gp=0.jpg','2019-01-14 14:53:07'),(5,5,'https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1548565794695&di=e170d741ffc3f73ca8fa71070ef79432&imgtype=0&src=http%3A%2F%2Fimgsrc.baidu.com%2Fimgad%2Fpic%2Fitem%2F0df3d7ca7bcb0a469e7772006063f6246b60afb6.jpg','2019-01-14 14:53:07'),(6,6,'https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1548565794693&di=3a9691566a544ed16b90e79c661cbd2b&imgtype=0&src=http%3A%2F%2Fm.360buyimg.com%2Fn12%2Fg7%2FM03%2F05%2F1B%2FrBEHZlBP8WIIAAAAAAEFJmHCCA0AABJGQNnfG8AAQU-762.jpg%2521q70.jpg','2019-01-14 14:53:07'),(7,7,'https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1548565794689&di=ac8e7f8ab00c58ef9b808a8b260a9934&imgtype=0&src=http%3A%2F%2Fm.360buyimg.com%2Fmobilecms%2Fs750x750_jfs%2Ft14485%2F256%2F2200825182%2F308601%2Fd4915bf4%2F5a7bba24N31ac5449.jpg%2521q80.jpg','2019-01-14 14:53:07'),(8,8,'https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1548565854974&di=8a0c81f6cacff117722233d4a5a254b9&imgtype=0&src=http%3A%2F%2Fimg10.360buyimg.com%2Fn0%2Fjfs%2Ft682%2F191%2F384961768%2F357836%2Fb3eceef0%2F54634052Nb580c3e5.jpg','2019-01-14 14:53:07'),(9,9,'https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1548565854971&di=6546513e51add20d725f1a40f673e7d5&imgtype=0&src=http%3A%2F%2Fimg003.hc360.cn%2Fm2%2FM07%2F98%2F04%2FwKhQcVQ2sSqEBRH-AAAAAGd_wj0485.jpg','2019-01-14 14:53:07');

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

insert  into `settlement`(`class_id`,`total_money`,`coach_money`,`platform_money`,`rate`,`crtime`) values (1,'500.00','40.00','10.00',0.8,'2019-01-25 21:07:10');

/*Table structure for table `site_info` */

DROP TABLE IF EXISTS `site_info`;

CREATE TABLE `site_info` (
  `pkey` varchar(50) NOT NULL COMMENT '关键字',
  `pvalue` varchar(500) NOT NULL COMMENT '值',
  `uptime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`pkey`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='站点基本信息';

/*Data for the table `site_info` */

insert  into `site_info`(`pkey`,`pvalue`,`uptime`) values ('客服电话','4008221542','2019-01-19 11:17:39'),('用户注册返现金额','20','2019-01-19 11:17:39'),('站点名称','sunny baby','2019-01-19 11:17:39'),('邀请返现金额','30','2019-01-19 11:17:39');

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
  `username` varchar(64) NOT NULL COMMENT '用户名',
  `password` varchar(64) DEFAULT NULL COMMENT '密码',
  `name` varchar(20) DEFAULT NULL COMMENT '姓名',
  `sex` tinyint(4) DEFAULT '0' COMMENT '性别0男 1女',
  `phone` varchar(11) NOT NULL COMMENT '电话号码',
  `birthday` date DEFAULT NULL COMMENT '生日',
  `headimg` varchar(500) DEFAULT NULL COMMENT '头像图片地址',
  `cash` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '账户余额',
  `state` tinyint(4) NOT NULL DEFAULT '0' COMMENT '0正常 1受限',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`),
  UNIQUE KEY `IX_USERNAME` (`username`),
  UNIQUE KEY `IX_PHONE` (`phone`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8 COMMENT='注册用户信息表';

/*Data for the table `student` */

insert  into `student`(`id`,`username`,`password`,`name`,`sex`,`phone`,`birthday`,`headimg`,`cash`,`state`,`crtime`) values (1,'xyz','e10adc3949ba59abbe56e057f20f883e','陈乔朋',0,'15271459965','2019-06-27','http://img3.redocn.com/tupian/20150318/shengdanquanbiankuangsucai_4020876.jpg','112.00',0,'2019-01-14 14:58:12'),(2,'space','54ea1cafa9e86fe45877ef16e38a8774','庄仕华',0,'17033215468','2019-01-01','http://img1.imgtn.bdimg.com/it/u=93745383,492158812&fm=26&gp=0.jpg','0.00',0,'2019-01-14 15:00:26'),(12,'oqol35CUc3zO2cPEmXmDnC5YNL1k','','Tina',0,'18601647920','0001-01-01',NULL,'-5295.00',0,'2019-01-26 14:32:03'),(13,'oqol35CYq6duEqrtKSg__mHWl0X8','','李雷',0,'13281250775','0001-01-01',NULL,'2044.00',0,'2019-01-30 10:49:05');

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

insert  into `student_coupon`(`student_id`,`coupon_id`,`count`,`state`,`crtime`) values (10,2,1,0,'2019-01-28 13:57:26'),(12,1,0,0,'2019-01-08 17:22:05'),(12,3,5,0,'2019-01-25 21:07:41'),(13,1,3,0,'2019-01-30 11:21:59'),(13,2,1,1,'2019-01-30 11:22:05'),(13,3,8,0,'2019-01-30 11:23:00');

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
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8 COMMENT='场馆信息';

/*Data for the table `venue` */

insert  into `venue`(`id`,`name`,`code`,`campus_id`,`address`,`summary`,`state`,`crtime`) values (1,'世纪城洗浴中心','SJC',1,'世纪城南路999号','免费',0,'2019-01-08 11:24:15'),(2,'清华园大澡堂','QYH',2,'清华大学第三食堂旁','1块钱个小时',0,'2019-01-14 14:44:34'),(3,'龙帅养生馆','ls',3,'asdfadfadsf','dddd',0,'2019-01-26 22:09:54'),(4,'adsadadsf','ss',1,'sss','fff',0,'2019-01-26 22:10:03'),(5,'tttttt','rr',2,'dddd','ffff',0,'2019-01-26 22:10:11'),(6,'wwww','ddd',3,'fff','gg',0,'2019-01-26 22:10:19'),(7,'eee','ff',1,'fff','gg',0,'2019-01-26 22:10:27');

/*Table structure for table `withdrawal` */

DROP TABLE IF EXISTS `withdrawal`;

CREATE TABLE `withdrawal` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `coach_id` int(11) NOT NULL COMMENT '教练id',
  `money` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '提现金额',
  `state` tinyint(4) NOT NULL DEFAULT '0' COMMENT '状态0成功 1失败',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COMMENT='提现记录';

/*Data for the table `withdrawal` */

insert  into `withdrawal`(`id`,`coach_id`,`money`,`state`,`crtime`) values (1,1,'3.00',0,'2019-01-26 22:20:32');

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
