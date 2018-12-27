/*
Navicat MySQL Data Transfer

Source Server         : 127.0.0.1
Source Server Version : 50720
Source Host           : 127.0.0.1:3306
Source Database       : demodb

Target Server Type    : MYSQL
Target Server Version : 50720
File Encoding         : 65001

Date: 2018-12-27 11:40:52
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for sys_admin
-- ----------------------------
DROP TABLE IF EXISTS `sys_admin`;
CREATE TABLE `sys_admin` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '主键Id',
  `uname` varchar(255) NOT NULL COMMENT '账号名',
  `upassword` varchar(50) NOT NULL COMMENT '登录密码',
  `role_id` bigint(20) DEFAULT '0' COMMENT '角色id (以后预留备用)',
  `public_key` varchar(1500) DEFAULT NULL COMMENT '公钥',
  `state` tinyint(1) NOT NULL DEFAULT '1' COMMENT '账号状态：0 禁用  1正常 ',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of sys_admin
-- ----------------------------
INSERT INTO `sys_admin` VALUES ('1', 'admin', 'U1A3VZX/iaAlJhsEJ35+Ww==', '0', 'JWVCTWT4FRMHI2ZG', '1');
