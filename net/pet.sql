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

/*Table structure for table `financial_flow` */

DROP TABLE IF EXISTS `financial_flow`;

CREATE TABLE `financial_flow` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `type` tinyint(4) NOT NULL DEFAULT '0' COMMENT '0无效 -1支出 1收入',
  `money` decimal(10,2) NOT NULL DEFAULT '0.00' COMMENT '金额（元）',
  `date` date NOT NULL COMMENT '日期',
  `remark` varchar(200) DEFAULT NULL COMMENT '备注',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`,`type`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8 COMMENT='资金流水';

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
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8 COMMENT='满就送';

/*Table structure for table `member` */

DROP TABLE IF EXISTS `member`;

CREATE TABLE `member` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '姓名',
  `py` varchar(50) DEFAULT NULL COMMENT '姓名简拼',
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

/*Table structure for table `memberpet` */

DROP TABLE IF EXISTS `memberpet`;

CREATE TABLE `memberpet` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `memberid` int(11) NOT NULL COMMENT '会员id',
  `petid` int(11) NOT NULL COMMENT 'pet类型id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8 COMMENT='会员喜欢的宠物类型';

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

/*Table structure for table `product` */

DROP TABLE IF EXISTS `product`;

CREATE TABLE `product` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `barcode` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '条码',
  `name` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '名称',
  `py` varchar(100) DEFAULT NULL COMMENT '简拼',
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
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8 COMMENT='商品信息';

/*Table structure for table `railcard` */

DROP TABLE IF EXISTS `railcard`;

CREATE TABLE `railcard` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `phone` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '手机号',
  `name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '主人姓名',
  `petname` varchar(50) DEFAULT NULL COMMENT '宠物名字',
  `py` varchar(50) DEFAULT NULL COMMENT '宠物名字简拼',
  `petage` tinyint(4) DEFAULT '0' COMMENT '宠物年龄（岁）',
  `pettype` varchar(20) DEFAULT NULL COMMENT '宠物类型',
  `times` int(11) DEFAULT '0' COMMENT '最大可使用次数',
  `lefttimes` int(11) DEFAULT '0' COMMENT '剩余使用次数',
  `money` decimal(20,2) DEFAULT '0.00' COMMENT '最大可使用金额',
  `payType` tinyint(4) NOT NULL DEFAULT '0' COMMENT '支付方式 1现金 2微信 3支付宝 4余额 5刷卡 6其他',
  `starttime` date DEFAULT NULL COMMENT '开始时间',
  `endtime` date DEFAULT NULL COMMENT '结束时间',
  `remark` varchar(200) DEFAULT NULL COMMENT '备注',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8 COMMENT='优惠卡';

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

/*Table structure for table `recharge` */

DROP TABLE IF EXISTS `recharge`;

CREATE TABLE `recharge` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `sno` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '流水号',
  `memberid` int(11) NOT NULL DEFAULT '0' COMMENT '会员id',
  `money` decimal(20,2) NOT NULL DEFAULT '0.00' COMMENT '充值金额',
  `paymoney` decimal(20,2) NOT NULL DEFAULT '0.00' COMMENT '实际支付金额',
  `payType` tinyint(4) NOT NULL DEFAULT '0' COMMENT '支付方式 1现金 2微信 3支付宝 4余额 5刷卡 6其他',
  `balance` decimal(20,2) NOT NULL DEFAULT '0.00' COMMENT '当前余额',
  `remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '备注',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `IX_SNO` (`sno`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8 COMMENT='充值记录';

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
) ENGINE=InnoDB AUTO_INCREMENT=97 DEFAULT CHARSET=utf8 COMMENT='做活动的商品';

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
) ENGINE=InnoDB AUTO_INCREMENT=108 DEFAULT CHARSET=utf8 COMMENT='活动规则';

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

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
