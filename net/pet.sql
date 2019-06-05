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
) ENGINE=InnoDB AUTO_INCREMENT=38 DEFAULT CHARSET=utf8 COMMENT='商品分类';

/*Data for the table `category` */

insert  into `category`(`id`,`name`,`pid`,`url`,`state`,`index`,`remark`,`crtime`) values (1,'狗狗商品',0,NULL,1,0,NULL,'2019-06-05 18:27:00'),(2,'宠物狗粮',1,NULL,1,0,NULL,'2019-06-05 18:27:00'),(3,'狗罐头/妙鲜包',1,NULL,1,0,NULL,'2019-06-05 18:27:01'),(4,'狗狗零食',1,NULL,1,0,NULL,'2019-06-05 18:27:01'),(5,'狗狗保健品',1,NULL,1,0,NULL,'2019-06-05 18:27:01'),(6,'狗狗医疗',1,NULL,1,0,NULL,'2019-06-05 18:27:01'),(7,'狗狗沐浴露',1,NULL,1,0,NULL,'2019-06-05 18:27:01'),(8,'狗狗日用品',1,NULL,1,0,NULL,'2019-06-05 18:27:01'),(9,'狗狗美容',1,NULL,1,0,NULL,'2019-06-05 18:27:01'),(10,'狗狗玩具',1,NULL,1,0,NULL,'2019-06-05 18:27:01'),(11,'狗狗衣服/狗窝',1,NULL,1,0,NULL,'2019-06-05 18:27:01'),(12,'出行装备',1,NULL,1,0,NULL,'2019-06-05 18:27:01'),(13,'猫猫商品',0,NULL,1,0,NULL,'2019-06-05 18:27:01'),(14,'宠物猫粮',13,NULL,1,0,NULL,'2019-06-05 18:27:01'),(15,'猫罐头/妙鲜包',13,NULL,1,0,NULL,'2019-06-05 18:27:01'),(16,'猫猫零食',13,NULL,1,0,NULL,'2019-06-05 18:27:02'),(17,'猫猫保健',13,NULL,1,0,NULL,'2019-06-05 18:27:02'),(18,'猫猫医疗',13,NULL,1,0,NULL,'2019-06-05 18:27:02'),(19,'猫猫香波',13,NULL,1,0,NULL,'2019-06-05 18:27:02'),(20,'猫猫日用品',13,NULL,1,0,NULL,'2019-06-05 18:27:02'),(21,'猫猫玩具',13,NULL,1,0,NULL,'2019-06-05 18:27:02'),(22,'猫衣服/猫窝',13,NULL,1,0,NULL,'2019-06-05 18:27:02'),(23,'美容器材',13,NULL,1,0,NULL,'2019-06-05 18:27:02'),(24,'宠物服务',0,NULL,1,0,NULL,'2019-06-05 18:27:02'),(25,'宠物洗澡',24,NULL,1,0,NULL,'2019-06-05 18:27:02'),(26,'宠物美容',24,NULL,1,0,NULL,'2019-06-05 18:27:02'),(27,'宠物寄养',24,NULL,1,0,NULL,'2019-06-05 18:27:02'),(28,'宠物SPA药浴',24,NULL,1,0,NULL,'2019-06-05 18:27:02'),(29,'宠物安全检疫',24,NULL,1,0,NULL,'2019-06-05 18:27:02'),(30,'宠物训导',24,NULL,1,0,NULL,'2019-06-05 18:27:02'),(31,'宠物摄影',24,NULL,1,0,NULL,'2019-06-05 18:27:03'),(32,'活体销售',0,NULL,1,0,NULL,'2019-06-05 18:27:03'),(33,'小型犬',32,NULL,1,0,NULL,'2019-06-05 18:27:03'),(34,'中型犬',32,NULL,1,0,NULL,'2019-06-05 18:27:03'),(35,'大型犬',32,NULL,1,0,NULL,'2019-06-05 18:27:03'),(36,'猫咪',32,NULL,1,0,NULL,'2019-06-05 18:27:03'),(37,'异宠',32,NULL,1,0,NULL,'2019-06-05 18:27:03');

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
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8 COMMENT='限时折扣';

/*Data for the table `discount` */

