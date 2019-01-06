/*
SQLyog Ultimate v12.08 (64 bit)
MySQL - 6.0.11-alpha-community : Database - sunny
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
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(50) NOT NULL COMMENT '名称',
  `parent` INT(11) NOT NULL DEFAULT '0' COMMENT '父级id',
  `type` TINYINT(4) NOT NULL DEFAULT '0' COMMENT '类型0课程 1通用商品',
  `state` TINYINT(4) NOT NULL DEFAULT '0' COMMENT '状态0正常 1禁用',
  `crtime` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=INNODB DEFAULT CHARSET=latin1 COMMENT='商品分类';

/*Table structure for table `deliver` */

DROP TABLE IF EXISTS `deliver`;

CREATE TABLE `deliver` (
  `id` VARCHAR(10) NOT NULL,
  `name` VARCHAR(10) DEFAULT NULL,
  `state` TINYINT(4) DEFAULT '0' COMMENT '0正常 1禁用',
  PRIMARY KEY (`id`)
) ENGINE=INNODB DEFAULT CHARSET=latin1 COMMENT='配送方式';

/*Table structure for table `discount` */

DROP TABLE IF EXISTS `discount`;

CREATE TABLE `discount` (
  `id` INT(11) NOT NULL,
  `name` VARCHAR(50) NOT NULL COMMENT '折扣名称',
  `summary` VARCHAR(200) DEFAULT NULL COMMENT '简介',
  `money` DECIMAL(10,2) NOT NULL DEFAULT '0' COMMENT '立减金额',
  `state` TINYINT(4) DEFAULT '0' COMMENT '状态0正常 1禁用',
  `crtime` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=INNODB DEFAULT CHARSET=latin1 COMMENT='折扣基本信息';

/*Table structure for table `order` */

DROP TABLE IF EXISTS `order`;

CREATE TABLE `order` (
  `order_id` VARCHAR(64) NOT NULL COMMENT '订单号',
  `money` DECIMAL(10,2) DEFAULT '0' COMMENT '实际支付金额',
  `descount_money` DECIMAL(10,2) DEFAULT '0' COMMENT '商品折扣金额',
  `coupon_money` DECIMAL(10,2) DEFAULT '0' COMMENT '优惠券抵扣金额',
  `receiver` VARCHAR(20) DEFAULT NULL COMMENT '收货人',
  `phone` VARCHAR(11) DEFAULT NULL COMMENT '电话号码',
  `address` VARCHAR(200) DEFAULT NULL COMMENT '收货地址',
  `message` VARCHAR(200) DEFAULT NULL COMMENT '买家留言',
  `deliver_id` INT(11) DEFAULT NULL COMMENT '配送方式',
  `freight` DECIMAL(10,2) DEFAULT '0' COMMENT '运费',
  `state` TINYINT(4) DEFAULT '0' COMMENT '状态0未支付1已支付2已发货3已收货',
  `crdate` DATE DEFAULT NULL COMMENT '日期',
  `crtime` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '时间',
  PRIMARY KEY (`order_id`)
) ENGINE=INNODB DEFAULT CHARSET=latin1 COMMENT='购物订单信息';

/*Table structure for table `order_coupon` */

DROP TABLE IF EXISTS `order_coupon`;

CREATE TABLE `order_coupon` (
  `order_id` INT(11) NOT NULL COMMENT '订单id',
  `product_id` INT(11) NOT NULL COMMENT '商品id',
  `coupon_id` INT(11) NOT NULL COMMENT '优惠券id',
  `name` VARCHAR(50) NOT NULL COMMENT '优惠券名称',
  `price` DECIMAL(10,2) NOT NULL DEFAULT '0' COMMENT '优惠券金额',
  `count` INT(11) NOT NULL COMMENT '优惠券数量',
  `money` DECIMAL(10,2) NOT NULL DEFAULT '0' COMMENT '优惠总金额',
  PRIMARY KEY (`order_id`,`product_id`,`coupon_id`)
) ENGINE=INNODB DEFAULT CHARSET=latin1 COMMENT='订单优惠券信息';

/*Table structure for table `order_discount` */

DROP TABLE IF EXISTS `order_discount`;

CREATE TABLE `order_discount` (
  `order_id` INT(11) NOT NULL COMMENT '订单id',
  `product_id` INT(11) NOT NULL COMMENT '商品id',
  `discount_id` INT(11) NOT NULL COMMENT '折扣id',
  `name` VARCHAR(50) NOT NULL COMMENT '折扣名称',
  `money` DECIMAL(10,2) NOT NULL DEFAULT '0' COMMENT '立减金额',
  PRIMARY KEY (`order_id`,`product_id`,`discount_id`)
) ENGINE=INNODB DEFAULT CHARSET=latin1 COMMENT='订单折扣信息';

/*Table structure for table `order_product` */

DROP TABLE IF EXISTS `order_product`;

CREATE TABLE `order_product` (
  `order_id` VARCHAR(64) NOT NULL COMMENT '订单编号',
  `product_id` INT(11) NOT NULL COMMENT '商品id',
  `product_name` VARCHAR(100) DEFAULT NULL COMMENT '商品名称',
  `count` INT(11) DEFAULT '0' COMMENT '数量',
  `price` DECIMAL(10,2) DEFAULT '0' COMMENT '单价',
  `total_amount` DECIMAL(10,2) DEFAULT '0' COMMENT '总金额',
  `discount_amount` DECIMAL(10,2) DEFAULT '0' COMMENT '折扣金额',
  `coupon_amount` DECIMAL(10,2) DEFAULT '0' COMMENT '优惠券抵扣金额',
  PRIMARY KEY (`order_id`,`product_id`)
) ENGINE=INNODB DEFAULT CHARSET=latin1 COMMENT='订单中的商品信息';

/*Table structure for table `order_product_specification_detail` */

DROP TABLE IF EXISTS `order_product_specification_detail`;

CREATE TABLE `order_product_specification_detail` (
  `order_id` VARCHAR(64) NOT NULL COMMENT '订单id',
  `product_id` INT(11) NOT NULL DEFAULT '0' COMMENT '商品id',
  `specification_detail_id` INT(11) NOT NULL DEFAULT '0' COMMENT '规格详情id',
  `price` DECIMAL(10,2) DEFAULT '0' COMMENT '价格',
  PRIMARY KEY (`order_id`,`product_id`,`specification_detail_id`)
) ENGINE=INNODB DEFAULT CHARSET=latin1 COMMENT='订单商品的规格与价格表';

/*Table structure for table `product` */

DROP TABLE IF EXISTS `product`;

CREATE TABLE `product` (
  `id` INT(11) NOT NULL AUTO_INCREMENT COMMENT '自增主键',
  `name` VARCHAR(100) NOT NULL COMMENT '商品名称',
  `code` VARCHAR(20) NOT NULL COMMENT '商品编码',
  `category_id` INT(11) NOT NULL DEFAULT '0' COMMENT '类别',
  `summary` VARCHAR(200) DEFAULT NULL COMMENT '简介',
  `detail_id` INT(11) DEFAULT '0' COMMENT '详细描述',
  `stock` INT(11) DEFAULT '9999' COMMENT '库存数量',
  `min_buy_count` INT(4) DEFAULT '1' COMMENT '单次最少购买量',
  `max_buy_count` INT(4) DEFAULT '9999' COMMENT '单次最大购买量',
  `state` TINYINT(4) DEFAULT '0' COMMENT '状态0正常 1禁用',
  `crtime` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=INNODB DEFAULT CHARSET=latin1 COMMENT='商品信息';

/*Table structure for table `product_detail` */

DROP TABLE IF EXISTS `product_detail`;

CREATE TABLE `product_detail` (
  `product_id` INT(11) NOT NULL,
  `detail` VARCHAR(5000) DEFAULT NULL COMMENT '商品详情',
  PRIMARY KEY (`product_id`)
) ENGINE=INNODB DEFAULT CHARSET=latin1 COMMENT='商品详细描述';

/*Table structure for table `product_discount` */

DROP TABLE IF EXISTS `product_discount`;

CREATE TABLE `product_discount` (
  `id` INT(11) NOT NULL,
  `product_id` INT(11) NOT NULL COMMENT '商品id',
  `discount_id` INT(11) NOT NULL COMMENT '折扣信息id',
  `start_time` DATETIME DEFAULT NULL COMMENT '折扣开始时间',
  `end_time` DATETIME DEFAULT NULL COMMENT '折扣结束时间',
  `state` TINYINT(4) DEFAULT '0' COMMENT '状态0正常 1禁用',
  `crttime` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=INNODB DEFAULT CHARSET=latin1 COMMENT='商品折扣信息';

/*Table structure for table `product_specification_detail` */

DROP TABLE IF EXISTS `product_specification_detail`;

CREATE TABLE `product_specification_detail` (
  `product_id` INT(11) NOT NULL COMMENT '商品id',
  `specification_detail_id` INT(11) NOT NULL COMMENT '规格详情id',
  `price` DECIMAL(10,2) DEFAULT '0' COMMENT '价格',
  `state` TINYINT(4) DEFAULT '0' COMMENT '0正常 1禁用',
  `crtime` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`product_id`,`specification_detail_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='商品各规格与价格表';

/*Table structure for table `receiver_address` */

DROP TABLE IF EXISTS `receiver_address`;

CREATE TABLE `receiver_address` (
  `id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL COMMENT '用户id',
  `phone` varchar(11) DEFAULT NULL COMMENT '收货电话',
  `address` varchar(200) NOT NULL COMMENT '收货地址',
  `crtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='收货地址';

/*Table structure for table `specification` */

DROP TABLE IF EXISTS `specification`;

CREATE TABLE `specification` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL COMMENT '名称',
  `summary` varchar(500) DEFAULT NULL COMMENT '简介',
  `state` tinyint(4) DEFAULT '0' COMMENT '0正常 1禁用',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='规格分类';

/*Table structure for table `specification_detail` */

DROP TABLE IF EXISTS `specification_detail`;

CREATE TABLE `specification_detail` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `specification_id` int(11) NOT NULL COMMENT '规格类型id',
  `name` varchar(50) NOT NULL COMMENT '名称',
  `summary` varchar(500) DEFAULT NULL COMMENT '描述',
  `state` tinyint(4) DEFAULT '0' COMMENT '0正常 1禁用',
  `crtime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='规格详情';

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
