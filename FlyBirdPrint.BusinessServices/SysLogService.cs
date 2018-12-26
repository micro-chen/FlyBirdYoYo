using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using FlyBirdYoYo.DomainEntity;
using FlyBirdYoYo.DbManage;
using FlyBirdYoYo.Data.Repository;
using FlyBirdYoYo.Utilities.Interface;

using FlyBirdYoYo.DomainEntity.ViewModel;

namespace FlyBirdYoYo.BusinessServices
{
    public  class SysLogService: BaseService, IBusinessBaseService
    {
        #region 属性集合

        #endregion

        #region   字段集合
        private SysLogRepository dal_SysLog;
        #endregion

        #region  构造函数

        public SysLogService()
        {
            this.dal_SysLog = Singleton<SysLogRepository>.Instance;
        }

        #endregion

        #region   系统业务方法


        #region  Insert操作

        /// <summary>
        /// 添加单个SysLogModel对象方法(可返回对应数据表中 的此实体ID)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddOneSysLogModel (SysLogModel entity)
        {
            var result = false;

            try
            {
                var entityID = dal_SysLog.Insert (entity);
                if (entityID > 0)
                {
                    entity.Id = entityID;
                    result = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// 批量插入SysLogModel对象方法(不能返回对应数据表中 的ID)
        /// </summary>
        /// <param name="entities"></param>
        /// <returns>返回操作结果</returns>
        public bool AddMulitiSysLogModels (IEnumerable<SysLogModel> entities)
        {
            var result = false;
            try
            {
                result = dal_SysLog.InsertMulitiEntities (entities);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        #endregion


        #region Update 更新操作
        /// <summary>
        /// 更新单个SysLogModel实体模型
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateOneSysLogModel (SysLogModel entity)
        {
            var result = false;

            try
            {
                result = dal_SysLog.Update (entity) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 更新SysLogModel元素 通过  符合条件的
        /// </summary>
        /// <param name="entity">携带值的载体</param>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        public bool UpdateSysLogModelsByCondition (SysLogModel entity, Expression<Func<SysLogModel, bool>> predicate)
        {
            var result = false;

            try
            {
                result = dal_SysLog.UpdateByCondition (entity, predicate) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        #endregion


        #region Select   查询操作
        /// <summary>
        /// 通过主键获取单个SysLogModel元素
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public SysLogModel GetSysLogElementById (long id)
        {
            SysLogModel result = null;

            try
            {
                result = dal_SysLog.GetElementById (id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 通过特定的条件查询出SysLogModel元素
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public SysLogModel GetSysLogFirstOrDefaultByCondition (Expression<Func<SysLogModel, bool>> predicate)
        {
            SysLogModel result = null;

            try
            {
                result = dal_SysLog.GetFirstOrDefaultByCondition (predicate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }



        /// <summary>
        /// 通过特定的条件查询出SysLogModel元素集合
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<SysLogModel> GetSysLogElementsByCondition (Expression<Func<SysLogModel, bool>> predicate)
        {
            List<SysLogModel> result = null;

            try
            {
                result = dal_SysLog.GetElementsByCondition (predicate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }



        #endregion


        #region Delete 删除操作
        /// <summary>
        /// 删除一个SysLogModel实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool DeleteOneSysLogModel (SysLogModel entity)
        {
            var result = false;

            try
            {
                result = dal_SysLog.Delete (entity) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        // <summary>
        /// 删除符合条件的SysLogModel实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool DeleteMulitiSysLogByCondition (Expression<Func<SysLogModel, bool>> predicate)
        {
            var result = false;

            try
            {
                result = dal_SysLog.DeleteByCondition (predicate) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion

        #endregion


        #region 自定义业务方法
        #endregion
    }
}