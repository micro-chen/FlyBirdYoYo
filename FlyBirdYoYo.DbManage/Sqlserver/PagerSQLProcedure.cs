
using System;
using System.Data;
using System.Data.SqlClient;
using FlyBirdYoYo.Utilities;
namespace FlyBirdYoYo.DbManage
{

    /// <summary>
    /// 检测是否存在需要的分页存储过程
    /// </summary>
    public static class PagerSQLProcedure
    {



        private static object _locker = new object();
     
        private static bool IsExistSQLProcedure(DbConnConfig dbConfig, string procName)
        {
            var conStr = dbConfig.ConnString;
            if (string.IsNullOrEmpty(conStr))
            {
                throw new Exception("全局数据库连接未设定！");
            }

            //从数据库中检索  是否存在此存储过程
            var sql = string.Format("SELECT COUNT(0) FROM sysobjects a WHERE a.name='{0}' AND a.type='P'", procName);

            using (var conn = new SqlConnection(conStr))
            {
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }

                using (var cmd = new SqlCommand(sql, conn))
                {
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        private static void CreateSQLProcedure(DbConnConfig dbConfig, string sqlCommand)
        {
            var conStr = dbConfig.ConnString;
            if (string.IsNullOrEmpty(conStr))
            {
                throw new Exception("全局数据库连接未设定！");
            }



            using (var conn = new SqlConnection(conStr))
            {
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }

                using (var cmd = new SqlCommand(sqlCommand, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }



 
        /// <summary>
        /// 通用mssql-分页脚本
        /// </summary>
        public static readonly string PAGE_SQL_CORE = @"
DECLARE @TotalRecords int = 0;
DECLARE @TotalPageCount int =0;--输出总页数
DECLARE  @ResetOrder bit -- 1表示读取数据的时候 排序要反过来
DECLARE @SQLString  NVARCHAR(MAX);
DECLARE @ParamDef NVARCHAR(800)


        BEGIN
                    SET NOCOUNT ON;

-----------------第一步 ：查询限制条件的组合
                                    DECLARE @WhereString1 NVARCHAR(MAX);
        DECLARE @WhereString2 NVARCHAR(MAX);
        IF @ConditionWhere IS NULL OR @ConditionWhere = N'' BEGIN
            SELECT @WhereString1 = N'';
		                                SELECT @WhereString2 = N'WHERE ';
        END
        ELSE BEGIN
            SELECT @WhereString1 = N'WHERE ' + @ConditionWhere;
		                                SELECT @WhereString2 = N'WHERE ' + @ConditionWhere + N' AND ';
	                                END
	                                
	                                -----------设置完毕查询条件后 查询本次符合条件的记录数 页数
                                 DECLARE @sqlCmd NVARCHAR(MAX)


                                   IF @ConditionWhere IS NULL OR @ConditionWhere = N'' 

                                    BEGIN
		                                --没有查询条件

                                        SET @sqlCmd='SELECT @TotalRecords = COUNT(*)  FROM '+ @TableNameOrSqlCmd
                                         SET @ParamDef=N'@TotalRecords int output ' + {2}
                                           exec sp_executesql @sqlCmd,@ParamDef, @TotalRecords output {3}
                                      END

                                    ELSE BEGIN

                                      SET @sqlCmd = 'SELECT @TotalRecords = COUNT(*)  FROM ' + @TableNameOrSqlCmd + ' ' + @WhereString1
                                      SET @ParamDef=N'@TotalRecords int output ' + {2}
                                           exec sp_executesql @sqlCmd,@ParamDef,@TotalRecords output {3}

                                    END
	                                ---------总记录数有值的时候
                                           IF(@TotalRecords>0)

                                        BEGIN
                                            DECLARE @modNum INT --求模运算
                                            SET @modNum=@TotalRecords%@PageSize
                                             IF @modNum=0--整除尽
                                                SET @TotalPageCount=@TotalRecords/@PageSize
                                            ELSE--有余数
                                                SET @TotalPageCount=@TotalRecords/@PageSize+1
		                                	
		                                END	
------------开始查询，组合SQL语句
                                    IF @PageIndex = 0 BEGIN
                                        SELECT @SQLString = N'SELECT TOP ' + STR(@PageSize)
			                                + N' ' + @SelectFields
			                                + N' FROM ' + @TableNameOrSqlCmd 
			                                 + @WhereString1 + '
			                                ORDER BY ' + @SortField;

                                        IF @IsDesc = 1

                                            SELECT @SQLString = @SQLString + ' DESC';
        SET @ResetOrder = 0

                                    END
                                    ELSE BEGIN------------下面对页码 页数进行了再次确认统计

                                        SET @SQLString = '';
        DECLARE @GetFromLast BIT
        IF @TotalRecords=-1
			                                SET @GetFromLast = 0

                                        ELSE BEGIN

                                            DECLARE @TotalPage INT,@ResidualCount INT

                                            SET @ResidualCount = @TotalRecords % @PageSize

                                            IF @ResidualCount = 0

                                                SET @TotalPage = @TotalRecords / @PageSize

                                            ELSE
                                                SET @TotalPage=@TotalRecords/@PageSize+1
			                                IF @PageIndex>@TotalPage/2 --从最后页算上来
                                                SET @GetFromLast=1
			                                ELSE
                                                SET @GetFromLast=0
			                                IF @GetFromLast = 1 BEGIN
                                                  IF @PageIndex=@TotalPage-1 BEGIN
                                                      IF @ResidualCount=0
						                                SET @ResidualCount = @PageSize;
        SELECT @SQLString = N'SELECT top ' + STR(@ResidualCount)
						                                + N' ' + @SelectFields
						                                + N' FROM ' + @TableNameOrSqlCmd
						                                + @WhereString1 + '
						                                ORDER BY ' + @SortField;

                                                    IF @IsDesc = 0--反过来
                                                        SELECT @SQLString = @SQLString + ' DESC';
					                                SET @ResetOrder = 1

                                                END
                                                ELSE IF @PageIndex>@TotalPage-1 BEGIN --已经超过最大页数
                                                   SELECT @SQLString = N'SELECT ' + @SelectFields
						                                + N' FROM ' + @TableNameOrSqlCmd + ' WHERE 1=2'
					                                SET @ResetOrder = 0

                                                END
                                                ELSE BEGIN
                                                    SET @PageIndex=@TotalPage-(@PageIndex+1)
					                                IF @IsDesc = 1

                                                        SET @IsDesc = 0

                                                    ELSE
                                                        SET @IsDesc=1
					                                SET @ResetOrder = 1

                                                END
                                            END

                                            ELSE
                                                SET @ResetOrder=0
		                                END

                                        IF @SQLString='' BEGIN
                                            DECLARE @TopCount INT

                                            IF @GetFromLast = 1 BEGIN
                                                  IF @ResidualCount > 0
					                                SET @TopCount = @PageSize * (@PageIndex - 1) + @ResidualCount;
        ELSE
            SET @TopCount=@PageSize* (@PageIndex)+@ResidualCount;
				                                IF @TopCount = 0

                                                    SET @TopCount = @PageSize;
        END
        ELSE

                                                SET @TopCount = @PageSize * @PageIndex

                                            IF @IsDesc = 1

                                              SELECT @SQLString = 'SELECT TOP ' + STR(@PageSize)
                                                + N' * '
				                                + N' FROM ( SELECT ROW_NUMBER() OVER (ORDER BY '
												+@SortField
												+ N' DESC) AS RowNumber,'
												+@SelectFields
												+' FROM ' 
											    +@TableNameOrSqlCmd
												+@WhereString1
												 +') AS TMP '
												 +'WHERE TMP.RowNumber > '
												 +STR(@PageSize* @PageIndex);

        ELSE
            SELECT @SQLString = 'SELECT TOP ' + STR(@PageSize)
				                                + N' * '
				                                + N' FROM ( SELECT ROW_NUMBER() OVER (ORDER BY '
												+@SortField
												+ N' ASC) AS RowNumber,'
												+@SelectFields
												+' FROM ' 
											    +@TableNameOrSqlCmd
												+@WhereString1
												 +') AS TMP '
												 +'WHERE TMP.RowNumber > '
												 +STR(@PageSize* @PageIndex);
        END
    END

        SELECT @TotalRecords AS TotalRows, @TotalPageCount AS TotalPages;

         SET @ParamDef={1}
         EXEC sp_executesql @SQLString {0} , @ParamDef   {3};-- 0为标识;如果不是参数化的，那么为 -- 字符注释后面的 分页执行语句注入参数 1 。2 为具体的@参数名
           
        END";


//        需要注意的是：
//1、要求动态Sql和动态Sql参数列表必须是NVARCHAR

//2、动态Sql的参数列表与外部提供值的参数列表顺序必需一致

//3、一旦使用了 '@name = value' 形式之后，所有后续的参数就必须以 '@name = value' 的形式传递，比如：
    }
}
