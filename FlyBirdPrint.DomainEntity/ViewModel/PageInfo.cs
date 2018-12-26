using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyBirdYoYo.DomainEntity.ViewModel
{

    /// <summary>
    /// 分页信息
    /// </summary>
    public class PageInfo
    {

        const int DEFAULT_PAGE_SIZE = 15;

        public PageInfo()
        {
            this.Size = DEFAULT_PAGE_SIZE;
        }


 

        /// <summary>
        /// 页大小
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// 总数目
        /// </summary>
        public int TotalElements { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages
        {
            get
            {
                var pages = 0;
                if (this.TotalElements <= 0)
                {
                    return 0;
                }
                if (this.Size <= 0)
                {
                    this.Size = DEFAULT_PAGE_SIZE;
                }
                var mode = this.TotalElements % this.Size;
                if (mode == 0)
                {
                    pages = this.TotalElements / this.Size;
                }
                else
                {
                    pages = (this.TotalElements / this.Size) + 1;//除不尽+1
                }
                return pages;
            }
        }



        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageNumber { get; set; }



        /// <summary>
        /// 上一页 页码
        /// </summary>
        public int PreviousPageNumber
        {
            get
            {
                var num = this.PageNumber - 1;
                if (num <= 0)
                {
                    num = 1;
                }
                return num;
            }
        }

        /// <summary>
        /// 下一页 页码
        /// </summary>
        public int NextPageNumber
        {
            get
            {
                var num = this.PageNumber + 1;
                if (num >= TotalPages)
                {
                    num = TotalPages;
                }
                return num;
            }
        }




    }
}
