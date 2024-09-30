using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Add this to use TextMeshPro
using NativeWebSocket;

public class Connection : MonoBehaviour
{
    WebSocket websocket;
    public TextMeshPro textMeshPro;

    public event Action<string> OnMessageReceived;

    private bool isFirstArduinoAccess = true;

    async void Start()
    {
        websocket = new WebSocket("ws://192.168.1.84:3000");

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
            Debug.Log("OnMessage!");

            var message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("Received from Arduino: " + message);

            if (isFirstArduinoAccess)
            {
                UpdateText(message);
                isFirstArduinoAccess = false;
            }

            OnMessageReceived?.Invoke(message);
        };

        Invoke("SendWebSocketMessage", 0.3f);

        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }

    async void SendWebSocketMessage()
    {
        if (websocket.State == WebSocketState.Open)
        {
            await websocket.SendText("8003126511234");
        }
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }

    void UpdateText(string message)
    {
        if (textMeshPro != null)
        {
            textMeshPro.text = message;
        }
    }

    public void SendText(string message)
    {
        if (websocket.State == WebSocketState.Open)
        {
            websocket.SendText(message);
        }
    }
}
