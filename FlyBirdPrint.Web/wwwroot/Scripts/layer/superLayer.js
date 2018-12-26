

var superLayer = {
    loading: function (config) {
        var options = { shade: 0.3 }
        if (config.icon == undefined) {
            config.icon = 2;
        }
        if (config.time != undefined) {
            options.time = config.time;
        }
        var index = layer.load(config.icon, options);
        return index;
    },
    alert: function (config) {
        var title = '提示';
        var btns = ['确定'];
        if (config.title == undefined) {
            layer.open({
                content: config.content,
                btn: btns,
                end: config.end,//弹窗销毁触发的事件函数
                shadeClose: false,
                yes: function (index) {
                    if (config.yes != undefined) {
                        config.yes();
                    }
                    layer.close(index);
                }
            });
        }
        else {
            title = config.title;
            layer.open({
                title: title,
                content: config.content,
                btn: btns,
                end: config.end,//弹窗销毁触发的事件函数
                shadeClose: false,
                yes: function (index) {
                    if (config.yes != undefined) {
                        config.yes();
                    }
                    layer.close(index);
                }
            });
        }
    },
    confirm: function (config) {
        var options = { btn: ['确定', '取消'] };
        if (config.title != undefined) {
            options.title = config.title;
        }

        options.end = config.end;//弹窗销毁触发的事件函数
        layer.confirm(config.content, options, function (index, layero) {
            if (config.yes != undefined) {
                config.yes();
            }
            layer.close(index);
        }, function (index) {
            if (config.no != undefined)
                config.no();
            layer.close(index);
        });
    },
    msg: function (content) {
        layer.msg(content, { type: 1, time: 3000 });
    },
    html: function (config) {
        if (config.area == undefined) {
            config.area = ['500px', '300px'];
        }
        var index;
        index = layer.open({
            title: config.title,
            type: 1,
            area: config.area,
            closeBtn: 1,
            end:config.end,//弹窗销毁触发的事件函数
            shadeClose: false, //点击遮罩关闭
            content: config.content,
            success: function () {
                if (config.success != undefined) {
                    config.success();
                }
            }
        });
        return index;
    },
    ifream: function (config) {
        if (config.area == undefined) {
            config.area = ['500px', '300px'];
        }
        var index;
        index = layer.open({
            title: config.title,
            type: 2,
            area: config.area,
            closeBtn: 1,
            shadeClose: false, //点击遮罩关闭
            content: config.content,
            success: function () {
                if (config.success != undefined) {
                    config.success();
                }
            }
        });
        return index;
    },
    close: function (index) {
        layer.close(index);
    }
}
