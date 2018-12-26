/*
此文件中 封装了一些基本的js辅助方法
包括字符串的格式化  日期  数组扩展  类型判断等
*/
/*
获取当前URL中的参数
abc.html?id=123&url=http://www.maidq.com
方法去调用：alert(GetQueryString("url"));
*/
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)","i");//取url中的键值对 忽略大小写
    var r = window.location.search.substr(1).match(reg);
    if (r != null) {
        return unescape(r[2]);
    }

    return null;
}

/*格式化指定的url 。json参数对象，格式化为拼接好的参数
示范：baidu.com?bd=99&a=3fr&b=45
*/
var formatUrl = function (url, paras) {
    if (isNullOrEmpty(url)) {
        return null;
    }

    //起始字符
    var startChar = '?';
    if (url.indexOf('?') > -1) {
        startChar = '&';
    }

    var paramStr = "";


    for (var key in paras) {
        var value = paras[key];
        if (isNumber(value) || isString(value)) {
            paramStr += '&' + key + '=' + encodeURIComponent(value);
        }

    }

    return url + startChar + paramStr.substr(1);

}

/*
定义JQuery中对选择的集合Elements 进行迭代
*/
if (jQuery && !jQuery.fn.forEach) {
    $(function () {
        (function ($) {
            $.fn.extend({
                /**
                 * 扩展Jq对象的遍历，对Jq对象进行迭代
                 * @param {} predicate 
                 * @returns {} 
                 */
                forEach: function (predicate) {

                    if (this == null) {
                        throw new TypeError(' this is null or not defined');
                    }
                    // 1. Let O be the result of calling toObject() passing the
                    // |this| value as the argument.
                    var O = Object(this);

                    // 2. If isCallable(predicate) is false, throw a TypeError exception. 
                    if (typeof predicate !== "function") {
                        throw new TypeError(predicate + ' is not a function');
                    }

                    //3 call the jq  original API  for iteror
                    $.each(O, function (index, domEle) {
                        predicate($(domEle));
                    });
                }
            })
        })(jQuery);

    });
}

/*定义 html 标签的文本变化事件*/
if (jQuery && !jQuery.fn.onTextChanged) {
    $(function () {
        (function ($) {
            $.fn.extend({
                /**
                 * 扩展html 标记对象的文本变更事件
                 * @param {} callbackHandler 
                 * @returns 返回上次变更前的文本
                 */
                onTextChanged: function (callbackHandler) {
                    //回调函数必须是function
                    if (!isFunction(callbackHandler)) {
                        return;
                    }

                    //验证当前对象
                    var sender = $(this);
                    if (!isNullOrUndefined(this)) {
                        //获取当前p的文本
                        var currentText = sender.text();
                        if (isNullOrEmpty(currentText)) {
                            currentText = "";
                        }
                        var timer = new Timer(500);
                        timer.Elapsed = function () {
                            var latestText = sender.text();
                            if (isNullOrEmpty(latestText)) {
                                latestText = "";
                            }
                            //长度 或者文本发生变更 都算是 变更 
                            if (latestText.length != currentText.length || latestText != currentText) {
                                callbackHandler(currentText);//执行回调
                                currentText = latestText;

                            }
                        }
                        timer.Start();
                    }

                }
            })
        })(jQuery);

    });
}


/**
 * 返回 Select HTML元素的选择的项的索引
 * @param {} domObj 
 * @returns {} 
 */
var getSelectDomSelectedIndex = function (domObj) {

    var result = -1;
    if (isNullOrUndefined(domObj)) {
        return result;
    }

    result = domObj.prop('selectedIndex');
    if (isNullOrUndefined(result)) {
        result = -1;
    }

    return result;
}


/*字符串拼接 powered by wali
    Append():添加待拼接的字符串
    ToString():将待拼接的字符串拼接成字符串返回  split参数：已什么符号分割
    Clear():清空待拼接对象
    Remove():移出某个待拼接的对象 n参数：索引
*/
var StringBuilder = function () {
    this.data = [];
    this.Append = function () {
        this.data.push(arguments[0]);
        return this;
    };
    this.ToString = function (split) {
        if (typeof split == "undefined") {
            return this.data.join('');
        }
        return this.data.join(split);
    };
    this.Clear = function () {
        this.data = [];
    };
    this.IsEmpty = function () {
        return this.data.length > 0 ? false : true;
    };
    this.Remove = function (n) {
        if (n < 0)
            return this.data;
        else {
            this.data = this.data.slice(0, n).concat(this.data.slice(n + 1, this.length));
            return this.data;
        }
    };
};

//对象Obj 打印为字符串
function obj2string(o) {
    var r = [];
    if (typeof o == "string") {
        return "\"" + o.replace(/([\'\"\\])/g, "\\$1").replace(/(\n)/g, "\\n").replace(/(\r)/g, "\\r").replace(/(\t)/g, "\\t") + "\"";
    }
    if (typeof o == "object") {
        if (!o.sort) {
            for (var i in o) {
                r.push(i + ":" + obj2string(o[i]));
            }
            if (!!document.all && !/^\n?function\s*toString\(\)\s*\{\n?\s*\[native code\]\n?\s*\}\n?\s*$/.test(o.toString)) {
                r.push("toString:" + o.toString.toString());
            }
            r = "{" + r.join() + "}";
        } else {
            for (var i = 0; i < o.length; i++) {
                r.push(obj2string(o[i]))
            }
            r = "[" + r.join() + "]";
        }
        return r;
    }
    return o.toString();
}
/*获取上传input中的文件大小
*/
function getFileSize(targetFileUploadInput) {
    var fileSize = 0;

    if (isNullOrUndefined(targetFileUploadInput)) {
        return -1;
    }
    if (targetFileUploadInput instanceof jQuery) {
        targetFileUploadInput = targetFileUploadInput[0];
    }
    if (isIE() && !targetFileUploadInput.files) {    // IE浏览器

        var filePath = targetFileUploadInput.value; // 获得上传文件的绝对路径
        var fileSystem = new ActiveXObject("Scripting.FileSystemObject");
        // GetFile(path) 方法从磁盘获取一个文件并返回。
        var file = fileSystem.GetFile(filePath);
        fileSize = file.Size;    // 文件大小，单位：b
    }
    else {    // 非IE浏览器
        fileSize = targetFileUploadInput.files[0].size;
    }

    return fileSize;
}
/*
压缩json字符串
 * @param  inputString  
  * @param  ii  ；1 压缩 2 转义 3 压缩并转义 4 去除转义
 * @returns {} 
*/
var compressJsonStr = function (inputString, ii) {
    var text = inputString;
    if ((ii == 1 || ii == 3)) {
        text = text.split("\n").join(" ");
        var t = [];
        var inString = false;
        for (var i = 0, len = text.length; i < len; i++) {
            var c = text.charAt(i);
            if (inString && c === inString) {
                if (text.charAt(i - 1) !== '\\') {
                    inString = false;
                }
            }
            else if (!inString && (c === '"' || c === "'")) {
                inString = c;
            }
            else if (!inString && (c === ' ' || c === "\t")) {
                c = '';
            }
            t.push(c);
        }
        text = t.join('');
    }
    if ((ii == 2 || ii == 3)) {
        text = text.replace(/\\/g, "\\\\").replace(/\"/g, "\\\"");
    }
    if (ii == 4) {
        text = text.replace(/\\\\/g, "\\").replace(/\\\"/g, '\"');
    }
    return text;
};

/*
 重写数值类型的精度取值。解决在不同浏览器中的计算精度的问题
 https://www.cnblogs.com/wangsaiming/p/4644790.html
 */
Number.prototype.toFixed = function (d) {
    var s = this + "";
    if (!d) d = 0;
    if (s.indexOf(".") == -1) s += ".";
    s += new Array(d + 1).join("0");
    if (new RegExp("^(-|\\+)?(\\d+(\\.\\d{0," + (d + 1) + "})?)\\d*$").test(s)) {
        var s = "0" + RegExp.$2, pm = RegExp.$1, a = RegExp.$3.length, b = true;
        if (a == d + 2) {
            a = s.match(/\d/g);
            if (parseInt(a[a.length - 1]) > 4) {
                for (var i = a.length - 2; i >= 0; i--) {
                    a[i] = parseInt(a[i]) + 1;
                    if (a[i] == 10) {
                        a[i] = 0;
                        b = i != 1;
                    } else break;
                }
            }
            s = a.join("").replace(new RegExp("(\\d+)(\\d{" + d + "})\\d$"), "$1.$2");

        } if (b) s = s.substr(1);
        return (pm + s).replace(/\.$/, "");
    } return this + "";

};

/**
 * 扩展js 中的string 函数contains
 * @param {} substr 
 * @returns {} 
 */
String.prototype.contains = function (substr) {
    return this.indexOf(substr) >= 0;
};
/**
 * 扩展js 中的string 函数 toDate, 字符串转换为时间类型
 * @param {} substr 
 * @returns {} 
 */
String.prototype.toDate = function () {

    //var remindTime = "2008-04-02 10:08:44";
    var str = this;
    str = str.replace(/-/g, "/");
    str = str.replace("T", " ");
    var oDate = new Date(str);
    return oDate;
};

/**
 * 扩展js 中的string 函数format
 * @param {} args ，参数数组
 * @returns {} 
 */
String.prototype.format = function () {
    var args = arguments;
    return this.replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] != 'undefined'
            ? args[number]
            : match;
    });
};





// 对Date的扩展，将 Date 转化为指定格式的String
// 月(M)、日(d)、小时(H)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(f)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd HH:mm:ss.f") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d H:m:s.f")      ==> 2006-7-2 8:9:4.18 

Date.prototype.isValid = function () {
    // An invalid date object returns NaN for getTime() and NaN is the only
    // object not strictly equal to itself.
    return this.getTime() === this.getTime();
};  
Date.prototype.Format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "H+|h+": this.getHours(), //小时 --------*********仅支持24小时制**************
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "f+": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
};

//将指定的毫秒数加到此实例的值上 
Date.prototype.addMilliseconds = function (value) {
    var millisecond = this.getMilliseconds();
    this.setMilliseconds(millisecond + value);
    return this;
};
//将指定的秒数加到此实例的值上 
Date.prototype.addSeconds = function (value) {
    var second = this.getSeconds();
    this.setSeconds(second + value);
    return this;
};
//将指定的分钟数加到此实例的值上 
Date.prototype.addMinutes = function (value) {
    var minute = this.addMinutes();
    this.setMinutes(minute + value);
    return this;
};
//将指定的小时数加到此实例的值上 
Date.prototype.addHours = function (value) {
    var hour = this.getHours();
    this.setHours(hour + value);
    return this;
};
//将指定的天数加到此实例的值上 
Date.prototype.addDays = function (value) {
    var date = this.getDate();
    this.setDate(date + value);
    return this;
};
//将指定的星期数加到此实例的值上 
Date.prototype.addWeeks = function (value) {
    return this.addDays(value * 7);
};
//将指定的月份数加到此实例的值上 
Date.prototype.addMonths = function (value) {
    var month = this.getMonth();
    this.setMonth(month + value);
    return this;
};
//将指定的年份数加到此实例的值上 
Date.prototype.addYears = function (value) {
    var year = this.getFullYear();
    this.setFullYear(year + value);
    return this;
};
//获取时间的间隔（天）
Date.prototype.dateDiff = function (endDate) {
    var startDate = this;


    var startTime = new Date(Date.parse(startDate.replace(/-/g, "/"))).getTime();
    var endTime = new Date(Date.parse(endDate.replace(/-/g, "/"))).getTime();
    var days = Math.abs((startTime - endTime)) / (1000 * 60 * 60 * 24);
    return days;
};
//获取当前的日期时间 格式“yyyy-MM-dd HH:mm:ss”
Date.prototype.getNow = function () {
    return new Date().Format("yyyy-MM-dd HH:mm:ss");
}
//获取当前的日期的开始时间 格式“yyyy-MM-dd 00:00:00”
Date.prototype.getTodayBegin = function () {
    var now = this.Format("yyyy-MM-dd");
    return now + " 00:00:00";
}
//获取当前的日期的终止时间 格式“yyyy-MM-dd 23:59:59”
Date.prototype.getTodayEnd = function () {
    var now = this.Format("yyyy-MM-dd");
    return now + " 23:59:59";
}

/**
 * 自定义数据结构-C#方式的迭代
 */

Array.prototype.add = function (obj) {
    this.push(obj);
};
Array.prototype.addRange = function (items) {
    for (var i = 0; i < items.length; i++) {
        this.add(items[i]);
    }
};
/**
 * 清空数组
 * @returns {} 
 */
Array.prototype.clear = function () {
    this.splice(0, this.length);
};
Array.prototype.contains = function (obj) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] === obj) {
            return true;
        }
    }
    return false;
};
Array.prototype.convertAll = function (converter) {
    var list = new Array();
    for (var i = 0; i < this.length; i++) {
        list.add(converter(this[i]));
    }
    return list;
};
Array.prototype.remove = function (predicate) {
    for (var i = 0; i < this.length; i++) {
        if (predicate(this[i])) {
            this.splice(i, 1);//移除这个索引的元素
        }
    }
};

Array.prototype.removeAt = function (index) {
    if ((this[index])) {
        this.splice(index, 1);//移除这个索引的元素
    }

};



Array.prototype.find = function (predicate) {
    for (var i = 0; i < this.length; i++) {
        if (predicate(this[i])) {
            return this[i];
        }
    }
    return null;
};
Array.prototype.findAll = function (predicate) {
    var results = new Array();
    for (var i = 0; i < this.length; i++) {
        if (predicate(this[i])) {
            results.add(this[i]);
        }
    }
    return results;
};
Array.prototype.findIndex = function (predicate, index) {
    if (index === void 0) { index = 0; }
    for (var i = index || 0; i < this.length; i++) {
        if (predicate(this[i])) {
            return i;
        }
    }
    return -1;
};
Array.prototype.findLastIndex = function (predicate, index) {
    if (index === void 0) { index = this.length; }
    for (var i = index; i > -1; i--) {
        if (predicate(this[i])) {
            return i;
        }
    }
    return -1;
};
Array.prototype.forEach = function (action) {
    for (var i = 0; i < this.length; i++) {
        action(this[i]);
    }
};
/**
 * 以jqery 对象的方式迭代对象
 * @param {} action 
 * @returns {} 
 */
Array.prototype.forEachJq = function (action) {
    for (var i = 0; i < this.length; i++) {
        action($(this[i]));
    }
};
Array.prototype.getItem = function (index) {
    if (this[index]) {
        return this[index];
    }
};
Array.prototype.setItem = function (index, obj) {
    this[index] = obj;
};
Array.prototype.toArray = function () {
    var arr = [];
    for (var i = 0; i < this.length; i++) {
        arr.push(this[i]);
    }
    return arr;
};
//将数组转为逗号分割字符串
Array.prototype.toString = function (t) {
    //var str = "";
    //t=t ? t : ',';
    //for (var i = 0; i < this.length; i++) str += this[i] + t;
    //return str.length>0?str.substring(0,str.length-1):"";

    return this.join(t);
};
//Array.prototype.where = function (func) {
//    //for (var i = 0; i < this.length; i++) {
//    //    if (func(this[i])) return this[i];
//    //}
//    return this.findAll(func);
//};


/**
 * 分组
 * Demo:
 * var arr=[{"id":1,"name":"A"},
{"id":1,"name":"B"},
{"id":2,"name":"C"},];

    var g=arr.groupBy("id");
    for (var i = 0; i < g.Keys.length; i++) {
             var gv=g.GetItem(g.Keys[i]);
		     for (var k = 0; k < gv.length; k++) {
			     document.writeln(gv[k].name)
		     }
    }
 * @param {} key 属性（仅仅支持单个属性），并且key的值属性，不可为空
 * @returns {}  返回Map字典
 */
Array.prototype.groupBy = function (key) {

    if (isNullOrUndefined(key)) {
        throw new error("key is null!");
        return null;
    }

    var dicMap = new Map();

    for (var i = 0; i < this.length; i++) {
        var item = this[i];
        if (item && item[key]) {
            //存在这个属性，并有值
            var gKey = item[key];
            if (dicMap.Has(gKey)) {
                var hashObj = dicMap.GetItem(gKey);
                hashObj.push(item);//存在键，追加
            } else {
                //不存在存在键，创建
                var value = [];
                value.push(item);
                dicMap.Add(gKey, value);
            }
        }



    }
    return dicMap;
};

Array.prototype.trueForAll = function (predicate) {
    var results = new Array();
    for (var i = 0; i < this.length; i++) {
        if (!predicate(this[i])) {
            return false;
        }
    }
    return true;
};
Array.prototype.firstOrDefault = function (predicate) {
    if (!predicate || !isFunction(predicate)) {
        throw new Error("must use function type as predicate");
    }
    return this.find(predicate);

};
/** 
 * Map 字典类，使用方式：
 *   var map = new Map<string>();
       map.Add("a","111");
       map.Add("b", "222");
       map.Add("c", "333");

       var keys = map.getKeys();

       for (var i = 0; i < keys.length; i++) {
           document.write(map.GetItem(keys[i]));
       }
*/
var Map = (function () {
    function Map() {
        this.Items = {};
        this._Count = 0;
        this._Keys = [];
        this._Values = [];
    }

    /**
*获取数目
*/
    Map.prototype.getCount = function () {
        return this._Count;
    };

    /**
*获取所有的键
*/
    Map.prototype.getKeys = function () {
        return this._Keys;
    };
    /**
    *获取所有的值
    */
    Map.prototype.getValues = function () {
        return this._Values;
    };

    /**
     * 添加减值对象
     * @param {} key 
     * @param {} value 
     * @returns {} 
     */
    Map.prototype.Add = function (key, value) {
        try {
            this._Count += 1;
            this._Keys.push(key);
            this._Values.push(value);
            this.Items[key] = value;
        }
        catch (e) {
        }
    };
    Map.prototype.Has = function (key) {
        return key in this.Items;
    };
    Map.prototype.GetItem = function (key) {
        return this.Items[key];
    };
    return Map;
})();


var List = (function () {
    function List(list) {
        this.list = [];
        this.list = list || [];
    }
    Object.defineProperty(List.prototype, "length", {
        get: function () {
            return this.list.length;
        },
        enumerable: true,
        configurable: true
    });

    List.prototype.getCount = function () {
        return this.list.length;
    };

    List.prototype.add = function (obj) {
        this.list.push(obj);
    };
    List.prototype.addRange = function (items) {
        this.list = this.list.concat(items);
    };
    List.prototype.clear = function () {
        this.list = [];
    };
    List.prototype.contains = function (obj) {
        for (var i = 0; i < this.list.length; i++) {
            if (this.list[i] === obj) {
                return true;
            }
        }
        return false;
    };
    List.prototype.convertAll = function (converter) {
        var list = new List();
        for (var i = 0; i < this.list.length; i++) {
            list.add(converter(this.list[i]));
        }
        return list;
    };
    List.prototype.remove = function (predicate) {
        for (var i = 0; i < this.list.length; i++) {
            if (predicate(this.list[i])) {
                this.list.splice(i, 1);//移除这个索引的元素
            }
        }
    };

    List.prototype.removeAt = function (index) {
        if ((this.list[index])) {
            this.list.splice(index, 1);//移除这个索引的元素
        }

    };



    List.prototype.find = function (predicate) {
        for (var i = 0; i < this.list.length; i++) {
            if (predicate(this.list[i])) {
                return this.list[i];
            }
        }
        return null;
    };

    List.prototype.firstOrDefault = function (predicate) {
        if (!predicate || !isFunction(predicate)) {
            throw new Error("must use function type as List firstOrDefault predicate");
        }
        return this.find(predicate);

    };
    List.prototype.findAll = function (predicate) {
        var results = new List();
        for (var i = 0; i < this.list.length; i++) {
            if (predicate(this.list[i])) {
                results.add(this.list[i]);
            }
        }
        return results;
    };
    List.prototype.findIndex = function (predicate, index) {
        if (index === void 0) { index = 0; }
        for (var i = index || 0; i < this.list.length; i++) {
            if (predicate(this.list[i])) {
                return i;
            }
        }
        return -1;
    };
    List.prototype.findLastIndex = function (predicate, index) {
        if (index === void 0) { index = this.length; }
        for (var i = index; i > -1; i--) {
            if (predicate(this.list[i])) {
                return i;
            }
        }
        return -1;
    };
    List.prototype.forEach = function (action) {
        for (var i = 0; i < this.list.length; i++) {
            action(this.list[i]);
        }
    };

    /**
     * 以jqery 对象的方式迭代对象
     * @param {} action 
     * @returns {} 
     */
    List.prototype.forEachJq = function (action) {
        for (var i = 0; i < this.list.length; i++) {
            action($(this.list[i]));
        }
    };

    List.prototype.getItem = function (index) {
        if (this.list[index]) {
            return this.list[index];
        }
    };
    List.prototype.setItem = function (index, obj) {
        this.list[index] = obj;
    };
    List.prototype.toArray = function () {
        var arr = [];
        for (var i = 0; i < this.list.length; i++) {
            arr.push(this.list[i]);
        }
        return arr;
    };
    /**
     * 分组
     * Demo:
     * var arr=[{"id":1,"name":"A"},
    {"id":1,"name":"B"},
    {"id":2,"name":"C"},];
    
        var g=arr.groupBy("id");
        for (var i = 0; i < g.Keys.length; i++) {
                 var gv=g.GetItem(g.Keys[i]);
                 for (var k = 0; k < gv.length; k++) {
                     document.writeln(gv[k].name)
                 }
        }
     * @param {} key 属性（仅仅支持单个属性），并且key的值属性，不可为空
     * @returns {}  返回Map字典
     */
    List.prototype.groupBy = function (key) {

        if (isNullOrUndefined(key)) {
            throw new error("key is null!");
            return null;
        }

        var dicMap = new Map();

        for (var i = 0; i < this.list.length; i++) {
            var item = this.list[i];
            if (item && item[key]) {
                //存在这个属性，并有值
                var gKey = item[key];
                if (dicMap.Has(gKey)) {
                    var hashObj = dicMap.GetItem(gKey);
                    hashObj.push(item);//存在键，追加
                } else {
                    //不存在存在键，创建
                    var value = [];
                    value.push(item);
                    dicMap.Add(gKey, value);
                }
            }



        }
        return dicMap;
    };


    List.prototype.trueForAll = function (predicate) {
        var results = new List();
        for (var i = 0; i < this.list.length; i++) {
            if (!predicate(this.list[i])) {
                return false;
            }
        }
        return true;
    };
    return List;
}());




