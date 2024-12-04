using System;
using UnityEngine;
using UnityEngine.UI;
using NativeWebSocket;
using TMPro;

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

    /// <summary>
    /// The slider to activate upon receiving a specific message.
    /// </summary>
    public Slider targetSlider;

    /// <summary>
    /// The button to activate upon receiving a specific message.
    /// </summary>
    public Button targetButton;

    /// <summary>
    /// The TextMeshProUGUI component displaying the number to send.
    /// </summary>
    public TextMeshProUGUI numberText;

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
            ActivateUIElements();
        }
    }

    /// <summary>
    /// Activates the target slider and button, and sets up the button click listener.
    /// </summary>
    private void ActivateUIElements()
    {
        if (targetSlider != null)
        {
            targetSlider.gameObject.SetActive(true);
        }

        if (targetButton != null)
        {
            targetButton.gameObject.SetActive(true);

            // 버튼 클릭 시 Step 진행하도록 리스너 추가.
            targetButton.onClick.AddListener(OnButtonClick);
        }
    }

    /// <summary>
    /// Handles button click to proceed to the next step and send number.
    /// </summary>
    private void OnButtonClick()
    {
        // Step 전환
        SetGameObjectActiveState(nextStepObject, true);
        SetGameObjectActiveState(currentStepObject, false);

        // 버튼 클릭 후 리스너 제거 (중복 방지)
        targetButton.onClick.RemoveListener(OnButtonClick);

        // Send the number from the text as a WebSocket message
        SendNumberToArduino();

        // 비활성화: targetSlider와 targetButton을 비활성화
        DeactivateUIElements();

        // Notify Arduino that the next step is ready.
        // SendReadyMessageToArduino();
    }

    /// <summary>
    /// Sends the number displayed in the TextMeshProUGUI component to the Arduino via WebSocket.
    /// </summary>
    private async void SendNumberToArduino()
    {
        if (arduinoWebSocket.State == WebSocketState.Open && numberText != null)
        {
            if (int.TryParse(numberText.text, out int number) && number >= 0 && number <= 10)
            {
                string numberMessage = $"{{\"pain\": {number}}}";
                await arduinoWebSocket.SendText(numberMessage);
                Debug.Log("Number sent to Arduino: " + number);
            }
            else
            {
                Debug.LogError("Invalid number format or out of range.");
            }
        }
    }


    /// <summary>
    /// Deactivates the target slider and button.
    /// </summary>
    private void DeactivateUIElements()
    {
        if (targetSlider != null)
        {
            targetSlider.gameObject.SetActive(false);
        }

        if (targetButton != null)
        {
            targetButton.gameObject.SetActive(false);
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