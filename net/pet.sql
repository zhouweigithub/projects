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
) ENGINE=InnoDB AUTO_INCREMENT=39 DEFAULT CHARSET=utf8 COMMENT='商品分类';

/*Data for the table `category` */

insert  into `category`(`id`,`name`,`pid`,`url`,`state`,`index`,`remark`,`crtime`) values (1,'狗狗商品',0,NULL,1,0,NULL,'2019-06-05 18:27:00'),(2,'宠物狗粮',1,NULL,1,0,NULL,'2019-06-05 18:27:00'),(3,'狗罐头/妙鲜包',1,NULL,1,0,NULL,'2019-06-05 18:27:01'),(4,'狗狗零食',1,NULL,1,0,NULL,'2019-06-05 18:27:01'),(5,'狗狗保健品',1,NULL,1,0,NULL,'2019-06-05 18:27:01'),(6,'狗狗医疗',1,NULL,1,0,NULL,'2019-06-05 18:27:01'),(7,'狗狗沐浴露',1,NULL,1,0,NULL,'2019-06-05 18:27:01'),(8,'狗狗日用品',1,NULL,1,0,NULL,'2019-06-05 18:27:01'),(9,'狗狗美容',1,NULL,1,0,NULL,'2019-06-05 18:27:01'),(10,'狗狗玩具',1,NULL,1,0,NULL,'2019-06-05 18:27:01'),(11,'狗狗衣服/狗窝',1,NULL,1,0,NULL,'2019-06-05 18:27:01'),(12,'出行装备',1,NULL,1,0,NULL,'2019-06-05 18:27:01'),(13,'猫猫商品',0,NULL,1,0,NULL,'2019-06-05 18:27:01'),(14,'宠物猫粮',13,NULL,1,0,NULL,'2019-06-05 18:27:01'),(15,'猫罐头/妙鲜包',13,NULL,1,0,NULL,'2019-06-05 18:27:01'),(16,'猫猫零食',13,NULL,1,0,NULL,'2019-06-05 18:27:02'),(17,'猫猫保健',13,NULL,1,0,NULL,'2019-06-05 18:27:02'),(18,'猫猫医疗',13,NULL,1,0,NULL,'2019-06-05 18:27:02'),(19,'猫猫香波',13,NULL,1,0,NULL,'2019-06-05 18:27:02'),(20,'猫猫日用品',13,NULL,1,0,NULL,'2019-06-05 18:27:02'),(21,'猫猫玩具',13,NULL,1,0,NULL,'2019-06-05 18:27:02'),(22,'猫衣服/猫窝',13,NULL,1,0,NULL,'2019-06-05 18:27:02'),(23,'美容器材',13,NULL,1,0,NULL,'2019-06-05 18:27:02'),(24,'宠物服务',0,NULL,1,0,NULL,'2019-06-05 18:27:02'),(25,'宠物洗澡',24,NULL,1,0,NULL,'2019-06-05 18:27:02'),(26,'宠物美容',24,NULL,1,0,NULL,'2019-06-05 18:27:02'),(27,'宠物寄养',24,NULL,1,0,NULL,'2019-06-05 18:27:02'),(28,'宠物SPA药浴',24,NULL,1,0,NULL,'2019-06-05 18:27:02'),(29,'宠物安全检疫',24,NULL,1,0,NULL,'2019-06-05 18:27:02'),(30,'宠物训导',24,NULL,1,0,NULL,'2019-06-05 18:27:02'),(31,'宠物摄影',24,NULL,1,0,NULL,'2019-06-05 18:27:03'),(32,'活体销售',0,NULL,1,0,NULL,'2019-06-05 18:27:03'),(33,'小型犬',32,NULL,1,0,NULL,'2019-06-05 18:27:03'),(34,'中型犬',32,NULL,1,0,NULL,'2019-06-05 18:27:03'),(35,'大型犬',32,NULL,1,0,NULL,'2019-06-05 18:27:03'),(36,'猫咪',32,NULL,1,0,NULL,'2019-06-05 18:27:03'),(37,'异宠',32,NULL,1,0,NULL,'2019-06-05 18:27:03');

/*Table structure for table `discount` */

DROP TABLE IF EXISTS `discount`;

