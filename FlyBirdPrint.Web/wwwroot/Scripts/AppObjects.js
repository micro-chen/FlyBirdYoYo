/*
应用中定义的对象，例如枚举、交互参数对象等
*/
/*-----------------------------------------*/
/*支持的平台*/
var SupportPlatformEnum = {
    //天猫
    Tmall: 0,
    //淘宝
    Taobao: 1,
    //京东
    Jingdong: 2,
    //拼多多
    Pdd: 3,
    //唯品会
    Vip: 4,
    //国美
    Guomei: 5,
    //苏宁
    Suning: 6,
    //当当
    Dangdang: 7,
    //一号店
    Yhd: 8,
    ////美丽说
    //Meilishuo : 9,
    //蘑菇街
    Mogujie: 10,
    ////折800
    //Zhe800 : 11,
    //一淘
    ETao: 12,
    //阿里妈妈
    Alimama: 13

};
/*排序枚举*/
var OrderRuleEnum = {
    /// <summary>
    /// 默认排序
    /// </summary>
    Default: 0,
    /// <summary>
    /// 正序排列
    /// </summary>
    ASC : 1,

    /// <summary>
    /// 倒序
    /// </summary>
    DESC:2
};

/*品牌标签*/
var BrandTag = function(){
    this.BrandId = "";
    this.BrandName = "";
    this.IconUrl = "";
    this.CharIndex = "";
    this.FilterField = "";
    this.Platform = 0;
};
/*关键词Tag 标签*/
var KeyWordTag = function(){
    this.GroupShowName = "";
    this.TagName = "";
    this.FilterFiled = "";
    this.Value = "";
    this.Platform = 0;
};

/*关键词Tag 标签 组*/
var KeyWordTagGroup = function(){
    this.GroupName = "";
    this.Tags = [];
};
/*查询商品需要的基础参数模型*/
var BaseFetchWebPageArgument =function() {

    this.Platform = 0;
    this.IsNeedResolveHeaderTags = true;
    //this.ResolvedUrl = "";
    this.KeyWord = "";
    this.Brands = [];
    this.TagGroup = null;
    this.OrderFiledName = "";
    this.OrderFiled = null;
    this.FromPrice = 0;
    this.ToPrice = 0;
    this.PageIndex = 0;
    this.AttachParas = { "key-1": 1 };

};

/*查询商品的Data模型*/
var SearchProductViewModel = function () {
    this.KeyWord = '';
    this.IsNeedResolveHeaderTags = true;
    this.Brands = [];
    this.Tags = [];
    this.Products = [];
};