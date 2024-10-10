using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;

public class FinishPalpation : MonoBehaviour
{
    WebSocket websocket;
    public event Action<string> OnMessageReceived;

    private bool isFirstArduinoAccess = true;

    public GameObject[] gameObjectsToDeactivate; // Array of GameObjects to deactivate
    public GameObject audioFinish;

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

        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
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
            // Deactivate all GameObjects in the array
            foreach (var obj in gameObjectsToDeactivate)
            {
                SetActiveState(obj, false);
            }
            // Activate audioFinish GameObject
            SetActiveState(audioFinish, true);
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