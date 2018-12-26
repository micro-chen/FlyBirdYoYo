namespace FlyBirdYoYo.DbManage
{
    /// <summary>
    /// 数据库  当前状态  
    /// </summary>
    public enum DbState
    {
        /// <summary>
        /// 关闭的
        /// </summary>
        Closed,
        /// <summary>
        /// 打开的（已经设置初始化完毕）
        /// </summary>
        Opened
    }
}
