using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using FlyBirdYoYo.DbManage;
using FlyBirdYoYo.Utilities.TypeFinder;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Transactions;
using FlyBirdYoYo.Utilities.Logging;

namespace MySql.Data.MySqlClient
{
    /// <summary>
    /// 基于mysql 的批量插入类
    /// 核心是 mysql 的load data LOAD DATA INFILE Syntax
    /// 使用mysql ado.net 的MySqlBulkLoader 实现
    /// </summary>
    public class MysqlBuckCopy<TElement> : IDisposable where TElement : BaseEntity, new()
    {

        /// <summary>
        /// 默认超时时间
        /// </summary>
        private const int DEFAUT_TIME_OUT = 60;


        /// <summary>
        /// 数据库连接字符串
        /// 方便调试查看，对程序实例的Trace 跟踪
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 写入的目标表
        /// </summary>
        public string TableName { get; set; }


        /// <summary>
        /// 超时时间 （秒）
        /// </summary>
        public int TimeOut
        {
            get; set;
        }


        private string _TempDataDir;
        /// <summary>
        /// 临时数据文件目录
        /// </summary>
        public string TempDataDir
        {
            get
            {
                if (string.IsNullOrEmpty(_TempDataDir))
                {
                    _TempDataDir = Path.Combine(AppDomainTypeFinder.GetBinDirectory(), "temp");
                }
                if (!Directory.Exists(_TempDataDir))
                {
                    Directory.CreateDirectory(_TempDataDir);
                }
                return _TempDataDir;
            }
        }


        #region 构造函数

        /// <summary>
        /// 构造MysqlBuckCopy 实例
        /// 制定特定的数据库连接，表
        /// </summary>
        /// <param name="_connStr"></param>
        /// <param name="_timeOut"></param>
        public MysqlBuckCopy(string _connStr, int _timeOut = DEFAUT_TIME_OUT)
        {
            this.ConnectionString = _connStr;
            this.TimeOut = _timeOut;
        }



        /// <summary>
        ///  将列表数据批量写入数据库
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="tableName"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public bool WriteToServer(IEnumerable<TElement> dataList, string tableName, IDbTransaction transaction = null)
        {
            bool result = false;
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException("the tablename must not be null or empty! ");
            }

            this.TableName = tableName;

            try
            {


                //1 将数据写到临时文本文件
                // 2导入数据文件到Db
                //3 移除临时文件

                string dataFile = this.CreateCSVFile(ref dataList, tableName);

                //等待返回异步任务结果
                var operateResult = this.ImportDataFromFile(dataFile, transaction);
                if (operateResult > 0)
                {
                    result = true;
                }
                //导入完毕后 移除文件
                File.Delete(dataFile);

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;
        }

        /// <summary>
        /// 将文本数据 导入到库
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        private int ImportDataFromFile(string filePath, IDbTransaction transaction = null)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(string.Concat("the data file can not be found at ", filePath));
            }


            using (MySqlConnection conn = new MySqlConnection(this.ConnectionString))
            {
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }

                IDbTransaction dbTran = transaction;

                bool isInnerTran = false;