insert  into `discount`(`id`,`name`,`type`,`way`,`coupon`,`fullsend`,`starttime`,`endtime`,`state`,`crtime`) values (23,'ttt',2,0,0,0,'2019-06-04 00:00:00','2019-06-20 00:00:00',0,'2019-06-04 12:15:29'),(25,'ttt',0,0,0,0,'2019-06-04 00:00:00','2019-06-20 00:00:00',1,'2019-06-04 14:14:37');

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
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 COMMENT='满就送';

/*Data for the table `fullsend` */

insert  into `fullsend`(`id`,`name`,`type`,`starttime`,`endtime`,`state`,`crtime`) values (4,'j',2,'2019-06-03 00:00:00','2019-06-20 00:00:00',0,'2019-06-03 20:51:34');

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
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COMMENT='会员信息';

/*Data for the table `member` */

insert  into `member`(`id`,`name`,`phone`,`email`,`sex`,`money`,`discount`,`remark`,`crtime`) values (1,'李四','15248754876','xbox@qq.com',1,'120.00',9.00,'哈哈','2019-05-27 18:36:56'),(2,'陈忠和','15984575521','split@163.com',0,'1862.00',7.00,'nothing','2019-05-28 19:50:28');

/*Table structure for table `memberpet` */

DROP TABLE IF EXISTS `memberpet`;

CREATE TABLE `memberpet` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `memberid` int(11) NOT NULL COMMENT '会员id',
  `petid` int(11) NOT NULL COMMENT 'pet类型id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8 COMMENT='会员喜欢的宠物类型';

/*Data for the table `memberpet` */

insert  into `memberpet`(`id`,`memberid`,`petid`) values (16,0,1),(17,0,3),(18,1,1),(19,1,2),(20,1,4);

/*Table structure for table `order` */

DROP TABLE IF EXISTS `order`;

