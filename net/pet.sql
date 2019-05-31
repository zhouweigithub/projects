/*
SQLyog Ultimate v12.08 (64 bit)
MySQL - 8.0.11 : Database - spetmall
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`spetmall` /*!40100 DEFAULT CHARACTER SET utf8 */;

/*Table structure for table `category` */

DROP TABLE IF EXISTS `category`;

CREATE TABLE `category` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '名称',
  `pid` int(11) NOT NULL DEFAULT '0' COMMENT '父id',
  `url` varchar(500) DEFAULT NULL COMMENT '外部链接',
  `state` tinyint(4) DEFAULT '1' COMMENT '是否启用',
  `index` int(11) DEFAULT '0' COMMENT '排序',
  `remark` varchar(500) DEFAULT NULL COMMENT '备注',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8 COMMENT='商品分类';

/*Data for the table `category` */

insert  into `category`(`id`,`name`,`pid`,`url`,`state`,`index`,`remark`,`crtime`) values (1,'ohmygod',0,'xxx',1,5,'rrr','2019-05-30 16:47:18'),(3,'ttt',0,'ww',1,3,'rr','2019-05-30 17:56:20'),(4,'rrhh',1,'yy',1,0,'fff','2019-05-30 17:56:29'),(5,'ggg',4,'ddd',1,0,'ggg','2019-05-30 17:56:36'),(6,'uuu8',3,'gg',1,0,'jj','2019-05-30 19:34:50'),(7,'gggg',1,'ccc',1,2,'gg','2019-05-30 19:36:20'),(8,'ppp',1,'kkk',1,3,';;','2019-05-30 19:36:44'),(9,'yyy',3,'x',1,0,'f','2019-05-30 20:11:49'),(10,'x',0,'f',1,2,'g','2019-05-30 20:12:04'),(11,'ter',0,'r',1,0,'r','2019-05-30 20:13:30'),(12,'sd',5,'g',1,0,'d','2019-05-30 20:13:53'),(13,'d',1,'f',1,0,'f','2019-05-30 20:14:05');

/*Table structure for table `discount` */

DROP TABLE IF EXISTS `discount`;

CREATE TABLE `discount` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '名称',
  `type` tinyint(4) DEFAULT '0' COMMENT '类型 0按店铺 1按分类 2按商品',
  `way` tinyint(4) DEFAULT '0' COMMENT '方式 0按件折扣 1按价格折扣',
  `coupon` tinyint(4) DEFAULT '0' COMMENT '是否能同时使用优惠券',
  `fullsend` tinyint(4) DEFAULT '0' COMMENT '是否能同时使用满就减',
  `starttime` datetime DEFAULT NULL COMMENT '开始时间',
  `endtime` datetime DEFAULT NULL COMMENT '结束时间',
  `state` tinyint(4) DEFAULT '1' COMMENT '状态 0关闭 1启用',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='限时折扣';

/*Data for the table `discount` */

/*Table structure for table `fullsend` */

DROP TABLE IF EXISTS `fullsend`;

CREATE TABLE `fullsend` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '名称',
  `type` tinyint(4) DEFAULT '0' COMMENT '类型 0按店铺 1按分类 2按商品',
  `starttime` datetime DEFAULT NULL COMMENT '开始时间',
  `endtime` datetime DEFAULT NULL COMMENT '结束时间',
  `state` tinyint(4) DEFAULT '1' COMMENT '状态 0关闭 1启用',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='满就送';

/*Data for the table `fullsend` */

/*Table structure for table `member` */

DROP TABLE IF EXISTS `member`;

