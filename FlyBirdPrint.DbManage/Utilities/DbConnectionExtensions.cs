using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Linq;
using System.Threading.Tasks;


using FlyBirdYoYo.DbManage;
using FlyBirdYoYo.Utilities.Logging;

namespace System.Data
{
    public static class DbConnectionExtensions
    {
        
        private static Dictionary<string, bool> kv_RegisterIsDataBaseNeedLog = new Dictionary<string, bool>();

        /// <summary>
        /// 打出sql命令参数
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sqlCmd"></param>
        /// <param name="sqlParas"></param>
        /// <param name="forceOutPut">强制输入sql日志，默认：false</param>
        public static void SqlOutPutToLogAsync(this IDbConnection conn,string sqlCmd, object sqlParas = null,bool forceOutPut=false)
        {
            string dbName = conn.Database;
          
            if (true==forceOutPut)
            {
                //直接输入日志
                SqlOutPutToLogAsync(sqlCmd, sqlParas);
                return;
            }

            //-----------如果不是强制输出，那么检索配置是否输出-------
            bool isNeedOutLog = false;
            if (kv_RegisterIsDataBaseNeedLog.ContainsKey(dbName))
            {
                isNeedOutLog = kv_RegisterIsDataBaseNeedLog[dbName];
            }
            else
            {
                //从全部的配置中加载出dbConfig
                if (GlobalDBConnection.AllDbConnConfigs.IsNotEmpty())
                {
                    var dbConfig = GlobalDBConnection.AllDbConnConfigs.Values.FirstOrDefault(x => x.Database == dbName);
                    if (null != dbConfig)
                    {
                        isNeedOutLog = dbConfig.SqlOutPut;
                        kv_RegisterIsDataBaseNeedLog.Add(dbName, isNeedOutLog);
                    }
                }
            }
          


            //----------检索完毕后，再次确认是否需要输出日志-----------
            if (isNeedOutLog==true)
            {
                SqlOutPutToLogAsync(sqlCmd, sqlParas);
            }
            
        }


        /// <summary>
        /// 打出sql命令参数
        /// </summary>
        /// <param name="sqlCmd"></param>
        /// <param name="sqlParas"></param>
        internal static void SqlOutPutToLogAsync(string sqlCmd, object sqlParas = null)
        {
          
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();
                var logModel = new SqlLogModel
                {
                    SqlCmd = sqlCmd,
                    SqlParas = sqlParas,
                    CallingFrames = trace.GetFrames()
                };


                Task.Factory.StartNew((state) =>
                {

                    SqlLogModel paras = state as SqlLogModel;

                    var frames = paras.CallingFrames;

                    StringBuilder sb_callFrame = new StringBuilder();
                    for (int u = 1; u < frames.Length; ++u)
                    {
                        System.Reflection.MethodBase mb = frames[u].GetMethod();
                        if (mb.DeclaringType == null)
                        {
                            continue;
                        }

                        sb_callFrame.AppendLine(string.Format("[CALL STACK][{0}]: {1}.{2}", u, mb.DeclaringType.FullName, mb.Name));

                        if (mb.DeclaringType.IsSubclassOf(FlyBirdYoYo.Utilities.Contanst.CallingEntryPointBasetype))
                        {
                            break;//跳过到mvc action 即可
                        }
                    }

                    var sb = new StringBuilder();
                    sb.AppendFormat("------------------sql excute begin at : --------------------");
                    sb.AppendLine(sb_callFrame.ToString());
                    sb.AppendLine("sqlCmd : " + paras.SqlCmd);
                    if (null != sqlParas)
                    {
                        sb.AppendLine("sqlParas : " + paras.SqlParas.ToJson());

                    }
                    sb.AppendFormat("------------------sql excute end  --------------------");


                    Logger.Info(sb.ToString());

                }, logModel);
            
        }
    }
}
