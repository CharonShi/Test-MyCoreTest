
var ApiUrl = "http://localhost:53333";

var versions = "未知";

BaseApi = {
    Ver: "1.0",
    BaseUrl: "http://localhost:53333",
    /*
    Ajax 访问Api接口方法
    Lonze 2015/7/2
    option: 该参数与jQuery.ajax的option参数一致，比jQuery.ajax的option多一项noAccess的回调
    wesley 2020/4/8 调整处理逻辑，配合layer UI 使用
    */
    ajax: function (option) {
        var success = option.success;
        var error = option.error;
        var noAccess = option.noAccess;
        delete option.noAccess;
        var contentType_ = option.contentType;
        option.success = function (result) {
            if (success == null) {
                console.log("没有定义 success 方法，返回参数：" + result);
            }
            success(result, result.data);

        };
        //option.crossDomain = true;

        option.url = BaseApi.BaseUrl + option.url;

        if (IsNullOrEmpty(contentType_) && contentType_ != false) {
            //option.contentType = "application/json;charset=utf-8";
        }

        option.headers = { "ShiToken": getCookie("ShiToken") };
        //option.dataType = "json";
        option.error = function (jqXHR, textStatus, errorThrown) {

            var result = JSON.parse(jqXHR.responseText);
            //返回到登录界面
            if (result.code == 103) {
                window.location.href = "login.html";
                return false;
            }
            if (error != null) {
                error(result.msg);
            } else { // 若用户未自定义异常处理方法
                alert("服务端异常：" + result.msg);
            }

        };

        $.ajax(option);
    }
};



(function ($) {
    var BrowserMatch = {
        init: function () {
            this.browser = this.getBrowser().browser || "未知浏览器";  //获取浏览器名
            this.version = this.getBrowser().version || "未知浏览器版本号";  //获取浏览器版本
            this.OS = this.getOS() + " " + this.getDigits() || "未知操作系统"; //系统版本号 
        },
        getOS: function () {  //判断所处操作系统
            var sUserAgent = navigator.userAgent.toLowerCase();

            var isWin = (navigator.platform == "Win32") || (navigator.platform == "Win64") || (navigator.platform == "wow64");

            var isMac = (navigator.platform == "Mac68K") || (navigator.platform == "MacPPC") || (navigator.platform == "Macintosh") || (navigator.platform == "MacIntel");
            if (isMac) return "Mac";
            var isUnix = (navigator.platform == "X11") && !isWin && !isMac;
            if (isUnix) return "Unix";
            var isLinux = (String(navigator.platform).indexOf("Linux") > -1);
            var bIsAndroid = sUserAgent.toLowerCase().match(/android/i) == "android";
            if (isLinux) {
                if (bIsAndroid) return "Android";
                else return "Linux";
            }
            if (isWin) {
                var isWin2K = sUserAgent.indexOf("Windows nt 5.0") > -1 || sUserAgent.indexOf("Windows 2000") > -1;
                if (isWin2K) return "Win2000";
                var isWinXP = sUserAgent.indexOf("Windows nt 5.1") > -1 || sUserAgent.indexOf("Windows XP") > -1
                sUserAgent.indexOf("Windows XP") > -1;
                if (isWinXP) return "WinXP";
                var isWin2003 = sUserAgent.indexOf("Windows nt 5.2") > -1 || sUserAgent.indexOf("Windows 2003") > -1;
                if (isWin2003) return "Win2003";
                var isWinVista = sUserAgent.indexOf("Windows nt 6.0") > -1 || sUserAgent.indexOf("Windows Vista") > -1;
                if (isWinVista) return "WinVista";
                var isWin7 = sUserAgent.indexOf("Windows nt 6.1") > -1 || sUserAgent.indexOf("Windows 7") > -1;
                if (isWin7) return "Win7";
                var isWin8 = sUserAgent.indexOf("windows nt 6.2") > -1 || sUserAgent.indexOf("Windows 8") > -1;
                if (isWin8) return "Win8";
                var isWin10 = sUserAgent.indexOf("windows nt 10.0") > -1 || sUserAgent.indexOf("Windows 10") > -1;
                if (isWin10) return "Win10";
            }
            return "其他";
        },
        getDigits: function () { //判断当前操作系统的版本号 
            var sUserAgent = navigator.userAgent.toLowerCase();
            var is64 = sUserAgent.indexOf("win64") > -1 || sUserAgent.indexOf("wow64") > -1;
            if (is64) {
                return "64位";
            } else {
                return "32位";
            }
        },
        getBrowser: function () {  // 获取浏览器名
            var rMsie = /(msie\s|trident\/7)([\w\.]+)/;
            var rTrident = /(trident)\/([\w.]+)/;
            var rEdge = /(chrome)\/([\w.]+)/;//IE

            var rFirefox = /(firefox)\/([\w.]+)/;  //火狐
            var rOpera = /(opera).+version\/([\w.]+)/;  //旧Opera
            var rNewOpera = /(opr)\/(.+)/;  //新Opera 基于谷歌
            var rChrome = /(chrome)\/([\w.]+)/; //谷歌 
            var rUC = /(chrome)\/([\w.]+)/;//UC
            var rMaxthon = /(chrome)\/([\w.]+)/;//遨游
            var r2345 = /(chrome)\/([\w.]+)/;//2345
            var rQQ = /(chrome)\/([\w.]+)/;//QQ
            //var rMetasr =  /(metasr)\/([\w.]+)/;//搜狗
            var rSafari = /version\/([\w.]+).*(safari)/;

            var ua = navigator.userAgent.toLowerCase();


            var matchBS, matchBS2;

            //IE 低版
            matchBS = rMsie.exec(ua);
            if (matchBS != null) {
                matchBS2 = rTrident.exec(ua);
                if (matchBS2 != null) {
                    switch (matchBS2[2]) {
                        case "4.0":
                            return {
                                browser:
                                    "Microsoft IE",
                                version: "IE: 8"  //内核版本号
                            };
                            break;
                        case "5.0":
                            return {
                                browser:
                                    "Microsoft IE",
                                version: "IE: 9"
                            };
                            break;
                        case "6.0":
                            return {
                                browser:
                                    "Microsoft IE",
                                version: "IE: 10"
                            };
                            break;
                        case "7.0":
                            return {
                                browser:
                                    "Microsoft IE",
                                version: "IE: 11"
                            };
                            break;
                        default:
                            return {
                                browser:
                                    "Microsoft IE",
                                version: "Undefined"
                            };
                    }
                } else {
                    return {
                        browser: "Microsoft IE",
                        version: "IE:" + matchBS[2] || "0"
                    };
                }
            }
            //IE最新版
            matchBS = rEdge.exec(ua);
            if ((matchBS != null) && (!(window.attachEvent))) {
                return {
                    browser: "Microsoft Edge",
                    version: "Chrome/" + matchBS[2] || "0"
                };
            }
            //UC浏览器					  
            matchBS = rUC.exec(ua);
            if ((matchBS != null) && (!(window.attachEvent))) {
                return {
                    browser: "UC",
                    version: "Chrome/" + matchBS[2] || "0"
                };
            }
            //火狐浏览器
            matchBS = rFirefox.exec(ua);
            if ((matchBS != null) && (!(window.attachEvent))) {
                return {
                    browser: "火狐",
                    version: "Firefox/" + matchBS[2] || "0"
                };
            }
            //Oper浏览器					 
            matchBS = rOpera.exec(ua);
            if ((matchBS != null) && (!(window.attachEvent))) {
                return {
                    browser: "Opera",
                    version: "Chrome/" + matchBS[2] || "0"
                };
            }
            //遨游
            matchBS = rMaxthon.exec(ua);
            if ((matchBS != null) && (!(window.attachEvent))) {
                return {
                    browser: "遨游",
                    version: "Chrome/" + matchBS[2] || "0"
                };
            }
            //2345浏览器					  
            matchBS = r2345.exec(ua);
            if ((matchBS != null) && (!(window.attachEvent))) {
                return {
                    browser: "2345",
                    version: "Chrome/ " + matchBS[2] || "0"
                };
            }
            //QQ浏览器					  
            matchBS = rQQ.exec(ua);
            if ((matchBS != null) && (!(window.attachEvent))) {
                return {
                    browser: "QQ",
                    version: "Chrome/" + matchBS[2] || "0"
                };
            }
            //Safari（苹果）浏览器
            matchBS = rSafari.exec(ua);
            if ((matchBS != null) && (!(window.attachEvent)) && (!(window.chrome)) && (!(window.opera))) {
                return {
                    browser: "Safari",
                    version: "Safari/" + matchBS[1] || "0"
                };
            }
            //谷歌浏览器
            matchBS = rChrome.exec(ua);
            if ((matchBS != null) && (!(window.attachEvent))) {
                matchBS2 = rNewOpera.exec(ua);
                if (matchBS2 == null) {
                    return {
                        browser: "谷歌",
                        version: "Chrome/" + matchBS[2] || "0"
                    };
                } else {
                    return {
                        browser: "Opera",
                        version: "opr/" + matchBS2[2] || "0"
                    };
                }
            }
        }
    };
    BrowserMatch.init();

    versions = BrowserMatch.version
    //alert("当前浏览器为：" + BrowserMatch.browser + "\n版本为：" + BrowserMatch.version + "\n所处操作系统为：" + BrowserMatch.OS);
})(jQuery);




