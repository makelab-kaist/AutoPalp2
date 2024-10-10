using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;

public class Connection : MonoBehaviour
{
    WebSocket websocket;
    public event Action<string> OnMessageReceived;

    private bool isFirstArduinoAccess = true;

    public GameObject nextObject;
    public GameObject currObject;

    async void Start()
    {
        websocket = new WebSocket("ws://192.168.0.167:3000");

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
                isFirstArduinoAccess = false;
            }

            OnMessageReceived?.Invoke(message);
        };

        OnMessageReceived += HandleMessage;

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
            string message = "{\"cmd\": \"setReady\", \"value\": true}";
            await websocket.SendText(message);
        }
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }

    public void SendText(string message)
    {
        if (websocket.State == WebSocketState.Open)
        {
            websocket.SendText(message);
        }
    }

    private void HandleMessage(string message)
    {
        if (message.Contains("data"))
        {
            SetActiveState(nextObject, true);
            SetActiveState(currObject, false);

            SendWebSocketMessage();
        }
    }

    private void SetActiveState(GameObject obj, bool isActive)
    {
        if (obj != null)
        {
            obj.SetActive(isActive);
        }
    }
}