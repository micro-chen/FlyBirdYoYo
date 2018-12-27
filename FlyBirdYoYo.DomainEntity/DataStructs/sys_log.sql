/*
Navicat MySQL Data Transfer

Source Server         : 127.0.0.1
Source Server Version : 50720
Source Host           : 127.0.0.1:3306
Source Database       : demodb

Target Server Type    : MYSQL
Target Server Version : 50720
File Encoding         : 65001

Date: 2018-12-27 11:41:01
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for sys_log
-- ----------------------------
DROP TABLE IF EXISTS `sys_log`;
CREATE TABLE `sys_log` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',
  `sys_user_id` bigint(20) NOT NULL COMMENT '超管账号Id。系统日志则为0',
  `log_type` int(5) DEFAULT NULL COMMENT '日志类型：1 登录 2  进入商铺 3 其他日志',
  `level` int(5) DEFAULT NULL COMMENT '日志级别',
  `log_content` varchar(500) DEFAULT NULL COMMENT '日志内容',
  `ip_address` varchar(36) DEFAULT NULL COMMENT '请求的Ip地址',
  `create_time` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of sys_log
-- ----------------------------
INSERT INTO `sys_log` VALUES ('1', '5', '1', '2', '超管账号：wali , 登录系统！', '::ffff:127.0.0.1', '2018-12-23 01:23:06');
INSERT INTO `sys_log` VALUES ('2', '5', '1', '2', '超管账号：wali , 登录系统！', '::ffff:127.0.0.1', '2018-12-23 01:26:20');
INSERT INTO `sys_log` VALUES ('3', '5', '1', '2', '超管账号：wali , 登录系统！', '::ffff:127.0.0.1', '2018-12-23 14:16:58');
INSERT INTO `sys_log` VALUES ('4', '5', '1', '2', '超管账号：wali , 登录系统！', '::ffff:127.0.0.1', '2018-12-23 14:17:43');
INSERT INTO `sys_log` VALUES ('5', '5', '1', '2', '超管账号：wali , 登录系统！', '::ffff:127.0.0.1', '2018-12-23 14:19:04');
INSERT INTO `sys_log` VALUES ('6', '5', '1', '2', '超管账号：wali , 登录系统！', '::ffff:127.0.0.1', '2018-12-23 14:20:33');
INSERT INTO `sys_log` VALUES ('7', '5', '1', '2', '超管账号：wali , 登录系统！', '::ffff:127.0.0.1', '2018-12-23 14:21:22');
INSERT INTO `sys_log` VALUES ('8', '5', '1', '2', '超管账号：wali , 登录系统！', '::ffff:127.0.0.1', '2018-12-23 14:22:46');
INSERT INTO `sys_log` VALUES ('9', '5', '1', '2', '超管账号：wali , 登录系统！', '::ffff:127.0.0.1', '2018-12-23 14:25:16');
INSERT INTO `sys_log` VALUES ('10', '5', '1', '2', '超管账号：wali , 登录系统！', '::ffff:127.0.0.1', '2018-12-23 19:01:11');
INSERT INTO `sys_log` VALUES ('11', '5', '1', '2', '超管账号：wali , 登录系统！', '::ffff:127.0.0.1', '2018-12-23 19:12:12');
INSERT INTO `sys_log` VALUES ('12', '5', '1', '2', '超管账号：wali , 登录系统！', '::ffff:127.0.0.1', '2018-12-23 22:43:13');
INSERT INTO `sys_log` VALUES ('13', '5', '1', '2', '超管账号：wali , 登录系统！', '::ffff:127.0.0.1', '2018-12-23 22:45:04');
