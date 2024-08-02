using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Add this to use TextMeshPro
using NativeWebSocket;

public class Connection : MonoBehaviour
{
    WebSocket websocket;
    public TextMeshPro textMeshPro; // Reference to the TextMeshPro component

    // Define an event for message received
    public event Action<string> OnMessageReceived;

    // Start is called before the first frame update
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

            // Decode the byte array to a string
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("Received from Arduino: " + message);

            // Update the 3D text
            UpdateText(message);

            // Trigger the event
            OnMessageReceived?.Invoke(message);
        };

        // Keep sending messages at every 0.3s
        Invoke("SendWebSocketMessage", 0.3f);

        // waiting for messages
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
            // For Arduino
            await websocket.SendText("A");
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
            textMeshPro.text = "Arduino: " + message;
        }
    }

    // Method to send messages
    public void SendText(string message)
    {
        if (websocket.State == WebSocketState.Open)
        {
            websocket.SendText(message);
        }
    }
}