CREATE TABLE `discount` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '名称',
  `type` tinyint(4) DEFAULT '0' COMMENT '类型 0按店铺 1按分类 2按商品',
  `way` tinyint(4) DEFAULT '0' COMMENT '方式 0按件折扣 1按价格折扣',
  `fullsend` tinyint(4) DEFAULT '0' COMMENT '是否能同时使用满就减',
  `starttime` datetime DEFAULT NULL COMMENT '开始时间',
  `endtime` datetime DEFAULT NULL COMMENT '结束时间',
  `state` tinyint(4) DEFAULT '1' COMMENT '状态 0关闭 1启用',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8 COMMENT='限时折扣';

/*Data for the table `discount` */

insert  into `discount`(`id`,`name`,`type`,`way`,`fullsend`,`starttime`,`endtime`,`state`,`crtime`) values (28,'test2',0,0,1,'2019-06-11 00:00:00','2019-06-30 00:00:00',0,'2019-06-11 17:15:44'),(29,'ttt',2,0,1,'2019-06-11 00:00:00','2019-06-17 00:00:00',0,'2019-06-11 17:16:31');

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
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8 COMMENT='满就送';

/*Data for the table `fullsend` */

insert  into `fullsend`(`id`,`name`,`type`,`starttime`,`endtime`,`state`,`crtime`) values (5,'test2',0,'2019-06-11 00:00:00','2019-06-30 00:00:00',1,'2019-06-11 17:18:20'),(6,'ttt',2,'2019-06-11 00:00:00','2019-06-25 00:00:00',0,'2019-06-11 17:19:19'),(7,'test2',1,'2019-06-11 00:00:00','2019-06-25 00:00:00',0,'2019-06-11 18:41:38');

/*Table structure for table `member` */

DROP TABLE IF EXISTS `member`;

