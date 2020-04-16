//connect to native app
var port = null;
var nativeHostName = "com.encootech.bottime";//chrome与本地程序通信的桥梁，根据该名称进行配置项的寻找。windows下在注册表HKEY_CURRENT_USER\Software\Google\Chrome\NativeMessagingHosts内寻找，linux下在目录/etc/opt/chrome/native-messaging-hosts/寻找该名称的json文件（本例子为com.ctrip.ops.mysql.callapp.json）

//onNativeDisconnect
function onDisconnected() {
    //alert("连接到FastDownload服务失败: " + chrome.runtime.lastError.message);
    port = null;
    console.log("Inside onDisconnected(): " + chrome.runtime.lastError.message);
}

function onNativeMessage(message) {
    console.log("Received message: " + JSON.stringify(message));
}

//connect to native host and get the communicatetion port
function connectToNativeHost() {
    port = chrome.runtime.connectNative(nativeHostName);//根据配置文件连接到本地程序
    port.onMessage.addListener(onNativeMessage);
    port.onDisconnect.addListener(onDisconnected);
}

//调用connectToNativeHost函数连接到本地程序，完成后使用port.postMessage函数将纤细传递给应用程序
//将信息写入I/O流与本地程序通信
function getClickHandler() {
    return function (info, tab) {
        console.log(info.linkUrl);
        if (port == null) {
            connectToNativeHost();
        }
        port.postMessage(info.linkUrl);
    };
};

//在浏览器启动时即创建右键菜单，在页面链接上右击鼠标会显示该菜单，当点击"start program"的时候就会调用getClickHandler（）函数处理
chrome.contextMenus.create({
    "title": "发送消息",
    "type": "normal",
    "id": "BotTime Demo",
    "contexts": ["link"],
    "enabled": true,
    "onclick": getClickHandler()
});