CREATE TABLE `member` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '姓名',
  `phone` varchar(11) DEFAULT '0' COMMENT '电话号码',
  `email` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '电子邮件',
  `sex` tinyint(4) DEFAULT '0' COMMENT '性别0男 1女',
  `money` decimal(10,0) DEFAULT '0' COMMENT '余额',
  `discount` double(20,2) DEFAULT '0.00' COMMENT '打几折',
  `remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '备注',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COMMENT='会员信息';

/*Data for the table `member` */

insert  into `member`(`id`,`name`,`phone`,`email`,`sex`,`money`,`discount`,`remark`,`crtime`) values (1,'李四','15248754876','xbox@qq.com',1,'120',9.00,'哈哈','2019-05-27 18:36:56'),(2,'陈忠和','15984575521','split@163.com',0,'1862',7.00,'nothing','2019-05-28 19:50:28');

/*Table structure for table `memberpet` */

DROP TABLE IF EXISTS `memberpet`;

CREATE TABLE `memberpet` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `memberid` int(11) NOT NULL COMMENT '会员id',
  `petid` int(11) NOT NULL COMMENT 'pet类型id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8 COMMENT='会员喜欢的宠物类型';

/*Data for the table `memberpet` */

insert  into `memberpet`(`id`,`memberid`,`petid`) values (12,1,6),(13,1,2),(14,1,3),(15,1,5),(16,0,1),(17,0,3);

/*Table structure for table `order` */

DROP TABLE IF EXISTS `order`;

CREATE TABLE `order` (
  `id` varchar(15) NOT NULL COMMENT '订单号',
  `productMoney` decimal(10,0) NOT NULL DEFAULT '0' COMMENT '商品总金额',
  `payMoney` decimal(10,0) NOT NULL DEFAULT '0' COMMENT '实际支付总金额',
  `discountMoney` decimal(10,0) NOT NULL DEFAULT '0' COMMENT '优惠金额',
  `adjustMomey` decimal(10,0) NOT NULL DEFAULT '0' COMMENT '高价金额',
  `payType` tinyint(4) NOT NULL DEFAULT '0' COMMENT '支付方式',
  `memberid` int(11) DEFAULT '0' COMMENT '会员id',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='订单';

/*Data for the table `order` */

/*Table structure for table `orderproduct` */

DROP TABLE IF EXISTS `orderproduct`;

CREATE TABLE `orderproduct` (
  `orderid` varchar(15) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '订单号',
  `productid` int(11) NOT NULL DEFAULT '0' COMMENT '商品id',
  `price` decimal(10,0) NOT NULL DEFAULT '0' COMMENT '价格',
  `count` int(11) NOT NULL DEFAULT '0' COMMENT '数量',
  `money` decimal(10,0) NOT NULL DEFAULT '0' COMMENT '商品总金额',
  `discountMoney` decimal(10,0) NOT NULL DEFAULT '0' COMMENT '优惠总金额',
  `payMoney` decimal(10,0) NOT NULL DEFAULT '0' COMMENT '实际支付总金额',
  PRIMARY KEY (`orderid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `orderproduct` */

/*Table structure for table `pet` */

DROP TABLE IF EXISTS `pet`;

CREATE TABLE `pet` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '名称',
  `type` tinyint(4) DEFAULT '0' COMMENT '分类0小型犬 1中型犬 2大型犬 3宠物猫 4异宠',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8 COMMENT='宠物分类';

/*Data for the table `pet` */

insert  into `pet`(`id`,`name`,`type`,`crtime`) values (1,'小型犬',0,'2019-05-27 18:46:49'),(2,'中型犬',1,'2019-05-27 18:47:00'),(3,'大型犬',2,'2019-05-27 18:47:07'),(4,'宠物猫',3,'2019-05-27 18:47:16'),(5,'异宠',4,'2019-05-27 18:47:25'),(6,'小型犬2',0,'2019-05-27 18:47:40');

/*Table structure for table `product` */

DROP TABLE IF EXISTS `product`;

CREATE TABLE `product` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `barcode` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '条码',
  `name` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '名称',
  `category` int(11) DEFAULT '0' COMMENT '分类',
  `store` int(11) DEFAULT '0' COMMENT '库存',
  `sales` int(11) DEFAULT '0' COMMENT '销量',
  `warn` int(11) DEFAULT '0' COMMENT '库存警戒值',
  `cost` decimal(10,0) DEFAULT '0' COMMENT '成本',
  `price` decimal(10,0) DEFAULT '0' COMMENT '卖价',
  `ismemberdiscount` tinyint(4) DEFAULT '0' COMMENT '是否启用会员折扣',
  `thumbnail` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '缩略图',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 COMMENT='商品信息';

/*Data for the table `product` */

insert  into `product`(`id`,`barcode`,`name`,`category`,`store`,`sales`,`warn`,`cost`,`price`,`ismemberdiscount`,`thumbnail`,`crtime`) values (1,'1','应用宝',0,5,8,3,'1','2',1,'/images/upload/20190525042208972.jpeg','2019-05-25 14:39:19'),(2,'1','应用宝',0,8,5,3,'1','2',1,'/images/upload/20190525042219194.jpeg','2019-05-25 14:41:26'),(3,'3','泰迪专享',0,10,0,5,'1','4',1,'/images/upload/20190525042230377.jpeg','2019-05-25 16:20:49'),(4,'77','test2',0,2,0,1,'1','2',1,'/images/upload/20190525050450622.jpeg','2019-05-25 16:24:56');

/*Table structure for table `railcard` */

DROP TABLE IF EXISTS `railcard`;