CREATE TABLE `member` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '姓名',
  `phone` varchar(11) DEFAULT '0' COMMENT '电话号码',
  `email` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '电子邮件',
  `sex` tinyint(4) DEFAULT '0' COMMENT '性别0男 1女',
  `money` decimal(10,2) DEFAULT '0.00' COMMENT '余额',
  `discount` double(20,2) DEFAULT '0.00' COMMENT '打几折',
  `remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '备注',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `IX_phone` (`phone`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 COMMENT='会员信息';

/*Data for the table `member` */

insert  into `member`(`id`,`name`,`phone`,`email`,`sex`,`money`,`discount`,`remark`,`crtime`) values (1,'李四','15248754876','xbox@qq.com',1,'4.00',0.00,'哈哈','2019-05-27 18:36:56'),(2,'陈忠和','15984575521','split@163.com',0,'1856.12',7.00,'nothing','2019-05-28 19:50:28'),(3,'陈思诚','15235462184','split@163.com',1,'8.00',8.50,'钱多多客户哦','2019-06-12 12:43:46'),(4,'jeans','12545487652','xbox@qq.com',1,'2500.00',0.00,'优质客户','2019-06-12 13:04:39');

/*Table structure for table `memberpet` */

DROP TABLE IF EXISTS `memberpet`;

CREATE TABLE `memberpet` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `memberid` int(11) NOT NULL COMMENT '会员id',
  `petid` int(11) NOT NULL COMMENT 'pet类型id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8 COMMENT='会员喜欢的宠物类型';

/*Data for the table `memberpet` */

insert  into `memberpet`(`id`,`memberid`,`petid`) values (18,1,1),(19,1,2),(20,1,4),(22,3,1),(23,0,2),(24,4,2);

/*Table structure for table `order` */

DROP TABLE IF EXISTS `order`;

CREATE TABLE `order` (
  `id` varchar(15) NOT NULL COMMENT '订单号',
  `productMoney` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '商品总金额',
  `payMoney` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '实际支付总金额',
  `discountMoney` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '优惠金额',
  `adjustMomey` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '调价金额',
  `costMoney` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '总成本',
  `profitMoney` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '总利润',
  `payType` tinyint(4) NOT NULL DEFAULT '0' COMMENT '支付方式 1现金 2微信 3支付宝 4余额 5刷卡 6其他',
  `memberid` int(11) DEFAULT '0' COMMENT '会员id',
  `remark` varchar(200) DEFAULT NULL COMMENT '备注',
  `state` tinyint(4) NOT NULL DEFAULT '0' COMMENT '0正常订单 1临时挂单',
  `crdate` date NOT NULL COMMENT '创建日期',
  `crtime` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='订单';

/*Data for the table `order` */

insert  into `order`(`id`,`productMoney`,`payMoney`,`discountMoney`,`adjustMomey`,`costMoney`,`profitMoney`,`payType`,`memberid`,`remark`,`state`,`crdate`,`crtime`) values ('106064246687757','4.00','2.40','1.60','0.00','2.00','0.40',1,0,NULL,0,'2019-06-06','2019-06-06 16:18:00'),('328144776050740','2.00','1.20','0.80','0.00','1.00','0.20',2,0,NULL,0,'2019-06-06','2019-06-06 16:09:35'),('338887708315071','2.00','1.08','0.92','0.00','1.00','0.08',4,1,NULL,0,'2019-06-06','2019-06-06 15:04:26'),('341080611612674','4.00','2.40','1.60','0.00','2.00','0.40',1,0,NULL,0,'2019-06-06','2019-06-06 16:18:38'),('347417200374565','2.00','0.84','1.16','0.00','1.00','-0.16',4,2,NULL,0,'2019-06-06','2019-06-06 15:59:49'),('355826267300702','6.00','3.60','2.40','0.00','3.00','0.60',1,1,NULL,0,'2019-06-06','2019-06-06 15:28:12'),('368584875844862','18.00','17.00','1.00','0.00','7.00','10.00',3,0,NULL,0,'2019-06-12','2019-06-12 15:58:27'),('408872628104775','2.00','1.20','0.80','0.00','1.00','0.20',4,1,NULL,0,'2019-06-05','2019-06-06 15:03:02'),('430626715448386','2.00','1.20','0.80','0.00','1.00','0.20',3,0,NULL,0,'2019-06-04','2019-06-06 16:08:39'),('505433032102807','2.00','1.08','0.92','0.03','1.00','0.08',4,1,NULL,0,'2019-06-06','2019-06-06 14:58:40'),('544440764621235','4.00','4.00','0.00','0.00','2.00','2.00',3,4,NULL,0,'2019-06-12','2019-06-12 17:12:17'),('548026256872346','8.00','4.80','3.20','0.00','3.00','1.80',2,0,'要大号哦',0,'2019-06-06','2019-06-06 12:42:57'),('577180721044510','2.00','0.84','1.16','0.00','1.00','-0.16',4,2,NULL,0,'2019-06-06','2019-06-06 15:45:40'),('652507008615002','14.00','5.88','8.12','0.00','5.00','0.88',3,2,NULL,0,'2019-06-06','2019-06-06 20:31:33'),('684525461251804','2.00','1.20','0.80','0.00','1.00','0.20',1,0,NULL,0,'2019-06-06','2019-06-06 16:15:10'),('706164105642374','2.00','0.84','1.16','0.00','1.00','-0.16',0,2,NULL,1,'2019-06-06','2019-06-06 16:19:28'),('736286646876404','2.00','0.84','1.16','0.00','1.00','-0.16',4,2,NULL,0,'2019-06-06','2019-06-06 16:20:23'),('773223178172438','2.00','0.84','1.16','0.00','1.00','-0.16',4,2,NULL,0,'2019-06-06','2019-06-06 15:34:09'),('825624131223287','2.00','1.08','0.92','0.00','1.00','0.08',4,1,NULL,0,'2019-06-06','2019-06-06 15:08:28'),('826251214813535','24.00','12.96','11.04','0.00','6.00','6.96',3,1,NULL,0,'2019-06-06','2019-06-06 12:43:29'),('852177784613186','6.00','2.52','3.48','0.00','3.00','-0.48',4,2,NULL,0,'2019-06-06','2019-06-06 16:02:40');

/*Table structure for table `orderproduct` */

DROP TABLE IF EXISTS `orderproduct`;

CREATE TABLE `orderproduct` (
  `orderid` varchar(15) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '订单号',
  `productid` int(11) NOT NULL DEFAULT '0' COMMENT '商品id',
  `price` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '原始价格',
  `count` int(11) NOT NULL DEFAULT '0' COMMENT '数量',
  `money` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '商品总金额',
  `discountMoney` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '优惠总金额',
  `payMoney` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '实际支付总金额',
  `barcode` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '条码',
  `category` int(11) NOT NULL DEFAULT '0' COMMENT '分类',
  `cost` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '成本价',
  `costMoney` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '成本总金额',
  `profitMoney` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '总利润',
  `ismemberdiscount` tinyint(4) NOT NULL DEFAULT '0' COMMENT '购买时是否启用了会员折扣',
  `name` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '名称',
  `thumbnail` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '缩略图',
  PRIMARY KEY (`orderid`,`productid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `orderproduct` */