/**
 * @ngdoc function
 * @name sUndefined
 * @module ng
 * @kind function
 *
 * @description
 * Determines if a reference is undefined.
 *
 * @param {*} value Reference to check.
 * @returns {boolean} True if `value` is undefined.
 */
function isUndefined(value) { return typeof value === 'undefined'; }


/**
 * @ngdoc function
 * @name sDefined
 * @module ng
 * @kind function
 *
 * @description
 * Determines if a reference is defined.
 *
 * @param {*} value Reference to check.
 * @returns {boolean} True if `value` is defined.
 */
function isDefined(value) { return typeof value !== 'undefined'; }


/**
 * @ngdoc function
 * @name sObject
 * @module ng
 * @kind function
 *
 * @description
 * Determines if a reference is an `Object`. Unlike `typeof` in JavaScript, `null`s are not
 * considered to be objects. Note that JavaScript arrays are objects.
 *
 * @param {*} value Reference to check.
 * @returns {boolean} True if `value` is an `Object` but not `null`.
 */
function isObject(value) {
    // http://jsperf.com/isobject4
    return value !== null && typeof value === 'object';
}


/**
 * Determine if a value is an object with a null prototype
 *
 * @returns {boolean} True if `value` is an `Object` with a null prototype
 */
function isBlankObject(value) {
    return value !== null && typeof value === 'object' && !getPrototypeOf(value);
}


/**
 * @ngdoc function
 * @name sString
 * @module ng
 * @kind function
 *
 * @description
 * Determines if a reference is a `String`.
 *
 * @param {*} value Reference to check.
 * @returns {boolean} True if `value` is a `String`.
 */
function isString(value) { return typeof value === 'string'; }


/**
 * @ngdoc function
 * @name sNumber
 * @module ng
 * @kind function
 *
 * @description
 * Determines if a reference is a `Number`.
 *
 * This includes the "special" numbers `NaN`, `+Infinity` and `-Infinity`.
 *
 * If you wish to exclude these then you can use the native
 * [`isFinite'](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/isFinite)
 * method.
 *
 * @param {*} value Reference to check.
 * @returns {boolean} True if `value` is a `Number`.
 */

function isNumber(value) {

    return !isNaN(parseFloat(value)) && isFinite(value);

}
/*重写js 内置的浮点精确函数
防止精度丢失问题
*/
function toFixed(num, s) {
    var times = Math.pow(10, s)
    var des = num * times + 0.5
    des = parseInt(des, 10) / times
    return des
}


/* 判断字符串 是否为数字
*/
function isNumberString(str) {

    if (isNullOrEmpty(str)) {
        return false;
    }

    return isNumber(str);

    //if (str.lastIndexOf('-') > 0) {
    //    return false;
    //}
    //var result = true;
    //var numStr = '0123456789';
    //var pos = 0;
    //if (str.lastIndexOf('-') === 0) {
    //    pos = 1;
    //}
    //for (var i = pos; i < str.length; i++) {
    //    var ch = str[i];

    //    if (numStr.indexOf(ch) < 0) {
    //        result = false;
    //        break;
    //    }
    //}
    //return result;
}


/**
 * @ngdoc function
 * @name sDate
 * @module ng
 * @kind function
 *
 * @description
 * Determines if a value is a date.
 *
 * @param {*} value Reference to check.
 * @returns {boolean} True if `value` is a `Date`.
 */
function isDate(value) {
    return Object.prototype.toString.call(value) === '[object Date]';
}


/**
 * @ngdoc function
 * @name sArray
 * @module ng
 * @kind function
 *
 * @description
 * Determines if a reference is an `Array`.
 *
 * @param {*} value Reference to check.
 * @returns {boolean} True if `value` is an `Array`.
 */


function isArray(value) {
    return Object.prototype.toString.call(value) === '[object Array]';
}
/**
 * 是否为List类型
 * @param {} value 
 * @returns {} 
 */
function isList(value) {
    return value instanceof List;
}

/**
 * @ngdoc function
 * @name sFunction
 * @module ng
 * @kind function
 *
 * @description
 * Determines if a reference is a `Function`.
 *
 * @param {*} value Reference to check.
 * @returns {boolean} True if `value` is a `Function`.
 */
function isFunction(value) { return typeof value === 'function'; }


/**
 * Determines if a value is a regular expression object.
 *
 * @private
 * @param {*} value Reference to check.
 * @returns {boolean} True if `value` is a `RegExp`.
 */
function isRegExp(value) {
    return Object.prototype.toString.call(value) === '[object RegExp]';
}


/**
 * Checks if `obj` is a window object.
 *
 * @private
 * @param {*} obj Object to check
 * @returns {boolean} True if `obj` is a window obj.
 */
function isWindow(obj) {
    return obj && obj.window === obj;
}


function isScope(obj) {
    return obj && obj.$evalAsync && obj.$watch;
}


function isFile(obj) {
    return Object.prototype.toString.call(obj) === '[object File]';
}


function isFormData(obj) {
    return Object.prototype.toString.call(obj) === '[object FormData]';
}


function isBlob(obj) {
    return Object.prototype.toString.call(obj) === '[object Blob]';
}


function isBoolean(value) {
    return typeof value === 'boolean';
}

function isNullOrUndefined(obj) {

    if (isUndefined(obj)) {
        return true;
    } else if (null == obj) {
        return true;
    }
    return false;
}

function isNullOrEmpty(str) {


    if (!isNullOrUndefined(str)) {
        if (!isString(str) || str.length > 0) {
            return false;
        }

    }
    return true;
}

function trimSpecialStr(str) {
    return str.replace("&#", "");
}

function trim(str) {
    return str.replace(/(^\s*)|(\s*$)/g, '');
}


function ltrim(str) {
    return str.replace(/^\s*/g, '');
}


function rtrim(str) {
    return str.replace(/\s*$/, '');
}


function equals(str1, str2) {
    if (str1 == str2) {
        return true;
    }
    return false;
}


function equalsIgnoreCase(str1, str2) {
    if (str1.toUpperCase() == str2.toUpperCase()) {
        return true;
    }
    return false;
}


function isChinese(str) {
    var str = str.replace(/(^\s*)|(\s*$)/g, '');
    if (!(/^[\u4E00-\uFA29]*$/.test(str)
            && (!/^[\uE7C7-\uE7F3]*$/.test(str)))) {
        return false;
    }
    return true;
}


function isEmail(str) {
    if (/^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$/.test(str)) {
        return true
    }
    return false;
}
/**
 * 
 * @param {} str image url or name
 * @returns {} 
 */
function isImgOfJpeg(str) {
    var objReg = new RegExp("[.]+(jpg|jpeg)$", "gi");
    if (objReg.test(str)) {
        return true;
    }
    return false;
}

function isImg(str) {
    var objReg = new RegExp("[.]+(jpg|jpeg|swf|gif|png)$", "gi");
    if (objReg.test(str)) {
        return true;
    }
    return false;
}


function isInteger(str) {
    if (/^-?\d+$/.test(str)) {
        return true;
    }
    return false;
}
/**
 * 是否为拉丁字符
 * @param {} str 
 * @returns {} 
 */
function isEnglishChar(str) {
    if (/^[a-z|A-Z]$/.test(str)) {
        return true;
    }
    return false;
}

function isFloat(str) {
    if (/^(-?\d+)(\.\d+)?$/.test(str)) {
        return true;
    }
    return false;
}


function isMobile(str) {
    if (/^1[35]\d{9}/.test(str)) {
        return true;
    }
    return false;
}


function isPhone(str) {
    if (/^(0[1-9]\d{1,2}-)\d{7,8}(-\d{1,8})?/.test(str)) {
        return true;
    }
    return false;
}

function isIP(str) {
    var reg = /^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$/;
    if (reg.test(str)) {
        return true;
    }
    return false;
}


function isDateTimeString(str) {
    if (isNullOrEmpty(str)) {
        return  false;
    }
    var time = str.toDate();
    return time.isValid();
}

/**
*仅仅把字符串中包含的中文转化为 unicode
 */
function GB2312ToUnicode(theString) {
    var theString = escape(theString).replace(/%u/gi, '\\u');
    return theString.replace(/%7b/gi, '{').replace(/%7d/gi, '}').replace(/%3a/gi, ':').replace(/%2c/gi, ',').replace(/%27/gi, '\'').replace(/%22/gi, '"').replace(/%5b/gi, '[').replace(/%5d/gi, ']');
}


/**
*字符串转化为 unicode
 */
function converCharToUnicode(theString) {
    var unicodeString = '';
    for (var i = 0; i < theString.length; i++) {
        var theUnicode = theString.charCodeAt(i).toString(16).toUpperCase();
        while (theUnicode.length < 4) {
            theUnicode = '0' + theUnicode;
        }
        theUnicode = '\\u' + theUnicode;
        unicodeString += theUnicode;
    }
    return unicodeString;
}

/**
* unicode转化为字符串
 */
function convertUnicodeToChar(str) {
    str = eval("'" + str + "'");
    return str;
}
function dec2hex(textString) {
    return (textString + 0).toString(16).toUpperCase();
}

// converts a single hex number to a character
// note that no checking is performed to ensure that this is just a hex number, eg. no spaces etc
// hex: string, the hex codepoint to be converted
function hex2char(hex) {
    var result = '';
    var n = parseInt(hex, 16);
    if (n <= 0xFFFF) { result += String.fromCharCode(n); }
    else if (n <= 0x10FFFF) {
        n -= 0x10000
        result += String.fromCharCode(0xD800 | (n >> 10)) + String.fromCharCode(0xDC00 | (n & 0x3FF));
    }
    else { result += 'hex2Char error: Code point out of range: ' + dec2hex(n); }
    return result;
}

/**
 * 生成一个guid标识
*如：e6ba7194-0d69-4499-bd36-710dc9c08a70
 * @returns {} 
 */
function generateUUID() {

    function randCode() {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    }

    var guid = (randCode() + randCode() + "-" + randCode() + "-4" + randCode().substr(0, 3) + "-" + randCode() + "-" + randCode() + randCode() + randCode()).toLowerCase();
    return guid;
}

/*
获取时间戳
getTime()返回数值的单位是毫秒
*/
function getTimeToken() {
    var timestamp = new Date().getTime();
    return timestamp;

}

function isIE() { //ie 6+
    if (!!window.ActiveXObject || "ActiveXObject" in window)
        return true;
    else
        return false;
}



/*!
 * console.js v0.2.0 (https://github.com/yanhaijing/console.js)
 * Copyright 2013 yanhaijing. All Rights Reserved
 * Licensed under MIT (https://github.com/yanhaijing/console.js/blob/master/MIT-LICENSE.txt)
 */
; (function (g) {
    'use strict';
    //judge if console has defined
    if (!isIE()) {// 非ie 浏览器 不用定义console
        return;
    }

    var _console = g.console || {};
    var methods = ['assert', 'clear', 'count', 'debug', 'dir', 'dirxml', 'exception', 'error', 'group', 'groupCollapsed', 'groupEnd', 'info', 'log', 'profile', 'profileEnd', 'table', 'time', 'timeEnd', 'timeStamp', 'trace', 'warn'];

    var console = { version: '0.2.0' };
    var key;
    for (var i = 0, len = methods.length; i < len; i++) {
        key = methods[i];
        console[key] = function (key) {
            return function () {
                if (typeof _console[key] === 'undefined') {
                    return 0;
                }
                // 添加容错处理
                try {
                    Function.prototype.apply.call(_console[key], _console, arguments);
                } catch (exp) {
                }
            };
        }(key);
    }

    g.console = console;
}(window));

//---------------------------------------------------js 实现 基于later的弹出框------begin-------------------------------------------

/*加载弹层框架-layer.js*/
////document.write("<script type='text/javascript'  src='/js/common/layer/layer.js'></script>");
/////*加载弹层框架-sweetalert*/
////document.write("<script type='text/javascript' src='/js/ServiceJs/sweetalert/sweetalert.min.js'></script>");
////document.write("<link type='text/css' href='/css/ServiceCss/sweetalert/sweetalert.css' rel='stylesheet'/>");
// 定义 全局MessageBox 对象
if (!window.MessageBox) {
    MessageBox = {

        /*显示右边箭头的消息提示*/
        "tips": function (whereSelector, msg) {
            return layer.tips(msg, whereSelector, {
                tips: [2, '#3595CC'],
                time: 4000
            });
        },
        /*显示错误的带箭头的红色消息提示，不自动消失*/
        "tipsError": function (whereSelector, msg) {
            return layer.tips(msg, whereSelector, {
                tips: [1, '#CA0C16'],
                time: 9000000000
            });
        },

        "toast": function (msg, onCloseHandler) {
            //信息框-例2
            var showTime = 2000;
            var timer = new Timer(showTime);
            var msgIndex = layer.msg(msg);
            timer.Elapsed = function () {
                layer.close(msgIndex);
                (function () {
                    if (!isNullOrUndefined(onCloseHandler)) {
                        onCloseHandler();//关闭窗口的时候，触发的事件回调
                    }
                })();

                //触发完毕后销毁timer
                timer.Stop();
                timer = null;
            };
            timer.Start();

        },
        /*显示加载圈圈*/
        "loading": function (config) {
            var options = {
                title: false,
                closeBtn: 0,
                btn: false,
                content: '数据加载中...',
                icon: 16,
                shade: 0.1,
                time: 1000000,
                area: ['150px', '66px'],
                maxWidth: 180,
                maxHeight: 28
            };

            if (!isNullOrUndefined(config)) {
                if (!isNullOrUndefined(config.time)) {
                    options.time = config.time;
                }
                if (!isNullOrUndefined(config.shade)) {
                    options.shade = config.shade;
                }
            }

            var index = layer.open(options);
            return index;
        },
        /*弹窗显示url页面*/
        "alertUrl" : function (url, title, width, height,onCloseHandler) {
            var config = {
                "title": title,
                "success":onCloseHandler,
                "area": ['500px', '300px'],
                "content":""
            }
            var bodyHeight = (300 - 42);//42是头部的高度
            if (!isNullOrEmpty(width)) {
                if (!width.toString().contains("px")) {
                    width = width.toString() + "px";
                }
            } else {
                width = "500px;"
            }
            if (!isNullOrEmpty(height)) {
                if (!height.toString().contains("px")) {

                    bodyHeight = height - 50 + "px";

                    height = height.toString() + "px";
                  
                } else {
                    bodyHeight = height.replace("/px/gi", "") - 50+"px";
                }
            } else {
                height = "300px";
            }
            config.area = [width, height];


            var contentHtml = '<iframe src="' + url + '" style="width:100%;" frameborder="0"  height="' + bodyHeight + '"  ></iframe>';

            config.content = contentHtml;
            MessageBox.html(config);

        },
        /*显示html 内容*/
        "html": function (config) {
            if (config.area == undefined) {
                config.area = ['500px', '300px'];
            }
            var index;
            index = layer.open({
                title: config.title,
                type: 1,
                maxmin: true,
                moveOut: true,
                area: config.area,
                closeBtn: 1,
                end: config.end,//弹窗销毁触发的事件函数
                shadeClose: false, //点击遮罩关闭
                content: config.content,////这里content是一个DOM，注意：最好该元素要存放在body最外层，否则可能被其它的相对元素所影响
                success: function () {
                    if (config.success != undefined) {
                        config.success();
                    }
                }
            });
            return index;
        },
        closeAllLayerWindow: function () {
            layer.closeAll();
        },
        close: function (index) {
            layer.close(index);
        },

        /*仅仅显示提示信息*/
        "show": function (msg, content) {
            swal(msg, content);
        },
        /*弹出确认框*/
        "confirm": function (msg, confirmHandler, cancelHandler) {

            layer.confirm(msg, {
                icon: 3,
                title: '提示',
                btn: ['确定', '取消'] //按钮
            }, function (index) {
                if (isFunction(confirmHandler)) {
                    confirmHandler(index);
                }

            }, function (index) {
                if (isFunction(cancelHandler)) {
                    cancelHandler(index);
                }
            });

        },
        /*弹出成功提示框*/
        "success": function (msg, okHandler) {

            //swal({
            //    title: "操作成功",
            //    text: msg,
            //    type: "success"
            //},
            this.toast(msg, okHandler);

        },
        /*弹出错误提示*/
        "error": function (msg, okHandler) {
            swal({
                title: "错误提示",
                text: msg,
                type: "error"
            },
            function () {
                if (!isNullOrUndefined(okHandler)) {
                    okHandler();//点击ok  关闭窗口的时候，触发的事件回调
                }
            });
        },
        /*弹出错误提示2.0layer版本*/
        "error2": function (msg) {
            layer.alert(msg, { icon: 5,maxHeight:300 });
        },

    };
    window.MessageBox = MessageBox;//加载到全局定义
}

//---------------------------------------------------js 实现 基于later的弹出框------end-------------------------------------------


//---------------------------------------------------js 实现C# Timer------begin-------------------------------------------

