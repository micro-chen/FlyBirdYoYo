
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



        private static string PageSql_Core_Name = "DbManage_GetRecordsByPageSQLString";//执行分页的核心

        private static object _locker = new object();
        internal static void CheckAndCreatePagerSQLProcedure(DbConnConfig dbConfig)
        {

            lock (_locker)
            {
                //1检查数据库是否存在存储过程   //2创建
                //分页核心
                if (!IsExistSQLProcedure(dbConfig, PageSql_Core_Name))
                {
                    CreateSQLProcedure(dbConfig, PageSql_Core_SQLCommand);
                }
                //分页调用入口
                if (!IsExistSQLProcedure(dbConfig, Contanst.PageSql_Call_Name))
                {
                    CreateSQLProcedure(dbConfig, PageSql_Call_SQLCommand);
                }
            }

        }


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




        //调用入口sql存储过程
        private static string PageSql_Call_SQLCommand = @"CREATE PROCEDURE [dbo].[DbManage_GetRecordsByPage]
	                                    @PageIndex int,
	                                    @PageSize int,
	                                    @TableName NVARCHAR(MAX),
	                                    @SelectFields NVARCHAR(MAX) = '*', --查询字段，默认为 *
	                                    @ConditionWhere NVARCHAR(MAX) = N'',     --条件例如  DirectoryID=4
	                                    @SortField varchar(256) = '1',
	                                    @IsDesc bit = 0, --是否倒序
	                                    @TotalRecords int = -1 OUTPUT,
	                                   @TotalPageCount INT OUTPUT
                                    AS
                                    BEGIN

	                                    SET NOCOUNT ON;
	                                    DECLARE @SQLString NVARCHAR(MAX),@ResetOrder bit----------- 1表示读取数据的时候 排序要反过来
	                                    EXECUTE DbManage_GetRecordsByPageSQLString
						                                    @PageIndex,
						                                    @PageSize,
						                                    @TableName,
						                                    @SelectFields, --查询字段，默认为 *
						                                    @ConditionWhere,     --条件例如  DirectoryID=4
						                                    @SortField,
						                                    @IsDesc, --是否倒序
						                                    @TotalRecords OUTPUT,
						                                    @TotalPageCount  OUTPUT,--输出总页数,
						                                    @ResetOrder OUTPUT,
						                                    @SQLString OUTPUT
						print @SQLString;
	                                    EXEC (@SQLString);
	                                    RETURN @ResetOrder
                                    END;
";


        //执行分页的核心SQL 存储过程
        private static string PageSql_Core_SQLCommand = @"CREATE PROCEDURE [dbo].[DbManage_GetRecordsByPageSQLString]
	                                @PageIndex int,
	                                @PageSize int,
	                                @TableName NVARCHAR(MAX),
	                                @SelectFields NVARCHAR(MAX) = '*', --查询字段，默认为 *
	                                @ConditionWhere NVARCHAR(MAX) = N'',     --条件例如  DirectoryID=4
	                                @SortField varchar(256) = '',
	                                @IsDesc bit = 0, --是否倒序
	                                @TotalRecords int = -1 OUTPUT,
	                                @TotalPageCount int OUTPUT,--输出总页数
	                                @ResetOrder bit output,----------- 1表示读取数据的时候 排序要反过来
	                                @SQLString  NVARCHAR(MAX) output
                                AS
                                BEGIN
	                                SET NOCOUNT ON;

-------------------------第一步 ：查询限制条件的组合
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
	                                
	                                -----------设置完毕查询条件后  查询本次符合条件的记录数 页数
	                              DECLARE @sqlCmd   NVARCHAR(MAX)
	                                  
	                               IF @ConditionWhere IS NULL OR @ConditionWhere = N'' 
	                                BEGIN
		                                --没有查询条件
		                             
		                                SET @sqlCmd='SELECT @TotalRecords = COUNT(*)  FROM '+ @TableName
		                               	exec sp_executesql @sqlCmd,N'@TotalRecords int output',@TotalRecords output  
	                                END
	                                ELSE BEGIN
		                              SET @sqlCmd='SELECT @TotalRecords = COUNT(*)  FROM '+ @TableName+' '+@WhereString1
		                               	exec sp_executesql @sqlCmd,N'@TotalRecords int output',@TotalRecords output    
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
	                                
