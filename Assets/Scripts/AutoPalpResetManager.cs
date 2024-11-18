using System;
using UnityEngine;
using NativeWebSocket;

/// <summary>
/// Manages the WebSocket communication for resetting the AutoPalp system.
/// </summary>

public class AutoPalpResetManager : MonoBehaviour
{
    /// <summary>
    /// WebSocket instance for communication with the Arduino.
    /// </summary>
    private WebSocket arduinoWebSocket;

    /// <summary>
    /// Event triggered when a message is received from the WebSocket.
    /// </summary>
    public event Action<string> OnMessageReceived;

    async void Start()
    {
        // Initialize the WebSocket connection.
        arduinoWebSocket = new WebSocket("ws://192.168.0.167:3000");

        // WebSocket connection opened.
        arduinoWebSocket.OnOpen += () =>
        {
            Debug.Log("WebSocket connection opened!");
        };

        // WebSocket error handler.
        arduinoWebSocket.OnError += (error) =>
        {
            Debug.Log("WebSocket error: " + error);
        };

        // WebSocket connection closed.
        arduinoWebSocket.OnClose += (code) =>
        {
            Debug.Log("WebSocket connection closed with code: " + code);
        };

        // Message received from the WebSocket.
        arduinoWebSocket.OnMessage += (bytes) =>
        {
            string message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("Message received from Arduino: " + message);

            // Invoke the event for external message handling.
            OnMessageReceived?.Invoke(message);
        };

        // Send the initial reset command after a short delay.
        Invoke(nameof(SendResetCommand), 0.3f);

        // Connect to the WebSocket.
        await arduinoWebSocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        // Dispatch WebSocket messages for platforms requiring explicit message handling.
        arduinoWebSocket.DispatchMessageQueue();
#endif
    }

    /// <summary>
    /// Sends a reset command to the Arduino via WebSocket.
    /// </summary>
    private async void SendResetCommand()
    {
        if (arduinoWebSocket.State == WebSocketState.Open)
        {
            string resetCommand = "{\"cmd\": \"reset\"}";
            await arduinoWebSocket.SendText(resetCommand);
            Debug.Log("Reset command sent to Arduino.");
        }
    }

    /// <summary>
    /// Closes the WebSocket connection when the application quits.
    /// </summary>
    private async void OnApplicationQuit()
    {
        await arduinoWebSocket.Close();
    }
}