/* 定时器 实现
//var t = new Timer(500);
//t.Elapsed = function () {
//    document.write(99);
//};
//t.Start();
*/
var Timer = /** @class */ (function () {
    /*  构造函数
    *@interval 设置定时器的毫秒数
    */
    function Timer(interval) {
        this._enable = false;
        this.Interval = interval;
        var that = this;
        this._handlerInvoker = function () {
            if (that.Elapsed && that._enable == true) {
                that.Elapsed();
            }
        };
    }
    Object.defineProperty(Timer.prototype, "Interval", {
        get: function () {
            return this._interval;
        },
        set: function (value) {
            this._interval = value;
            //重新设定新的定时器
            if (this._jsInnerTimerObj) {
                window.clearInterval(this._jsInnerTimerObj);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Timer.prototype, "Elapsed", {
        get: function () {
            return this._elapsed;
        },
        set: function (value) {
            /*if (!isFunction(value)) {
                throw new Error("the Elapsed value must be function!");
            }*/
            this._elapsed = value;
            //只有设置了触发事件才定时，没有回调的定时器是无意义的定时执行其中的匿名方法
            this._jsInnerTimerObj = window.setInterval(this._handlerInvoker, this.Interval);
        },
        enumerable: true,
        configurable: true
    });
    Timer.prototype.Start = function () {
        this._enable = true;
    };
    Timer.prototype.Stop = function () {
        this._enable = false;
    };
    return Timer;
}());
//---------------------------------------------------js 实现C# Timer------end-------------------------------------------

//---------------------------------------------------js 中文转拼音------begin-------------------------------------------

/*中文拼音处理
基于实现：NPinyin.net
https://github.com/cjjer/npinyin
http://code.google.com/p/npinyin/
*/
var PinYin = {

    /*获取字符串中的中文拼音首字母*/
    GetInitials: function (text) {

        //text = text.Trim();
        var chars = [];
        for (var i = 0; i < text.length; ++i) {
            var py = this.GetPinyin(text[i]);
            if (py != "") chars.push(py[0]);
        }

        return chars.join('').toUpperCase();
    },
    GetChHashIndex: function (ch) {
        return ch.charCodeAt() % this.chCodes.length
    },

    GetPinyin: function (ch) {
        var hash = this.GetChHashIndex(ch);
        for (var i = 0; i < this.chHashes[hash].length; ++i) {
            var index = this.chHashes[hash][i];
            var pos = this.chCodes[index].indexOf(ch, 7);
            if (pos != -1)
                return this.chCodes[index].substring(0, 6);
        }
        return ch;
    },

    chCodes: new Array(
        "a     :阿啊吖嗄腌锕", "ai    :爱埃碍矮挨唉哎哀皑癌蔼艾隘捱嗳嗌嫒瑷暧砹锿霭", "an    :安按暗岸案俺氨胺鞍谙埯揞犴庵桉铵鹌黯", "ang   :昂肮盎", "ao    :凹奥敖熬翱袄傲懊澳坳拗嗷岙廒遨媪骜獒聱螯鏊鳌鏖", "ba    :把八吧巴拔霸罢爸坝芭捌扒叭笆疤跋靶耙茇菝岜灞钯粑鲅魃", "bai   :百白败摆柏佰拜稗捭掰", "ban   :办半板班般版拌搬斑扳伴颁扮瓣绊阪坂钣瘢癍舨", "bang  :帮棒邦榜梆膀绑磅蚌镑傍谤蒡浜", "bao   :报保包剥薄胞暴宝饱抱爆堡苞褒雹豹鲍葆孢煲鸨褓趵龅", "bei   :北被倍备背辈贝杯卑悲碑钡狈惫焙孛陂邶埤萆蓓呗悖碚鹎褙鐾鞴", "ben   :本奔苯笨畚坌贲锛", "beng  :泵崩绷甭蹦迸嘣甏", "bi    :比必避闭辟笔壁臂毕彼逼币鼻蔽鄙碧蓖毙毖庇痹敝弊陛匕俾荜荸薜吡哔狴庳愎滗濞弼妣婢嬖璧畀铋秕裨筚箅篦舭襞跸髀", "bian  :变边便编遍辩扁辨鞭贬卞辫匾弁苄忭汴缏飚煸砭碥窆褊蝙笾鳊", "biao  :表标彪膘婊骠杓飑飙镖镳瘭裱鳔髟", "bie   :别鳖憋瘪蹩", "bin   :宾彬斌濒滨摈傧豳缤玢槟殡膑镔髌鬓", "bing  :并病兵柄冰丙饼秉炳禀邴摒", "bo    :波播伯拨博勃驳玻泊菠钵搏铂箔帛舶脖膊渤亳啵饽檗擘礴钹鹁簸跛踣", "bu    :不部步布补捕卜哺埠簿怖卟逋瓿晡钚钸醭", "ca    :擦嚓礤", "cai   :采才材菜财裁彩猜睬踩蔡", "can   :参残蚕灿餐惭惨孱骖璨粲黪", "cang  :藏仓苍舱沧", "cao   :草槽操糙曹嘈漕螬艚", "ce    :测策侧册厕恻", "cen   :岑涔", "ceng  :层蹭", "cha   :查差插察茶叉茬碴搽岔诧猹馇汊姹杈楂槎檫锸镲衩", "chai  :柴拆豺侪钗瘥虿", "chan  :产铲阐搀掺蝉馋谗缠颤冁谄蒇廛忏潺澶羼婵骣觇禅镡蟾躔", "chang :长常场厂唱肠昌倡偿畅猖尝敞伥鬯苌菖徜怅惝阊娼嫦昶氅鲳", "chao  :朝超潮巢抄钞嘲吵炒怊晁耖", "che   :车彻撤扯掣澈坼砗", "chen  :陈沉称衬尘臣晨郴辰忱趁伧谌谶抻嗔宸琛榇碜龀", "cheng :成程称城承乘呈撑诚橙惩澄逞骋秤丞埕噌枨柽塍瞠铖铛裎蛏酲", "chi   :持尺齿吃赤池迟翅斥耻痴匙弛驰侈炽傺坻墀茌叱哧啻嗤彳饬媸敕眵鸱瘛褫蚩螭笞篪豉踟魑", "chong :虫充冲崇宠茺忡憧铳舂艟", "chou  :抽仇臭酬畴踌稠愁筹绸瞅丑俦帱惆瘳雠", "chu   :出处除初础触楚锄储橱厨躇雏滁矗搐亍刍怵憷绌杵楮樗褚蜍蹰黜", "chuai :揣搋啜膪踹", "chuan :传船穿串川椽喘舛遄巛氚钏舡", "chuang:床创窗闯疮幢怆", "chui  :吹垂锤炊捶陲棰槌", "chun  :春纯醇椿唇淳蠢莼鹑蝽", "chuo  :戳绰辍踔龊", "ci    :此次刺磁雌词茨疵辞慈瓷赐茈呲祠鹚糍", "cong  :从丛聪葱囱匆苁淙骢琮璁", "cou   :凑楱辏腠", "cu    :粗促醋簇蔟徂猝殂酢蹙蹴", "cuan  :篡蹿窜汆撺爨镩", "cui   :催脆淬粹摧崔瘁翠萃啐悴璀榱毳隹", "cun   :存村寸忖皴", "cuo   :错措撮磋搓挫厝嵯脞锉矬痤鹾蹉", "da    :大打达答搭瘩耷哒嗒怛妲疸褡笪靼鞑", "dai   :代带待袋戴呆歹傣殆贷逮怠埭甙呔岱迨骀绐玳黛", "dan   :单但弹担蛋淡胆氮丹旦耽郸掸惮诞儋萏啖殚赕眈疸瘅聃箪", "dang  :党当档挡荡谠凼菪宕砀裆", "dao   :到道导刀倒稻岛捣盗蹈祷悼叨忉氘纛", "de    :的得德锝", "deng  :等灯登邓蹬瞪凳噔嶝戥磴镫簦", "di    :地第低敌底帝抵滴弟递堤迪笛狄涤翟嫡蒂缔氐籴诋谛邸荻嘀娣绨柢棣觌砥碲睇镝羝骶", "dia   :嗲", "dian  :电点垫典店颠淀掂滇碘靛佃甸惦奠殿阽坫巅玷钿癜癫簟踮", "diao  :调掉吊碉叼雕凋刁钓铞铫貂鲷", "die   :迭跌爹碟蝶谍叠垤堞揲喋牒瓞耋蹀鲽", "ding  :定顶钉丁订盯叮鼎锭仃啶玎腚碇町疔耵酊", "diu   :丢铥", "dong  :动东冬懂洞冻董栋侗恫垌咚岽峒氡胨胴硐鸫", "dou   :斗豆兜抖陡逗痘蔸窦蚪篼", "du    :度都毒独读渡杜堵镀顿督犊睹赌肚妒芏嘟渎椟牍蠹笃髑黩", "duan  :断端段短锻缎椴煅簖", "dui   :对队堆兑怼憝碓", "dun   :盾吨顿蹲敦墩囤钝遁沌炖砘礅盹镦趸", "duo   :多夺朵掇哆垛躲跺舵剁惰堕咄哚沲缍柁铎裰踱", "e     :而二尔儿恶额恩俄耳饵蛾饿峨鹅讹娥厄扼遏鄂噩谔垩苊莪萼呃愕屙婀轭腭锇锷鹗颚鳄", "ei    :诶", "en    :恩蒽摁", "er    :而二尔儿耳饵洱贰佴迩珥铒鸸鲕", "fa    :发法阀乏伐罚筏珐垡砝", "fan   :反翻范犯饭繁泛番凡烦返藩帆樊矾钒贩蕃蘩幡梵燔畈蹯", "fang  :方放防访房纺仿妨芳肪坊邡枋钫舫鲂", "fei   :非肥飞费废肺沸菲匪啡诽吠芾狒悱淝妃绯榧腓斐扉镄痱蜚篚翡霏鲱", "fen   :分粉奋份粪纷芬愤酚吩氛坟焚汾忿偾瀵棼鲼鼢", "feng  :风封蜂丰缝峰锋疯奉枫烽逢冯讽凤俸酆葑唪沣砜", "fou   :否缶", "fu    :复服副府夫负富附福伏符幅腐浮辅付腹妇孵覆扶辐傅佛缚父弗甫肤氟敷拂俘涪袱抚俯釜斧脯腑赴赋阜讣咐匐凫郛芙苻茯莩菔拊呋幞怫滏艴孚驸绂绋桴赙祓砩黻黼罘稃馥蚨蜉蝠蝮麸趺跗鲋鳆", "ga    :噶嘎尬尕尜旮钆", "gai   :改该盖概钙溉丐陔垓戤赅", "gan   :干杆感敢赶甘肝秆柑竿赣坩苷尴擀泔淦澉绀橄旰矸疳酐", "gang  :刚钢缸纲岗港杠冈肛戆罡筻", "gao   :高搞告稿膏篙皋羔糕镐睾诰郜藁缟槔槁杲锆", "ge    :个各革格割歌隔哥铬阁戈葛搁鸽胳疙蛤鬲仡哿圪塥嗝搿膈硌镉袼虼舸骼", "gen   :根跟亘茛哏艮", "geng  :更耕颈庚羹埂耿梗哽赓绠鲠", "gong  :工公共供功攻巩贡汞宫恭龚躬弓拱珙肱蚣觥", "gou   :够构沟狗钩勾购苟垢佝诟岣遘媾缑枸觏彀笱篝鞲", "gu    :鼓固古骨故顾股谷估雇孤姑辜菇咕箍沽蛊嘏诂菰崮汩梏轱牯牿臌毂瞽罟钴锢鸪痼蛄酤觚鲴鹘", "gua   :挂刮瓜剐寡褂卦诖呱栝胍鸹", "guai  :怪乖拐", "guan  :关管观官灌贯惯冠馆罐棺倌莞掼涫盥鹳矜鳏", "guang :光广逛咣犷桄胱", "gui   :规贵归硅鬼轨龟桂瑰圭闺诡癸柜跪刽匦刿庋宄妫桧炅晷皈簋鲑鳜", "gun   :滚辊棍衮绲磙鲧", "guo   :国过果锅郭裹馘埚掴呙帼崞猓椁虢聒蜾蝈", "ha    :哈铪", "hai   :还海害孩骸氦亥骇嗨胲醢", "han   :含焊旱喊汉寒汗函韩酣憨邯涵罕翰撼捍憾悍邗菡撖阚瀚晗焓顸颔蚶鼾", "hang  :航夯杭沆绗珩颃", "hao   :好号毫耗豪郝浩壕嚎蒿薅嗥嚆濠灏昊皓颢蚝", "he    :和合河何核赫荷褐喝贺呵禾盒菏貉阂涸鹤诃劾壑嗬阖纥曷盍颌蚵翮", "hei   :黑嘿", "hen   :很狠痕恨", "heng  :横衡恒哼亨蘅桁", "hong  :红洪轰烘哄虹鸿宏弘黉訇讧荭蕻薨闳泓", "hou   :后候厚侯喉猴吼堠後逅瘊篌糇鲎骺", "hu    :护互湖呼户弧乎胡糊虎忽瑚壶葫蝴狐唬沪冱唿囫岵猢怙惚浒滹琥槲轷觳烀煳戽扈祜瓠鹄鹕鹱笏醐斛", "hua   :化花话划滑华画哗猾骅桦砉铧", "huai  :坏怀淮槐徊踝", "huan  :环换欢缓患幻焕桓唤痪豢涣宦郇奂萑擐圜獾洹浣漶寰逭缳锾鲩鬟", "huang :黄簧荒皇慌蝗磺凰惶煌晃幌恍谎隍徨湟潢遑璜肓癀蟥篁鳇", "hui   :会回灰挥辉汇毁慧恢绘惠徽蛔悔卉晦贿秽烩讳诲诙茴荟蕙咴哕喙隳洄浍彗缋珲晖恚虺蟪麾", "hun   :混浑荤昏婚魂诨馄阍溷", "huo   :活或火货获伙霍豁惑祸劐藿攉嚯夥钬锪镬耠蠖", "ji    :级及机极几积给基记己计集即际季激济技击继急剂既纪寄挤鸡迹绩吉脊辑籍疾肌棘畸圾稽箕饥讥姬缉汲嫉蓟冀伎祭悸寂忌妓藉丌亟乩剞佶偈诘墼芨芰荠蒺蕺掎叽咭哜唧岌嵴洎屐骥畿玑楫殛戟戢赍觊犄齑矶羁嵇稷瘠虮笈笄暨跻跽霁鲚鲫髻麂", "jia   :加家架价甲夹假钾贾稼驾嘉枷佳荚颊嫁伽郏葭岬浃迦珈戛胛恝铗镓痂瘕袷蛱笳袈跏", "jian  :间件见建坚减检践尖简碱剪艰渐肩键健柬鉴剑歼监兼奸箭茧舰俭笺煎缄硷拣捡荐槛贱饯溅涧僭谏谫菅蒹搛湔蹇謇缣枧楗戋戬牮犍毽腱睑锏鹣裥笕翦趼踺鲣鞯", "jiang :将降讲江浆蒋奖疆僵姜桨匠酱茳洚绛缰犟礓耩糨豇", "jiao  :较教交角叫脚胶浇焦搅酵郊铰窖椒礁骄娇嚼矫侥狡饺缴绞剿轿佼僬艽茭挢噍峤徼姣敫皎鹪蛟醮跤鲛", "jie   :结阶解接节界截介借届街揭洁杰竭皆秸劫桔捷睫姐戒藉芥疥诫讦拮喈嗟婕孑桀碣疖颉蚧羯鲒骱", "jin   :进金近紧斤今尽仅劲浸禁津筋锦晋巾襟谨靳烬卺荩堇噤馑廑妗缙瑾槿赆觐衿", "jing  :经精京径井静竟晶净境镜景警茎敬惊睛竞荆兢鲸粳痉靖刭儆阱菁獍憬泾迳弪婧肼胫腈旌", "jiong :炯窘迥扃", "jiu   :就九旧究久救酒纠揪玖韭灸厩臼舅咎疚僦啾阄柩桕鸠鹫赳鬏", "ju    :具据局举句聚距巨居锯剧矩拒鞠拘狙疽驹菊咀沮踞俱惧炬倨讵苣苴莒掬遽屦琚椐榘榉橘犋飓钜锔窭裾趄醵踽龃雎鞫", "juan  :卷捐鹃娟倦眷绢鄄狷涓桊蠲锩镌隽", "jue   :决觉绝掘撅攫抉倔爵诀厥劂谲矍蕨噘噱崛獗孓珏桷橛爝镢蹶觖", "jun   :军均菌君钧峻俊竣浚郡骏捃皲筠麇", "ka    :卡喀咖咯佧咔胩", "kai   :开凯揩楷慨剀垲蒈忾恺铠锎锴", "kan   :看刊坎堪勘砍侃莰戡龛瞰", "kang  :抗康炕慷糠扛亢伉闶钪", "kao   :考靠拷烤尻栲犒铐", "ke    :可克科刻客壳颗棵柯坷苛磕咳渴课嗑岢恪溘骒缂珂轲氪瞌钶锞稞疴窠颏蝌髁", "ken   :肯啃垦恳裉", "keng  :坑吭铿", "kong  :孔空控恐倥崆箜", "kou   :口扣抠寇芤蔻叩眍筘", "ku    :苦库枯酷哭窟裤刳堀喾绔骷", "kua   :跨夸垮挎胯侉", "kuai  :快块筷侩蒯郐哙狯脍", "kuan  :宽款髋", "kuang :况矿狂框匡筐眶旷诓诳邝圹夼哐纩贶", "kui   :奎溃馈亏盔岿窥葵魁傀愧馗匮夔隗蒉揆喹喟悝愦逵暌睽聩蝰篑跬", "kun   :困昆坤捆悃阃琨锟醌鲲髡", "kuo   :扩括阔廓蛞", "la    :拉啦蜡腊蓝垃喇辣剌邋旯砬瘌", "lai   :来赖莱崃徕涞濑赉睐铼癞籁", "lan   :兰烂蓝览栏婪拦篮阑澜谰揽懒缆滥岚漤榄斓罱镧褴", "lang  :浪朗郎狼琅榔廊莨蒗啷阆锒稂螂", "lao   :老劳牢涝捞佬姥酪烙唠崂栳铑铹痨耢醪", "le    :了乐勒肋仂叻泐鳓", "lei   :类雷累垒泪镭蕾磊儡擂肋羸诔嘞嫘缧檑耒酹", "leng  :冷棱楞塄愣", "li    :理里利力立离例历粒厘礼李隶黎璃励犁梨丽厉篱狸漓鲤莉荔吏栗砾傈俐痢沥哩俪俚郦坜苈莅蓠藜呖唳喱猁溧澧逦娌嫠骊缡枥栎轹戾砺詈罹锂鹂疠疬蛎蜊蠡笠篥粝醴跞雳鲡鳢黧", "lia   :俩", "lian  :连联练炼脸链莲镰廉怜涟帘敛恋蔹奁潋濂琏楝殓臁裢裣蠊鲢", "liang :量两粮良亮梁凉辆粱晾谅墚椋踉靓魉", "liao  :料疗辽僚撩聊燎寥潦撂镣廖蓼尥嘹獠寮缭钌鹩", "lie   :列裂烈劣猎冽埒捩咧洌趔躐鬣", "lin   :林磷临邻淋麟琳霖鳞凛赁吝蔺啉嶙廪懔遴檩辚膦瞵粼躏", "ling  :领另零令灵岭铃龄凌陵拎玲菱伶羚酃苓呤囹泠绫柃棂瓴聆蛉翎鲮", "liu   :流六留刘硫柳馏瘤溜琉榴浏遛骝绺旒熘锍镏鹨鎏", "long  :龙垄笼隆聋咙窿拢陇垅茏泷珑栊胧砻癃", "lou   :漏楼娄搂篓陋偻蒌喽嵝镂瘘耧蝼髅", "lu    :路率露绿炉律虑滤陆氯鲁铝录旅卢吕芦颅庐掳卤虏麓碌赂鹿潞禄戮驴侣履屡缕垆撸噜闾泸渌漉逯璐栌榈橹轳辂辘氇胪膂镥稆鸬鹭褛簏舻鲈", "luan  :卵乱峦挛孪滦脔娈栾鸾銮", "lue   :略掠锊", "lun   :论轮伦抡仑沦纶囵", "luo   :落罗螺洛络逻萝锣箩骡裸骆倮蠃荦捋摞猡泺漯珞椤脶镙瘰雒", "m     :呒", "ma    :马麻吗妈骂嘛码玛蚂唛犸嬷杩蟆", "mai   :麦脉卖买埋迈劢荬霾", "man   :满慢曼漫蔓瞒馒蛮谩墁幔缦熳镘颟螨鳗鞔", "mang  :忙芒盲茫氓莽邙漭硭蟒", "mao   :毛矛冒貌贸帽猫茅锚铆卯茂袤茆峁泖瑁昴牦耄旄懋瞀蟊髦", "me    :么麽", "mei   :没每美煤霉酶梅妹眉玫枚媒镁昧寐媚莓嵋猸浼湄楣镅鹛袂魅", "men   :们门闷扪焖懑钔", "meng  :孟猛蒙盟梦萌锰檬勐甍瞢懵朦礞虻蜢蠓艋艨", "mi    :米密迷蜜秘眯醚靡糜谜弥觅泌幂芈谧蘼咪嘧猕汨宓弭脒祢敉糸縻麋", "mian  :面棉免绵眠冕勉娩缅沔渑湎腼眄", "miao  :苗秒描庙妙瞄藐渺喵邈缈缪杪淼眇鹋", "mie   :灭蔑咩蠛篾", "min   :民敏抿皿悯闽苠岷闵泯缗玟珉愍黾鳘", "ming  :命明名鸣螟铭冥茗溟暝瞑酩", "miu   :谬", "mo    :磨末模膜摸墨摩莫抹默摹蘑魔沫漠寞陌谟茉蓦馍嫫殁镆秣瘼耱貊貘", "mou   :某谋牟侔哞眸蛑蝥鍪", "mu    :亩目木母墓幕牧姆穆拇牡暮募慕睦仫坶苜沐毪钼", "n     :嗯", "na    :那南哪拿纳钠呐娜捺肭镎衲", "nai   :耐奶乃氖奈鼐艿萘柰", "nan   :南难男喃囝囡楠腩蝻赧", "nang  :囊攮囔馕曩", "nao   :脑闹挠恼淖孬垴呶猱瑙硇铙蛲", "ne    :呢讷", "nei   :内馁", "nen   :嫩恁", "neng  :能", "ni    :你泥尼逆拟尿妮霓倪匿腻溺伲坭猊怩昵旎慝睨铌鲵", "nian  :年念粘蔫拈碾撵捻酿廿埝辇黏鲇鲶", "niang :娘", "niao  :尿鸟茑嬲脲袅", "nie   :镍啮涅捏聂孽镊乜陧蘖嗫颞臬蹑", "nin   :您", "ning  :宁凝拧柠狞泞佞苎咛甯聍", "niu   :牛扭钮纽狃忸妞", "nong  :农弄浓脓侬哝", "nou   :耨", "nu    :女奴努怒弩胬孥驽恧钕衄", "nuan  :暖", "nue   :虐", "nuo   :诺挪懦糯傩搦喏锘", "o     :欧偶哦鸥殴藕呕沤讴噢怄瓯耦", "ou    :欧偶鸥殴藕呕沤讴怄瓯耦", "pa    :怕派爬帕啪趴琶葩杷筢", "pai   :派排拍牌哌徘湃俳蒎", "pan   :判盘叛潘攀磐盼畔胖爿泮袢襻蟠蹒", "pang  :旁乓庞耪胖彷滂逄螃", "pao   :跑炮刨抛泡咆袍匏狍庖脬疱", "pei   :配培陪胚呸裴赔佩沛辔帔旆锫醅霈", "pen   :喷盆湓", "peng  :碰棚蓬朋捧膨砰抨烹澎彭硼篷鹏堋嘭怦蟛", "pi    :批皮坯脾疲砒霹披劈琵毗啤匹痞僻屁譬丕仳陴邳郫圮鼙芘擗噼庀淠媲纰枇甓睥罴铍癖疋蚍蜱貔", "pian  :片偏篇骗谝骈犏胼翩蹁", "piao  :票漂飘瓢剽嘌嫖缥殍瞟螵", "pie   :撇瞥丿苤氕", "pin   :品贫频拼苹聘拚姘嫔榀牝颦", "ping  :平评瓶凭苹乒坪萍屏俜娉枰鲆", "po    :破迫坡泼颇婆魄粕叵鄱珀攴钋钷皤笸", "pou   :剖裒掊", "pu    :普谱扑埔铺葡朴蒲仆莆菩圃浦曝瀑匍噗溥濮璞氆镤镨蹼", "qi    :起其气期七器齐奇汽企漆欺旗畦启弃歧栖戚妻凄柒沏棋崎脐祈祁骑岂乞契砌迄泣讫亓俟圻芑芪萁萋葺蕲嘁屺岐汔淇骐绮琪琦杞桤槭耆欹祺憩碛颀蛴蜞綦綮蹊鳍麒", "qia   :恰掐洽葜髂", "qian  :前千钱浅签迁铅潜牵钳谴扦钎仟谦乾黔遣堑嵌欠歉倩佥阡芊芡茜荨掮岍悭慊骞搴褰缱椠肷愆钤虔箬箝", "qiang :强枪抢墙腔呛羌蔷戕嫱樯戗炝锖锵镪襁蜣羟跄", "qiao  :桥瞧巧敲乔蕉橇锹悄侨鞘撬翘峭俏窍劁诮谯荞愀憔樵硗跷鞒", "qie   :切且茄怯窃郄惬妾挈锲箧", "qin   :亲侵勤秦钦琴芹擒禽寝沁芩揿吣嗪噙溱檎锓覃螓衾", "qing  :情清青轻倾请庆氢晴卿擎氰顷苘圊檠磬蜻罄箐謦鲭黥", "qiong :穷琼邛茕穹蛩筇跫銎", "qiu   :求球秋丘邱囚酋泅俅巯犰湫逑遒楸赇虬蚯蝤裘糗鳅鼽", "qu    :去区取曲渠屈趋驱趣蛆躯娶龋诎劬蕖蘧岖衢阒璩觑氍朐祛磲鸲癯蛐蠼麴瞿黢", "quan  :全权圈劝泉醛颧痊拳犬券诠荃悛绻辁畎铨蜷筌鬈", "que   :确却缺炔瘸鹊榷雀阕阙悫", "qun   :群裙逡", "ran   :然燃染冉苒蚺髯", "rang  :让壤嚷瓤攘禳穰", "rao   :绕扰饶荛娆桡", "re    :热惹", "ren   :人认任仁刃忍壬韧妊纫仞荏葚饪轫稔衽", "reng  :仍扔", "ri    :日", "rong  :容溶荣熔融绒戎茸蓉冗嵘狨榕肜蝾", "rou   :肉揉柔糅蹂鞣", "ru    :如入儒乳茹蠕孺辱汝褥蓐薷嚅洳溽濡缛铷襦颥", "ruan  :软阮朊", "rui   :瑞锐蕊芮蕤枘睿蚋", "run   :润闰", "ruo   :弱若偌", "sa    :撒萨洒卅仨挲脎飒", "sai   :塞赛腮鳃噻", "san   :三散叁伞馓毵糁", "sang  :桑丧嗓搡磉颡", "sao   :扫搔骚嫂埽缫缲臊瘙鳋", "se    :色瑟涩啬铯穑", "sen   :森", "seng  :僧", "sha   :沙杀砂啥纱莎刹傻煞杉唼歃铩痧裟霎鲨", "shai  :筛晒", "shan  :山闪善珊扇陕苫杉删煽衫擅赡膳汕缮剡讪鄯埏芟潸姗嬗骟膻钐疝蟮舢跚鳝", "shang :上商伤尚墒赏晌裳垧绱殇熵觞", "shao  :少烧稍绍哨梢捎芍勺韶邵劭苕潲蛸筲艄", "she   :社设射摄舌涉舍蛇奢赊赦慑厍佘猞滠歙畲麝", "shen  :深身神伸甚渗沈肾审申慎砷呻娠绅婶诜谂莘哂渖椹胂矧蜃", "sheng :生胜声省升盛绳剩圣牲甥嵊晟眚笙", "shi   :是时十使事实式识世试石什示市史师始施士势湿适食失视室氏蚀诗释拾饰驶狮尸虱矢屎柿拭誓逝嗜噬仕侍恃谥埘莳蓍弑轼贳炻铈螫舐筮酾豕鲥鲺", "shou  :手受收首守授寿兽售瘦狩绶艏", "shu   :数书树属术输述熟束鼠疏殊舒蔬薯叔署枢梳抒淑赎孰暑曙蜀黍戍竖墅庶漱恕丨倏塾菽摅沭澍姝纾毹腧殳秫", "shua  :刷耍唰", "shuai :衰帅摔甩蟀", "shuan :栓拴闩涮", "shuang:双霜爽孀", "shui  :水谁睡税", "shun  :顺吮瞬舜", "shuo  :说硕朔烁蒴搠妁槊铄", "si    :四思死斯丝似司饲私撕嘶肆寺嗣伺巳厮兕厶咝汜泗澌姒驷缌祀锶鸶耜蛳笥", "song  :松送宋颂耸怂讼诵凇菘崧嵩忪悚淞竦", "sou   :搜艘擞嗽叟薮嗖嗾馊溲飕瞍锼螋", "su    :素速苏塑缩俗诉宿肃酥粟僳溯夙谡蔌嗉愫涑簌觫稣", "suan  :算酸蒜狻", "sui   :随穗碎虽岁隋绥髓遂隧祟谇荽濉邃燧眭睢", "sun   :损孙笋荪狲飧榫隼", "suo   :所缩锁索蓑梭唆琐唢嗦嗍娑桫睃羧", "ta    :他它她塔踏塌獭挞蹋闼溻遢榻沓铊趿鳎", "tai   :台太态胎抬泰苔酞汰邰薹肽炱钛跆鲐", "tan   :谈碳探炭坦贪滩坍摊瘫坛檀痰潭谭毯袒叹郯澹昙忐钽锬", "tang  :堂糖唐塘汤搪棠膛倘躺淌趟烫傥帑溏瑭樘铴镗耥螗螳羰醣", "tao   :套讨逃陶萄桃掏涛滔绦淘鼗啕洮韬焘饕", "te    :特忒忑铽", "teng  :腾疼藤誊滕", "ti    :提题体替梯惕剔踢锑蹄啼嚏涕剃屉倜悌逖缇鹈裼醍", "tian  :天田添填甜恬舔腆掭忝阗殄畋", "tiao  :条跳挑迢眺佻祧窕蜩笤粜龆鲦髫", "tie   :铁贴帖萜餮", "ting  :听停庭挺廷厅烃汀亭艇莛葶婷梃铤蜓霆", "tong  :同通统铜痛筒童桶桐酮瞳彤捅佟仝茼嗵恸潼砼", "tou   :头投透偷钭骰", "tu    :图土突途徒凸涂吐兔屠秃堍荼菟钍酴", "tuan  :团湍抟彖疃", "tui   :推退腿颓蜕褪煺", "tun   :吞屯臀氽饨暾豚", "tuo   :脱拖托妥椭鸵陀驮驼拓唾乇佗坨庹沱柝橐砣箨酡跎鼍", "wa    :瓦挖哇蛙洼娃袜佤娲腽", "wai   :外歪", "wan   :完万晚弯碗顽湾挽玩豌丸烷皖惋宛婉腕剜芄菀纨绾琬脘畹蜿", "wang  :往王望网忘妄亡旺汪枉罔尢惘辋魍", "wei   :为位委围维唯卫微伟未威危尾谓喂味胃魏伪违韦畏纬巍桅惟潍苇萎蔚渭尉慰偎诿隈葳薇囗帏帷崴嵬猥猬闱沩洧涠逶娓玮韪軎炜煨痿艉鲔", "wen   :问温文稳纹闻蚊瘟吻紊刎阌汶璺雯", "weng  :嗡翁瓮蓊蕹", "wo    :我握窝蜗涡沃挝卧斡倭莴喔幄渥肟硪龌", "wu    :无五物武务误伍舞污悟雾午屋乌吴诬钨巫呜芜梧吾毋捂侮坞戊晤勿兀仵阢邬圬芴唔庑怃忤浯寤迕妩婺骛杌牾焐鹉鹜痦蜈鋈鼯", "xi    :系席西习细吸析喜洗铣稀戏隙希息袭锡烯牺悉惜溪昔熙硒矽晰嘻膝夕熄汐犀檄媳僖兮隰郗菥葸蓰奚唏徙饩阋浠淅屣嬉玺樨曦觋欷熹禊禧皙穸蜥螅蟋舄舾羲粞翕醯鼷", "xia   :下夏吓狭霞瞎虾匣辖暇峡侠厦呷狎遐瑕柙硖罅黠", "xian  :线现先县限显鲜献险陷宪纤掀弦腺锨仙咸贤衔舷闲涎嫌馅羡冼苋莶藓岘猃暹娴氙燹祆鹇痫蚬筅籼酰跣跹霰", "xiang :想向相象响项箱乡香像详橡享湘厢镶襄翔祥巷芗葙饷庠骧缃蟓鲞飨", "xiao  :小消削效笑校销硝萧肖孝霄哮嚣宵淆晓啸哓崤潇逍骁绡枭枵筱箫魈", "xie   :些写斜谢协械卸屑鞋歇邪胁蟹泄泻楔蝎挟携谐懈偕亵勰燮薤撷獬廨渫瀣邂绁缬榭榍躞", "xin   :新心信锌芯辛欣薪忻衅囟馨昕歆鑫", "xing  :行性形型星兴醒姓幸腥猩惺刑邢杏陉荇荥擤饧悻硎", "xiong :雄胸兄凶熊匈汹芎", "xiu   :修锈休袖秀朽羞嗅绣咻岫馐庥溴鸺貅髹", "xu    :续许须需序虚絮畜叙蓄绪徐墟戌嘘酗旭恤婿诩勖圩蓿洫溆顼栩煦盱胥糈醑", "xuan  :选旋宣悬玄轩喧癣眩绚儇谖萱揎泫渲漩璇楦暄炫煊碹铉镟痃", "xue   :学血雪穴靴薛谑泶踅鳕", "xun   :训旬迅讯寻循巡勋熏询驯殉汛逊巽埙荀蕈薰峋徇獯恂洵浔曛醺鲟", "ya    :压亚呀牙芽雅蚜鸭押鸦丫崖衙涯哑讶伢垭揠岈迓娅琊桠氩砑睚痖", "yan   :验研严眼言盐演岩沿烟延掩宴炎颜燕衍焉咽阉淹蜒阎奄艳堰厌砚雁唁彦焰谚厣赝俨偃兖谳郾鄢菸崦恹闫阏湮滟妍嫣琰檐晏胭焱罨筵酽魇餍鼹", "yang  :样养氧扬洋阳羊秧央杨仰殃鸯佯疡痒漾徉怏泱炀烊恙蛘鞅", "yao   :要药摇腰咬邀耀疟妖瑶尧遥窑谣姚舀夭爻吆崾徭幺珧杳轺曜肴鹞窈繇鳐", "ye    :也业页叶液夜野爷冶椰噎耶掖曳腋靥谒邺揶晔烨铘", "yi    :一以义意已移医议依易乙艺益异宜仪亿遗伊役衣疑亦谊翼译抑忆疫壹揖铱颐夷胰沂姨彝椅蚁倚矣邑屹臆逸肄裔毅溢诣翌绎刈劓佚佾诒圯埸懿苡荑薏弈奕挹弋呓咦咿噫峄嶷猗饴怿怡悒漪迤驿缢殪轶贻旖熠眙钇镒镱痍瘗癔翊蜴舣羿翳酏黟", "yin   :因引阴印音银隐饮荫茵殷姻吟淫寅尹胤鄞垠堙茚吲喑狺夤洇氤铟瘾窨蚓霪龈", "ying  :应影硬营英映迎樱婴鹰缨莹萤荧蝇赢盈颖嬴郢茔莺萦蓥撄嘤膺滢潆瀛瑛璎楹媵鹦瘿颍罂", "yo    :哟唷", "yong  :用勇永拥涌蛹庸佣臃痈雍踊咏泳恿俑壅墉喁慵邕镛甬鳙饔", "you   :有由又油右友优幼游尤诱犹幽悠忧邮铀酉佑釉卣攸侑莠莜莸呦囿宥柚猷牖铕疣蚰蚴蝣鱿黝鼬", "yu    :于与育鱼雨玉余遇预域语愈渔予羽愚御欲宇迂淤盂榆虞舆俞逾愉渝隅娱屿禹芋郁吁喻峪狱誉浴寓裕豫驭禺毓伛俣谀谕萸蓣揄圄圉嵛狳饫馀庾阈鬻妪妤纡瑜昱觎腴欤於煜熨燠聿钰鹆鹬瘐瘀窬窳蜮蝓竽臾舁雩龉", "yuan  :员原圆源元远愿院缘援园怨鸳渊冤垣袁辕猿苑垸塬芫掾沅媛瑗橼爰眢鸢螈箢鼋", "yue   :月越约跃曰阅钥岳粤悦龠瀹樾刖钺", "yun   :运云匀允孕耘郧陨蕴酝晕韵郓芸狁恽愠纭韫殒昀氲熨", "za    :杂咱匝砸咋咂", "zai   :在再载栽灾哉宰崽甾", "zan   :赞咱暂攒拶瓒昝簪糌趱錾", "zang  :脏葬赃奘驵臧", "zao   :造早遭燥凿糟枣皂藻澡蚤躁噪灶唣", "ze    :则择责泽仄赜啧帻迮昃笮箦舴", "zei   :贼", "zen   :怎谮", "zeng  :增曾憎赠缯甑罾锃", "zha   :扎炸闸铡轧渣喳札眨栅榨乍诈揸吒咤哳砟痄蚱齄", "zhai  :寨摘窄斋宅债砦瘵", "zhan  :战展站占瞻毡詹沾盏斩辗崭蘸栈湛绽谵搌旃", "zhang :张章掌仗障胀涨账樟彰漳杖丈帐瘴仉鄣幛嶂獐嫜璋蟑", "zhao  :照找招召赵爪罩沼兆昭肇诏棹钊笊", "zhe   :这着者折哲浙遮蛰辙锗蔗谪摺柘辄磔鹧褶蜇赭", "zhen  :真针阵镇振震珍诊斟甄砧臻贞侦枕疹圳蓁浈缜桢榛轸赈胗朕祯畛稹鸩箴", "zheng :争正政整证征蒸症郑挣睁狰怔拯帧诤峥徵钲铮筝", "zhi   :之制治只质指直支织止至置志值知执职植纸致枝殖脂智肢秩址滞汁芝吱蜘侄趾旨挚掷帜峙稚炙痔窒卮陟郅埴芷摭帙忮彘咫骘栉枳栀桎轵轾贽胝膣祉祗黹雉鸷痣蛭絷酯跖踬踯豸觯", "zhong :中种重众钟终忠肿仲盅衷冢锺螽舯踵", "zhou  :轴周洲州皱骤舟诌粥肘帚咒宙昼荮啁妯纣绉胄碡籀酎", "zhu   :主注著住助猪铸株筑柱驻逐祝竹贮珠朱诸蛛诛烛煮拄瞩嘱蛀伫侏邾茱洙渚潴杼槠橥炷铢疰瘃竺箸舳翥躅麈", "zhua  :抓", "zhuai :拽", "zhuan :转专砖撰赚篆啭馔颛", "zhuang:装状壮庄撞桩妆僮", "zhui  :追锥椎赘坠缀惴骓缒", "zhun  :准谆肫窀", "zhuo  :捉桌拙卓琢茁酌啄灼浊倬诼擢浞涿濯焯禚斫镯", "zi    :子自资字紫仔籽姿兹咨滋淄孜滓渍谘嵫姊孳缁梓辎赀恣眦锱秭耔笫粢趑觜訾龇鲻髭", "zong  :总纵宗综棕鬃踪偬枞腙粽", "zou   :走邹奏揍诹陬鄹驺鲰", "zu    :组族足阻祖租卒诅俎菹镞", "zuan  :钻纂攥缵躜", "zui   :最罪嘴醉蕞", "zun   :尊遵撙樽鳟", "zuo   :作做左座坐昨佐柞阼唑嘬怍胙祚"
        ),

    //document.write(chCodes[0])
    chHashes: new Array(
        [23, 70, 96, 128, 154, 165, 172, 195], [25, 35, 87, 108, 120, 128, 132, 137, 168, 180, 325, 334, 336, 353, 361, 380], [23, 34, 46, 81, 82, 87, 134, 237, 255, 288, 317, 322, 354, 359], [7, 11, 37, 49, 53, 56, 131, 132, 146, 176, 315, 372], [11, 69, 73, 87, 96, 103, 159, 175, 195, 296, 298, 359, 361], [57, 87, 115, 126, 149, 244, 282, 298, 308, 345, 355], [19, 37, 117, 118, 141, 154, 196, 216, 267, 301, 327, 333, 337, 347], [4, 11, 59, 61, 62, 87, 119, 169, 183, 198, 262, 334, 362, 380], [37, 135, 167, 170, 246, 250, 334, 341, 351, 354, 386, 390, 398], [5, 6, 52, 55, 76, 146, 165, 244, 256, 266, 300, 318, 331], [6, 71, 94, 129, 137, 141, 169, 179, 225, 226, 235, 248, 289, 290, 333, 345, 391], [0, 33, 37, 62, 90, 131, 205, 246, 268, 343, 349, 380], [31, 62, 85, 115, 117, 150, 159, 167, 171, 204, 215, 252, 343], [69, 81, 98, 140, 165, 195, 239, 240, 259, 265, 329, 368, 375, 392, 393], [13, 81, 82, 123, 132, 144, 154, 165, 334, 336, 345, 348, 349, 355, 367, 377, 383], [31, 32, 44, 57, 76, 83, 87, 129, 151, 172, 176, 183, 184, 193, 221, 235, 285, 288, 305], [10, 14, 60, 76, 85, 97, 115, 125, 128, 130, 286, 288, 301, 313, 382], [62, 128, 136, 175, 211, 240, 254, 273, 274, 317, 330, 344, 349, 360, 380], [29, 47, 52, 116, 126, 127, 130, 133, 191, 284, 288, 306, 353, 361, 383], [1, 15, 25, 67, 83, 90, 117, 121, 150, 228, 308, 324, 336, 351, 386], [34, 37, 67, 101, 103, 117, 127, 165, 168, 254, 267, 272, 274, 288, 305, 310, 323, 329, 333, 358, 378], [5, 74, 103, 135, 163, 165, 171, 244, 262, 266, 334, 352, 390, 397], [4, 17, 95, 125, 165, 186, 203, 221, 252, 282, 317, 333, 339, 348, 351, 353], [74, 79, 81, 84, 92, 110, 116, 117, 131, 132, 154, 199, 241, 251, 300, 306, 349, 359, 383, 387], [40, 83, 127, 144, 161, 188, 249, 288, 344, 382, 388], [8, 55, 61, 76, 85, 98, 111, 127, 186, 230, 241, 247, 267, 287, 327, 341, 344, 347, 359, 364], [20, 59, 69, 80, 117, 129, 176, 186, 191, 237, 275, 289, 309, 338, 375, 380], [5, 15, 25, 35, 40, 129, 174, 236, 274, 337, 347], [14, 22, 47, 56, 87, 120, 129, 144, 155, 160, 237, 283, 284, 309, 327, 337, 365, 372], [1, 14, 47, 132, 198, 254, 255, 300, 310, 335, 336, 372], [2, 36, 64, 96, 125, 176, 184, 190, 211, 271, 308, 315, 367], [20, 76, 79, 81, 110, 117, 120, 129, 182, 192, 235, 353, 378], [37, 83, 88, 92, 111, 127, 243, 303, 324, 325, 348, 353, 359, 371, 377], [5, 87, 90, 124, 127, 180, 259, 288, 290, 302, 312, 313, 324, 332], [55, 62, 89, 98, 108, 132, 168, 240, 248, 322, 325, 327, 347, 353, 391, 396], [4, 8, 13, 35, 37, 39, 41, 64, 111, 174, 212, 245, 248, 251, 263, 288, 335, 373, 375], [10, 39, 93, 110, 168, 227, 228, 254, 288, 336, 378, 381], [75, 92, 122, 176, 198, 211, 214, 283, 334, 353, 359, 379, 386], [5, 8, 13, 19, 57, 87, 104, 125, 130, 176, 202, 249, 252, 290, 309, 391], [88, 132, 173, 176, 235, 247, 253, 292, 324, 328, 339, 359], [19, 32, 61, 84, 87, 118, 120, 125, 129, 132, 181, 190, 288, 290, 331, 355, 359, 366], [13, 25, 46, 126, 140, 157, 165, 225, 226, 252, 288, 304, 327, 353, 378], [12, 14, 26, 56, 72, 95, 131, 132, 134, 142, 253, 298, 337, 361, 391], [4, 18, 37, 49, 87, 93, 196, 225, 226, 246, 248, 250, 255, 310, 354, 358], [64, 87, 110, 111, 128, 135, 151, 165, 177, 188, 191, 268, 312, 334, 352, 354, 357, 371], [10, 17, 19, 30, 40, 48, 81, 97, 125, 129, 130, 182, 234, 305, 328, 393], [13, 69, 80, 114, 192, 200, 235, 343, 345, 353, 354, 360, 374, 378, 383], [83, 87, 94, 105, 107, 124, 144, 153, 219, 290, 298, 324, 349, 358, 367], [10, 36, 142, 169, 221, 232, 241, 246, 346, 347, 375, 383, 390], [26, 104, 126, 143, 176, 186, 241, 247, 250, 318, 320, 333, 360], [66, 92, 116, 148, 191, 215, 254, 333, 334, 335, 336, 351, 353, 358, 380], [9, 37, 55, 56, 76, 79, 90, 111, 122, 124, 161, 192, 247, 313, 353, 359, 374], [17, 30, 34, 56, 64, 68, 90, 125, 151, 168, 176, 188, 286, 333, 338, 360], [26, 143, 173, 182, 190, 194, 246, 284, 286, 328, 333, 355, 357, 360, 362, 363, 377, 380], [1, 13, 87, 122, 168, 171, 186, 201, 297, 328, 349, 352, 380], [18, 39, 61, 88, 98, 123, 129, 131, 148, 162, 165, 243, 285, 314, 340, 349, 360, 377, 378], [67, 98, 117, 118, 122, 128, 156, 174, 184, 207, 244, 250, 330, 335, 342, 372, 375], [13, 38, 63, 160, 180, 185, 189, 190, 219, 248, 253, 275, 297, 318, 355], [1, 44, 47, 93, 107, 172, 235, 276, 281, 287, 290, 306, 333, 334, 337, 347, 353, 376], [13, 15, 32, 125, 127, 157, 165, 176, 236, 344, 350, 381], [47, 65, 93, 134, 159, 174, 218, 282, 318, 336, 358, 373, 379], [7, 17, 40, 66, 102, 141, 154, 159, 165, 172, 174, 177, 328, 329, 334, 348, 379, 382], [4, 34, 36, 76, 79, 122, 127, 138, 176, 241, 267, 309, 334, 367, 382], [9, 17, 33, 46, 90, 103, 125, 138, 144, 157, 185, 198, 224, 250, 260, 291, 326, 343, 349, 377, 381], [29, 31, 53, 58, 134, 138, 193, 287, 305, 308, 333, 334], [13, 64, 83, 93, 129, 192, 227, 244, 397], [7, 8, 14, 78, 85, 103, 138, 175, 176, 200, 203, 234, 301, 313, 361], [13, 75, 87, 111, 244, 253, 288, 321, 339, 341, 357, 395], [4, 14, 42, 64, 69, 108, 110, 117, 122, 131, 159, 163, 188, 198, 200, 206, 244, 292, 300, 354, 390], [14, 37, 73, 87, 129, 135, 144, 176, 182, 300, 346, 352, 380, 383], [23, 50, 87, 143, 171, 186, 191, 223, 290, 333, 334, 364, 378, 380, 388, 391, 393], [5, 14, 23, 36, 62, 71, 76, 95, 99, 128, 176, 211, 229, 357], [12, 33, 47, 70, 81, 90, 97, 119, 122, 131, 189, 190, 191, 235, 244, 253, 320, 350, 359], [10, 13, 23, 93, 110, 120, 135, 171, 195, 250, 293, 298, 329, 344, 354], [13, 29, 37, 163, 169, 200, 211, 214, 217, 236, 246, 249, 282, 327, 349, 353, 362, 372], [5, 13, 23, 41, 57, 62, 76, 89, 111, 135, 195, 234, 248, 314, 334, 341, 349, 380], [17, 35, 57, 117, 121, 206, 235, 243, 265, 329, 358, 374], [13, 28, 41, 55, 69, 101, 103, 126, 138, 198, 267, 276, 288, 313, 334, 335, 339, 354, 376, 383, 394], [11, 13, 19, 36, 38, 58, 75, 124, 232, 235, 265, 286, 298, 330, 333, 359], [4, 19, 25, 43, 110, 125, 165, 331, 334, 341, 349, 355, 372], [40, 55, 64, 70, 117, 126, 127, 135, 160, 172, 173, 186, 270, 318, 338, 344, 378], [122, 176, 198, 238, 246, 284, 286, 290, 318, 329, 337, 381, 394], [23, 36, 37, 44, 117, 124, 198, 204, 233, 248, 282, 288, 297, 314, 332, 336, 388], [15, 33, 54, 64, 75, 85, 115, 127, 165, 196, 229, 237, 254, 307, 327, 335, 349, 383], [22, 87, 121, 127, 161, 180, 248, 250, 276, 313, 324, 347, 349, 355, 357, 359], [14, 48, 67, 88, 130, 131, 172, 188, 195, 203, 267, 282, 333, 339, 350, 392], [22, 31, 37, 98, 118, 132, 135, 137, 142, 151, 243, 244, 282, 305, 333, 349, 350, 351, 353, 358, 374], [15, 42, 67, 75, 125, 134, 189, 255, 261, 309, 334, 350, 380, 382], [10, 39, 87, 97, 105, 109, 125, 137, 225, 226, 253, 329, 341, 354, 363, 372], [5, 17, 42, 64, 80, 111, 120, 169, 175, 206, 237, 267, 288, 290, 324, 351, 364, 390], [3, 33, 55, 75, 91, 97, 103, 132, 187, 220, 232, 234, 240, 288, 301, 330, 336, 337, 338, 340, 359, 374, 380, 382], [13, 87, 98, 125, 126, 127, 128, 250, 330, 341, 353, 360, 374, 382, 391], [59, 66, 75, 125, 135, 172, 192, 230, 231, 255, 256, 276, 300, 306, 339, 349, 353, 390], [25, 36, 56, 90, 107, 125, 127, 142, 165, 195, 244, 246, 319, 347, 355, 375, 380], [2, 33, 35, 36, 72, 74, 87, 92, 111, 131, 145, 176, 244, 248, 282, 333, 355, 359], [5, 39, 127, 134, 137, 200, 240, 283, 284, 343, 344, 372], [9, 32, 37, 80, 96, 104, 110, 117, 154, 176, 244, 297, 298, 339, 353, 374, 381], [38, 51, 64, 76, 80, 93, 96, 134, 150, 173, 275, 290, 340, 347, 359, 363, 380], [55, 89, 111, 126, 157, 159, 162, 182, 188, 244, 253, 280, 334, 359, 384, 398], [59, 64, 75, 81, 97, 105, 115, 125, 155, 198, 248, 262, 319, 323, 376], [13, 41, 76, 125, 127, 130, 134, 135, 159, 167, 183, 229, 230, 240, 246, 308, 319, 329, 333, 334, 340, 344, 363, 382], [8, 13, 19, 31, 70, 76, 79, 96, 127, 153, 163, 165, 184, 227, 230, 247, 255, 336, 337, 348, 353, 357, 361, 362], [71, 87, 111, 121, 130, 142, 150, 160, 175, 224, 248, 314, 336, 353, 357, 359], [67, 84, 101, 130, 287, 288, 332, 333, 359, 361, 377], [34, 52, 90, 100, 125, 135, 165, 173, 320, 341, 352, 359, 382, 392], [13, 18, 39, 55, 62, 87, 248, 255, 290, 327, 349, 353, 355, 360, 383], [1, 9, 12, 29, 32, 36, 82, 139, 140, 149, 153, 165, 167, 180, 185, 231, 241, 244, 274, 299, 309, 329, 355, 362], [48, 66, 98, 107, 120, 122, 125, 135, 190, 195, 198, 215, 253, 256, 280, 282, 307, 320, 334, 349, 353, 355], [1, 7, 13, 25, 64, 98, 139, 144, 166, 176, 206, 236, 262, 330, 362], [37, 55, 116, 123, 125, 131, 165, 234, 266, 276, 328, 329, 342, 349, 353, 359, 391], [126, 137, 191, 215, 239, 288, 290, 321, 324, 333, 334, 338, 349, 353, 362, 379], [50, 57, 87, 93, 98, 115, 134, 148, 174, 229, 251, 260, 285, 298, 313, 348, 349, 350], [5, 13, 31, 45, 69, 81, 108, 122, 127, 160, 165, 176, 179, 237, 244, 301, 316, 352, 360], [5, 87, 95, 98, 101, 132, 135, 159, 167, 190, 203, 217, 234, 235, 247, 289, 333, 341, 343, 352], [22, 56, 66, 85, 87, 93, 126, 127, 163, 230, 243, 248, 254, 280, 301, 305, 334, 357], [13, 19, 53, 59, 76, 91, 117, 122, 195, 298, 303, 309, 337, 345, 398], [9, 54, 84, 107, 125, 127, 135, 144, 156, 173, 176, 202, 215, 231, 234, 246, 266, 282, 335, 336, 347, 351, 374], [11, 15, 30, 31, 40, 57, 58, 87, 88, 113, 186, 244, 245, 256, 308, 334, 377], [62, 111, 176, 196, 228, 231, 288, 294, 302, 306, 350, 353, 375, 378, 392], [119, 131, 133, 154, 161, 179, 198, 232, 234, 265, 301, 314, 344, 353, 378], [67, 84, 123, 172, 175, 176, 182, 229, 290, 359, 360, 375, 383, 393], [33, 36, 39, 102, 116, 136, 137, 208, 234, 256, 307, 329, 341, 347, 376, 380], [13, 27, 32, 80, 95, 108, 131, 165, 167, 180, 190, 200, 235, 241, 244, 323, 330, 339, 372], [1, 18, 37, 62, 67, 82, 85, 118, 125, 147, 159, 169, 174, 243, 284, 307, 313, 318, 355, 391, 396], [10, 87, 91, 135, 169, 176, 215, 246, 267, 282, 295, 320, 345, 353, 380], [2, 11, 13, 29, 90, 124, 131, 132, 170, 174, 176, 229, 246, 258, 298, 336, 344, 349], [14, 37, 42, 71, 128, 152, 185, 218, 288, 304, 315, 353, 362, 380, 391], [17, 20, 36, 73, 93, 128, 163, 194, 211, 217, 282, 290, 320, 354, 383], [9, 26, 32, 101, 127, 169, 178, 183, 191, 236, 244, 310, 330, 336, 345, 353, 360, 372, 380, 394], [7, 13, 64, 78, 81, 90, 115, 133, 164, 169, 244, 246, 269, 278, 290, 292, 310, 320, 353, 360, 364, 366, 380], [8, 65, 81, 84, 91, 126, 129, 158, 183, 184, 194, 254, 262, 333, 334, 339, 351, 363, 382], [44, 87, 96, 97, 125, 161, 173, 177, 183, 188, 189, 209, 235, 288, 315, 334, 351], [50, 56, 60, 62, 67, 71, 105, 149, 154, 158, 164, 167, 185, 221, 285, 288, 308, 337, 344, 353], [6, 10, 37, 62, 74, 79, 81, 128, 139, 154, 167, 198, 228, 244, 267, 290, 302, 368, 394], [6, 30, 35, 36, 62, 65, 71, 112, 153, 163, 167, 180, 186, 195, 249, 286, 303, 329, 334], [158, 241, 282, 324, 332, 334, 351, 353, 363, 365], [17, 89, 117, 144, 165, 180, 185, 198, 229, 244, 290, 334, 335, 380], [20, 32, 45, 57, 64, 66, 120, 135, 144, 176, 192, 244, 297, 301, 354, 381], [1, 7, 35, 62, 74, 122, 159, 170, 172, 238, 239, 307, 308, 338, 349, 350, 359, 366, 368, 375, 382, 383], [7, 9, 23, 66, 92, 103, 111, 135, 182, 203, 246, 247, 265, 285, 288, 303, 317, 329, 348], [13, 39, 74, 87, 127, 135, 144, 193, 212, 243, 270, 290, 303, 315, 375, 376], [33, 36, 40, 59, 101, 120, 127, 244, 285, 287, 309, 339, 391], [4, 10, 39, 195, 268, 284, 336, 354, 359, 375, 381], [39, 42, 62, 79, 83, 84, 101, 109, 132, 138, 202, 215, 277, 353, 358, 359], [10, 39, 46, 73, 84, 87, 132, 170, 192, 219, 232, 246, 288, 320, 337], [10, 12, 56, 87, 91, 101, 132, 227, 254, 301, 303, 333, 343, 347, 351], [7, 8, 15, 18, 82, 105, 130, 232, 250, 290, 316, 332, 348, 350], [36, 109, 110, 125, 154, 191, 193, 246, 265, 348, 349, 350, 378, 383], [12, 16, 45, 57, 87, 92, 101, 105, 129, 130, 155, 167, 218, 292, 293, 327, 349, 354, 361], [30, 59, 64, 121, 125, 149, 163, 188, 212, 250, 348, 350, 351, 352, 353, 378, 380], [1, 69, 130, 138, 194, 200, 239, 260, 264, 357, 380, 381, 382, 396], [7, 10, 19, 40, 57, 61, 125, 137, 141, 212, 239, 251, 310, 333, 347, 359, 380, 383], [20, 28, 50, 97, 109, 134, 157, 162, 184, 199, 244, 246, 286, 352, 353, 360, 373, 380], [35, 62, 87, 96, 122, 127, 136, 142, 148, 155, 165, 186, 196, 227, 354, 380, 388], [81, 82, 101, 115, 125, 200, 243, 313, 351, 359, 367], [7, 19, 40, 61, 107, 108, 124, 154, 161, 244, 309, 329, 345, 379, 394], [10, 27, 48, 66, 75, 103, 116, 122, 128, 221, 228, 319, 322, 350, 377, 398], [2, 64, 74, 117, 130, 165, 172, 180, 191, 218, 221, 288, 299, 325, 347, 353, 355, 360], [5, 76, 79, 87, 106, 111, 137, 168, 180, 235, 243, 288, 315, 321, 338, 344, 348, 378, 382, 383], [0, 29, 31, 37, 40, 50, 88, 100, 129, 134, 137, 144, 174, 186, 203, 254, 310, 313, 329, 341, 359, 364], [69, 70, 71, 96, 115, 121, 130, 157, 159, 200, 230, 246, 250, 299, 318, 324, 353, 359, 380, 391], [7, 90, 95, 116, 127, 128, 135, 137, 141, 154, 161, 254, 330, 359, 379, 388], [10, 14, 56, 91, 108, 125, 130, 167, 211, 228, 246, 258, 280, 306, 324, 333, 336, 338, 379], [4, 5, 14, 57, 85, 98, 125, 135, 136, 176, 254, 334, 336, 337, 351, 358, 362, 379, 383], [1, 4, 13, 18, 19, 32, 50, 60, 62, 87, 117, 176, 211, 251, 329, 343, 359], [38, 56, 94, 103, 117, 125, 129, 144, 159, 176, 244, 251, 253, 324, 345, 353, 386, 390], [4, 22, 38, 47, 59, 64, 82, 97, 110, 135, 153, 176, 235, 236, 241, 287, 288, 303, 333, 347, 358, 359, 361], [2, 5, 20, 52, 97, 125, 127, 132, 135, 137, 174, 188, 191, 243, 288, 310, 334, 346, 348, 349, 362, 372, 378], [19, 35, 55, 98, 125, 131, 134, 147, 153, 246, 255, 390], [5, 59, 62, 129, 136, 153, 198, 225, 235, 239, 254, 295, 334, 338, 341, 359, 361], [8, 13, 51, 94, 121, 122, 125, 126, 129, 240, 272, 290, 297, 323, 352, 358, 376, 391, 395], [6, 111, 116, 122, 125, 131, 135, 164, 175, 200, 212, 221, 267, 287, 319, 328, 334, 344, 378], [83, 108, 143, 172, 176, 192, 198, 246, 262, 286, 287, 308, 338, 340, 343, 348, 353, 367, 380, 383], [39, 82, 92, 118, 126, 128, 144, 171, 211, 234, 244, 253, 328, 333, 339, 357, 359, 380], [37, 62, 64, 81, 97, 122, 125, 127, 137, 211, 246, 344, 360], [7, 29, 62, 67, 69, 81, 87, 107, 132, 151, 160, 229, 244, 284, 285, 317, 358, 387, 390], [13, 75, 76, 83, 87, 154, 165, 190, 212, 258, 285, 308, 309, 316, 320, 332, 336, 340, 352, 353, 354, 358, 383], [9, 19, 29, 46, 122, 125, 127, 130, 170, 171, 174, 180, 182, 232, 282, 290, 359, 362, 367], [13, 40, 71, 98, 101, 116, 125, 127, 169, 172, 175, 283, 288, 309, 311, 313, 323, 334, 353, 391], [3, 9, 70, 104, 118, 173, 200, 219, 246, 262, 288, 297, 309, 328, 329, 334, 341, 353], [32, 89, 93, 131, 132, 142, 199, 200, 214, 246, 287, 298, 307, 339, 348, 349, 357, 358, 368, 372, 391], [103, 134, 159, 176, 186, 235, 261, 276, 282, 290, 301, 317, 329, 345, 356], [10, 59, 125, 129, 130, 192, 217, 283, 318, 343, 345, 349, 353, 380, 383, 392], [19, 76, 79, 102, 107, 126, 155, 161, 180, 253, 288, 289, 290, 314, 329, 333, 334, 360, 368, 378, 394], [12, 92, 98, 105, 137, 149, 172, 196, 198, 244, 260, 262, 282, 298, 329, 345, 353, 368, 390], [31, 39, 79, 83, 121, 125, 167, 171, 186, 198, 288, 303, 306, 334, 337, 376], [13, 20, 36, 57, 98, 108, 114, 165, 171, 225, 226, 262, 269, 305, 309, 351, 377, 389], [13, 51, 71, 93, 110, 129, 130, 156, 165, 170, 173, 183, 191, 200, 211, 212, 255, 266, 299, 301, 329, 336, 348], [31, 56, 97, 122, 125, 129, 160, 188, 202, 204, 206, 225, 235, 247, 254, 255, 288, 334, 350, 362, 365, 367], [9, 32, 37, 70, 75, 87, 88, 96, 125, 130, 162, 163, 168, 169, 257, 285, 308, 310, 337, 373, 392], [18, 40, 42, 47, 73, 76, 85, 105, 108, 125, 130, 132, 134, 167, 191, 284, 310, 311, 344, 358, 361, 374, 378, 379], [5, 19, 29, 31, 48, 65, 98, 129, 131, 143, 165, 171, 172, 196, 198, 277, 296, 311, 317, 327, 351, 380], [51, 69, 96, 98, 117, 123, 130, 131, 148, 161, 168, 172, 176, 184, 202, 324, 332, 336, 348, 392], [1, 20, 37, 57, 70, 76, 79, 87, 165, 176, 234, 251, 333, 388], [8, 13, 134, 135, 153, 165, 169, 193, 195, 255, 273, 337, 348, 359, 360, 382, 391], [2, 14, 53, 71, 83, 127, 136, 144, 149, 208, 234, 235, 293, 301, 347, 352], [20, 40, 42, 95, 135, 141, 165, 199, 250, 290, 299, 308, 337, 338, 350, 353, 354, 355, 358, 380], [13, 19, 33, 35, 36, 49, 85, 121, 122, 127, 137, 158, 165, 282, 303, 320, 328, 334, 365, 367, 374], [17, 37, 123, 126, 127, 139, 140, 143, 167, 185, 192, 235, 254, 275, 315, 340, 349, 353, 362], [57, 72, 127, 159, 163, 165, 176, 199, 215, 218, 238, 254, 284, 288, 336, 339, 347, 352, 380, 395], [54, 69, 81, 101, 114, 121, 165, 206, 236, 313, 332, 338, 349, 358, 360, 362, 377], [29, 37, 43, 120, 127, 176, 193, 244, 246, 254, 284, 288, 336, 339, 372], [36, 56, 85, 122, 125, 126, 154, 232, 282, 308, 314, 315, 324, 336, 353, 359, 382], [7, 99, 104, 117, 124, 125, 143, 176, 239, 298, 318, 383], [13, 20, 71, 90, 108, 122, 176, 186, 214, 231, 247, 262, 267, 280, 286, 300, 332, 358, 377, 380, 385, 390, 393], [31, 65, 75, 79, 85, 91, 109, 110, 120, 159, 229, 235, 288, 298, 347, 355, 359, 379, 381], [38, 75, 82, 90, 99, 202, 248, 265, 324, 329, 350, 354, 355, 365], [7, 15, 72, 90, 117, 125, 140, 144, 171, 198, 269, 271, 282, 305, 325, 338, 343, 353], [13, 14, 20, 29, 37, 42, 45, 47, 165, 184, 244, 329, 341, 347, 372], [31, 36, 82, 99, 149, 154, 173, 182, 185, 200, 217, 251, 298, 329, 332, 333, 349, 353, 354, 355, 377, 383], [32, 44, 45, 52, 93, 97, 108, 114, 120, 144, 155, 172, 236, 240, 267, 272, 282, 288, 329, 333, 334, 343, 381], [35, 55, 57, 62, 95, 96, 98, 127, 131, 177, 262, 317, 318, 357, 359, 380, 388], [22, 24, 68, 103, 115, 119, 120, 125, 128, 156, 162, 184, 186, 235, 244, 327, 353, 358, 378, 380, 393], [29, 37, 62, 67, 81, 83, 93, 104, 110, 129, 132, 142, 172, 274, 298, 354, 380], [19, 45, 66, 87, 104, 108, 118, 155, 170, 176, 234, 286, 310, 313, 327, 329, 333, 347, 358, 368, 380, 383, 386], [10, 14, 32, 83, 96, 131, 165, 180, 205, 211, 249, 255, 286, 288, 292, 299, 312, 336, 338, 349, 368, 375], [2, 13, 48, 75, 85, 98, 116, 125, 126, 128, 135, 136, 151, 188, 195, 243, 280, 289, 333, 339, 349, 378, 382], [9, 19, 39, 45, 87, 106, 117, 125, 126, 127, 154, 165, 202, 211, 256, 309, 360, 397, 398], [14, 21, 65, 76, 87, 93, 97, 105, 131, 177, 212, 254, 294, 336, 349, 359, 381], [36, 55, 65, 70, 87, 93, 96, 98, 108, 127, 254, 337, 352, 359, 375, 380], [22, 42, 62, 82, 131, 132, 136, 158, 168, 196, 267, 305, 336], [45, 69, 74, 75, 81, 120, 123, 126, 127, 130, 150, 171, 191, 194, 313, 339, 368, 378, 379, 389, 398], [35, 43, 85, 98, 122, 131, 135, 176, 189, 250, 259, 277, 288, 303, 333, 336, 345, 376, 381, 387], [1, 6, 34, 87, 115, 129, 131, 202, 235, 252, 256, 263, 317, 328, 349, 372, 391], [3, 18, 42, 48, 84, 90, 92, 138, 193, 227, 288, 310, 315, 353, 375], [2, 10, 31, 66, 124, 145, 240, 314, 334], [32, 38, 84, 141, 165, 188, 193, 212, 346, 359, 379, 380], [10, 75, 81, 96, 111, 140, 179, 298, 309, 353, 357, 359, 380, 396], [2, 34, 121, 127, 132, 134, 184, 234, 244, 251, 262, 290, 308, 359, 380], [17, 24, 93, 172, 186, 198, 218, 234, 239, 250, 252, 255, 307, 309, 325, 334, 354, 359], [14, 18, 45, 50, 131, 174, 211, 237, 252, 267, 309, 334, 348, 351, 377, 391], [32, 61, 87, 97, 125, 126, 132, 184, 249, 252, 273, 284, 288, 339, 383, 398], [76, 81, 87, 127, 147, 161, 163, 199, 206, 306, 329, 340, 349, 353, 360, 383], [14, 16, 76, 87, 101, 169, 188, 243, 246, 251, 253, 269, 298, 355, 375, 380], [32, 79, 87, 103, 117, 125, 127, 177, 244, 301, 305, 317, 333, 338, 340, 342, 391], [4, 67, 76, 121, 127, 130, 140, 158, 165, 186, 193, 251, 301, 303, 330, 336], [11, 76, 83, 84, 87, 214, 248, 276, 299, 311, 320, 329, 332, 335, 371], [2, 4, 19, 40, 42, 71, 98, 119, 121, 137, 167, 262, 288, 295, 306, 339, 350, 382], [14, 40, 54, 90, 125, 129, 132, 146, 147, 165, 169, 176, 190, 253, 284, 303, 307, 316, 339, 342, 359, 389], [47, 59, 71, 103, 125, 126, 129, 130, 200, 206, 240, 254, 276, 282, 299, 303, 307, 318, 320, 336, 338, 357, 362, 380, 387, 392], [4, 22, 58, 102, 113, 115, 153, 167, 188, 212, 262, 286, 305, 333, 348, 354, 360, 371, 379, 386], [5, 6, 56, 61, 108, 128, 129, 164, 165, 177, 182, 225, 226, 235, 244, 246, 249, 310, 333, 348, 349, 381, 391], [18, 32, 33, 53, 56, 176, 186, 199, 200, 244, 246, 248, 259, 285, 289, 306, 358, 371, 373, 375, 379], [40, 43, 70, 76, 83, 84, 90, 93, 101, 125, 159, 204, 276, 282, 304, 320, 339, 351, 353, 367, 391], [14, 19, 59, 71, 76, 87, 93, 97, 105, 111, 120, 121, 122, 154, 171, 211, 231, 244, 286, 288, 341, 351], [10, 56, 65, 72, 92, 108, 123, 129, 212, 258, 329, 353, 359], [5, 76, 124, 127, 161, 172, 188, 244, 250, 266, 290, 318, 347, 351, 369, 382, 391, 395], [1, 33, 86, 120, 121, 130, 154, 162, 173, 192, 241, 244, 262, 338, 339, 343, 353, 380, 390], [1, 15, 22, 54, 57, 85, 126, 127, 176, 188, 248, 305, 332, 347, 349, 358, 367], [91, 111, 122, 125, 130, 178, 190, 224, 225, 226, 235, 286, 308, 329, 334, 345, 346, 349, 358, 362, 367], [16, 26, 51, 54, 84, 85, 98, 120, 272, 319, 349, 359, 360, 362, 377, 391, 398], [73, 85, 102, 109, 128, 153, 171, 184, 248, 249, 256, 298, 300, 335, 338, 340, 355, 370], [9, 108, 122, 131, 164, 168, 173, 176, 195, 218, 235, 286, 341, 350, 353, 358, 375, 377], [25, 62, 125, 140, 165, 173, 200, 225, 226, 243, 283, 286, 329, 343, 357, 366, 377], [10, 35, 58, 64, 98, 103, 125, 127, 129, 135, 141, 165, 169, 175, 189, 244, 258, 259, 306, 331, 333, 378, 380, 391], [54, 87, 89, 99, 116, 125, 129, 221, 246, 269, 324, 335, 348, 351], [85, 90, 103, 115, 131, 134, 165, 207, 282, 307, 313, 328, 346, 349, 380, 383, 387, 398], [10, 40, 74, 84, 160, 239, 253, 272, 282, 333, 344, 351, 359, 360, 379], [32, 38, 54, 74, 76, 117, 163, 171, 176, 217, 227, 250, 251, 280, 329, 330, 350, 378], [13, 20, 40, 107, 129, 135, 154, 158, 161, 163, 179, 206, 281, 315, 325, 351, 355, 359, 397], [0, 4, 37, 49, 62, 98, 117, 129, 177, 244, 285, 289, 306, 338, 360, 381], [36, 38, 43, 61, 71, 87, 120, 128, 172, 200, 235, 247, 251, 282, 299, 329, 341, 352, 355], [43, 71, 83, 85, 108, 117, 118, 121, 133, 138, 165, 206, 231, 254, 290, 291, 335, 336, 359, 362, 377], [29, 32, 71, 103, 122, 125, 198, 224, 244, 285, 303, 333, 335, 337], [54, 55, 82, 87, 101, 108, 127, 229, 230, 269, 290, 306, 349, 353], [9, 117, 126, 137, 154, 165, 167, 186, 192, 229, 277, 283, 301, 317, 365, 367, 372, 378], [4, 11, 19, 47, 51, 92, 110, 132, 137, 140, 290, 298, 361, 377, 379], [23, 83, 98, 134, 165, 170, 186, 190, 253, 269, 308, 322, 327, 332, 335, 344, 398], [60, 83, 111, 129, 173, 176, 186, 232, 306, 327, 329, 349, 355], [25, 31, 40, 56, 72, 95, 126, 144, 149, 161, 173, 240, 262, 332, 333, 356, 368, 391, 394], [91, 127, 134, 144, 155, 158, 161, 232, 251, 280, 287, 353, 380, 394], [37, 43, 57, 84, 87, 149, 175, 288, 330, 380], [8, 9, 83, 97, 120, 128, 158, 171, 193, 232, 287, 308, 309, 334, 355], [39, 40, 62, 82, 94, 98, 101, 144, 147, 205, 290, 333, 339, 353, 372, 397], [10, 20, 38, 125, 135, 138, 168, 180, 191, 203, 231, 250, 280, 301, 328, 345, 388], [44, 54, 64, 87, 117, 122, 127, 154, 234, 239, 244, 298, 329, 378, 383], [13, 62, 70, 97, 121, 176, 244, 267, 282, 318, 324, 334, 341, 353, 386, 388], [40, 89, 91, 117, 125, 131, 155, 173, 193, 244, 273, 277, 328, 333, 360, 382], [30, 47, 95, 108, 127, 165, 188, 211, 273, 349, 354, 368, 391], [19, 52, 87, 98, 100, 122, 125, 157, 159, 215, 217, 235, 254, 309, 336, 344, 349, 382], [19, 85, 87, 136, 144, 180, 190, 229, 310, 345, 365, 376, 390], [35, 52, 87, 113, 124, 135, 145, 167, 174, 225, 226, 244, 247, 300, 359], [10, 35, 69, 103, 129, 144, 165, 180, 230, 232, 329, 335, 353, 359, 371, 390], [5, 13, 80, 83, 135, 139, 142, 176, 179, 190, 205, 217, 282, 298, 308, 334, 353, 359], [24, 52, 67, 108, 135, 138, 153, 176, 231, 249, 283, 304, 337, 351, 353, 355], [90, 93, 127, 132, 136, 163, 165, 196, 284, 306, 353, 383], [20, 37, 103, 126, 135, 184, 204, 215, 221, 288, 300, 329, 339, 358, 383], [16, 36, 52, 99, 117, 136, 171, 190, 243, 244, 303, 315, 333, 349, 373, 382], [0, 57, 69, 98, 125, 129, 132, 158, 165, 190, 191, 193, 198, 254, 256, 285, 288, 303, 339, 346, 351, 391], [1, 13, 21, 87, 125, 132, 150, 204, 240, 249, 253, 265, 288, 334, 343, 348, 349, 359], [29, 40, 71, 80, 91, 99, 122, 203, 289, 290, 298, 329, 353, 380, 390], [2, 5, 36, 57, 93, 102, 135, 140, 314, 343, 398], [20, 59, 107, 193, 204, 246, 247, 336, 341, 342, 354, 359, 360, 383], [47, 71, 93, 111, 116, 120, 122, 130, 251, 286, 298, 299, 348], [21, 52, 56, 69, 76, 118, 120, 125, 137, 274, 280, 324, 327, 335, 339, 340], [23, 29, 57, 75, 98, 132, 149, 157, 160, 235, 244, 288, 327, 340, 354, 372, 377], [4, 22, 97, 103, 111, 129, 131, 151, 158, 176, 204, 248, 265, 309, 359, 391, 392], [15, 17, 73, 105, 115, 170, 186, 228, 255, 317, 321, 339, 349, 379, 380, 381], [17, 52, 72, 103, 188, 329, 342, 353, 358, 359, 374, 376, 380, 393], [40, 48, 74, 124, 135, 191, 225, 226, 237, 291, 300, 304, 310, 347, 359, 380, 396], [2, 36, 47, 57, 122, 125, 174, 188, 203, 224, 255, 325, 353, 359, 387], [13, 58, 69, 83, 115, 120, 134, 161, 165, 174, 175, 191, 246, 255, 280, 353, 357, 358, 359, 379], [1, 29, 47, 87, 89, 135, 176, 190, 209, 236, 304, 344, 348, 358, 359, 378], [8, 13, 40, 52, 58, 61, 71, 125, 144, 168, 189, 210, 260, 337, 338, 340, 347, 376, 380], [29, 90, 126, 127, 129, 136, 145, 159, 165, 188, 274, 284, 288, 316, 329, 358, 380], [2, 19, 103, 120, 123, 159, 165, 175, 177, 180, 238, 244, 251, 294, 329, 342, 345, 349, 357, 376, 392], [41, 42, 59, 71, 81, 98, 101, 117, 159, 171, 180, 240, 285, 290, 299, 344, 353], [83, 103, 108, 142, 175, 248, 290, 300, 321, 354, 365, 374, 382], [12, 67, 105, 130, 140, 171, 188, 192, 244, 276, 290, 302, 348, 349, 357, 360, 380], [4, 13, 36, 65, 75, 160, 165, 185, 198, 235, 293, 324, 327, 333, 345, 347, 375, 383], [37, 61, 80, 125, 234, 283, 290, 353, 359, 378, 383], [9, 32, 83, 110, 155, 248, 252, 288, 313], [37, 48, 52, 93, 167, 170, 179, 244, 267, 288, 296, 333, 335, 355, 374], [35, 92, 98, 153, 165, 184, 215, 233, 242, 290, 339, 355], [9, 38, 83, 121, 127, 165, 176, 235, 253, 305, 330, 337, 355, 358, 359], [35, 117, 122, 125, 132, 136, 183, 235, 254, 280, 285, 286, 329, 334, 338, 353, 372], [6, 87, 117, 125, 141, 144, 153, 157, 179, 215, 267, 272, 289, 329, 336, 359], [7, 14, 37, 82, 135, 147, 154, 202, 244, 290, 297, 298, 345, 355, 368, 383], [105, 135, 173, 244, 255, 280, 288, 299, 304, 307, 337, 338, 341, 344], [19, 31, 33, 77, 92, 99, 114, 151, 173, 202, 253, 318, 329, 333, 358, 371], [1, 8, 14, 30, 39, 120, 157, 172, 227, 229, 251, 257, 272, 339, 380], [19, 98, 171, 191, 213, 246, 289, 353, 357, 366, 374, 383], [8, 98, 125, 126, 144, 152, 244, 277, 282, 290, 322, 393], [17, 206, 211, 224, 336, 338, 386], [52, 55, 71, 99, 105, 191, 211, 215, 224, 246, 290, 300, 336, 339, 361], [15, 16, 44, 66, 96, 121, 127, 162, 167, 202, 219, 243, 244, 254, 282, 320, 345, 390], [7, 83, 92, 121, 130, 160, 177, 280, 308, 309, 339, 350, 352, 358, 380, 390], [67, 122, 144, 148, 170, 173, 184, 222, 280, 374], [2, 4, 15, 19, 115, 130, 136, 148, 172, 180, 243, 251, 313, 329, 333, 359, 364], [90, 98, 108, 124, 167, 176, 202, 254, 286, 351, 359], [80, 126, 135, 167, 212, 242, 243, 256, 283, 286, 295, 327, 337, 340, 346, 357, 358, 364], [19, 108, 125, 132, 149, 172, 180, 186, 200, 254, 286, 296, 339, 344, 350, 359, 391], [62, 65, 67, 105, 127, 129, 132, 250, 298, 307, 334, 344, 359, 383], [31, 59, 87, 107, 121, 131, 132, 160, 244, 246, 247, 253, 344, 360, 394], [4, 39, 76, 125, 130, 148, 168, 170, 191, 196, 298, 306, 327, 338, 345, 349, 360, 375], [13, 14, 32, 84, 98, 122, 126, 156, 188, 235, 255, 330, 336, 338, 375, 380, 389], [5, 18, 31, 54, 71, 74, 76, 81, 87, 93, 126, 129, 182, 303, 327, 353, 359, 373, 391], [13, 37, 64, 137, 138, 180, 244, 247, 251, 253, 269, 284, 308, 344, 374, 376], [5, 7, 10, 23, 35, 125, 168, 169, 187, 191, 192, 313, 337, 340, 342, 365], [62, 67, 122, 125, 165, 190, 217, 243, 254, 256, 265, 299, 318, 353, 394], [4, 24, 62, 92, 109, 118, 134, 143, 144, 176, 190, 199, 221, 299, 349, 380], [22, 35, 64, 74, 92, 113, 161, 172, 193, 282, 287, 307, 359, 393], [37, 50, 66, 75, 76, 78, 82, 87, 139, 159, 172, 176, 188, 231, 352, 371], [19, 31, 75, 121, 144, 152, 163, 171, 172, 198, 243, 246, 285, 288, 289, 333, 344, 347, 357, 398], [1, 15, 17, 51, 57, 65, 69, 127, 241, 244, 254, 259, 329, 336, 358], [9, 95, 117, 121, 125, 137, 204, 242, 247, 301, 309, 314, 334, 339, 350, 354, 358], [9, 61, 96, 111, 130, 163, 180, 211, 225, 226, 241, 253, 282, 283, 346, 355, 359, 380, 383], [94, 117, 121, 124, 126, 130, 135, 172, 199, 232, 286, 325, 336, 352, 362, 375], [110, 125, 163, 250, 265, 303, 329, 334, 391], [47, 72, 76, 111, 125, 157, 169, 245, 254, 285, 287, 297, 298, 336, 353, 359, 383], [62, 93, 115, 125, 127, 130, 174, 231, 308, 310, 329, 333, 355, 359, 390], [44, 116, 163, 167, 180, 191, 200, 245, 254, 329, 343, 345, 354, 364], [31, 62, 105, 108, 144, 145, 162, 173, 177, 191, 198, 247, 249, 344, 345, 348, 353], [29, 65, 66, 74, 83, 87, 125, 148, 165, 228, 334, 353, 359, 380, 383, 391], [2, 15, 125, 130, 239, 290, 312, 336, 337, 341, 398], [40, 76, 87, 114, 119, 120, 165, 229, 265, 313, 324, 349, 358, 383], [48, 62, 87, 91, 103, 186, 195, 212, 214, 315, 322, 327, 330, 338, 339], [9, 32, 85, 108, 135, 191, 224, 237, 257, 288, 307, 310, 313, 318, 329, 337, 352, 395], [87, 93, 102, 112, 129, 154, 171, 236, 317, 320, 349, 350, 359, 380], [1, 14, 92, 111, 137, 140, 186, 290, 329, 336, 354, 355, 378, 379, 383], [7, 26, 37, 47, 84, 101, 144, 153, 175, 180, 198, 232, 243, 305, 333, 353, 357, 383], [20, 58, 76, 93, 99, 127, 134, 154, 188, 206, 246, 312, 313, 324], [2, 12, 117, 125, 160, 167, 188, 206, 279, 285, 287, 301, 329, 332, 333, 336, 344, 362], [2, 76, 126, 127, 137, 165, 244, 288, 290, 339, 346, 351, 359, 365, 383], [66, 108, 136, 151, 174, 265, 344, 351, 353, 357, 378, 386], [8, 76, 87, 90, 111, 116, 124, 176, 198, 334, 337, 349, 359, 379, 394], [32, 36, 42, 76, 81, 125, 127, 205, 227, 262, 280, 288, 326, 336, 390, 398], [9, 32, 65, 83, 89, 93, 97, 122, 129, 178, 180, 215, 241, 246, 323, 332, 353, 362, 364, 380], [5, 24, 56, 127, 130, 155, 184, 191, 217, 235, 245, 339, 344, 358, 359, 362, 380], [14, 40, 64, 71, 93, 108, 131, 165, 188, 204, 217, 235, 237, 241, 248, 308, 309, 318, 380, 387], [17, 29, 34, 74, 125, 175, 184, 196, 211, 275, 301, 318, 327, 334, 349, 355, 358, 368], [15, 45, 110, 111, 116, 129, 132, 211, 247, 275, 286, 317, 333, 334, 377, 383], [4, 5, 59, 87, 103, 124, 125, 127, 130, 165, 241, 265, 299, 353, 360], [31, 120, 124, 135, 154, 197, 235, 243, 247, 248, 258, 309, 320, 335, 357], [50, 125, 127, 130, 137, 147, 171, 172, 267, 289, 301, 308, 325, 334, 337, 353, 360, 374, 391], [62, 64, 69, 87, 111, 118, 129, 134, 212, 239, 244, 246, 250, 254, 307, 322, 329, 370, 372], [54, 92, 128, 160, 198, 244, 248, 255, 284, 314, 335, 349, 358, 360, 376, 380], [9, 13, 29, 54, 72, 89, 110, 122, 126, 139, 158, 159, 163, 230, 304, 306, 313], [1, 9, 54, 95, 108, 132, 176, 193, 243, 251, 339, 378], [0, 96, 99, 135, 137, 184, 212, 232, 251, 315, 334], [140, 157, 165, 182, 235, 294, 314, 349, 354, 365], [14, 18, 31, 56, 117, 125, 138, 227, 246, 283, 334, 345, 352, 357, 361], [0, 71, 82, 130, 131, 144, 161, 235, 247, 301, 333, 335, 345, 353, 355, 359, 360, 374], [6, 23, 35, 117, 125, 141, 169, 200, 244, 288, 298, 338, 353, 379], [10, 98, 125, 127, 138, 153, 219, 244, 307, 350, 353, 366, 367], [9, 32, 40, 122, 126, 127, 170, 176, 300, 334, 350, 391], [6, 13, 31, 87, 89, 97, 125, 165, 171, 173, 176, 244, 331, 348, 373], [10, 61, 87, 105, 123, 125, 127, 195, 260, 265, 323, 361, 362], [2, 20, 90, 124, 353, 354, 378, 382], [5, 48, 58, 83, 98, 117, 125, 126, 196, 198], [13, 37, 50, 64, 66, 79, 99, 132, 135, 244, 247, 380], [57, 165, 235, 238, 248, 272, 287, 299, 327, 329, 334, 350, 353, 380], [55, 66, 118, 125, 130, 169, 250, 255, 271, 314, 324, 338, 353], [7, 31, 62, 84, 103, 105, 111, 126, 132, 149, 154, 191, 250, 334, 372, 375], [56, 81, 114, 117, 120, 124, 127, 128, 154, 254, 290, 317, 345, 354], [4, 13, 86, 101, 153, 191, 193, 231, 243, 258, 283, 288, 308, 353, 387, 392], [5, 37, 58, 62, 67, 84, 87, 176, 237, 267, 333, 334, 347], [1, 7, 74, 110, 165, 168, 182, 233, 288, 305, 309, 315, 347, 351, 353, 358, 360, 375], [57, 84, 129, 138, 165, 243, 244, 259, 280, 282, 290, 380, 383]
    ),

};
//---------------------------------------------------js 中文转拼音------end-------------------------------------------


//---------------------------------------------------js 解析Uri------begin-------------------------------------------

/*!
 * jsUri
 * https://github.com/derek-watson/jsUri
Pass any URL into the constructor:

var uri = new Uri('http://user:pass@www.test.com:81/index.html?q=books#fragment')
Use property methods to get at the various parts:

uri.protocol()    // http
uri.userInfo()    // user:pass
uri.host()        // www.test.com
uri.port()        // 81
uri.path()        // /index.html
uri.query()       // q=books
uri.anchor()      // fragment
Property methods accept an optional value to set:

uri.protocol('https')
uri.toString()    // https://user:pass@www.test.com:81/index.html?q=books#fragment

uri.host('mydomain.com')
uri.toString()    // https://user:pass@mydomain.com:81/index.html?q=books#fragment
Chainable setter methods help you compose strings:

new Uri()
    .setPath('/archives/1979/')
    .setQuery('?page=1')                   // /archives/1979?page=1

new Uri()
    .setPath('/index.html')
    .setAnchor('content')
    .setHost('www.test.com')
    .setPort(8080)
    .setUserInfo('username:password')
    .setProtocol('https')
    .setQuery('this=that&some=thing')      // https://username:password@www.test.com:8080/index.html?this=that&some=thing#content

new Uri('http://www.test.com')
    .setHost('www.yahoo.com')
    .setProtocol('https')                  // https://www.yahoo.com
Query param methods
Returns the first query param value for the key:

new Uri('?cat=1&cat=2&cat=3').getQueryParamValue('cat')             // 1
Returns all query param values for the given key:

new Uri('?cat=1&cat=2&cat=3').getQueryParamValues('cat')            // [1, 2, 3]
Internally, query key/value pairs are stored as a series of two-value arrays in the Query object:

new Uri('?a=b&c=d').query().params                  // [ ['a', 'b'], ['c', 'd']]
Add query param values:

new Uri().addQueryParam('q', 'books')               // ?q=books

new Uri('http://www.github.com')
    .addQueryParam('testing', '123')
    .addQueryParam('one', 1)                        // http://www.github.com/?testing=123&one=1

// insert param at index 0
new Uri('?b=2&c=3&d=4').addQueryParam('a', '1', 0)  // ?a=1&b=2&c=3&d=4
Replace every query string parameter named key with newVal:

new Uri().replaceQueryParam('page', 2)     // ?page=2

new Uri('?a=1&b=2&c=3')
    .replaceQueryParam('a', 'eh')          // ?a=eh&b=2&c=3

new Uri('?a=1&b=2&c=3&c=4&c=5&c=6')
    .replaceQueryParam('c', 'five', '5')   // ?a=1&b=2&c=3&c=4&c=five&c=6
Removes instances of query parameters named key:

new Uri('?a=1&b=2&c=3')
    .deleteQueryParam('a')                 // ?b=2&c=3

new Uri('test.com?a=1&b=2&c=3&a=eh')
    .deleteQueryParam('a', 'eh')           // test.com/?a=1&b=2&c=3
Test for the existence of query parameters named key:

new Uri('?a=1&b=2&c=3')
    .hasQueryParam('a')                    // true

new Uri('?a=1&b=2&c=3')
    .hasQueryParam('d')                    // false
Create an identical URI object with no shared state:

var baseUri = new Uri('http://localhost/')

baseUri.clone().setProtocol('https')   // https://localhost/
baseUri                                // http://localhost/
This project incorporates the parseUri regular expression by Steven Levithan.
 */

/*globals define, module */

(function (global) {

    var re = {
        starts_with_slashes: /^\/+/,
        ends_with_slashes: /\/+$/,
        pluses: /\+/g,
        query_separator: /[&;]/,
        uri_parser: /^(?:(?![^:@]+:[^:@\/]*@)([^:\/?#.]+):)?(?:\/\/)?((?:(([^:@\/]*)(?::([^:@\/]*))?)?@)?(\[[0-9a-fA-F:.]+\]|[^:\/?#]*)(?::(\d+|(?=:)))?(:)?)((((?:[^?#](?![^?#\/]*\.[^?#\/.]+(?:[?#]|$)))*\/?)?([^?#\/]*))(?:\?([^#]*))?(?:#(.*))?)/
    };

    /**
     * Define forEach for older js environments
     * @see https://developer.mozilla.org/en-US/docs/JavaScript/Reference/Global_Objects/Array/forEach#Compatibility
     */
    if (!Array.prototype.forEach) {
        Array.prototype.forEach = function (callback, thisArg) {
            var T, k;

            if (this == null) {
                throw new TypeError(' this is null or not defined');
            }

            var O = Object(this);
            var len = O.length >>> 0;

            if (typeof callback !== "function") {
                throw new TypeError(callback + ' is not a function');
            }

            if (arguments.length > 1) {
                T = thisArg;
            }

            k = 0;

            while (k < len) {
                var kValue;
                if (k in O) {
                    kValue = O[k];
                    callback.call(T, kValue, k, O);
                }
                k++;
            }
        };
    }

    /**
     * unescape a query param value
     * @param  {string} s encoded value
     * @return {string}   decoded value
     */
    function decode(s) {
        if (s) {
            s = s.toString().replace(re.pluses, '%20');
            s = decodeURIComponent(s);
        }
        return s;
    }

    /**
     * Breaks a uri string down into its individual parts
     * @param  {string} str uri
     * @return {object}     parts
     */
    function parseUri(str) {
        var parser = re.uri_parser;
        var parserKeys = ["source", "protocol", "authority", "userInfo", "user", "password", "host", "port", "isColonUri", "relative", "path", "directory", "file", "query", "anchor"];
        var m = parser.exec(str || '');
        var parts = {};

        parserKeys.forEach(function (key, i) {
            parts[key] = m[i] || '';
        });

        return parts;
    }

    /**
     * Breaks a query string down into an array of key/value pairs
     * @param  {string} str query
     * @return {array}      array of arrays (key/value pairs)
     */
    function parseQuery(str) {
        var i, ps, p, n, k, v, l;
        var pairs = [];

        if (typeof (str) === 'undefined' || str === null || str === '') {
            return pairs;
        }

        if (str.indexOf('?') === 0) {
            str = str.substring(1);
        }

        ps = str.toString().split(re.query_separator);

        for (i = 0, l = ps.length; i < l; i++) {
            p = ps[i];
            n = p.indexOf('=');

            if (n !== 0) {
                k = decode(p.substring(0, n));
                v = decode(p.substring(n + 1));
                pairs.push(n === -1 ? [p, null] : [k, v]);
            }

        }
        return pairs;
    }

    /**
     * Creates a new Uri object
     * @constructor
     * @param {string} str
     */
    function Uri(str) {
        this.uriParts = parseUri(str);
        this.queryPairs = parseQuery(this.uriParts.query);
        this.hasAuthorityPrefixUserPref = null;
    }

    /**
     * Define getter/setter methods
     */
    ['protocol', 'userInfo', 'host', 'port', 'path', 'anchor'].forEach(function (key) {
        Uri.prototype[key] = function (val) {
            if (typeof val !== 'undefined') {
                this.uriParts[key] = val;
            }
            return this.uriParts[key];
        };
    });

    /**
     * if there is no protocol, the leading // can be enabled or disabled
     * @param  {Boolean}  val
     * @return {Boolean}
     */
    Uri.prototype.hasAuthorityPrefix = function (val) {
        if (typeof val !== 'undefined') {
            this.hasAuthorityPrefixUserPref = val;
        }

        if (this.hasAuthorityPrefixUserPref === null) {
            return (this.uriParts.source.indexOf('//') !== -1);
        } else {
            return this.hasAuthorityPrefixUserPref;
        }
    };

    Uri.prototype.isColonUri = function (val) {
        if (typeof val !== 'undefined') {
            this.uriParts.isColonUri = !!val;
        } else {
            return !!this.uriParts.isColonUri;
        }
    };

    /**
     * Serializes the internal state of the query pairs
     * @param  {string} [val]   set a new query string
     * @return {string}         query string
     */
    Uri.prototype.query = function (val) {
        var s = '', i, param, l;

        if (typeof val !== 'undefined') {
            this.queryPairs = parseQuery(val);
        }

        for (i = 0, l = this.queryPairs.length; i < l; i++) {
            param = this.queryPairs[i];
            if (s.length > 0) {
                s += '&';
            }
            if (param[1] === null) {
                s += param[0];
            } else {
                s += param[0];
                s += '=';
                if (typeof param[1] !== 'undefined') {
                    s += encodeURIComponent(param[1]);
                }
            }
        }
        return s.length > 0 ? '?' + s : s;
    };

    /**
     * returns the first query param value found for the key
     * @param  {string} key query key
     * @return {string}     first value found for key
     */
    Uri.prototype.getQueryParamValue = function (key) {
        var param, i, l;
        for (i = 0, l = this.queryPairs.length; i < l; i++) {
            param = this.queryPairs[i];
            if (key === param[0]) {
                return param[1];
            }
        }
    };

    /**
     * returns an array of query param values for the key
     * @param  {string} key query key
     * @return {array}      array of values
     */
    Uri.prototype.getQueryParamValues = function (key) {
        var arr = [], i, param, l;
        for (i = 0, l = this.queryPairs.length; i < l; i++) {
            param = this.queryPairs[i];
            if (key === param[0]) {
                arr.push(param[1]);
            }
        }
        return arr;
    };

    /**
     * removes query parameters
     * @param  {string} key     remove values for key
     * @param  {val}    [val]   remove a specific value, otherwise removes all
     * @return {Uri}            returns self for fluent chaining
     */
    Uri.prototype.deleteQueryParam = function (key, val) {
        var arr = [], i, param, keyMatchesFilter, valMatchesFilter, l;

        for (i = 0, l = this.queryPairs.length; i < l; i++) {

            param = this.queryPairs[i];
            keyMatchesFilter = decode(param[0]) === decode(key);
            valMatchesFilter = param[1] === val;

            if ((arguments.length === 1 && !keyMatchesFilter) || (arguments.length === 2 && (!keyMatchesFilter || !valMatchesFilter))) {
                arr.push(param);
            }
        }

        this.queryPairs = arr;

        return this;
    };

    /**
     * adds a query parameter
     * @param  {string}  key        add values for key
     * @param  {string}  val        value to add
     * @param  {integer} [index]    specific index to add the value at
     * @return {Uri}                returns self for fluent chaining
     */
    Uri.prototype.addQueryParam = function (key, val, index) {
        if (arguments.length === 3 && index !== -1) {
            index = Math.min(index, this.queryPairs.length);
            this.queryPairs.splice(index, 0, [key, val]);
        } else if (arguments.length > 0) {
            this.queryPairs.push([key, val]);
        }
        return this;
    };

    /**
     * test for the existence of a query parameter
     * @param  {string}  key        check values for key
     * @return {Boolean}            true if key exists, otherwise false
     */
    Uri.prototype.hasQueryParam = function (key) {
        var i, len = this.queryPairs.length;
        for (i = 0; i < len; i++) {
            if (this.queryPairs[i][0] == key)
                return true;
        }
        return false;
    };

    /**
     * replaces query param values
     * @param  {string} key         key to replace value for
     * @param  {string} newVal      new value
     * @param  {string} [oldVal]    replace only one specific value (otherwise replaces all)
     * @return {Uri}                returns self for fluent chaining
     */
    Uri.prototype.replaceQueryParam = function (key, newVal, oldVal) {
        var index = -1, len = this.queryPairs.length, i, param;

        if (arguments.length === 3) {
            for (i = 0; i < len; i++) {
                param = this.queryPairs[i];
                if (decode(param[0]) === decode(key) && decodeURIComponent(param[1]) === decode(oldVal)) {
                    index = i;
                    break;
                }
            }
            if (index >= 0) {
                this.deleteQueryParam(key, decode(oldVal)).addQueryParam(key, newVal, index);
            }
        } else {
            for (i = 0; i < len; i++) {
                param = this.queryPairs[i];
                if (decode(param[0]) === decode(key)) {
                    index = i;
                    break;
                }
            }
            this.deleteQueryParam(key);
            this.addQueryParam(key, newVal, index);
        }
        return this;
    };

    /**
     * Define fluent setter methods (setProtocol, setHasAuthorityPrefix, etc)
     */
    ['protocol', 'hasAuthorityPrefix', 'isColonUri', 'userInfo', 'host', 'port', 'path', 'query', 'anchor'].forEach(function (key) {
        var method = 'set' + key.charAt(0).toUpperCase() + key.slice(1);
        Uri.prototype[method] = function (val) {
            this[key](val);
            return this;
        };
    });

    /**
     * Scheme name, colon and doubleslash, as required
     * @return {string} http:// or possibly just //
     */
    Uri.prototype.scheme = function () {
        var s = '';

        if (this.protocol()) {
            s += this.protocol();
            if (this.protocol().indexOf(':') !== this.protocol().length - 1) {
                s += ':';
            }
            s += '//';
        } else {
            if (this.hasAuthorityPrefix() && this.host()) {
                s += '//';
            }
        }

        return s;
    };

    /**
     * Same as Mozilla nsIURI.prePath
     * @return {string} scheme://user:password@host:port
     * @see  https://developer.mozilla.org/en/nsIURI
     */
    Uri.prototype.origin = function () {
        var s = this.scheme();

        if (this.userInfo() && this.host()) {
            s += this.userInfo();
            if (this.userInfo().indexOf('@') !== this.userInfo().length - 1) {
                s += '@';
            }
        }

        if (this.host()) {
            s += this.host();
            if (this.port() || (this.path() && this.path().substr(0, 1).match(/[0-9]/))) {
                s += ':' + this.port();
            }
        }

        return s;
    };

    /**
     * Adds a trailing slash to the path
     */
    Uri.prototype.addTrailingSlash = function () {
        var path = this.path() || '';

        if (path.substr(-1) !== '/') {
            this.path(path + '/');
        }

        return this;
    };

    /**
     * Serializes the internal state of the Uri object
     * @return {string}
     */
    Uri.prototype.toString = function () {
        var path, s = this.origin();

        if (this.isColonUri()) {
            if (this.path()) {
                s += ':' + this.path();
            }
        } else if (this.path()) {
            path = this.path();
            if (!(re.ends_with_slashes.test(s) || re.starts_with_slashes.test(path))) {
                s += '/';
            } else {
                if (s) {
                    s.replace(re.ends_with_slashes, '/');
                }
                path = path.replace(re.starts_with_slashes, '/');
            }
            s += path;
        } else {
            if (this.host() && (this.query().toString() || this.anchor())) {
                s += '/';
            }
        }
        if (this.query().toString()) {
            s += this.query().toString();
        }

        if (this.anchor()) {
            if (this.anchor().indexOf('#') !== 0) {
                s += '#';
            }
            s += this.anchor();
        }

        return s;
    };

    /**
     * Clone a Uri object
     * @return {Uri} duplicate copy of the Uri
     */
    Uri.prototype.clone = function () {
        return new Uri(this.toString());
    };

    /**
     * export via AMD or CommonJS, otherwise leak a global
     */
    if (typeof define === 'function' && define.amd) {
        define(function () {
            return Uri;
        });
    } else if (typeof module !== 'undefined' && typeof module.exports !== 'undefined') {
        module.exports = Uri;
    } else {
        global.Uri = Uri;
    }
}(this));
//---------------------------------------------------js 解析Uri------end-------------------------------------------


//---------------------------------------------------解决js  数字运算的失去精度的问题------begin-------------------------------------------

/**数字结算
解决js 的数字运算的失真bug
https://github.com/fzred/calculatorjs
calc.add(0.1, 0.2) // 0.3
calc.sub(0.1, 0.2) // -0.1
calc.mul(0.1, 0.2) // 0.02
calc.div(0.1, 0.2) // 0.5
calc.round(0.555, 2) // 0.56
*/

(function webpackUniversalModuleDefinition(root, factory) {
    if (typeof exports === 'object' && typeof module === 'object')
        module.exports = factory();
    else if (typeof define === 'function' && define.amd)
        define("calc", [], factory);
    else if (typeof exports === 'object')
        exports["calc"] = factory();
    else
        root["calc"] = factory();
})(this, function () {
    return /******/ (function (modules) { // webpackBootstrap
        /******/ 	// The module cache
        /******/ 	var installedModules = {};
        /******/
        /******/ 	// The require function
        /******/ 	function __webpack_require__(moduleId) {
            /******/
            /******/ 		// Check if module is in cache
            /******/ 		if (installedModules[moduleId]) {
                /******/ 			return installedModules[moduleId].exports;
                /******/
            }
            /******/ 		// Create a new module (and put it into the cache)
            /******/ 		var module = installedModules[moduleId] = {
                /******/ 			i: moduleId,
                /******/ 			l: false,
                /******/ 			exports: {}
                /******/
            };
            /******/
            /******/ 		// Execute the module function
            /******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
            /******/
            /******/ 		// Flag the module as loaded
            /******/ 		module.l = true;
            /******/
            /******/ 		// Return the exports of the module
            /******/ 		return module.exports;
            /******/
        }
        /******/
        /******/
        /******/ 	// expose the modules object (__webpack_modules__)
        /******/ 	__webpack_require__.m = modules;
        /******/
        /******/ 	// expose the module cache
        /******/ 	__webpack_require__.c = installedModules;
        /******/
        /******/ 	// identity function for calling harmony imports with the correct context
        /******/ 	__webpack_require__.i = function (value) { return value; };
        /******/
        /******/ 	// define getter function for harmony exports
        /******/ 	__webpack_require__.d = function (exports, name, getter) {
            /******/ 		if (!__webpack_require__.o(exports, name)) {
                /******/ 			Object.defineProperty(exports, name, {
                    /******/ 				configurable: false,
                    /******/ 				enumerable: true,
                    /******/ 				get: getter
                    /******/
                });
                /******/
            }
            /******/
        };
        /******/
        /******/ 	// getDefaultExport function for compatibility with non-harmony modules
        /******/ 	__webpack_require__.n = function (module) {
            /******/ 		var getter = module && module.__esModule ?
            /******/ 			function getDefault() { return module['default']; } :
            /******/ 			function getModuleExports() { return module; };
            /******/ 		__webpack_require__.d(getter, 'a', getter);
            /******/ 		return getter;
            /******/
        };
        /******/
        /******/ 	// Object.prototype.hasOwnProperty.call
        /******/ 	__webpack_require__.o = function (object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
        /******/
        /******/ 	// __webpack_public_path__
        /******/ 	__webpack_require__.p = "";
        /******/
        /******/ 	// Load entry module and return exports
        /******/ 	return __webpack_require__(__webpack_require__.s = 2);
        /******/
    })
    /************************************************************************/
    /******/([
    /* 0 */
    /***/ (function (module, exports, __webpack_require__) {

        "use strict";


        /**
         * 补0
         * @param {*} num 0个数
         */
        function padding0(num) {
            var str = '';
            while (num--) {
                str += '0';
            } return str;
        }

        /**
         * 将科学记数法转为普通字符串
         * @param {Number} number
         */
        function noExponent(number) {
            var data = String(number).split(/[eE]/);
            if (data.length == 1) return data[0];

            var z = '';
            var sign = number < 0 ? '-' : '';
            var str = data[0].replace('.', '');
            var mag = Number(data[1]) + 1;

            if (mag < 0) {
                z = sign + '0.';
                while (mag++) {
                    z += '0';
                } return z + str.replace(/^\-/, '');
            }
            mag -= str.length;
            while (mag--) {
                z += '0';
            } return str + z;
        }

        function split(number) {
            var str = void 0;
            if (number < 1e-6) {
                str = noExponent(number);
            } else {
                str = number + '';
            }
            var index = str.lastIndexOf('.');
            if (index < 0) {
                return [str, 0];
            } else {
                return [str.replace('.', ''), str.length - index - 1];
            }
        }

        /**
         * 计算
         * @param {*} l 操作数1
         * @param {*} r 操作数2
         * @param {*} sign 操作符
         * @param {*} f 精度
         */
        function operate(l, r, sign, f) {
            switch (sign) {
                case '+':
                    return (l + r) / f;
                case '-':
                    return (l - r) / f;
                case '*':
                    return l * r / (f * f);
                case '/':
                    return l / r;
            }
        }

        /**
         * 解决小数精度问题
         * @param {*} l 操作数1
         * @param {*} r 操作数2
         * @param {*} sign 操作符
         * fixedFloat(0.3, 0.2, '-')
         */
        function fixedFloat(l, r, sign) {
            var arrL = split(l);
            var arrR = split(r);
            var fLen = Math.max(arrL[1], arrR[1]);

            if (fLen === 0) {
                return operate(l, r, sign, 1);
            }
            var f = Math.pow(10, fLen);
            if (arrL[1] !== arrR[1]) {
                if (arrL[1] > arrR[1]) {
                    arrR[0] += padding0(arrL[1] - arrR[1]);
                } else {
                    arrL[0] += padding0(arrR[1] - arrL[1]);
                }
            }
            return operate(+arrL[0], +arrR[0], sign, f);
        }

        /**
         * 加
         */
        function add(l, r) {
            return fixedFloat(l, r, '+');
        }

        /**
         * 减
         */
        function sub(l, r) {
            return fixedFloat(l, r, '-');
        }

        /**
         * 乘
         */
        function mul(l, r) {
            return fixedFloat(l, r, '*');
        }

        /**
         * 除
         */
        function div(l, r) {
            return fixedFloat(l, r, '/');
        }

        /**
         * 四舍五入
         * @param {*} number
         * @param {*} fraction
         */
        function round(number, fraction) {
            return Math.round(number * Math.pow(10, fraction)) / Math.pow(10, fraction);
        }

        module.exports = {
            add: add, sub: sub, mul: mul, div: div, round: round
        };

        /***/
    }),
    /* 1 */
    /***/ (function (module, exports, __webpack_require__) {

        "use strict";


        var precisionCalc = __webpack_require__(0);

        var NUMBER_TOKEN = 1;
        var ADD_OPERATOR_TOKEN = 2;
        var SUB_OPERATOR_TOKEN = 3;
        var MUL_OPERATOR_TOKEN = 4;
        var DIV_OPERATOR_TOKEN = 5;
        var LEFT_PAREN_TOKEN = 6;
        var RIGHT_PAREN_TOKEN = 7;
        var END_TOKEN = 8;

        var INITIAL_STATUS = 1; // 初始化
        var IN_INT_PART_STATUS = 2; // 整数
        var IN_FRAC_PART_STATUS = 4; // 小数

        var tokensEnum = {
            '+': ADD_OPERATOR_TOKEN,
            '-': SUB_OPERATOR_TOKEN,
            '*': MUL_OPERATOR_TOKEN,
            '/': DIV_OPERATOR_TOKEN,
            '(': LEFT_PAREN_TOKEN,
            ')': RIGHT_PAREN_TOKEN
        };

        function getToken(str) {
            var linePos = 0;
            var curStr = void 0;

            var tokens = [];

            var status = INITIAL_STATUS; // 初始化状态

            while (str[linePos]) {
                curStr = str[linePos];
                var token = tokensEnum[curStr];
                if (token) {
                    tokens.push({
                        type: token
                    });
                    status = INITIAL_STATUS;
                } else if (/[0-9]/.test(curStr)) {
                    if (status == INITIAL_STATUS) {
                        // 数字开始
                        tokens.push({
                            type: NUMBER_TOKEN,
                            value: curStr
                        });
                        status = IN_INT_PART_STATUS;
                    } else if (status == IN_INT_PART_STATUS || status == IN_FRAC_PART_STATUS) {
                        // 追加数字
                        var curToken = tokens[tokens.length - 1];
                        curToken.value += curStr;
                    }
                } else if (curStr == '.') {
                    // 小数点
                    if (status == IN_INT_PART_STATUS) {
                        // 输入整数状态才能有小数点
                        status = IN_FRAC_PART_STATUS;
                        var _curToken = tokens[tokens.length - 1];
                        _curToken.value += '.';
                    } else {
                        throw '语法错误';
                    }
                } else if (curStr == ' ') {
                    // 空格
                    status = INITIAL_STATUS;
                } else {
                    throw '语法错误';
                }

                linePos++;
            }

            tokens.push({
                type: END_TOKEN
            });
            return tokens;
        }

        function parseExpression(str) {
            var tokens = getToken(str);
            var curPos = 0;
            var curToken = tokens[curPos];

            function nextToken() {
                curToken = tokens[curPos++];
                return curToken;
            }

            function aheadToken() {
                curToken = tokens[--curPos];
            }

            function parsePrimaryExpression() {
                var value = 0;
                var minusFlog = false; // 是否负数

                nextToken();

                if (curToken.type == SUB_OPERATOR_TOKEN) {
                    minusFlog = true;
                    nextToken();
                }

                if (curToken.type == NUMBER_TOKEN) {
                    value = curToken.value;
                } else if (curToken.type == LEFT_PAREN_TOKEN) {
                    // 优先计算 ( ) 里的表达式
                    value = parseExpression();
                    nextToken();
                    if (curToken.type != RIGHT_PAREN_TOKEN) {
                        throw '缺少 ) ';
                    }
                }
                value = Number(value);
                if (minusFlog) {
                    value = -value;
                }
                return value;
            }

            function parseTerm() {
                var v1 = void 0;
                var v2 = void 0;
                v1 = parsePrimaryExpression();
                while (true) {
                    var token = nextToken();
                    if (token.type != MUL_OPERATOR_TOKEN && token.type != DIV_OPERATOR_TOKEN) {
                        aheadToken();
                        break;
                    }
                    v2 = parsePrimaryExpression();
                    if (token.type == MUL_OPERATOR_TOKEN) {
                        v1 = precisionCalc.mul(v1, v2);
                    } else if (token.type == DIV_OPERATOR_TOKEN) {
                        v1 = precisionCalc.div(v1, v2);
                    }
                }
                return v1;
            }

            function parseExpression() {
                var v1 = void 0;
                var v2 = void 0;
                v1 = parseTerm();
                while (true) {
                    var token = nextToken();
                    if (token.type != ADD_OPERATOR_TOKEN && token.type != SUB_OPERATOR_TOKEN) {
                        aheadToken();
                        break;
                    }
                    v2 = parseTerm();
                    if (token.type == ADD_OPERATOR_TOKEN) {
                        v1 = precisionCalc.add(v1, v2);
                    } else if (token.type == SUB_OPERATOR_TOKEN) {
                        v1 = precisionCalc.sub(v1, v2);
                    }
                }
                return v1;
            }

            return parseExpression();
        }

        module.exports = {
            parseExpression: parseExpression
        };

        /***/
    }),
    /* 2 */
    /***/ (function (module, exports, __webpack_require__) {

        "use strict";


        var _require = __webpack_require__(1),
            parseExpression = _require.parseExpression;

        var precisionCalc = __webpack_require__(0);

        function calc(str) {
            return parseExpression(str);
        }

        for (var key in precisionCalc) {
            calc[key] = precisionCalc[key];
        }

        module.exports = calc;

        /***/
    })
    /******/]);
});

//---------------------------------------------------解决js  数字运算的失去精度的问题------end-------------------------------------------



//---------------------------------------------------easyui doCellTip 扩展datagrid的cell 内容超出后的提示tip------begin-------------------------------------------


/*easy ui的datagrid扩展--表格tips*/
/**
入参列表
doCellTip方法的参数包含以下属性：

名称	参数类型	描述以及默认值
onlyShowInterrupt	string	是否只有在文字被截断时才显示tip，默认值为false，即所有单元格都显示tip。
specialShowFields	Array	需要特殊定义显示的列，比如要求鼠标经过name列时提示standName列(可以是隐藏列)的内容,specialShowFields参数可以传入：[{field:'name',showField:'standName'}]。
width	string	tip的宽度，例如'200px'。
使用示例
function doCellTips() {
    $('#dg').datagrid('doCellTip', {
        onlyShowInterrupt : onlyShowInterrupt,
        specialShowFields : [ {
            field : 'status',
            showField : 'statusDesc'
        } ],
        
    });
    $('#dg').datagrid('cancelCellTip');//取消提示
}

 */

if ($.fn.datagrid) {


    $.extend($.fn.datagrid.methods, {
        /**
         * 开打提示功能
         * @param {} jq
         * @param {} params 提示消息框的样式
         * @return {}
         */
        doCellTip: function (jq, params) {
            var msgIndex = -1;
            var targetTd = null;
            function showTip(showParams, td, e, dg) {
                console.log("showTip......");

                //无文本，不提示。
                if ($(td).text() == "") return;

                //方案1：title显示
                $(td).attr("title", showParams.content);
                //方案2：下面的是基于layer的tips显示----
                //msgIndex = 1;
                ////集成layer 提示
                ////拆分n个字一行
                //var sb_Content = new StringBuilder();
                //for (var i = 0; i < showParams.content.length; i++) {
                //    if (i > 0 && i % 20 === 0) {
                //        sb_Content.Append("<br/>");
                //    }
                //    var charAtContent = showParams.content[i];
                //    sb_Content.Append(charAtContent);
                //}
                ////弹出tips显示内容
                //var htmlContent = sb_Content.ToString();
                //var msgIndex = layer.tips(htmlContent, $(td), {
                //    tips: [1, '#3595CC'],
                //    time: 1000000,
                //    maxWidth: 300
                //});
                //return msgIndex;


            };

      
            jq.parent().parent().mouseout(function () {
                if (msgIndex >= 0) {
                    //关闭弹窗
                    layer.close(msgIndex);
                    msgIndex = -1;
                }
            })

            return jq.each(function () {
                var grid = $(this);
                var options = $(this).data('datagrid');

                var panel = grid.datagrid('getPanel').panel('panel');
                panel.find('.datagrid-body').each(function () {
                    var delegateEle = $(this).find('> div.datagrid-body-inner').length ? $(this).find('> div.datagrid-body-inner')[0] : this;
                    $(delegateEle).undelegate('td', 'mouseover').undelegate('td', 'mouseout').undelegate('td', 'mousemove').delegate('td[field]', {
                        'mouseover': function (e) {
                            //console.log("mouseover......");

                            //if($(this).attr('field')===undefined) return;
                            var that = this;

                            if (targetTd != this) {
                                //关闭弹窗
                                layer.close(msgIndex);
                                msgIndex = -1;
                            }
                            targetTd = this
                            var setField = null;

                            if (msgIndex >= 0) {
                                return;
                            }

                            if (params.specialShowFields && params.specialShowFields.sort) {
                                for (var i = 0; i < params.specialShowFields.length; i++) {
                                    if (params.specialShowFields[i].field == $(this).attr('field')) {
                                        setField = params.specialShowFields[i];
                                    }
                                }
                            }
                            if (setField == null) {
                                options.factContent = $(this).find('>div').clone().css({ 'margin-left': '-5000px', 'width': 'auto', 'display': 'inline', 'position': 'absolute' }).appendTo('body');
                                var factContentWidth = options.factContent.width();
                                params.content = $(this).text();
                                if (params.onlyShowInterrupt) {
                                    var cellWidth=($(this).width()-15);//缩进cell 15个像素
                                    if (factContentWidth > cellWidth) {
                                        msgIndex = showTip(params, this, e, grid);
                                    }
                                } else {
                                    msgIndex = showTip(params, this, e, grid);
                                }
                            } else {
                                panel.find('.datagrid-body').each(function () {
                                    var trs = $(this).find('tr[datagrid-row-index="' + $(that).parent().attr('datagrid-row-index') + '"]');
                                    trs.each(function () {
                                        var td = $(this).find('> td[field="' + setField.showField + '"]');
                                        if (td.length) {
                                            params.content = td.text();
                                        }
                                    });
                                });
                                msgIndex = showTip(params, this, e, grid);
                            }
                        },
                        'mouseout': function (e) {
                            //console.log("mouseout......");
                            e.stopPropagation();//禁止冒泡

                            if (options.factContent) {
                                options.factContent.remove();
                                options.factContent = null;

                            }


                        }
                    });
                });

            });
        },
        /**
         * 关闭消息提示功能
         * @param {} jq
         * @return {}
         */
        cancelCellTip: function (jq) {
            return jq.each(function () {
                var data = $(this).data('datagrid');
                if (data.factContent) {
                    data.factContent.remove();
                    data.factContent = null;
                }
                var panel = $(this).datagrid('getPanel').panel('panel');
                panel.find('.datagrid-body').undelegate('td', 'mouseover').undelegate('td', 'mouseout').undelegate('td', 'mousemove')
            });
        }
    });


}

//---------------------------------------------------easyui doCellTip 扩展datagrid的cell 内容超出后的提示tip------end-------------------------------------------



//---------------------------------------------------simple cache component------begin-------------------------------------------


/*
* WebStorageCache - 0.0.3
 https://github.com/WQTeam/web-storage-cache
var wsCache = new WebStorageCache();

// 缓存字符串'wqteam' 到 'username' 中, 超时时间100秒
wsCache.set('username', 'wqteam', {exp : 100});

// 超时截止日期，可用使用Date类型
var nextYear = new Date();
nextYear.setFullYear(nextYear.getFullYear() + 1);
wsCache.set('username', 'wqteam', {exp : nextYear});

// 获取缓存中 'username' 的值
wsCache.get('username');

// 缓存简单js对象，默认使用序列化方法为JSON.stringify。可以通过初始化wsCache的时候配置serializer.serialize
wsCache.set('user', { name: 'Wu', organization: 'wqteam'});

// 读取缓存中的简单js对象 - 默认使用反序列化方法为JSON.parse。可以通过初始化wsCache的时候配置serializer.deserialize
var user = wsCache.get('user');
alert(user.name + ' belongs to ' + user.organization);

// 删除缓存中 'username'
wsCache.delete('username');

// 手工删除所有超时CacheItem,
wsCache.deleteAllExpires();

// 清除客户端中所有缓存
wsCache.clear();

// 为已存在的（未超时的）缓存值设置新的超时时间。
wsCache.touch('username', 1);

// 如果缓存中没有key为username2的缓存，则添加username2。反之什么都不做
wsCache.add('username2', 'wqteam', {exp : 1});

// 如果缓存中有key为username的缓存，则替换为新值。反之什么都不做
wsCache.replace('username', 'new wqteam', {exp : 1});

// 检查当前选择作为缓存的storage是否被用户浏览器支持。
//如果不支持调用WebStorageCache API提供的方法将什么都不做。
wsCache.isSupported();
* https://github.com/WQTeam/web-storage-cache
*
* This is free and unencumbered software released into the public domain.
*/
(function (root, factory) {
    if (typeof define === 'function' && define.amd) {
        define(factory);
    } else if (typeof exports === 'object') {
        module.exports = factory();
    } else {
        root.WebStorageCache = factory();
    }
}(this, function () {
    "use strict";

    var _maxExpireDate = new Date('Fri, 31 Dec 9999 23:59:59 UTC');
    var _defaultExpire = _maxExpireDate;

    // https://github.com/jeromegn/Backbone.localStorage/blob/master/backbone.localStorage.js#L63
    var defaultSerializer = {
        serialize: function (item) {
            return JSON.stringify(item);
        },
        // fix for "illegal access" error on Android when JSON.parse is
        // passed null
        deserialize: function (data) {
            return data && JSON.parse(data);
        }
    };

    function _extend(obj, props) {
        for (var key in props) obj[key] = props[key];
        return obj;
    }

    /**
    * https://github.com/gsklee/ngStorage/blob/master/ngStorage.js#L52
    *
    * When Safari (OS X or iOS) is in private browsing mode, it appears as
    * though localStorage is available, but trying to call .setItem throws an
    * exception below: "QUOTA_EXCEEDED_ERR: DOM Exception 22: An attempt was
    * made to add something to storage that exceeded the quota."
    */
    function _isStorageSupported(storage) {
        var supported = false;
        if (storage && storage.setItem) {
            supported = true;
            var key = '__' + Math.round(Math.random() * 1e7);
            try {
                storage.setItem(key, key);
                storage.removeItem(key);
            } catch (err) {
                supported = false;
            }
        }
        return supported;
    }

    // get storage instance
    function _getStorageInstance(storage) {
        var type = typeof storage;
        if (type === 'string' && window[storage] instanceof Storage) {
            return window[storage];
        }
        return storage;
    }

    function _isValidDate(date) {
        return Object.prototype.toString.call(date) === '[object Date]' && !isNaN(date.getTime());
    }

    function _getExpiresDate(expires, now) {
        now = now || new Date();

        if (typeof expires === 'number') {
            expires = expires === Infinity ?
                _maxExpireDate : new Date(now.getTime() + expires * 1000);
        } else if (typeof expires === 'string') {
            expires = new Date(expires);
        }

        if (expires && !_isValidDate(expires)) {
            throw new Error('`expires` parameter cannot be converted to a valid Date instance');
        }

        return expires;
    }

    // http://crocodillon.com/blog/always-catch-localstorage-security-and-quota-exceeded-errors
    function _isQuotaExceeded(e) {
        var quotaExceeded = false;
        if (e) {
            if (e.code) {
                switch (e.code) {
                    case 22:
                        quotaExceeded = true;
                        break;
                    case 1014:
                        // Firefox
                        if (e.name === 'NS_ERROR_DOM_QUOTA_REACHED') {
                            quotaExceeded = true;
                        }
                        break;
                }
            } else if (e.number === -2147024882) {
                // Internet Explorer 8
                quotaExceeded = true;
            }
        }
        return quotaExceeded;
    }

    // cache item constructor
    function CacheItemConstructor(value, exp) {
        // createTime
        this.c = (new Date()).getTime();
        exp = exp || _defaultExpire;
        var expires = _getExpiresDate(exp);
        // expiresTime
        this.e = expires.getTime();
        this.v = value;
    }

    function _isCacheItem(item) {
        if (typeof item !== 'object') {
            return false;
        }
        if (item) {
            if ('c' in item && 'e' in item && 'v' in item) {
                return true;
            }
        }
        return false;
    }

    // check cacheItem If effective
    function _checkCacheItemIfEffective(cacheItem) {
        var timeNow = (new Date()).getTime();
        return timeNow < cacheItem.e;
    }

    function _checkAndWrapKeyAsString(key) {
        if (typeof key !== 'string') {
            console.warn(key + ' used as a key, but it is not a string.');
            key = String(key);
        }
        return key;
    }

    // cache api
    var CacheAPI = {

        set: function (key, value, options) { },

        get: function (key) { },

        delete: function (key) { },
        // Try the best to clean All expires CacheItem.
        deleteAllExpires: function () { },
        // Clear all keys
        clear: function () { },
        // Add key-value item to memcached, success only when the key is not exists in memcached.
        add: function (key, options) { },
        // Replace the key's data item in cache, success only when the key's data item is exists in cache.
        replace: function (key, value, options) { },
        // Set a new options for an existing key.
        touch: function (key, exp) { }
    };

    // cache api
    var CacheAPIImpl = {

        set: function (key, val, options) {

            key = _checkAndWrapKeyAsString(key);

            options = _extend({ force: true }, options);

            if (val === undefined) {
                return this.delete(key);
            }

            var value = defaultSerializer.serialize(val);

            var cacheItem = new CacheItemConstructor(value, options.exp);
            try {
                this.storage.setItem(key, defaultSerializer.serialize(cacheItem));
            } catch (e) {
                if (_isQuotaExceeded(e)) { //data wasn't successfully saved due to quota exceed so throw an error
                    this.quotaExceedHandler(key, value, options, e);
                } else {
                    console.error(e);
                }
            }

            return val;
        },
        get: function (key) {
            key = _checkAndWrapKeyAsString(key);
            var cacheItem = null;
            try {
                cacheItem = defaultSerializer.deserialize(this.storage.getItem(key));
            } catch (e) {
                return null;
            }
            if (_isCacheItem(cacheItem)) {
                if (_checkCacheItemIfEffective(cacheItem)) {
                    var value = cacheItem.v;
                    return defaultSerializer.deserialize(value);
                } else {
                    this.delete(key);
                }
            }
            return null;
        },

        delete: function (key) {
            key = _checkAndWrapKeyAsString(key);
            this.storage.removeItem(key);
            return key;
        },

        deleteAllExpires: function () {
            var length = this.storage.length;
            var deleteKeys = [];
            var _this = this;
            for (var i = 0; i < length; i++) {
                var key = this.storage.key(i);
                var cacheItem = null;
                try {
                    cacheItem = defaultSerializer.deserialize(this.storage.getItem(key));
                } catch (e) { }

                if (cacheItem !== null && cacheItem.e !== undefined) {
                    var timeNow = (new Date()).getTime();
                    if (timeNow >= cacheItem.e) {
                        deleteKeys.push(key);
                    }
                }
            }
            deleteKeys.forEach(function (key) {
                _this.delete(key);
            });
            return deleteKeys;
        },

        clear: function () {
            this.storage.clear();
        },

        add: function (key, value, options) {
            key = _checkAndWrapKeyAsString(key);
            options = _extend({ force: true }, options);
            try {
                var cacheItem = defaultSerializer.deserialize(this.storage.getItem(key));
                if (!_isCacheItem(cacheItem) || !_checkCacheItemIfEffective(cacheItem)) {
                    this.set(key, value, options);
                    return true;
                }
            } catch (e) {
                this.set(key, value, options);
                return true;
            }
            return false;
        },

        replace: function (key, value, options) {
            key = _checkAndWrapKeyAsString(key);
            var cacheItem = null;
            try {
                cacheItem = defaultSerializer.deserialize(this.storage.getItem(key));
            } catch (e) {
                return false;
            }
            if (_isCacheItem(cacheItem)) {
                if (_checkCacheItemIfEffective(cacheItem)) {
                    this.set(key, value, options);
                    return true;
                } else {
                    this.delete(key);
                }
            }
            return false;
        },

        touch: function (key, exp) {
            key = _checkAndWrapKeyAsString(key);
            var cacheItem = null;
            try {
                cacheItem = defaultSerializer.deserialize(this.storage.getItem(key));
            } catch (e) {
                return false;
            }
            if (_isCacheItem(cacheItem)) {
                if (_checkCacheItemIfEffective(cacheItem)) {
                    this.set(key, this.get(key), { exp: exp });
                    return true;
                } else {
                    this.delete(key);
                }
            }
            return false;
        }
    };

    /**
    * Cache Constructor
    */
    function CacheConstructor(options) {

        // default options
        var defaults = {
            storage: 'localStorage',
            exp: Infinity  //An expiration time, in seconds. default never .
        };

        var opt = _extend(defaults, options);

        var expires = opt.exp;

        if (expires && typeof expires !== 'number' && !_isValidDate(expires)) {
            throw new Error('Constructor `exp` parameter cannot be converted to a valid Date instance');
        } else {
            _defaultExpire = expires;
        }

        var storage = _getStorageInstance(opt.storage);

        var isSupported = _isStorageSupported(storage);

        this.isSupported = function () {
            return isSupported;
        };

        if (isSupported) {

            this.storage = storage;

            this.quotaExceedHandler = function (key, val, options, e) {
                console.warn('Quota exceeded!');
                if (options && options.force === true) {
                    var deleteKeys = this.deleteAllExpires();
                    console.warn('delete all expires CacheItem : [' + deleteKeys + '] and try execute `set` method again!');
                    try {
                        options.force = false;
                        this.set(key, val, options);
                    } catch (err) {
                        console.warn(err);
                    }
                }
            };

        } else {  // if not support, rewrite all functions without doing anything
            _extend(this, CacheAPI);
        }

    }

    CacheConstructor.prototype = CacheAPIImpl;

    return CacheConstructor;

}));

//---------------------------------------------------simple cache component------end-------------------------------------------


//---------------------------------------------------simple jq validator------begin-------------------------------------------
/*
使用jquery.validator的rule进行字段验证
提示使用layer.js
*/
window.validatePage = function () {
    return true;
};
if (jQuery && !jQuery.fn.validate) {

    var msgTipTable = {
        required: "这是必填字段!",
        email: "请输入有效的电子邮件地址!",
        url: "请输入有效的网址!",
        date: "请输入有效的日期格式yyyy-MM-dd HH:mm:ss !",
        number: "请输入有效的数字!",
        //equalTo: "你的输入不相同!",
        //extension: "请输入有效的后缀!",
        maxlength: "最多可以输入 {0} 个字符!",
        minlength: "最少要输入 {0} 个字符!",
        //rangelength: "请输入长度在 {0} 到 {1} 之间的字符串!",
        //range: "请输入范围在 {0} 到 {1} 之间的数值!",
        max: "请输入不大于 {0} 的数值!",
        min: "请输入不小于 {0} 的数值!"
    };

  

    $(function () {

        (function ($) {
            $.fn.extend({
                /**
                 * 扩展Jq对象的validate.对标注的validate中的rules 进行验证
                 */
                validate: function () {

                    console.log('validate...');

                    //debugger;

                    var sender = $(this);
                    
                    var validateRule = sender.attr("validate");
                    if (isNullOrEmpty(validateRule)) {
                        console.log('no rule');
                        return;
                    }
                    var toValidateContent=sender.val()||sender.text();
                    var rules = validateRule.split(",");
                    rules.remove(function(ru){
                        if (ru.contains("event")) {
                            return true;
                        };
                    });

                    var errMsg = "";

                    for (var i = 0; i < rules.length; i++) {
                        var ru = rules[i];
                        var kvPair = ru.split(":");
                        var ruleName = kvPair[0].trim();
                        var ruleValue = kvPair[1].trim();

                        if (isNullOrEmpty(ruleValue) || ruleValue == false || ruleValue == 'false') {
                            continue;
                        }

                        if (ruleName === "required") {
                         
                            if(isNullOrEmpty(toValidateContent)){
                                errMsg = msgTipTable["required"];
                                break;
                            }
                            
                        }
                        else if (ruleName === "email") {
                            if (!isEmail(ruleValue)) {
                                errMsg = msgTipTable["email"].format(ruleValue);

                                break;
                            }
                        }
                        else if (ruleName === "date") {
                            if (!isDateTimeString(ruleValue)) {
                                errMsg = msgTipTable["date"].format(ruleValue);

                                break;
                            }
                        }
                        else if (ruleName === "minlength") {
                            if (toValidateContent.length < ruleValue) {
                                errMsg = msgTipTable["minlength"].format(ruleValue);

                                break;
                            }
                        }
                        else if (ruleName === "maxlength") {
                            if (toValidateContent.length > ruleValue) {
                                errMsg = msgTipTable["maxlength"].format(ruleValue);

                                break;
                            }
                        }
                        else if (ruleName === "number") {
                            if (!isNumber(toValidateContent)) {
                                errMsg = msgTipTable["number"];

                                break;
                            }
                        }
                        else if (ruleName === "max") {
                            if (!isNumber(toValidateContent)) {
                                errMsg = msgTipTable["number"];
                                break;
                            }
                            if (calc.sub(toValidateContent, ruleValue) > 0) {
                                errMsg = msgTipTable["max"].format(ruleValue);
                                break;
                            }
                        }
                        else if (ruleName === "min") {
                            if (!isNumber(toValidateContent)) {
                                errMsg = msgTipTable["number"];
                                break;
                            }
                            if (calc.sub(toValidateContent, ruleValue) < 0) {
                                errMsg = msgTipTable["min"].format(ruleValue);
                                break;
                            }
                        } else {
                            throw new Error("enmmmm,not implection! ")
                        }

                        }

                        if (!isNullOrEmpty(errMsg)) {
                            //var idx=MessageBox.tipsError(sender, errMsg);
                            var fieldName = sender.attr("data-field-name");
                            if (!isNullOrEmpty(fieldName)) {
                                errMsg = ("字段：【{0}】".format(fieldName)) + errMsg;
                            }
                            var idx = MessageBox.error2(errMsg);
                            sender.attr('isvalid', false);
                            sender.attr('errindex', idx);
                      
                            return false;
                        
                        } else {
                            sender.attr('isvalid', true);
                            var idx = sender.attr('errindex');
                            if (idx) {
                                MessageBox.close(idx);
                            }

                            return true;
                        }
                     
                    }
                })
        })(jQuery);


        /*--------register events--------*/
        // 检测页面上的全部带有validate的属性input 标记
        var needValidateDoms = $("input[validate]");
        if (needValidateDoms && needValidateDoms.length>0) {
            needValidateDoms.forEach(function (dom) {
                var validateRule = dom.attr("validate");
                if (!isNullOrEmpty(validateRule)) {
                    var rules = validateRule.split(",").find(function (ruleItem) {
                        if (ruleItem
                            && ruleItem.indexOf("event") > -1
                            && ruleItem.indexOf(":") > -1) {
                            var eventName = ruleItem.split(":")[1];
                            dom.bind(eventName, function () {
                                //触发绑定事件
                                //debugger;
                                dom.validate();
                            });
                        }
                    })

                }
            });
        }


        //页面合法验证
        window.validatePage = function () {
            if (!needValidateDoms || needValidateDoms.length <= 0) {
                return true;
            }

            for (var i = 0; i < needValidateDoms.length; i++) {
                var dom = $(needValidateDoms[i]);
                var result = dom.validate();
                if (false == result) {
                    return false;
                }
            }
             

            return true;
        };
        
    });
}
//---------------------------------------------------simple jq validator------end-------------------------------------------
