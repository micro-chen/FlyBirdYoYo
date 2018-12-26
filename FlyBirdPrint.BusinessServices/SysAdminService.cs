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
    public  class SysAdminService: BaseService, IBusinessBaseService
    {
        #region 属性集合

        #endregion

        #region   字段集合
        private SysAdminRepository dal_SysAdmin;
        #endregion

        #region  构造函数

        public SysAdminService()
        {
            this.dal_SysAdmin = Singleton<SysAdminRepository>.Instance;
        }

        #endregion

        #region   系统业务方法


        #region  Insert操作

        /// <summary>
        /// 添加单个SysAdminModel对象方法(可返回对应数据表中 的此实体ID)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddOneSysAdminModel (SysAdminModel entity)
        {
            var result = false;

            try
            {
                var entityID = dal_SysAdmin.Insert (entity);
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
        /// 批量插入SysAdminModel对象方法(不能返回对应数据表中 的ID)
        /// </summary>
        /// <param name="entities"></param>
        /// <returns>返回操作结果</returns>
        public bool AddMulitiSysAdminModels (IEnumerable<SysAdminModel> entities)
        {
            var result = false;
            try
            {
                result = dal_SysAdmin.InsertMulitiEntities (entities);
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
        /// 更新单个SysAdminModel实体模型
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateOneSysAdminModel (SysAdminModel entity)
        {
            var result = false;

            try
            {
                result = dal_SysAdmin.Update (entity) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 更新SysAdminModel元素 通过  符合条件的
        /// </summary>
        /// <param name="entity">携带值的载体</param>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        public bool UpdateSysAdminModelsByCondition (SysAdminModel entity, Expression<Func<SysAdminModel, bool>> predicate)
        {
            var result = false;

            try
            {
                result = dal_SysAdmin.UpdateByCondition (entity, predicate) > 0 ? true : false;
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
        /// 通过主键获取单个SysAdminModel元素
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public SysAdminModel GetSysAdminElementById (long id)
        {
            SysAdminModel result = null;

            try
            {
                result = dal_SysAdmin.GetElementById (id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 通过特定的条件查询出SysAdminModel元素
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public SysAdminModel GetSysAdminFirstOrDefaultByCondition (Expression<Func<SysAdminModel, bool>> predicate)
        {
            SysAdminModel result = null;

            try
            {
                result = dal_SysAdmin.GetFirstOrDefaultByCondition (predicate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }



        /// <summary>
        /// 通过特定的条件查询出SysAdminModel元素集合
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<SysAdminModel> GetSysAdminElementsByCondition (Expression<Func<SysAdminModel, bool>> predicate)
        {
            List<SysAdminModel> result = null;

            try
            {
                result = dal_SysAdmin.GetElementsByCondition (predicate);
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
        /// 删除一个SysAdminModel实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool DeleteOneSysAdminModel (SysAdminModel entity)
        {
            var result = false;

            try
            {
                result = dal_SysAdmin.Delete (entity) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        // <summary>
        /// 删除符合条件的SysAdminModel实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool DeleteMulitiSysAdminByCondition (Expression<Func<SysAdminModel, bool>> predicate)
        {
            var result = false;

            try
            {
                result = dal_SysAdmin.DeleteByCondition (predicate) > 0 ? true : false;
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