                //没有传递事务，且不再事务环境上下文中
                if (null == dbTran && null == Transaction.Current)
                {
                    dbTran = conn.BeginTransaction();
                    isInnerTran = true;
                }
                try
                {
                    //step 1 load data to db
                    MySqlBulkLoader bcp = new MySqlBulkLoader(conn);
                    bcp.TableName = this.TableName;
                    bcp.Timeout = this.TimeOut;
                    bcp.FieldTerminator = ",";
                    bcp.FieldQuotationCharacter = '"';
                    bcp.EscapeCharacter = '"';
                    bcp.LineTerminator = "\r\n";
                    bcp.FileName = filePath;
                    bcp.NumberOfLinesToSkip = 0;

                    int result = bcp.Load();


                    //批量插入模式的 自增id不连续 这是mysql 的设计  预留不确定的自增数  反正自增不连续不影响业务
                    #region 资料


                    /*
                     http://www.cnblogs.com/zhoujinyi/p/3433823.html
                    0：通过表锁的方式进行，也就是所有类型的insert都用AUTO-inc locking。
                    1：默认值，对于simple insert 自增长值的产生使用互斥量对内存中的计数器进行累加操作，对于bulk insert 则还是使用表锁的方式进行。
                    2：对所有的insert-like 自增长值的产生使用互斥量机制完成，性能最高，并发插入可能导致自增值不连续，可能会导致Statement 的 Replication 出现不一致，使用该模式，需要用 Row Replication的模式
                     
                    ALTER TABLE students  AUTO_INCREMENT = 11; 

                    show variables like 'innodb_autoinc_lock_mode';

                     SELECT Auto_increment
                    FROM information_schema.`TABLES`
                    WHERE Table_Schema='demodb'
                    AND table_name = 'students'
                     */

                    #endregion
                    if (isInnerTran == true)
                    {
                        //内部事务，完毕后提交
                        dbTran.Commit();
                    }

                    return result;

                }
                catch (Exception ex)
                {
                    if (null != dbTran)
                    {
                        dbTran.Rollback();
                    }
                    throw ex;
                }
                finally
                {
                    if (null != dbTran)
                    {
                        dbTran.Dispose();
                    }

                    //异步删除文件:
                    Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            File.Delete(filePath);
                        }
                        catch(Exception ex)
                        {
                            Logger.Error($"执行批量插入后，移除临时文件出粗！文件路径：{filePath}，错误信息：{ex.ToString()}");
                        }

                    });

                }


            }
        }



        /// <summary>
        /// 将数据列表写入到文件 并返回文件全路径
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="dataList"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private string CreateCSVFile(ref IEnumerable<TElement> dataList, string tableName)
        {
            //生成一个随机码 防止guid出现重复
            string fileToken = StringExtension.GenerateRandomSequence(4);
            string fileName = string.Format("{0}-{1}-{2}.csv", tableName, Guid.NewGuid(), fileToken);
            string strFilePath = Path.Combine(this.TempDataDir, fileName);

            //If file does not exist then create it and right data into it..
            if (!File.Exists(strFilePath))
            {
                FileStream fs = new FileStream(strFilePath, FileMode.Create, FileAccess.Write);
                fs.Close();
                fs.Dispose();
            }
            using (StreamWriter sw = new StreamWriter(strFilePath, false))
            {



                //获取该实体的属性集合
                var entity = new TElement();
                var propertys = entity.GetCurrentEntityProperties();
                int cellCount = propertys.Length;
                foreach (var item in dataList)
                {
                    var dicValuePairs = item.GetSettedValuePropertyDic();//获取此实体设置过值的键值对

                    //以半角逗号（即,）作分隔符，列为空也要表达其存在。  
                    //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。  
                    //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。

                    for (int i = 0; i < propertys.Length; i++)
                    {
                        //对每一个cell 进行数据写入
                        var propty = propertys[i];


                        object cellValue = null;
                        dicValuePairs.TryGetValue(propty.Name, out cellValue);
                        if (null != cellValue)
                        {
                            string cellContent = cellValue.ToString();
                            if (cellContent.Contains(","))
                            {
                                cellContent = string.Concat("\"", cellContent.Replace("\"", "\"\""), "\"");
                            }

                            sw.Write(cellContent);
                        }
                        else
                        {
                            sw.Write(@"NULL");//为空的列  此版本的mysql ado.net  不支持 \N 字符的导入，请使用关键词 NULL 写入空列
                        }

                        if (i < cellCount - 1)
                        {
                            sw.Write(",");//写入站位符号
                        }
                    }
                    //写完一行后 追加换行
                    sw.Write(sw.NewLine);
                }

            }

            return strFilePath;
        }

        #endregion


        #region Disposable



        //是否回收完毕
        bool _disposed;
        public void Dispose()
        {

            //置空连接字符串 并且释放连接资源
            this.ConnectionString = null;
            Dispose(true);
            // This class has no unmanaged resources but it is possible that somebody could add some in a subclass.
            GC.SuppressFinalize(this);

        }
        //这里的参数表示示是否需要释放那些实现IDisposable接口的托管对象
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return; //如果已经被回收，就中断执行
            if (disposing)
            {
                //TODO:释放那些实现IDisposable接口的托管对象

            }
            //TODO:释放非托管资源，设置对象为null
            _disposed = true;
        }



        #endregion


    }
}
