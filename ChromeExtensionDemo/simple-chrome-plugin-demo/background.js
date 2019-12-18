//Author: jyx
//Date: 2014.10.11
//Description: This is a javaScript file use for handle contextMenus action
//When click the contextMenus, it will sent the infomation to native app

//connect to native app
var port = null;
var nativeHostName = "com.ctrip.ops.mysql.callapp";//chrome与本地程序通信的桥梁，根据该名称进行配置项的寻找。windows下在注册表HKEY_CURRENT_USER\Software\Google\Chrome\NativeMessagingHosts内寻找，linux下在目录/etc/opt/chrome/native-messaging-hosts/寻找该名称的json文件（本例子为com.ctrip.ops.mysql.callapp.json）

//onNativeDisconnect
function onDisconnected() {
    //alert("连接到FastDownload服务失败: " + chrome.runtime.lastError.message);
    port = null;
}

function onNativeMessage(message) {
    console.log("Received message: <b>" + JSON.stringify(message) + "</b>");
}

//connect to native host and get the communicatetion port
function connectToNativeHost() {
    port = chrome.runtime.connectNative(nativeHostName);//根据配置文件连接到本地程序
    port.onDisconnect.addListener(onDisconnected);
}

//调用connectToNativeHost函数连接到本地程序，完成后使用port.postMessage函数将纤细传递给应用程序
//将信息写入I/O流与本地程序通信
function getClickHandler() {
    return function (info, tab) {
        console.log(info.linkUrl);
        connectToNativeHost();
        port.postMessage(info.linkUrl);
    };
};

//在浏览器启动时即创建右键菜单，在页面链接上右击鼠标会显示该菜单，当点击"start program"的时候就会调用getClickHandler（）函数处理
chrome.contextMenus.create({
    "title": "start program",
    "type": "normal",
    "id": "callapp",
    "contexts": ["link"],
    "enabled": true,
    "onclick": getClickHandler()
});