insert  into `orderproduct`(`orderid`,`productid`,`price`,`count`,`money`,`discountMoney`,`payMoney`,`barcode`,`category`,`cost`,`costMoney`,`profitMoney`,`ismemberdiscount`,`name`,`thumbnail`) values ('106064246687757',1,'2.00',2,'4.00','1.60','2.40','1',2,'1.00','2.00','0.40',0,'应用宝','/images/upload/20190604022249300.jpeg'),('328144776050740',2,'2.00',1,'2.00','0.80','1.20','1',3,'1.00','1.00','0.20',0,'呵呵','/images/upload/20190604022306613.jpeg'),('338887708315071',2,'2.00',1,'2.00','0.92','1.08','1',3,'1.00','1.00','0.08',1,'呵呵','/images/upload/20190604022306613.jpeg'),('341080611612674',1,'2.00',2,'4.00','1.60','2.40','1',2,'1.00','2.00','0.40',0,'应用宝','/images/upload/20190604022249300.jpeg'),('347417200374565',1,'2.00',1,'2.00','1.16','0.84','1',2,'1.00','1.00','-0.16',1,'应用宝','/images/upload/20190604022249300.jpeg'),('355826267300702',1,'2.00',2,'4.00','1.60','2.40','1',2,'1.00','2.00','0.40',1,'应用宝','/images/upload/20190604022249300.jpeg'),('355826267300702',2,'2.00',1,'2.00','0.80','1.20','1',3,'1.00','1.00','0.20',1,'呵呵','/images/upload/20190604022306613.jpeg'),('368584875844862',1,'2.00',3,'6.00','0.62','5.38','1',2,'1.00','3.00','2.38',0,'正品萨迪卡玫瑰5乒乓球拍玫瑰7层纯木乒乓球底板直横拍可刻字','/images/upload/20190604022249300.jpeg'),('368584875844862',2,'2.00',2,'4.00','0.38','3.62','1',2,'1.00','2.00','1.62',0,'呵呵','/images/upload/20190604022306613.jpeg'),('368584875844862',3,'4.00',2,'8.00','0.00','8.00','3',4,'1.00','2.00','6.00',0,'泰迪专享','/images/upload/20190604022330100.jpeg'),('408872628104775',2,'2.00',1,'2.00','0.80','1.20','1',3,'1.00','1.00','0.20',0,'呵呵','/images/upload/20190604022306613.jpeg'),('430626715448386',2,'2.00',1,'2.00','0.80','1.20','1',3,'1.00','1.00','0.20',0,'呵呵','/images/upload/20190604022306613.jpeg'),('505433032102807',2,'2.00',1,'2.00','0.92','1.08','1',3,'1.00','1.00','0.08',1,'呵呵','/images/upload/20190604022306613.jpeg'),('544440764621235',1,'2.00',1,'2.00','0.00','2.00','1',2,'1.00','1.00','1.00',1,'正品萨迪卡玫瑰5乒乓球拍玫瑰7层纯木乒乓球底板直横拍可刻字','/images/upload/20190604022249300.jpeg'),('544440764621235',2,'2.00',1,'2.00','0.00','2.00','1',2,'1.00','1.00','1.00',1,'呵呵','/images/upload/20190604022306613.jpeg'),('548026256872346',3,'4.00',1,'4.00','1.60','2.40','3',4,'1.00','1.00','1.40',0,'泰迪专享','/images/upload/20190604022330100.jpeg'),('548026256872346',4,'2.00',2,'4.00','1.60','2.40','77',4,'1.00','2.00','0.40',0,'test2','/images/upload/20190604022337062.jpeg'),('577180721044510',1,'2.00',1,'2.00','1.16','0.84','1',2,'1.00','1.00','-0.16',1,'应用宝','/images/upload/20190604022249300.jpeg'),('652507008615002',2,'2.00',2,'4.00','2.32','1.68','1',3,'1.00','2.00','-0.32',1,'呵呵','/images/upload/20190604022306613.jpeg'),('652507008615002',3,'4.00',2,'8.00','4.64','3.36','3',4,'1.00','2.00','1.36',1,'泰迪专享','/images/upload/20190604022330100.jpeg'),('652507008615002',4,'2.00',1,'2.00','1.16','0.84','77',4,'1.00','1.00','-0.16',1,'test2','/images/upload/20190604022337062.jpeg'),('684525461251804',2,'2.00',1,'2.00','0.80','1.20','1',3,'1.00','1.00','0.20',0,'呵呵','/images/upload/20190604022306613.jpeg'),('706164105642374',2,'2.00',1,'2.00','1.16','0.84','1',3,'1.00','1.00','-0.16',1,'呵呵','/images/upload/20190604022306613.jpeg'),('736286646876404',2,'2.00',1,'2.00','1.16','0.84','1',3,'1.00','1.00','-0.16',1,'呵呵','/images/upload/20190604022306613.jpeg'),('773223178172438',1,'2.00',1,'2.00','1.16','0.84','1',2,'1.00','1.00','-0.16',1,'应用宝','/images/upload/20190604022249300.jpeg'),('825624131223287',1,'2.00',1,'2.00','0.92','1.08','1',2,'1.00','1.00','0.08',1,'应用宝','/images/upload/20190604022249300.jpeg'),('826251214813535',3,'4.00',6,'24.00','11.04','12.96','3',4,'1.00','6.00','6.96',1,'泰迪专享','/images/upload/20190604022330100.jpeg'),('852177784613186',1,'2.00',2,'4.00','2.32','1.68','1',2,'1.00','2.00','-0.32',1,'应用宝','/images/upload/20190604022249300.jpeg'),('852177784613186',2,'2.00',1,'2.00','1.16','0.84','1',3,'1.00','1.00','-0.16',1,'呵呵','/images/upload/20190604022306613.jpeg');

