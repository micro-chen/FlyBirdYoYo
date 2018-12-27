# FlyBirdYoYo
FlyBirdYoYo 是一个快速开发Web项目的整体框架 ,基于Asp.net Core 2.2 .
开发效率 简直不能再高了，代码一键生成。
# 项目总览
使用asp.net core 2.2 开发跨平台的Web项目，越来越成熟。得益于微软的.net core 开源技术。asp.net core 的社区正在迅速发展。
本项目目前使用主要框架 asp.net core webapi 
# 依赖技术：
Ioc：.net core 内置的依赖倒置；         
AutoMapper:用来实体跟业务DTO的类型快速转换；       
缓存支持：MemoryCache  +Redis ，使用配置策略 ，滑动切换缓存组件；       
Dapper：用来实现数据交互的核心；       
EnityFrameWork Lite: 用来进行自动SQL 生成。将Lambda 表达式转为sql  ，大幅度提高开发效率。减少DAO层的开发 和频繁更改；      
T4  开发模板: 使用T4模板， 快速搭建单表的 业务交互。实现Db First 的快速代码生成。使开发更加专注于业务层的开发。模板自身包含基础的CURD 和带条件的CURD
实现代码野蛮式开发而不失质量。       
单元测试：单元测试提供对项目全身进行测试，TDD开发，保证质量；       
数据库支持：Mysql(推荐)、Sqlserver，并预留其他数据库的扩展；      
Log4Net: 使用Log4net 记录日志，输出日志到文件和控制台，便于日志跟踪。       
ORM 自定义组件：基于 Dapper做核心，动态解析Lambda表达式，并可以配置输出查询sql 到日志；      

# 本项目亮点
1 使用asp.net core 进行跨平台开发，性能大幅度提升       
2 使用T4 模式的代码一键生成 增删改差，带条件的增删改差，自带分页，大幅度提升开发效率。      
3 项目组件核心代码，全部开源。上手简单  

# 开发工具
Visual Studio 2017      
代码生成器（T4）--基于动软代码生成器的模板：\FlyBirdYoYo\FlyBirdPrint.DbManage\CodeGenTempletes      

# 如何运行
下载安装.net core 2.2 SDK: https://dotnet.microsoft.com/download
克隆本项目到本地，然后 Visual Studio 2017 打开，F5  ->http://localhost:8003       
Congratulations!      

# 联系作者 
邮箱：1021776019@qq.com      
# 赞助作者
一个好的项目离不开大家的支持，您的赞助，将给我更加充沛的动力。
<br/>
<br/>
<img src="https://images2018.cnblogs.com/blog/371989/201805/371989-20180514183954632-2054296110.jpg" alt="" />