CREATE TABLE `railcard` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `phone` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '手机号',
  `name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '姓名',
  `times` int(11) DEFAULT '0' COMMENT '最大可使用次数',
  `lefttimes` int(11) DEFAULT '0' COMMENT '剩余使用次数',
  `money` decimal(20,2) DEFAULT '0.00' COMMENT '最大可使用金额',
  `leftmoney` decimal(20,2) DEFAULT '0.00' COMMENT '剩余金额',
  `starttime` date DEFAULT NULL COMMENT '开始时间',
  `endtime` date DEFAULT NULL COMMENT '结束时间',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 COMMENT='优惠卡';

/*Data for the table `railcard` */

insert  into `railcard`(`id`,`phone`,`name`,`times`,`lefttimes`,`money`,`leftmoney`,`starttime`,`endtime`,`crtime`) values (3,'9','ttt',6,2,'56.00','0.00','2019-05-28','2019-06-20','2019-05-28 13:48:25');

/*Table structure for table `railcard_record` */

DROP TABLE IF EXISTS `railcard_record`;

CREATE TABLE `railcard_record` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `railcardid` int(11) NOT NULL DEFAULT '0' COMMENT '优惠卡id',
  `times` int(11) NOT NULL DEFAULT '0' COMMENT '使用次数',
  `lefttimes` int(11) NOT NULL DEFAULT '0' COMMENT '剩余次数',
  `remark` varchar(200) DEFAULT NULL COMMENT '使用备注',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8 COMMENT='优惠卡使用记录';

/*Data for the table `railcard_record` */

insert  into `railcard_record`(`id`,`railcardid`,`times`,`lefttimes`,`remark`,`crtime`) values (1,2,2,3,'ggg','2019-05-28 13:53:28'),(2,1,1,3,'h','2019-05-28 13:58:44'),(3,1,1,2,'22','2019-05-28 14:18:02'),(4,1,2,0,'33','2019-05-28 14:18:28'),(5,1,4,0,'3','2019-05-28 14:18:51'),(6,1,0,0,'4','2019-05-28 14:23:24');

/*Table structure for table `recharge` */

DROP TABLE IF EXISTS `recharge`;

CREATE TABLE `recharge` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `sno` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '流水号',
  `memberid` int(11) NOT NULL DEFAULT '0' COMMENT '会员id',
  `money` decimal(20,2) NOT NULL DEFAULT '0.00' COMMENT '充值金额',
  `balance` decimal(20,2) NOT NULL DEFAULT '0.00' COMMENT '余额',
  `remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '备注',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `IX_SNO` (`sno`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COMMENT='充值记录';

/*Data for the table `recharge` */

insert  into `recharge`(`id`,`sno`,`memberid`,`money`,`balance`,`remark`,`crtime`) values (1,'201905271959507055',1,'3.00','8.00','4','2019-05-27 19:59:50'),(2,'201905272000059761',1,'4.00','12.00','u','2019-05-27 20:00:05');

/*Table structure for table `saleproduct` */

DROP TABLE IF EXISTS `saleproduct`;

CREATE TABLE `saleproduct` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `type` tinyint(4) DEFAULT '0' COMMENT '0折扣 1满就送',
  `ptype` tinyint(4) DEFAULT '0' COMMENT '0商品 1分类',
  `saleid` int(11) NOT NULL DEFAULT '0' COMMENT '活动id',
  `productid` int(11) DEFAULT '0' COMMENT '商品id',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='做活动的商品';

/*Data for the table `saleproduct` */

/*Table structure for table `salerule` */

DROP TABLE IF EXISTS `salerule`;

CREATE TABLE `salerule` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `saleid` int(11) DEFAULT '0' COMMENT '活动id',
  `type` tinyint(4) DEFAULT '0' COMMENT '0按件折扣、1按价格折扣',
  `aim` double DEFAULT '0' COMMENT '起始金额或数量',
  `sale` double DEFAULT '0' COMMENT '打几折或减多少金额',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='活动规则';

/*Data for the table `salerule` */

/*Table structure for table `temp_order` */

DROP TABLE IF EXISTS `temp_order`;

CREATE TABLE `temp_order` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userid` int(11) NOT NULL DEFAULT '0' COMMENT '营业员id',
  `memberid` int(11) NOT NULL DEFAULT '0' COMMENT '会员id',
  `paytype` tinyint(4) NOT NULL DEFAULT '0' COMMENT '支付方式',
  `products` varchar(10000) NOT NULL COMMENT '商品',
  `member_discount` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否启用会员折扣',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='挂单信息';

/*Data for the table `temp_order` */

/*Table structure for table `user` */

DROP TABLE IF EXISTS `user`;

CREATE TABLE `user` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '用户名',
  `password` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '密码',
  `name` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '姓名',
  `state` tinyint(4) DEFAULT NULL COMMENT '状态0正常 1受限',
  `lastlogintime` datetime DEFAULT NULL COMMENT '最后一次登录时间',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='用户信息';

/*Data for the table `user` */

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
