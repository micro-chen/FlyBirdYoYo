using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FlyBirdYoYo.DomainEntity.ViewModel
{
    /// <summary>
    /// 业务实体-视图模型容器
    /// </summary>
    public class BusinessViewModelContainer<T>
    {
        const string SUCCESS_CALL_MESSAGE = "调用成功！";

        public BusinessViewModelContainer()
        {
            this.Data = default(T);
            this.Status = 1;
            this.IsSuccess = true;
            this.Msg = SUCCESS_CALL_MESSAGE;
            this.PagerInfomation = new PageInfo();
        }

        /// <summary>
        /// 是否业务正确
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 结果状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 服务消息内容
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 数据承载
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 分页信息
        /// </summary>
        public PageInfo PagerInfomation { get; set; }
        /// <summary>
        /// 设置为失败结果
        /// </summary>
        /// <param name="errMsg"></param>
        public void SetFalied(string errMsg="")
        {
            this.IsSuccess = false;
            this.Status = 0;
            this.Msg = errMsg;
        }
        /// <summary>
        /// 判断数据是否是空白数据类型
        /// </summary>
         /// <returns></returns>
        public  bool IsNoDataViewModel()
        {
         
            if (null == this || this.Data == null)
            {
                return true;
            }

            try
            {
                //判断是否是集合类型的数据
                var dataType = this.Data.GetType();

                if (typeof(IEnumerable).IsAssignableFrom(dataType))
                {
                    //集合类型
                    if ((this.Data as IEnumerable<object>).Count() <= 0)
                    {
                        return true;
                    }
                }
            }
            catch { }

            return false;
        }
    }
}
