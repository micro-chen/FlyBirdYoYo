
var js_lib_version = "0.0.0";
if (window.jsVersion) {
    js_lib_version = window.jsVersion;
}

/*使用此boots 启动文件 将需要的脚本库 添加到页面*/
//-------基础js lib文件打包合并----------
document.write("<script src='/js/bundle-library.min.js?v=" + js_lib_version + "'><\/script>");

//------业务js--------
document.write("<script src='/js/appObjects.js?v=" + js_lib_version + "'><\/script>");
document.write("<script src='/js/Extension/applicationCore.js?v=" + js_lib_version + "'><\/script>");
document.write("<script src='/js/Extension/httpClient.js?v=" + js_lib_version + "'><\/script>");


if (!String.prototype.trim) {
    String.prototype.trim = function () {
        return this.replace(/^\s+|\s+$/, "");
    }
}
function is_array(input) {
    return typeof (input) === "object" && (input instanceof Array);
}
function convert_formated_hex_to_bytes(hex_str) {
    var count = 0,
        hex_arr,
        hex_data = [],
        hex_len,
        i;

    if (hex_str.trim() == "") return [];

    /// Check for invalid hex characters.
    if (/[^0-9a-fA-F\s]/.test(hex_str)) {
        return false;
    }

    hex_arr = hex_str.split(/([0-9a-fA-F]+)/g);
    hex_len = hex_arr.length;

    for (i = 0; i < hex_len; ++i) {
        if (hex_arr[i].trim() == "") {
            continue;
        }
        hex_data[count++] = parseInt(hex_arr[i], 16);
    }

    return hex_data;
}
function convert_formated_hex_to_string(s) {
    var byte_arr = convert_formated_hex_to_bytes(s);
    var res = "";
    for (var i = 0; i < byte_arr.length; i += 2) {
        res += String.fromCharCode(byte_arr[i] | (byte_arr[i + 1] << 8));
    }
    return res;
}
function convert_string_to_hex(s) {
    var byte_arr = [];
    for (var i = 0; i < s.length; i++) {
        var value = s.charCodeAt(i);
        byte_arr.push(value & 255);
        byte_arr.push((value >> 8) & 255);
    }
    return convert_to_formated_hex(byte_arr);
}

function convert_to_formated_hex(byte_arr) {
    var hex_str = "",
        i,
        len,
        tmp_hex;

    if (!is_array(byte_arr)) {
        return false;
    }

    len = byte_arr.length;

    for (i = 0; i < len; ++i) {
        if (byte_arr[i] < 0) {
            byte_arr[i] = byte_arr[i] + 256;
        }
        if (byte_arr[i] === undefined) {
            alert("Boom " + i);
            byte_arr[i] = 0;
        }
        tmp_hex = byte_arr[i].toString(16);

        // Add leading zero.
        if (tmp_hex.length == 1) tmp_hex = "0" + tmp_hex;

        if ((i + 1) % 16 === 0) {
            tmp_hex += "\n";
        } else {
            tmp_hex += "";//concat string with no space
        }

        hex_str += tmp_hex;
    }

    return hex_str.trim();
}

/*Define one global object to namespace*/
//init one namespace object and regist it to window 
var FlyBirdYoYo = {
    /*generate a api sign when page load to web browser*/
    apiSignFunc: function () {
        var timestamp = getTimeToken();
        var encodeString = ZlibString.compress(timestamp.toString());
        var hexString = convert_string_to_hex(encodeString);
        return encodeURIComponent(hexString);
    }
};
if (!window.FlyBirdYoYo) {
    window.FlyBirdYoYo = {};
}
