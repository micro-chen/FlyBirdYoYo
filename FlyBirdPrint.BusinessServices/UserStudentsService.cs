using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using FlyBirdYoYo.DomainEntity;
using FlyBirdYoYo.DbManage;
using FlyBirdYoYo.Data.Repository;
using FlyBirdYoYo.Utilities.Interface;
using FlyBirdYoYo.DomainEntity.ViewModel;
using FlyBirdYoYo.DomainEntity.QueryCondition;

namespace FlyBirdYoYo.BusinessServices
{
    public class UserStudentsService : BaseService, IBusinessBaseService
    {
        #region 属性集合

        #endregion

        #region   字段集合
        private UserStudentsRepository dal_UserStudents;
        #endregion

        #region  构造函数

        public UserStudentsService()
        {
            this.dal_UserStudents = Singleton<UserStudentsRepository>.Instance;
        }

        #endregion

        #region   系统业务方法


        #region  Insert操作

        /// <summary>
        /// 添加单个UserStudentsModel对象方法(可返回对应数据表中 的此实体ID)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddOneUserStudentsModel(UserStudentsModel entity)
        {
            var result = false;

            try
            {
                var entityID = dal_UserStudents.Insert(entity);
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
        /// 批量插入UserStudentsModel对象方法(不能返回对应数据表中 的ID)
        /// </summary>
        /// <param name="entities"></param>
        /// <returns>返回操作结果</returns>
        public bool AddMulitiUserStudentsModels(IEnumerable<UserStudentsModel> entities)
        {
            var result = false;
            try
            {
                result = dal_UserStudents.InsertMulitiEntities(entities);
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
        /// 更新单个UserStudentsModel实体模型
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateOneUserStudentsModel(UserStudentsModel entity)
        {
            var result = false;

            try
            {
                result = dal_UserStudents.Update(entity) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 更新UserStudentsModel元素 通过  符合条件的
        /// </summary>
        /// <param name="entity">携带值的载体</param>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        public bool UpdateUserStudentsModelsByCondition(UserStudentsModel entity, Expression<Func<UserStudentsModel, bool>> predicate)
        {
            var result = false;

            try
            {
                result = dal_UserStudents.UpdateByCondition(entity, predicate) > 0 ? true : false;
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
        /// 通过主键获取单个UserStudentsModel元素
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public UserStudentsModel GetUserStudentsElementById(int id)
        {
            UserStudentsModel result = null;

            try
            {
                result = dal_UserStudents.GetElementById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        /// <summary>
        /// 通过特定的条件查询出UserStudentsModel元素
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public UserStudentsModel GetUserStudentsElementByCondition(Expression<Func<UserStudentsModel, bool>> predicate)
        {
            UserStudentsModel result = null;

            try
            {
                result = dal_UserStudents.GetFirstOrDefaultByCondition(predicate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }



        /// <summary>
        /// 通过特定的条件查询出UserStudentsModel元素集合
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<UserStudentsModel> GetUserStudentsElementsByCondition(Expression<Func<UserStudentsModel, bool>> predicate)
        {
            List<UserStudentsModel> result = null;

            try
            {
                result = dal_UserStudents.GetElementsByCondition(predicate);
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
        /// 删除一个UserStudentsModel实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool DeleteOneUserStudentsModel(UserStudentsModel entity)
        {
            var result = false;

            try
            {
                result = dal_UserStudents.Delete(entity) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        // <summary>
        /// 删除符合条件的UserStudentsModel实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool DeleteMulitiUserStudentsByCondition(Expression<Func<UserStudentsModel, bool>> predicate)
        {
            var result = false;

            try
            {
                result = dal_UserStudents.DeleteByCondition(predicate) > 0 ? true : false;
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


        /// <summary>
        /// 分页查询搜索学生信息
        /// </summary>
        /// <param name="dataContainer"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public BusinessViewModelContainer<List<StudentDto>> SearchUserStudentsHandler(StudentQueryCondition condition)
        {
            BusinessViewModelContainer<List<StudentDto>> dataContainer = new BusinessViewModelContainer<List<StudentDto>>();
            try
            {
                if (null == condition)
                {
                    throw new Exception("查询条件不能为空！");
                }
                if (condition.PageNumber <= 0)
                {
                    condition.PageNumber = 1;
                }
                if (condition.PageSize > 100)
                {
                    condition.PageSize = 100;
                }

                //查询分页
                var dataListPager = this.dal_UserStudents.GetStudentssByPagerAndCondition(condition.KeyWord,condition.PageNumber,condition.PageSize);
                if (dataListPager != null && dataListPager.TotalRows > 0)
                {
                    //数据转换DTO
                    dataContainer.Data = dataListPager.DataList;

                    //初始化客户端分页信息
                    if (dataContainer.PagerInfomation == null)
                    {
                        dataContainer.PagerInfomation = new PageInfo();
                    }
                    dataContainer.PagerInfomation.PageNumber = condition.PageNumber;
                    dataContainer.PagerInfomation.Size = condition.PageSize;
                    dataContainer.PagerInfomation.TotalElements = dataListPager.TotalRows;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return dataContainer;
        }


      
        /// <summary>
        /// 自定义sql语句查询示范
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IEnumerable<StudentDto> GetListBySql(StudentDto model)
        {
            return this.dal_UserStudents.GetListBySql(model);
        }

            #endregion
        }
}