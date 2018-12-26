using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace FlyBirdYoYo.DbManage.Utilities
{
    /// <summary>
    /// 实体分页数据结构
    /// </summary>
    public class EntityPagerData<T> where T : BaseEntity
    {
        /// <summary>
        /// 页索引
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 当前页大小
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// 原始数据指针
        /// 注意：这里实际是原始数据集合的一个引用
        /// </summary>
        public IEnumerable<T> OriginalDataPointer { get; set; }

        public EntityPagerData(IEnumerable<T> _originalDataPointer,int _pageIndex,int _pageSize)
        {
            this.OriginalDataPointer = _originalDataPointer;

            this.PageIndex = _pageIndex;
            this.PageSize = _pageSize;

        }


        /// <summary>
        /// 获取当页的数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetCurrentPageDataList()
        {
            var currentPageEntities = this.OriginalDataPointer.Skip(this.PageIndex * EntityBatchBag.SingleBatchMaxSqlCount).Take(this.PageSize);
            return currentPageEntities;
        }
    }

    /// <summary>
    /// 将实体  集合进行 分页提取
    /// </summary>
    public static class EntityBatchBag
    {

        internal const int SingleBatchMaxSqlCount= 256;

        public static IList<EntityPagerData<T>> SplitToBatchBag<T>(this IEnumerable<T> source) where T : BaseEntity
        {



            var container = new List<EntityPagerData<T>>();

            var totalCount = source.Count();

            var pageSize = SingleBatchMaxSqlCount;

            var totalPages = totalCount / pageSize;

            if (totalPages == 0)
            {
                //不超过40条 直接返回原来的实体集合 不用分页
                var pagerDataModel = new EntityPagerData<T>(source, 0,totalCount);
                container.Add(pagerDataModel);

                return container;
            }
            else
            {
                //超过1页 包含1页
                if (totalCount % pageSize != 0)
                {
                    totalPages += 1;
                }

                for (int pageIndex = 0; pageIndex < totalPages; pageIndex++)
                {
                    EntityPagerData<T> pagerDataModel = null;

                    if (pageIndex==totalPages-1)
                    {
                        //最后一页的数据数码运算
                        var lastPageSize = totalCount - (pageIndex* pageSize);
                         pagerDataModel = new EntityPagerData<T>(source, pageIndex, lastPageSize);
                        container.Add(pagerDataModel);
                        break;
                    }

                     pagerDataModel = new EntityPagerData<T>(source, pageIndex, pageSize);
                    container.Add(pagerDataModel);
                }
            }

            return container;



        }
    }
}