CREATE TABLE `order` (
  `id` varchar(15) NOT NULL COMMENT '订单号',
  `productMoney` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '商品总金额',
  `payMoney` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '实际支付总金额',
  `discountMoney` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '优惠金额',
  `adjustMomey` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '调价金额',
  `payType` tinyint(4) NOT NULL DEFAULT '0' COMMENT '支付方式',
  `memberid` int(11) DEFAULT '0' COMMENT '会员id',
  `remark` varchar(200) DEFAULT NULL COMMENT '备注',
  `state` tinyint(4) DEFAULT '0' COMMENT '0正常订单 1临时挂单',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='订单';

/*Data for the table `order` */

insert  into `order`(`id`,`productMoney`,`payMoney`,`discountMoney`,`adjustMomey`,`payType`,`memberid`,`remark`,`state`,`crtime`) values ('042862702735862','6.00','3.60','2.40','0.00',2,0,NULL,0,'2019-06-04 20:44:23'),('081862117563173','20.00','1.60','9.20','0.80',3,1,'ttt',0,'2019-06-04 20:17:57'),('175363253240431','24.00','14.40','9.60','0.00',0,0,NULL,1,'2019-06-05 19:43:19'),('214334234618017','2.00','1.20','0.80','0.00',2,0,NULL,0,'2019-06-05 10:16:47'),('325255132743137','22.00','13.20','8.80','0.00',0,0,NULL,1,'2019-06-05 16:02:15'),('382214228470065','2.00','1.20','0.80','0.00',2,0,NULL,0,'2019-06-05 09:57:25'),('418726658274633','4.00','2.40','1.60','0.00',4,0,NULL,0,'2019-06-04 20:44:57'),('541306722146522','8.00','4.32','3.68','0.00',4,1,NULL,0,'2019-06-05 16:32:30'),('642147312228306','6.00','3.60','2.40','0.00',2,0,NULL,0,'2019-06-05 10:15:59'),('827688331730753','20.00','10.80','9.20','0.80',3,1,'ttt',0,'2019-06-04 20:21:49');

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
  `category` int(11) DEFAULT '0' COMMENT '分类',
  `ismemberdiscount` tinyint(4) DEFAULT '0' COMMENT '购买时是否启用了会员折扣',
  `name` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '名称',
  `thumbnail` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '缩略图',
  PRIMARY KEY (`orderid`,`productid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `orderproduct` */

insert  into `orderproduct`(`orderid`,`productid`,`price`,`count`,`money`,`discountMoney`,`payMoney`,`barcode`,`category`,`ismemberdiscount`,`name`,`thumbnail`) values ('042862702735862',1,'2.00',3,'6.00','2.40','3.60',NULL,0,0,NULL,NULL),('081862117563173',1,'2.00',2,'4.00','1.84','2.16',NULL,0,0,NULL,NULL),('081862117563173',2,'2.00',2,'4.00','1.84','2.16',NULL,0,0,NULL,NULL),('081862117563173',3,'4.00',2,'8.00','3.68','4.32',NULL,0,0,NULL,NULL),('081862117563173',4,'2.00',2,'4.00','1.84','2.16',NULL,0,0,NULL,NULL),('175363253240431',2,'2.00',4,'8.00','3.20','4.80','1',3,0,'呵呵','/images/upload/20190604022306613.jpeg'),('175363253240431',3,'4.00',3,'12.00','4.80','7.20','3',4,0,'泰迪专享','/images/upload/20190604022330100.jpeg'),('175363253240431',4,'2.00',2,'4.00','1.60','2.40','77',4,0,'test2','/images/upload/20190604022337062.jpeg'),('214334234618017',2,'2.00',1,'2.00','0.80','1.20',NULL,0,0,NULL,NULL),('325255132743137',2,'2.00',3,'6.00','2.40','3.60',NULL,0,0,NULL,NULL),('325255132743137',3,'4.00',3,'12.00','4.80','7.20',NULL,0,0,NULL,NULL),('325255132743137',4,'2.00',2,'4.00','1.60','2.40',NULL,0,0,NULL,NULL),('382214228470065',1,'2.00',1,'2.00','0.80','1.20',NULL,0,0,NULL,NULL),('418726658274633',1,'2.00',2,'4.00','1.60','2.40',NULL,0,0,NULL,NULL),('541306722146522',2,'2.00',2,'4.00','1.84','2.16','1',0,1,'呵呵','/images/upload/20190604022306613.jpeg'),('541306722146522',3,'4.00',1,'4.00','1.84','2.16','3',0,1,'泰迪专享','/images/upload/20190604022330100.jpeg'),('642147312228306',2,'2.00',1,'2.00','0.80','1.20',NULL,0,0,NULL,NULL),('642147312228306',3,'4.00',1,'4.00','1.60','2.40',NULL,0,0,NULL,NULL),('827688331730753',1,'2.00',2,'4.00','1.84','2.16',NULL,0,0,NULL,NULL),('827688331730753',2,'2.00',2,'4.00','1.84','2.16',NULL,0,0,NULL,NULL),('827688331730753',3,'4.00',2,'8.00','3.68','4.32',NULL,0,0,NULL,NULL),('827688331730753',4,'2.00',2,'4.00','1.84','2.16',NULL,0,0,NULL,NULL);

/*Table structure for table `pet` */

DROP TABLE IF EXISTS `pet`;

CREATE TABLE `pet` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '名称',
  `type` tinyint(4) DEFAULT '0' COMMENT '分类0小型犬 1中型犬 2大型犬 3宠物猫 4异宠',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=107 DEFAULT CHARSET=utf8 COMMENT='宠物分类';

/*Data for the table `pet` */

insert  into `pet`(`id`,`name`,`type`,`crtime`) values (1,'比熊',0,'2019-06-05 19:54:08'),(2,'博美',0,'2019-06-05 19:54:08'),(3,'巴哥',0,'2019-06-05 19:54:08'),(4,'比格犬',0,'2019-06-05 19:54:08'),(5,'巴吉度',0,'2019-06-05 19:54:08'),(6,'贵宾',0,'2019-06-05 19:54:08'),(7,'蝴蝶犬',0,'2019-06-05 19:54:08'),(8,'吉娃娃',0,'2019-06-05 19:54:08'),(9,'柯基犬',0,'2019-06-05 19:54:08'),(10,'马尔济斯',0,'2019-06-05 19:54:08'),(11,'牛头梗',0,'2019-06-05 19:54:08'),(12,'泰迪',0,'2019-06-05 19:54:08'),(13,'雪纳瑞',0,'2019-06-05 19:54:08'),(14,'西高地',0,'2019-06-05 19:54:08'),(15,'西施犬',0,'2019-06-05 19:54:09'),(16,'小鹿犬',0,'2019-06-05 19:54:09'),(17,'约克夏',0,'2019-06-05 19:54:09'),(18,'边境牧羊犬',1,'2019-06-05 19:54:09'),(19,'贝灵顿梗',1,'2019-06-05 19:54:09'),(20,'比特犬',1,'2019-06-05 19:54:09'),(21,'柴犬',1,'2019-06-05 19:54:09'),(22,'杜高',1,'2019-06-05 19:54:09'),(23,'大麦町犬',1,'2019-06-05 19:54:09'),(24,'法国斗牛犬',1,'2019-06-05 19:54:09'),(25,'古牧',1,'2019-06-05 19:54:09'),(26,'哈士奇',1,'2019-06-05 19:54:09'),(27,'加纳利犬',1,'2019-06-05 19:54:09'),(28,'可卡',1,'2019-06-05 19:54:09'),(29,'昆明犬',1,'2019-06-05 19:54:09'),(30,'库达犬',1,'2019-06-05 19:54:10'),(31,'拉布拉多',1,'2019-06-05 19:54:10'),(32,'腊肠犬',1,'2019-06-05 19:54:10'),(33,'马犬',1,'2019-06-05 19:54:10'),(34,'萨摩耶',1,'2019-06-05 19:54:10'),(35,'松狮',1,'2019-06-05 19:54:10'),(36,'沙皮犬',1,'2019-06-05 19:54:10'),(37,'史宾格',1,'2019-06-05 19:54:10'),(38,'喜乐蒂',1,'2019-06-05 19:54:10'),(39,'英国斗牛犬',1,'2019-06-05 19:54:10'),(40,'阿拉斯加',2,'2019-06-05 19:54:10'),(41,'伯恩山犬',2,'2019-06-05 19:54:10'),(42,'德国牧羊犬',2,'2019-06-05 19:54:10'),(43,'恶霸',2,'2019-06-05 19:54:10'),(44,'高加索',2,'2019-06-05 19:54:10'),(45,'金毛',2,'2019-06-05 19:54:11'),(46,'卡斯罗',2,'2019-06-05 19:54:11'),(47,'罗威纳',2,'2019-06-05 19:54:11'),(48,'拳师犬',2,'2019-06-05 19:54:11'),(49,'苏格兰牧羊犬',2,'2019-06-05 19:54:11'),(50,'威玛犬',2,'2019-06-05 19:54:11'),(51,'藏獒',2,'2019-06-05 19:54:11'),(52,'中亚牧羊犬',2,'2019-06-05 19:54:12'),(53,'波音达犬',2,'2019-06-05 19:54:12'),(54,'杜宾',2,'2019-06-05 19:54:12'),(55,'大白熊',2,'2019-06-05 19:54:12'),(56,'巨型贵宾',2,'2019-06-05 19:54:12'),(57,'秋田',2,'2019-06-05 19:54:12'),(58,'埃及猫',3,'2019-06-05 19:54:12'),(59,'阿比西尼亚猫',3,'2019-06-05 19:54:12'),(60,'奥西猫',3,'2019-06-05 19:54:12'),(61,'布偶猫',3,'2019-06-05 19:54:12'),(62,'波斯猫',3,'2019-06-05 19:54:12'),(63,'伯曼猫',3,'2019-06-05 19:54:13'),(64,'波米拉猫',3,'2019-06-05 19:54:13'),(65,'巴厘猫',3,'2019-06-05 19:54:13'),(66,'德文卷毛猫',3,'2019-06-05 19:54:13'),(67,'东方猫',3,'2019-06-05 19:54:13'),(68,'东奇尼猫',3,'2019-06-05 19:54:13'),(69,'俄罗斯蓝猫',3,'2019-06-05 19:54:13'),(70,'哈瓦那猫',3,'2019-06-05 19:54:13'),(71,'加拿大无毛猫',3,'2019-06-05 19:54:13'),(72,'金吉拉',3,'2019-06-05 19:54:13'),(73,'卡尔特猫',3,'2019-06-05 19:54:13'),(74,'科拉特猫',3,'2019-06-05 19:54:13'),(75,'柯尼斯卷毛猫',3,'2019-06-05 19:54:14'),(76,'美国短毛猫',3,'2019-06-05 19:54:14'),(77,'孟加拉豹猫',3,'2019-06-05 19:54:14'),(78,'孟买猫',3,'2019-06-05 19:54:14'),(79,'曼基康猫',3,'2019-06-05 19:54:14'),(80,'缅因猫',3,'2019-06-05 19:54:14'),(81,'内华达猫',3,'2019-06-05 19:54:14'),(82,'欧洲缅甸猫',3,'2019-06-05 19:54:14'),(83,'日本短尾猫',3,'2019-06-05 19:54:14'),(84,'苏格兰折耳猫',3,'2019-06-05 19:54:14'),(85,'斯芬克斯猫',3,'2019-06-05 19:54:14'),(86,'山东狮子猫',3,'2019-06-05 19:54:14'),(87,'沙特尔猫',3,'2019-06-05 19:54:14'),(88,'塞尔凯克卷毛猫',3,'2019-06-05 19:54:14'),(89,'索马里猫',3,'2019-06-05 19:54:14'),(90,'土耳其梵猫',3,'2019-06-05 19:54:15'),(91,'土耳其安哥拉猫',3,'2019-06-05 19:54:15'),(92,'威尔士猫',3,'2019-06-05 19:54:15'),(93,'暹罗猫',3,'2019-06-05 19:54:15'),(94,'西伯利亚森林猫',3,'2019-06-05 19:54:15'),(95,'喜马拉雅猫',3,'2019-06-05 19:54:15'),(96,'新加坡猫',3,'2019-06-05 19:54:15'),(97,'夏特尔猫',3,'2019-06-05 19:54:15'),(98,'英国短毛猫',3,'2019-06-05 19:54:15'),(99,'异国短毛猫',3,'2019-06-05 19:54:15'),(100,'中国狸花猫',3,'2019-06-05 19:54:15'),(101,'重点色短毛猫',3,'2019-06-05 19:54:15'),(102,'中华田园猫',3,'2019-06-05 19:54:15'),(103,'蜥蜴',4,'2019-06-05 19:54:15'),(104,'金花蛇',4,'2019-06-05 19:54:16'),(105,'蚂蚁',4,'2019-06-05 19:54:16'),(106,'龙猫',4,'2019-06-05 19:54:16');

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
  `ismemberdiscount` tinyint(4) DEFAULT '0' COMMENT '是否启用会员折扣',
  `thumbnail` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '缩略图',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 COMMENT='商品信息';

/*Data for the table `product` */

insert  into `product`(`id`,`barcode`,`name`,`category`,`store`,`sales`,`warn`,`cost`,`price`,`ismemberdiscount`,`thumbnail`,`crtime`) values (1,'1','应用宝',2,-1,0,6,'1.00','2.00',1,'/images/upload/20190604022249300.jpeg','2019-05-25 14:39:19'),(2,'1','呵呵',3,4,4,3,'1.00','2.00',1,'/images/upload/20190604022306613.jpeg','2019-05-25 14:41:26'),(3,'3','泰迪专享',4,8,2,5,'1.00','4.00',1,'/images/upload/20190604022330100.jpeg','2019-05-25 16:20:49'),(4,'77','test2',4,2,0,1,'1.00','2.00',1,'/images/upload/20190604022337062.jpeg','2019-05-25 16:24:56');

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
) ENGINE=InnoDB AUTO_INCREMENT=81 DEFAULT CHARSET=utf8 COMMENT='做活动的商品';

/*Data for the table `saleproduct` */

insert  into `saleproduct`(`id`,`type`,`ptype`,`saleid`,`productid`,`crtime`) values (3,0,1,12,9,'2019-06-02 13:22:34'),(74,1,0,4,3,'2019-06-04 12:21:41'),(79,0,0,23,3,'2019-06-04 14:17:57'),(80,0,0,23,4,'2019-06-04 14:17:57');

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
) ENGINE=InnoDB AUTO_INCREMENT=79 DEFAULT CHARSET=utf8 COMMENT='活动规则';

/*Data for the table `salerule` */

insert  into `salerule`(`id`,`saleid`,`type`,`aim`,`sale`,`crtime`) values (68,4,0,'10.00',2,'2019-06-04 12:21:41'),(69,4,0,'5.00',1,'2019-06-04 12:21:41'),(76,25,0,'1.00',6,'2019-06-04 14:14:37'),(77,23,0,'2.00',9,'2019-06-04 14:17:57'),(78,23,0,'3.00',8,'2019-06-04 14:17:57');

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