--------------------开始查询，组合SQL语句
	                                IF @PageIndex = 0 BEGIN
		                                SELECT @SQLString = N'SELECT TOP ' + STR(@PageSize)
			                                + N' ' + @SelectFields
			                                + N' FROM ' + @TableName 
			                                + N' '+ @WhereString1 + '
			                                ORDER BY ' + @SortField;
		                                IF @IsDesc = 1
			                                SELECT @SQLString = @SQLString + ' DESC';
		                                SET @ResetOrder=0	
	                                END
	                                ELSE BEGIN------------下面对页码 页数进行了再次确认统计
		                                SET @SQLString='';
		                                DECLARE @GetFromLast BIT
		                                IF @TotalRecords=-1
			                                SET @GetFromLast=0
		                                ELSE BEGIN
			                                DECLARE @TotalPage INT,@ResidualCount INT
			                                SET @ResidualCount=@TotalRecords%@PageSize
			                                IF @ResidualCount=0
				                                SET @TotalPage=@TotalRecords/@PageSize
			                                ELSE
				                                SET @TotalPage=@TotalRecords/@PageSize+1
			                                IF @PageIndex>@TotalPage/2 --从最后页算上来
				                                SET @GetFromLast=1
			                                ELSE
				                                SET @GetFromLast=0
			                                IF @GetFromLast=1 BEGIN
				                                IF @PageIndex=@TotalPage-1 BEGIN
					                                IF @ResidualCount=0
						                                SET @ResidualCount=@PageSize;
					                                SELECT @SQLString = N'SELECT top ' + STR(@ResidualCount)
						                                + N' ' + @SelectFields
						                                + N' FROM ' + @TableName
						                                + N' '+ @WhereString1 + '
						                                ORDER BY ' + @SortField;
					                                IF @IsDesc = 0--反过来
						                                SELECT @SQLString = @SQLString + ' DESC';
					                                SET @ResetOrder=1
				                                END 
				                                ELSE IF  @PageIndex>@TotalPage-1 BEGIN --已经超过最大页数
					                                SELECT @SQLString = N'SELECT ' + @SelectFields
						                                + N' FROM ' + @TableName + ' WHERE 1=2'
					                                SET @ResetOrder=0
				                                END
				                                ELSE BEGIN
					                                SET @PageIndex=@TotalPage-(@PageIndex+1)---
					                                IF @IsDesc=1
						                                SET @IsDesc=0
					                                ELSE
						                                SET @IsDesc=1
					                                SET @ResetOrder=1
				                                END  
			                                END
			                                ELSE 
				                                SET @ResetOrder=0
		                                END
		
		                                IF @SQLString='' BEGIN
			                                DECLARE @TopCount INT
			                                IF @GetFromLast=1 BEGIN
				                                IF @ResidualCount > 0
					                                SET @TopCount=@PageSize * (@PageIndex-1)+@ResidualCount;
				                                ELSE
					                                SET @TopCount=@PageSize * (@PageIndex)+@ResidualCount;
				                                IF @TopCount = 0
					                                SET @TopCount = @PageSize;
			                                END
			                                ELSE
				                                SET @TopCount=@PageSize * @PageIndex
			                                IF @IsDesc = 1
											  SELECT @SQLString = 'SELECT TOP ' + STR(@PageSize)
				                                + N' * '
				                                + N' FROM ( SELECT ROW_NUMBER() OVER (ORDER BY '
												+@SortField
												+ N' DESC) AS RowNumber,'
												+@SelectFields
												+' FROM ' 
											    +@TableName
												+@WhereString1
												 +') AS TMP '
												 +'WHERE TMP.RowNumber > '
												 +STR(@PageSize*@PageIndex);
				                           
			                                ELSE
				                                SELECT @SQLString = 'SELECT TOP ' + STR(@PageSize)
				                                + N' * '
				                                + N' FROM ( SELECT ROW_NUMBER() OVER (ORDER BY '
												+@SortField
												+ N' ASC) AS RowNumber,'
												+@SelectFields
												+' FROM ' 
											    +@TableName
												+@WhereString1
												 +') AS TMP '
												 +'WHERE TMP.RowNumber > '
												 +STR(@PageSize*@PageIndex);
		                                END
	                                END

                                END ";
    }
}