/*Table structure for table `pet` */

DROP TABLE IF EXISTS `pet`;

CREATE TABLE `pet` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '名称',
  `type` tinyint(4) NOT NULL DEFAULT '0' COMMENT '分类0小型犬 1中型犬 2大型犬 3宠物猫 4异宠',
  `state` tinyint(4) NOT NULL DEFAULT '0' COMMENT '0正常 1禁用',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=107 DEFAULT CHARSET=utf8 COMMENT='宠物分类';

/*Data for the table `pet` */

insert  into `pet`(`id`,`name`,`type`,`state`,`crtime`) values (1,'比熊',0,0,'2019-06-05 19:54:08'),(2,'博美',0,0,'2019-06-05 19:54:08'),(3,'巴哥',0,0,'2019-06-05 19:54:08'),(4,'比格犬',0,0,'2019-06-05 19:54:08'),(5,'巴吉度',0,0,'2019-06-05 19:54:08'),(6,'贵宾',0,0,'2019-06-05 19:54:08'),(7,'蝴蝶犬',0,0,'2019-06-05 19:54:08'),(8,'吉娃娃',0,0,'2019-06-05 19:54:08'),(9,'柯基犬',0,0,'2019-06-05 19:54:08'),(10,'马尔济斯',0,0,'2019-06-05 19:54:08'),(11,'牛头梗',0,0,'2019-06-05 19:54:08'),(12,'泰迪',0,0,'2019-06-05 19:54:08'),(13,'雪纳瑞',0,0,'2019-06-05 19:54:08'),(14,'西高地',0,0,'2019-06-05 19:54:08'),(15,'西施犬',0,0,'2019-06-05 19:54:09'),(16,'小鹿犬',0,0,'2019-06-05 19:54:09'),(17,'约克夏',0,0,'2019-06-05 19:54:09'),(18,'边境牧羊犬',1,0,'2019-06-05 19:54:09'),(19,'贝灵顿梗',1,0,'2019-06-05 19:54:09'),(20,'比特犬',1,0,'2019-06-05 19:54:09'),(21,'柴犬',1,0,'2019-06-05 19:54:09'),(22,'杜高',1,0,'2019-06-05 19:54:09'),(23,'大麦町犬',1,0,'2019-06-05 19:54:09'),(24,'法国斗牛犬',1,0,'2019-06-05 19:54:09'),(25,'古牧',1,0,'2019-06-05 19:54:09'),(26,'哈士奇',1,0,'2019-06-05 19:54:09'),(27,'加纳利犬',1,0,'2019-06-05 19:54:09'),(28,'可卡',1,0,'2019-06-05 19:54:09'),(29,'昆明犬',1,0,'2019-06-05 19:54:09'),(30,'库达犬',1,0,'2019-06-05 19:54:10'),(31,'拉布拉多',1,0,'2019-06-05 19:54:10'),(32,'腊肠犬',1,0,'2019-06-05 19:54:10'),(33,'马犬',1,0,'2019-06-05 19:54:10'),(34,'萨摩耶',1,0,'2019-06-05 19:54:10'),(35,'松狮',1,0,'2019-06-05 19:54:10'),(36,'沙皮犬',1,0,'2019-06-05 19:54:10'),(37,'史宾格',1,0,'2019-06-05 19:54:10'),(38,'喜乐蒂',1,0,'2019-06-05 19:54:10'),(39,'英国斗牛犬',1,0,'2019-06-05 19:54:10'),(40,'阿拉斯加',2,0,'2019-06-05 19:54:10'),(41,'伯恩山犬',2,0,'2019-06-05 19:54:10'),(42,'德国牧羊犬',2,0,'2019-06-05 19:54:10'),(43,'恶霸',2,0,'2019-06-05 19:54:10'),(44,'高加索',2,0,'2019-06-05 19:54:10'),(45,'金毛',2,0,'2019-06-05 19:54:11'),(46,'卡斯罗',2,0,'2019-06-05 19:54:11'),(47,'罗威纳',2,0,'2019-06-05 19:54:11'),(48,'拳师犬',2,0,'2019-06-05 19:54:11'),(49,'苏格兰牧羊犬',2,0,'2019-06-05 19:54:11'),(50,'威玛犬',2,0,'2019-06-05 19:54:11'),(51,'藏獒',2,0,'2019-06-05 19:54:11'),(52,'中亚牧羊犬',2,0,'2019-06-05 19:54:12'),(53,'波音达犬',2,0,'2019-06-05 19:54:12'),(54,'杜宾',2,0,'2019-06-05 19:54:12'),(55,'大白熊',2,0,'2019-06-05 19:54:12'),(56,'巨型贵宾',2,0,'2019-06-05 19:54:12'),(57,'秋田',2,0,'2019-06-05 19:54:12'),(58,'埃及猫',3,0,'2019-06-05 19:54:12'),(59,'阿比西尼亚猫',3,0,'2019-06-05 19:54:12'),(60,'奥西猫',3,0,'2019-06-05 19:54:12'),(61,'布偶猫',3,0,'2019-06-05 19:54:12'),(62,'波斯猫',3,0,'2019-06-05 19:54:12'),(63,'伯曼猫',3,0,'2019-06-05 19:54:13'),(64,'波米拉猫',3,0,'2019-06-05 19:54:13'),(65,'巴厘猫',3,0,'2019-06-05 19:54:13'),(66,'德文卷毛猫',3,0,'2019-06-05 19:54:13'),(67,'东方猫',3,0,'2019-06-05 19:54:13'),(68,'东奇尼猫',3,0,'2019-06-05 19:54:13'),(69,'俄罗斯蓝猫',3,0,'2019-06-05 19:54:13'),(70,'哈瓦那猫',3,0,'2019-06-05 19:54:13'),(71,'加拿大无毛猫',3,0,'2019-06-05 19:54:13'),(72,'金吉拉',3,0,'2019-06-05 19:54:13'),(73,'卡尔特猫',3,0,'2019-06-05 19:54:13'),(74,'科拉特猫',3,0,'2019-06-05 19:54:13'),(75,'柯尼斯卷毛猫',3,0,'2019-06-05 19:54:14'),(76,'美国短毛猫',3,0,'2019-06-05 19:54:14'),(77,'孟加拉豹猫',3,0,'2019-06-05 19:54:14'),(78,'孟买猫',3,0,'2019-06-05 19:54:14'),(79,'曼基康猫',3,0,'2019-06-05 19:54:14'),(80,'缅因猫',3,0,'2019-06-05 19:54:14'),(81,'内华达猫',3,0,'2019-06-05 19:54:14'),(82,'欧洲缅甸猫',3,0,'2019-06-05 19:54:14'),(83,'日本短尾猫',3,0,'2019-06-05 19:54:14'),(84,'苏格兰折耳猫',3,0,'2019-06-05 19:54:14'),(85,'斯芬克斯猫',3,0,'2019-06-05 19:54:14'),(86,'山东狮子猫',3,0,'2019-06-05 19:54:14'),(87,'沙特尔猫',3,0,'2019-06-05 19:54:14'),(88,'塞尔凯克卷毛猫',3,0,'2019-06-05 19:54:14'),(89,'索马里猫',3,0,'2019-06-05 19:54:14'),(90,'土耳其梵猫',3,0,'2019-06-05 19:54:15'),(91,'土耳其安哥拉猫',3,0,'2019-06-05 19:54:15'),(92,'威尔士猫',3,0,'2019-06-05 19:54:15'),(93,'暹罗猫',3,0,'2019-06-05 19:54:15'),(94,'西伯利亚森林猫',3,0,'2019-06-05 19:54:15'),(95,'喜马拉雅猫',3,0,'2019-06-05 19:54:15'),(96,'新加坡猫',3,0,'2019-06-05 19:54:15'),(97,'夏特尔猫',3,0,'2019-06-05 19:54:15'),(98,'英国短毛猫',3,0,'2019-06-05 19:54:15'),(99,'异国短毛猫',3,0,'2019-06-05 19:54:15'),(100,'中国狸花猫',3,0,'2019-06-05 19:54:15'),(101,'重点色短毛猫',3,0,'2019-06-05 19:54:15'),(102,'中华田园猫',3,0,'2019-06-05 19:54:15'),(103,'蜥蜴',4,0,'2019-06-05 19:54:15'),(104,'金花蛇',4,0,'2019-06-05 19:54:16'),(105,'蚂蚁',4,0,'2019-06-05 19:54:16'),(106,'龙猫',4,0,'2019-06-05 19:54:16');

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
  `cost` decimal(10,2) DEFAULT '0.00' COMMENT '成本',
  `price` decimal(10,2) DEFAULT '0.00' COMMENT '卖价',
  `ismemberdiscount` tinyint(4) DEFAULT '0' COMMENT '是否启用会员折扣（0不启用 1启用）',
  `thumbnail` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '缩略图',
  `remark` varchar(500) DEFAULT NULL COMMENT '备注',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `IX_BARCODE` (`barcode`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8 COMMENT='商品信息';

/*Data for the table `product` */

insert  into `product`(`id`,`barcode`,`name`,`category`,`store`,`sales`,`warn`,`cost`,`price`,`ismemberdiscount`,`thumbnail`,`remark`,`crtime`) values (1,'2','正品萨迪卡玫瑰5乒乓球拍玫瑰7层纯木乒乓球底板直横拍可刻字',2,4,16,6,'1.00','2.00',1,'/images/upload/20190604022249300.jpeg',NULL,'2019-05-25 14:39:19'),(2,'1','呵呵',2,9,0,8,'1.00','2.00',0,'/images/upload/20190604022306613.jpeg',NULL,'2019-05-25 14:41:26'),(3,'3','泰迪专享',4,11,2,5,'1.00','4.00',1,'/images/upload/20190604022330100.jpeg',NULL,'2019-05-25 16:20:49'),(4,'77','test2',4,17,3,5,'1.00','2.00',1,'/images/upload/20190604022337062.jpeg',NULL,'2019-05-25 16:24:56'),(5,'6353541892457','我的小可爱',4,10,0,10,'3.00','4.00',0,'/images/upload/20190611044753621.jpeg','uuuuu','2019-06-11 16:48:22');

/*Table structure for table `railcard` */

DROP TABLE IF EXISTS `railcard`;

CREATE TABLE `railcard` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `phone` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '手机号',
  `name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '主人姓名',
  `petname` varchar(50) DEFAULT NULL COMMENT '宠物名字',
  `petage` tinyint(4) DEFAULT '0' COMMENT '宠物年龄（岁）',
  `times` int(11) DEFAULT '0' COMMENT '最大可使用次数',
  `lefttimes` int(11) DEFAULT '0' COMMENT '剩余使用次数',
  `money` decimal(20,2) DEFAULT '0.00' COMMENT '最大可使用金额',
  `starttime` date DEFAULT NULL COMMENT '开始时间',
  `endtime` date DEFAULT NULL COMMENT '结束时间',
  `remark` varchar(200) DEFAULT NULL COMMENT '备注',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `IX_phone` (`phone`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 COMMENT='优惠卡';

/*Data for the table `railcard` */

insert  into `railcard`(`id`,`phone`,`name`,`petname`,`petage`,`times`,`lefttimes`,`money`,`starttime`,`endtime`,`remark`,`crtime`) values (3,'9','ttt',NULL,0,6,1,'56.00','2019-05-28','2019-06-20',NULL,'2019-05-28 13:48:25');

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
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8 COMMENT='优惠卡使用记录';

/*Data for the table `railcard_record` */

insert  into `railcard_record`(`id`,`railcardid`,`times`,`lefttimes`,`remark`,`crtime`) values (1,2,2,3,'ggg','2019-05-28 13:53:28'),(2,1,1,3,'h','2019-05-28 13:58:44'),(3,1,1,2,'22','2019-05-28 14:18:02'),(4,1,2,0,'33','2019-05-28 14:18:28'),(5,1,4,0,'3','2019-05-28 14:18:51'),(6,1,0,0,'4','2019-05-28 14:23:24'),(7,3,1,1,'rrrr','2019-06-12 16:41:32');

/*Table structure for table `recharge` */

DROP TABLE IF EXISTS `recharge`;

CREATE TABLE `recharge` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `sno` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '流水号',
  `memberid` int(11) NOT NULL DEFAULT '0' COMMENT '会员id',
  `money` decimal(20,2) NOT NULL DEFAULT '0.00' COMMENT '充值金额',
  `paymoney` decimal(20,2) NOT NULL DEFAULT '0.00' COMMENT '实际支付金额',
  `balance` decimal(20,2) NOT NULL DEFAULT '0.00' COMMENT '当前余额',
  `remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '备注',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `IX_SNO` (`sno`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 COMMENT='充值记录';

/*Data for the table `recharge` */

insert  into `recharge`(`id`,`sno`,`memberid`,`money`,`paymoney`,`balance`,`remark`,`crtime`) values (1,'201905271959507055',1,'3.00','0.00','8.00','4','2019-05-27 19:59:50'),(2,'201905272000059761',1,'4.00','0.00','12.00','u','2019-05-27 20:00:05'),(3,'201906121035443542',1,'4.00','2.00','4.00','','2019-06-12 10:35:44'),(4,'201906121305508712',4,'2500.00','2000.00','2500.00','净资产','2019-06-12 13:05:50');

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
) ENGINE=InnoDB AUTO_INCREMENT=91 DEFAULT CHARSET=utf8 COMMENT='做活动的商品';

/*Data for the table `saleproduct` */

insert  into `saleproduct`(`id`,`type`,`ptype`,`saleid`,`productid`,`crtime`) values (3,0,1,12,9,'2019-06-02 13:22:34'),(84,1,0,6,2,'2019-06-11 17:19:19'),(85,1,0,6,3,'2019-06-11 17:19:19'),(86,0,0,29,1,'2019-06-11 17:31:43'),(87,0,0,29,2,'2019-06-11 17:31:43'),(90,1,1,7,2,'2019-06-12 15:04:00');

/*Table structure for table `salerule` */

DROP TABLE IF EXISTS `salerule`;

CREATE TABLE `salerule` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `saleid` int(11) DEFAULT '0' COMMENT '活动id',
  `type` tinyint(4) DEFAULT '0' COMMENT '0按件折扣、1按价格折扣',
  `aim` decimal(10,2) DEFAULT '0.00' COMMENT '起始金额或数量',
  `sale` double DEFAULT '0' COMMENT '打几折或减多少金额',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=97 DEFAULT CHARSET=utf8 COMMENT='活动规则';

/*Data for the table `salerule` */

insert  into `salerule`(`id`,`saleid`,`type`,`aim`,`sale`,`crtime`) values (85,6,0,'8.00',3,'2019-06-11 17:19:19'),(86,29,0,'2.00',8,'2019-06-11 17:31:43'),(87,29,0,'3.00',7,'2019-06-11 17:31:43'),(88,28,0,'2.00',9,'2019-06-11 17:32:56'),(89,28,0,'3.00',8,'2019-06-11 17:32:56'),(92,7,0,'5.00',1,'2019-06-12 15:04:00'),(95,5,0,'5.00',1,'2019-06-12 16:10:00'),(96,5,0,'10.00',3,'2019-06-12 16:10:00');

/*Table structure for table `user` */

DROP TABLE IF EXISTS `user`;

CREATE TABLE `user` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '用户名',
  `password` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '密码',
  `name` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '姓名',
  `state` tinyint(4) DEFAULT NULL COMMENT '状态0正常 1受限',
  `lastlogintime` datetime DEFAULT NULL COMMENT '最后一次登录时间',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `IX_username` (`username`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COMMENT='用户信息';

/*Data for the table `user` */

insert  into `user`(`id`,`username`,`password`,`name`,`state`,`lastlogintime`,`crtime`) values (1,'hello','e10adc3949ba59abbe56e057f20f883e','陈成',0,'2019-06-10 15:00:44','2019-06-10 15:01:18');

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
