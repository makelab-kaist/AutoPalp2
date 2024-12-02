using System;
using UnityEngine;
using NativeWebSocket;

/// <summary>
/// Manages the sequence of palpation events, including communication with an Arduino via WebSocket.
/// </summary>
public class PalpationSequenceManager : MonoBehaviour
{
    /// <summary>
    /// WebSocket instance for communicating with the Arduino.
    /// </summary>
    private WebSocket arduinoWebSocket;

    /// <summary>
    /// Event triggered when a message is received from the WebSocket.
    /// </summary>
    public event Action<string> OnMessageReceived;

    /// <summary>
    /// Tracks whether this is the first interaction with the Arduino.
    /// </summary>
    private bool isFirstArduinoMessage = true;

    /// <summary>
    /// The next GameObject to activate in the sequence.
    /// </summary>
    public GameObject nextStepObject;

    /// <summary>
    /// The current GameObject to deactivate in the sequence.
    /// </summary>
    public GameObject currentStepObject;

    async void Start()
    {
        // Initialize the WebSocket connection to the Arduino.
        arduinoWebSocket = new WebSocket("ws://192.168.0.167:3000");

        // WebSocket connection opened.
        arduinoWebSocket.OnOpen += () => Debug.Log("WebSocket connection opened!");

        // WebSocket error handler.
        arduinoWebSocket.OnError += (error) => Debug.Log("WebSocket error: " + error);

        // WebSocket connection closed.
        arduinoWebSocket.OnClose += (code) => Debug.Log("WebSocket connection closed with code: " + code);

        // Message received from WebSocket.
        arduinoWebSocket.OnMessage += (bytes) =>
        {
            string message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("Message received from Arduino: " + message);

            if (isFirstArduinoMessage)
            {
                isFirstArduinoMessage = false;
            }

            // Invoke event for message processing.
            OnMessageReceived?.Invoke(message);
        };

        // Subscribe to the message handler.
        OnMessageReceived += ProcessArduinoMessage;

        // Send initial readiness message.
        Invoke(nameof(SendReadyMessageToArduino), 0.3f);

        // Connect to the WebSocket.
        await arduinoWebSocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        // Dispatch WebSocket messages for platforms that require explicit handling.
        arduinoWebSocket.DispatchMessageQueue();
#endif
    }

    /// <summary>
    /// Sends a "ready" command to the Arduino to signal readiness for the next step.
    /// </summary>
    private async void SendReadyMessageToArduino()
    {
        if (arduinoWebSocket.State == WebSocketState.Open)
        {
            string readyCommand = "{\"cmd\": \"setReady\", \"value\": true}";
            await arduinoWebSocket.SendText(readyCommand);
        }
    }

    /// <summary>
    /// Sends a custom text message to the Arduino.
    /// </summary>
    /// <param name="message">The message to send.</param>
    public async void SendCustomMessageToArduino(string message)
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
    /// <param name="message">The message received.</param>
    private void ProcessArduinoMessage(string message)
    {
        if (message.Contains("data"))
        {
            // Activate the next step object and deactivate the current step object.
            SetGameObjectActiveState(nextStepObject, true);
            SetGameObjectActiveState(currentStepObject, false);

            // Notify Arduino that the next step is ready.
            SendReadyMessageToArduino();
        }
    }

    /// <summary>
    /// Safely sets the active state of a GameObject.
    /// </summary>
    /// <param name="gameObject">The GameObject to modify.</param>
    /// <param name="isActive">The desired active state.</param>
    private void SetGameObjectActiveState(GameObject gameObject, bool isActive)
    {
        if (gameObject != null)
        {
            gameObject.SetActive(isActive);
        }
    }
}