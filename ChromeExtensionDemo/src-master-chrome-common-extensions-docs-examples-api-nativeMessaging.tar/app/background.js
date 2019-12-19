chrome.contextMenus.create({
    "title": "start program",
    "type": "normal",
    "id": "callapp",
    "contexts": ["link"],
    "enabled": true,
    "onclick": getClickHandler()
})

var port = null;

function getClickHandler() {
    return function (info, tab) {
        console.log(info.linkUrl);
        send("111");
    };
};