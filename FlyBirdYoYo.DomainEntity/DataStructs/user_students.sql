/*
Navicat MySQL Data Transfer

Source Server         : 127.0.0.1
Source Server Version : 50720
Source Host           : 127.0.0.1:3306
Source Database       : demodb

Target Server Type    : MYSQL
Target Server Version : 50720
File Encoding         : 65001

Date: 2018-09-17 14:53:29
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for user_students
-- ----------------------------
DROP TABLE IF EXISTS `user_students`;
CREATE TABLE `user_students` (
  `id` bigint(11) NOT NULL AUTO_INCREMENT,
  `age` int(11) DEFAULT NULL,
  `name` varchar(255) DEFAULT NULL,
  `sex` tinyint(1) DEFAULT NULL,
  `add_time` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `score` decimal(7,2) DEFAULT NULL,
  `longitude` double(15,6) DEFAULT NULL,
  `latitude` double(15,6) DEFAULT NULL,
  `has_pay` float DEFAULT NULL,
  `home_number` smallint(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `id-noclustered` (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=103 DEFAULT CHARSET=utf8;
