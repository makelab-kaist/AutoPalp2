using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;

/// <summary>
/// Handles communication with an Arduino via WebSocket and manages the game state after finishing palpation.
/// </summary>
public class PalpationCompletionHandler : MonoBehaviour
{
    /// <summary>
    /// The WebSocket instance for communication with the Arduino.
    /// </summary>
    private WebSocket arduinoWebSocket;

    /// <summary>
    /// Event triggered when a message is received from the WebSocket.
    /// </summary>
    public event Action<string> OnMessageReceived;

    /// <summary>
    /// Tracks whether this is the first access to the Arduino.
    /// </summary>
    private bool isFirstArduinoMessage = true;

    /// <summary>
    /// Array of GameObjects to deactivate when the palpation process is completed.
    /// </summary>
    public GameObject[] objectsToDeactivate;

    /// <summary>
    /// GameObject that plays a finish audio cue.
    /// </summary>
    public GameObject finishAudioObject;

    async void Start()
    {
        // Initialize the WebSocket connection to the Arduino.
        arduinoWebSocket = new WebSocket("ws://192.168.0.167:3000");

        // Event triggered when the WebSocket connection is opened.
        arduinoWebSocket.OnOpen += () =>
        {
            Debug.Log("WebSocket connection opened!");
        };

        // Event triggered when there is an error in the WebSocket connection.
        arduinoWebSocket.OnError += (error) =>
        {
            Debug.Log("WebSocket error: " + error);
        };

        // Event triggered when the WebSocket connection is closed.
        arduinoWebSocket.OnClose += (code) =>
        {
            Debug.Log("WebSocket connection closed with code: " + code);
        };

        // Event triggered when a message is received from the WebSocket.
        arduinoWebSocket.OnMessage += (bytes) =>
        {
            Debug.Log("Message received from Arduino!");

            var message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("Received message: " + message);

            // Process the first message from the Arduino differently.
            if (isFirstArduinoMessage)
            {
                isFirstArduinoMessage = false;
            }

            // Send a reset command to the Arduino after a short delay.
            Invoke(nameof(SendResetCommandToArduino), 0.3f);

            // Notify any listeners about the received message.
            OnMessageReceived?.Invoke(message);
        };

        // Subscribe to message handling logic.
        OnMessageReceived += ProcessArduinoMessage;

        // Connect to the WebSocket.
        await arduinoWebSocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        // Dispatch WebSocket messages on platforms where WebSocket behavior is thread-dependent.
        arduinoWebSocket.DispatchMessageQueue();
#endif
    }

    /// <summary>
    /// Sends a reset command to the Arduino via WebSocket.
    /// </summary>
    private async void SendResetCommandToArduino()
    {
        if (arduinoWebSocket.State == WebSocketState.Open)
        {
            string resetCommand = "{\"cmd\": \"reset\"}";
            await arduinoWebSocket.SendText(resetCommand);
        }
    }

    /// <summary>
    /// Sends a custom message to the Arduino via WebSocket.
    /// </summary>
    /// <param name="message">The message to send.</param>
    public async void SendCustomMessage(string message)
    {
        if (arduinoWebSocket.State == WebSocketState.Open)
        {
            await arduinoWebSocket.SendText(message);
        }
    }

    /// <summary>
    /// Closes the WebSocket connection when the application quits.
    /// </summary>
    private async void OnApplicationQuit()
    {
        await arduinoWebSocket.Close();
    }

    /// <summary>
    /// Processes messages received from the Arduino.
    /// </summary>
    /// <param name="message">The received message.</param>
    private void ProcessArduinoMessage(string message)
    {
        if (message.Contains("data"))
        {
            // Deactivate specified objects.
            foreach (var obj in objectsToDeactivate)
            {
                SetObjectActiveState(obj, false);
            }

            // Activate the finish audio cue.
            SetObjectActiveState(finishAudioObject, true);
        }
    }

    /// <summary>
    /// Safely sets the active state of a GameObject.
    /// </summary>
    /// <param name="obj">The GameObject to modify.</param>
    /// <param name="isActive">The desired active state.</param>
    private void SetObjectActiveState(GameObject obj, bool isActive)
    {
        if (obj != null)
        {
            obj.SetActive(isActive);
        }
    }
}