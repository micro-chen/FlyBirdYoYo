/// <reference path="../extension/applicationcore.js" />

/*bind to home page
注意：使用基于闭包的形式，防止 全局变量导致的变量冲突和引用问题
*/

$(function () {

    var homePage = {

        /*page controls begin*/
        //btn_save: $("#btn_save"),//保存按钮
        container_fixed_top: $('#container_fixed_top'),//搜索悬浮框
        txt_header_search_keyword: $('#txt_header_search_keyword'),//悬浮搜索框
        btn_search_top_clear: $('i.search-top-clear'),//悬浮搜索内容清空按钮
        btn_headerSearchIpt: $('#btn_headerSearchIpt'),//悬浮搜索按钮
        txt_search_keyword: $('#txt_search_keyword'),//搜索输入框
        btn_search_clear: $('i.search-bottom-clear'),//搜索内容清空按钮
        btn_search: $('#btn_search'),//搜索按钮
        container_hot_words: $('#container_hot_words'),//
        btn_goTop: $('#btn_goTop'),//
        container_filtered: $('#container_filtered'),//
        btn_clear_all_filter: $('#btn_clear_all_filter'),//
        filterForm: $('#filterForm'),//
        filter_brand: $('#facets-category-brand'),//
        container_category_brand: $('#container_category_brand'),//品牌区域
        txt_min_price: $('#txt_min_price'),//起始价格
        txt_max_price: $('#txt_max_price'),//上限价格

        /*page controls end*/

        /*page local storage begin*/
        local_brands: [],
        local_tags: [],
        local_products_tmall: null,
        local_products_taobao: null,
        /*page local storage end*/


        api_auto_complete_suggest: "api/servicebus/suggest",//搜索框自动完成api
        //平台商品检索配置

        api_search_tmall_products: "api/servicebus/search_tmall_products",//天猫商品检索
        api_search_taobao_products: "api/servicebus/search_taobao_products",//淘宝商品检索
        api_search_jd_products: "api/servicebus/search_jd_products",//京东商品检索
        api_search_pdd_products: "api/servicebus/search_pdd_products",//拼多多商品检索
        api_search_guomei_products: "api/servicebus/search_guomei_products",//国美商品检索
        api_search_guomei_price: "api/servicebus/search_guomei_price",//国美单个商品价格检索
        api_search_suning_products: "api/servicebus/search_suning_products",//苏宁商品检索
        api_search_dangdang_products: "api/servicebus/search_dangdang_products",//当当商品检索
        api_search_taoquan: "api/servicebus/search_taoquan",//淘宝天猫优惠券检索

        /*
         二类电商平台
         api_search_vip_products: "api/servicebus/search_vip_products",//唯品会商品检索
        //api_search_yhd_products: "api/servicebus/search_yhd_products",//一号店商品检索
        //api_search_mls_products: "api/servicebus/search_mls_products",//美丽说商品检索
        //api_search_mgj_products: "api/servicebus/search_mgj_products",//蘑菇街商品检索
        //api_search_etao_products: "api/servicebus/search_etao_products",//一淘商品检索

        */



        /*init page */
        init: function (agrs) {

            //debugger;
            /*头部悬浮设置*/
            this.initFixedHead();

            //点击保存按钮事件
            //this.btn_save.click(homePage.saveDetails);

            /*浮动搜索的自动完成事件*/
            this.txt_header_search_keyword.autocomplete({
                serviceUrl: this.api_auto_complete_suggest,
                dataType: "json",
                top: 8,
                width: "504px",
                deferRequestBy: 200,//不要立即请求 间隔一个缓冲
                paramName: "key",
                params: { "key": this.txt_header_search_keyword.val(), "sign": FlyBirdYoYo.apiSignFunc() },
                formatResult: function (suggestion, currentValue, idx) {


                    var processorStrong = function () {
                        // Do not replace anything if the current value is empty
                        if (!currentValue) {
                            return suggestion.value;
                        }
                        if (suggestion.value.indexOf(currentValue) < 0) {
                            return suggestion.value;
                        }

                        var pattern = '(' + $.Autocomplete.utils.escapeRegExChars(currentValue) + ')';

                        var fullText = '<strong>' + suggestion.value + '<\/strong>';
                        return fullText
                            .replace(new RegExp(pattern, 'gi'), '<\/strong>$1<strong>');
                        //.replace(/&/g, '&amp;')
                        //.replace(/</g, '&lt;')
                        //.replace(/>/g, '&gt;')
                        //.replace(/"/g, '&quot;')
                        //.replace(/&lt;(\/?strong)&gt;/g, '<$1>');

                    }
                    var displayText = processorStrong();
                    //console.log(idx);
                    var backColor = "";
                    //色彩差异
                    if (idx === 0) {
                        backColor = "#f58c85";
                    } else if (idx === 1) {
                        backColor = "#fcbc4b";
                    } else if (idx === 2) {
                        backColor = "#a1d958";
                    }
                    if (backColor != "") {
                        return "<em class='hot' style='background-color:{2};'>{0}</em> <span class=''>{1}</span>".format(idx + 1, displayText, backColor);
                    }
                    return "<em class='hot'>{0}</em> <span class=''>{1}</span>".format(idx + 1, displayText);

                },
                onSelect: function (suggestion) {
                    console.log('You selected: ' + suggestion.value + ', ' + suggestion.data);
                    if (!isNullOrEmpty(suggestion.value)) {
                        homePage.txt_header_search_keyword.val(suggestion.value);
                        homePage.txt_search_keyword.val(suggestion.value);
                        homePage.btn_search.click();
                    }

                }
            });
            //搜索输入框自动完成事件
            this.txt_search_keyword.autocomplete({
                serviceUrl: this.api_auto_complete_suggest,
                dataType: "json",
                top: 2,
                width: "678px",
                deferRequestBy: 200,//不要立即请求 间隔一个缓冲
                paramName: "key",
                params: { "key": this.txt_search_keyword.val(), "sign": FlyBirdYoYo.apiSignFunc() },
                formatResult: function (suggestion, currentValue, idx) {


                    var processorStrong = function () {
                        // Do not replace anything if the current value is empty
                        if (!currentValue) {
                            return suggestion.value;
                        }
                        if (suggestion.value.indexOf(currentValue) < 0) {
                            return suggestion.value;
                        }

                        var pattern = '(' + $.Autocomplete.utils.escapeRegExChars(currentValue) + ')';

                        var fullText = '<strong>' + suggestion.value + '<\/strong>';
                        return fullText
                            .replace(new RegExp(pattern, 'gi'), '<\/strong>$1<strong>');
                        //.replace(/&/g, '&amp;')
                        //.replace(/</g, '&lt;')
                        //.replace(/>/g, '&gt;')
                        //.replace(/"/g, '&quot;')
                        //.replace(/&lt;(\/?strong)&gt;/g, '<$1>');

                    }
                    var displayText = processorStrong();
                    //console.log(idx);
                    var backColor = "";
                    //色彩差异
                    if (idx === 0) {
                        backColor = "#f58c85";
                    } else if (idx === 1) {
                        backColor = "#fcbc4b";
                    } else if (idx === 2) {
                        backColor = "#a1d958";
                    }
                    if (backColor !== "") {
                        return "<em class='hot' style='background-color:{2};'>{0}</em> <span class=''>{1}</span>".format(idx + 1, displayText, backColor);
                    }
                    return "<em class='hot'>{0}</em> <span class=''>{1}</span>".format(idx + 1, displayText);

                },
                onSelect: function (suggestion) {
                    console.log('You selected: ' + suggestion.value + ', ' + suggestion.data);
                    if (!isNullOrEmpty(suggestion.value)) {
                        homePage.txt_header_search_keyword.val(suggestion.value);
                        homePage.txt_search_keyword.val(suggestion.value);
                        homePage.btn_search.click();
                    }

                }
            });

            /*搜索输入框的keyup事件*/
            this.txt_header_search_keyword.keyup("top", homePage.searchKeywordKeyupHandler);
            this.txt_search_keyword.keyup("common", homePage.searchKeywordKeyupHandler);

            /*搜索内容清除按钮事件*/
            $("i.search-clear").mouseover(function () {
                var sender = $(this);
                sender.addClass("search-clear-highlight");
            }).mouseout(function () {
                var sender = $(this);
                sender.removeClass("search-clear-highlight");
            }).click(function () {
                var sender = $(this);
                homePage.txt_header_search_keyword.val("");
                homePage.txt_search_keyword.val("");
                sender.hide();
            });

            /*搜索按钮点击事件*/
            this.btn_headerSearchIpt.click(homePage.btnSearchHandler);
            this.btn_search.click(homePage.btnSearchHandler);

            /*注册热搜词点击事件*/
            this.container_hot_words.find("a[data-word]").click(homePage.hotWordSelectHandler);


            /*回到顶部*/
            this.btn_goTop.click(function () {
                $('body,html').animate({
                    scrollTop: 0
                }, 200);
            });


            this.btn_clear_all_filter.click(homePage.clearAllFilterResetSerch);

        },

        /*清空全部过滤条件，并重新搜索商品*/
        clearAllFilterResetSerch: function () {
            var sender = $(this);
            homePage.container_filtered.hide();
            //1 清空过滤条件
            homePage.container_filtered.find(".filter-item").remove();
            //2 重新搜索
            //关键词+排序
            //向api发送商品搜索
            var paras = new BaseFetchWebPageArgument();
            // 关键词
            paras.KeyWord = homePage.txt_search_keyword.val();
            //不需要从新绘制tag
            paras.IsNeedResolveHeaderTags = false;
            //价格区间
            paras.FromPrice = homePage.txt_min_price.val();
            paras.ToPrice = homePage.txt_max_price.val();

            //排序字段
            var domSort = homePage.filterForm.find("a.active.common-xCtJ-count");
            if (domSort && domSort.length > 0) {
                paras.OrderFiledName = domSort.attr("data-val");//保存客户端选中的排序字段值并在服务端解析映射
            }
            homePage.searchProductsHandler(paras);
        },

        /*热搜词点击事件*/
        hotWordSelectHandler: function () {
            var sender = $(this);
            var word = sender.attr("data-word");
            homePage.txt_header_search_keyword.val(word);
            homePage.txt_search_keyword.val(word);
        },
        /*悬浮头部*/
        initFixedHead: function () {

            var initHead = function (Ths) {

                var scrollTop = Ths.scrollTop();

                if (scrollTop >= 230) {
                    //debugger
                    homePage.container_fixed_top.slideDown(100);//下拉浮动显示
                    var domAutoComplete = $("div.autocomplete-suggestions");
                    domAutoComplete.hide();


                } else {
                    homePage.container_fixed_top.slideUp(100);//隐藏
                }
                var commHead = $("#v3-common-header");
                if (scrollTop >= 124) {
                    var newNum = 140 - parseInt(scrollTop - 124);
                    commHead.css("top", newNum + "px");

                    $("#v3-header").css("background-position-y", "-" + parseInt((scrollTop - 124) / 2) + "px");
                } else {
                    commHead.css("top", "140px");
                }
                if (scrollTop <= 10) {
                    $("#v3-header").css("background-position-y", "0");
                }
            }

            initHead($(window));

            $(window).scroll(function () {
                initHead($(this));
            });

        },


        /*输入关键词的按键事件*/
        searchKeywordKeyupHandler: function (event) {
            var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
            if (keyCode === 13) {
                homePage.btn_search.blur();
                homePage.btnSearchHandler();
            }


            var from = event.data;
            if (from === "top") {
                //如果内容不为空 显示清空按钮
                var keyWord = homePage.txt_header_search_keyword.val();
                homePage.txt_search_keyword.val(keyWord);
                if (isNullOrEmpty(keyWord)) {
                    homePage.btn_search_top_clear.hide();
                } else {
                    homePage.btn_search_top_clear.show();
                }
            } else {
                //如果内容不为空 显示清空按钮
                var keyWord = homePage.txt_search_keyword.val();
                homePage.txt_header_search_keyword.val(keyWord);
                if (isNullOrEmpty(keyWord)) {
                    homePage.btn_search_clear.hide();
                } else {
                    homePage.btn_search_clear.show();
                }
            }

        },

        /*search handler
        搜索按钮点击事件，将关键词发送到服务端进行检索商品
        */
        btnSearchHandler: function () {
            console.log("has btnSearchHandler");

            var keyWord = homePage.txt_search_keyword.val();
            if (isNullOrEmpty(keyWord)) {
                var warnInfo = "请输入：" + homePage.txt_search_keyword.attr("placeholder");
                MessageBox.toast(warnInfo);
                return;
            }

            //向api发送商品搜索
            var paras = new BaseFetchWebPageArgument();
            // 关键词
            paras.KeyWord = keyWord;

            homePage.searchProductsHandler(paras);



        },
        /*触发搜索商品总入口*/
        searchProductsHandler: function (paras) {
            //清空原来的品牌 tags 等过滤
            homePage.resetFilterCateZone();


            homePage.handler_api_search_tmall_products(paras);
            homePage.handler_api_search_taobao_products(paras);
            homePage.handler_api_search_jd_products(paras);
            homePage.handler_api_search_pdd_products(paras);
            homePage.handler_api_search_guomei_products(paras);
            homePage.handler_api_search_suning_products(paras);
            homePage.handler_api_search_dangdang_products(paras);
        },
        //天猫商品检索
        handler_api_search_tmall_products: function (paras) {
            var queryAddress = homePage.api_search_tmall_products;
            if (isNullOrUndefined(paras)) {
                throw new Error("天猫参数不正确！");
                return;
            }
            paras.Platform = SupportPlatformEnum.Tmall;
            //1 天猫的区域显示加载图标
            httpClient.post(queryAddress, paras, homePage.callBackHandler_api_search_tmall_products);
        },
        //淘宝商品检索
        handler_api_search_taobao_products: function (paras) {
            var queryAddress = homePage.api_search_taobao_products;
            if (isNullOrUndefined(paras)) {
                throw new Error("淘宝参数不正确！");
                return;
            }
            paras.Platform = SupportPlatformEnum.Taobao;
            httpClient.post(queryAddress, paras, homePage.callBackHandler_api_search_taobao_products);
        },
        //京东商品检索
        handler_api_search_jd_products: function (paras) {
            var queryAddress = homePage.api_search_jd_products;
            if (isNullOrUndefined(paras)) {
                throw new Error("京东参数不正确！");
                return;
            }
            paras.Platform = SupportPlatformEnum.Jingdong;
            httpClient.post(queryAddress, paras, homePage.callBackHandler_api_search_jd_products);
        },
        //拼多多商品检索
        handler_api_search_pdd_products: function (paras) {
            var queryAddress = homePage.api_search_pdd_products;
            if (isNullOrUndefined(paras)) {
                throw new Error("拼多多参数不正确！");
                return;
            }
            paras.Platform = SupportPlatformEnum.Pdd;
            httpClient.post(queryAddress, paras, homePage.callBackHandler_api_search_pdd_products);
        },
        //国美商品检索
        handler_api_search_guomei_products: function (paras) {
            var queryAddress = homePage.api_search_guomei_products;
            if (isNullOrUndefined(paras)) {
                throw new Error("国美参数不正确！");
                return;
            }
            paras.Platform = SupportPlatformEnum.Guomei;
            httpClient.post(queryAddress, paras, homePage.callBackHandler_api_search_guomei_products);
        },
        //苏宁商品检索
        handler_api_search_suning_products: function (paras) {
            var queryAddress = homePage.api_search_suning_products;
            if (isNullOrUndefined(paras)) {
                throw new Error("苏宁参数不正确！");
                return;
            }
            paras.Platform = SupportPlatformEnum.Suning;
            httpClient.post(queryAddress, paras, homePage.callBackHandler_api_search_suning_products);
        },
        ////唯品会商品检索
        //handler_api_search_vip_products: function () {
        //    var queryAddress = homePage.api_search_vip_products;
        //    var paras = {};
        //    httpClient.post(queryAddress, paras, homePage.callBackHandler_api_search_vip_products);
        //},
        //当当商品检索
        handler_api_search_dangdang_products: function (paras) {
            var queryAddress = homePage.api_search_dangdang_products;
            if (isNullOrUndefined(paras)) {
                throw new Error("当当参数不正确！");
                return;
            }
            paras.Platform = SupportPlatformEnum.Dangdang;
            httpClient.post(queryAddress, paras, homePage.callBackHandler_api_search_dangdang_products);
        },



        //淘宝天猫优惠券检索
        handler_api_search_taoquan: function () {
            var queryAddress = homePage.api_search_taoquan;
            var paras = {};
            httpClient.post(api_search_taoquan, paras, homePage.callBackHandler_api_search_taoquan);
        },

        /*重置筛选区域*/
        resetFilterCateZone: function () {

            //1 清空品牌 
            homePage.container_category_brand.children("li").remove();
            // 2清空tags
            $("#v3-filter").find("div.facets-category-common").remove();
        },
        /*渲染品牌*/
        renderBrandsHandler: function (lstBrands) {
            if (isNullOrUndefined(lstBrands)) {
                return;
            }

            //////////////1 检查本地是否有品牌集合对象
            //////////////2 追加到品牌区域 并追加到本地集合对象

            var sb_BrandHtml = new StringBuilder();
            var conter_brand = 0;
            lstBrands.forEach(function (itemBrand) {

                //优先检索是否已经存在此名称的品牌
                var filterExists = "li.c-brand>a[name='{0}']".format(itemBrand.BrandName);
                var existsBrandDom = homePage.container_category_brand.find(filterExists);
                var isHasExistBrand = existsBrandDom.length > 0 ? true : false;

                if (!isHasExistBrand) {


                    //显示模式
                    sb_BrandHtml.Append('<li class="c-brand" brand-value="{0}">'.format(itemBrand.CharIndex));

                    var content =
                        '<a href="javascript:void(0)"    class="facet" name="{3}" {4}>{3}<i></i><dl class="brandBox"><dd data-id="{0}"  data-filter="{1}" data-platfom="{2}" data-name="{3}"></dd></dl></a>'
                            .format(itemBrand.BrandId,
                            itemBrand.FilterField,
                            itemBrand.Platform,
                            itemBrand.BrandName,
                            "{0}");

                    if (!isNullOrEmpty(itemBrand.IconUrl)) {
                        //有图模式
                        var imageMode = 'style ="background-image:url({0})" '.format(itemBrand.IconUrl);
                        sb_BrandHtml.Append(
                            content.format(imageMode)
                        );
                    } else {
                        //无图模式
                        sb_BrandHtml.Append(
                            content.format('style ="text-indent:0;"')
                        );
                    }


                    sb_BrandHtml.Append('</li>');

                } else {
                    //针对已经存在同名的品牌，汇聚到同名节点中，并检查是否可以替换图
                    var brandBox = existsBrandDom.find(".brandBox");

                    var cellBrand = '<dd data-id="{0}"  data-filter="{1}" data-platfom="{2}" data-name="{3}"></dd>'.format(itemBrand.BrandId,
                        itemBrand.FilterField,
                        itemBrand.Platform,
                        itemBrand.BrandName);
                    //追加到容器
                    brandBox.append(cellBrand);

                    ////是否有图
                    //if (!isNullOrEmpty(itemBrand.IconUrl)) {
                    //    var hasImage = brandBox.parent().css("background-image").indexOf("url") < 0 ? false : true;
                    //    if (!hasImage) {
                    //        var imgValue = 'url({0})'.format(itemBrand.IconUrl);
                    //        brandBox.parent().removeAttr("style");
                    //        brandBox.parent().css("background-image", imgValue);
                    //    }

                    //}
                }


            });

            //追加品牌节点
            homePage.container_category_brand.append(sb_BrandHtml.ToString());

            //绑定事件
            var allBrandDoms = homePage.container_category_brand.find("li.c-brand>a");
            allBrandDoms.click(function () {
                var sender = $(this);
                console.log(sender.attr("name"));

            });

        },

        /*渲染tags*/
        renderTagsHandler: function (lstTags) {
            if (isNullOrUndefined(lstTags)) {
                return;
            }

            //对每个tagGroup 进行渲染独立的区域
            //1 对相同名称的group 进行合并
            // 2 groupname 不同的，group  放到独立的区域，显示tag 最多的区域
            // 3相同名称的 放到一个组
            // 4 超过4个大区域，第五个区域放剩余的taggroup，可以下拉显示tag 那种


        },

        /*处理接收天猫的数据*/
        callBackHandler_api_search_tmall_products: function (jsonData, callBackParas) {
            console.log('callBackHandler_api_search_tmall_products');
            if (isNullOrUndefined(jsonData) || isNullOrUndefined(jsonData.Data)) {
                return;
            }
            var data = jsonData.Data;

            if (data.IsNeedResolveHeaderTags) {
                if (data.Brands && data.Brands.length > 0) {
                    homePage.renderBrandsHandler(data.Brands);
                }
                if (data.Tags && data.Tags.length > 0) {
                    homePage.renderTagsHandler(data.Tags);
                }
            }

            if (data.Products && data.Products.length > 0) {
                renderProductsHandler(data.Products);
            }

        },

        /*处理接收淘宝的数据*/
        callBackHandler_api_search_taobao_products: function (jsonData) {
            console.log('callBackHandler_api_search_taobao_products');
            var data = jsonData.Data;
            if (data.IsNeedResolveHeaderTags) {
                if (data.Brands && data.Brands.length > 0) {
                    homePage.renderBrandsHandler(data.Brands);
                }
                if (data.Tags && data.Tags.length > 0) {
                    homePage.renderTagsHandler(data.Tags);
                }
            }
        },
        callBackHandler_api_search_jd_products: function (jsonData) {
            console.log('callBackHandler_api_search_jd_products');
            var data = jsonData.Data;

            if (data.IsNeedResolveHeaderTags) {
                if (data.Brands && data.Brands.length > 0) {
                    homePage.renderBrandsHandler(data.Brands);
                }
                if (data.Tags && data.Tags.length > 0) {
                    homePage.renderTagsHandler(data.Tags);
                }
            }
        },

        callBackHandler_api_search_pdd_products: function (jsonData) {
            console.log('callBackHandler_api_search_pdd_products');

        },
        //callBackHandler_api_search_vip_products: function (jsonData) {
        //    console.log('callBackHandler_api_search_vip_products');
        //    processSearchGoodsData(jsonData);
        //},
        callBackHandler_api_search_guomei_products: function (jsonData) {
            console.log('callBackHandler_api_search_guomei_products');
            var data = jsonData.Data;

            if (data.IsNeedResolveHeaderTags) {
                if (data.Brands && data.Brands.length > 0) {
                    homePage.renderBrandsHandler(data.Brands);
                }
                if (data.Tags && data.Tags.length > 0) {
                    homePage.renderTagsHandler(data.Tags);
                }
            }
        },
        callBackHandler_api_search_suning_products: function (jsonData) {
            console.log('callBackHandler_api_search_suning_products');
            if (isNullOrUndefined(jsonData)) {
                return;
            }

            var data = jsonData.Data;

            if (data.IsNeedResolveHeaderTags) {
                if (data.Brands && data.Brands.length > 0) {
                    homePage.renderBrandsHandler(data.Brands);
                }
                if (data.Tags && data.Tags.length > 0) {
                    homePage.renderTagsHandler(data.Tags);
                }
            }
        },
        //callBackHandler_api_search_yhd_products: function (jsonData) {
        //    console.log('callBackHandler_api_search_yhd_products');
        //    processSearchGoodsData(jsonData);
        //},
        //callBackHandler_api_search_mls_products: function (jsonData) {
        //    console.log('callBackHandler_api_search_mls_products');
        //    processSearchGoodsData(jsonData);
        //},
        //callBackHandler_api_search_mgj_products: function (jsonData) {
        //    console.log('callBackHandler_api_search_mgj_products');
        //    processSearchGoodsData(jsonData);
        //},
        callBackHandler_api_search_dangdang_products: function (jsonData) {
            console.log('callBackHandler_api_search_dangdang_products');

            var data = jsonData.Data;

            if (data.IsNeedResolveHeaderTags) {
                if (data.Brands && data.Brands.length > 0) {
                    homePage.renderBrandsHandler(data.Brands);
                }
                if (data.Tags && data.Tags.length > 0) {
                    homePage.renderTagsHandler(data.Tags);
                }
            }
        },

        //callBackHandler_api_search_etao_products: function (jsonData) {
        //    console.log('callBackHandler_api_search_etao_products');
        //    processSearchGoodsData(jsonData);
        //},
        callBackHandler_api_search_taoquan: function (jsonData) {
            console.log('callBackHandler_api_search_taoquan');

        },
    };

    //regist to global
    FlyBirdYoYo.HomePage = homePage;
    //init page object
    homePage.init();
});