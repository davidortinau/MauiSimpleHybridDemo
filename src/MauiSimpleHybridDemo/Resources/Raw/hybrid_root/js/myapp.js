function SendMessageToCSharp() {
    var message = document.getElementById('messageInput').value;
    HybridWebView.SendRawMessageToDotNet(message);
}

function LogMessageFromCSharp(msg) {
    var receivedMessagesDiv = document.getElementById("receivedMessages");
    receivedMessagesDiv.innerHTML += msg + '</br>';

    if (msg.includes("bot")) {
        HybridWebView.SendRawMessageToDotNet("bot");
    }
}
