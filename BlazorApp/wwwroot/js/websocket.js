console.log("Starting to load websocket.js");

window.connectWebSocket = (dotNetHelper) => {
    console.log("connectWebSocket function called with helper:", dotNetHelper);
    const socket = new WebSocket('ws://localhost:5000/ws');

    socket.onmessage = (event) => {
        console.log("WebSocket message received:", event.data);
        dotNetHelper.invokeMethodAsync('OnMessageReceived', event.data);
    };

    socket.onopen = () => {
        console.log("WebSocket connection opened");
    };

    socket.onclose = () => {
        console.log("WebSocket connection closed");
    };
        
    socket.onerror = (error) => {
        console.error("WebSocket error:", error);
    };

    window.webSocket = socket;
};

window.closeWebSocket = () => {
    if (window.webSocket && window.webSocket.readyState !== WebSocket.CLOSED) {
        window.webSocket.close();
        console.log("WebSocket closed manually");
    }
};

console.log("websocket.js fully loaded, connectWebSocket defined:", typeof window.connectWebSocket);