//判断字符串是否为空
//<param name="str">字符串</param>
function IsNullOrEmpty(str) {
    if (str == null || str == undefined || typeof str == undefined || str == "null" || str == "undefined" || str == "") {
        return true;
    }
    else {
        return false;
    }
}



//设置cookie
function setCookie(key, value) {
    //var oDate=new Date();
    //oDate.setDate(oDate.getDate()+iDay);
    var curDate = new Date();
    //当前时间戳
    var curTamp = curDate.getTime();
    //当日凌晨的时间戳,减去一毫秒是为了防止后续得到的时间不会达到00:00:00的状态
    var curWeeHours = new Date(curDate.toLocaleDateString()).getTime() - 1;
    //当日已经过去的时间（毫秒）
    var passedTamp = curTamp - curWeeHours;
    //当日剩余时间
    var leftTamp = 24 * 60 * 60 * 1000 - passedTamp;
    var leftTime = new Date();
    leftTime.setTime(leftTamp + curTamp);
    document.cookie = key + '=' + value + ';expires=' + leftTime.toGMTString() + ";path=/";
}

//获取cookie
function getCookie(cookie_name) {
    var allcookies = document.cookie;
    var cookie_pos = allcookies.indexOf(cookie_name);   //索引的长度

    // 如果找到了索引，就代表cookie存在，
    // 反之，就说明不存在。
    if (cookie_pos != -1) {
        // 把cookie_pos放在值的开始，只要给值加1即可。
        cookie_pos += cookie_name.length + 1;
        var cookie_end = allcookies.indexOf(";", cookie_pos);

        if (cookie_end == -1) {
            cookie_end = allcookies.length;
        }

        var value = unescape(allcookies.substring(cookie_pos, cookie_end));
    }
    return value;
}

