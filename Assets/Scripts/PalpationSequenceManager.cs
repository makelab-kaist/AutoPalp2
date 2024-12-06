using System;
using System.Collections.Generic;
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
    /// List of step GameObjects for sequential activation/deactivation.
    /// </summary>
    public List<GameObject> stepObjects;

    /// <summary>
    /// List of final step GameObjects for activation/deactivation at the end of the sequence.
    /// </summary>
    public List<GameObject> finalStepObjects;

    /// <summary>
    /// GameObject for the final audio element to activate at the end of the sequence.
    /// </summary>
    public GameObject finalAudio;

    /// <summary>
    /// Index of the current step in the sequence.
    /// </summary>
    private int currentStepIndex = 0;

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
    /// Handles button click to proceed to the next step and send the number to the Arduino.
    /// </summary>
    private void OnButtonClick()
    {
        // Proceed to the next step in the sequence.
        ProceedToNextStep();

        // Remove the button click listener to prevent duplicate actions.
        targetButton.onClick.RemoveListener(OnButtonClick);

        // Send the number to the Arduino.
        SendNumberToArduino();

        if (targetSlider != null)
            targetSlider.value = 5;

        // Deactivate the UI elements (slider and button).
        DeactivateUIElements();
    }

    /// <summary>
    /// Sends the number displayed in the TextMeshProUGUI component to the Arduino via WebSocket.
    /// </summary>
    private async void SendNumberToArduino()
    {
        if (arduinoWebSocket.State == WebSocketState.Open && numberText != null)
        {
            if (int.TryParse(numberText.text, out int number))
            {
                string numberMessage = $"{{\"pain\": {number}}}";
                await arduinoWebSocket.SendText(numberMessage);
                Debug.Log("Number sent to Arduino: " + number);
            }
        }
    }

    /// <summary>
    /// Moves to the next step in the sequence by updating active GameObjects based on index.
    /// </summary>
    private void ProceedToNextStep()
    {
        if (currentStepIndex < stepObjects.Count)
        {
            SetGameObjectActiveState(stepObjects[currentStepIndex], false);

            currentStepIndex++;

            if (currentStepIndex < stepObjects.Count)
            {
                SetGameObjectActiveState(stepObjects[currentStepIndex], true);
                SendReadyMessageToArduino();
            }
            else
            {
                DeactivateAllAndActivateFinalObject();
                SendResetCommandToArduino();
                Debug.Log("All steps completed.");

                // Quit the application after a short delay.
                Invoke(nameof(QuitApplication), 2f);
            }
        }
    }

    /// <summary>
    /// Activates the final audio and deactivates all final step objects.
    /// </summary>
    private void DeactivateAllAndActivateFinalObject()
    {
        SetGameObjectActiveState(finalAudio, true);
        
        foreach (var obj in finalStepObjects)
        {
            SetGameObjectActiveState(obj, false);
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

        /// <summary>
    /// Quits the application after the final step.
    /// </summary>
    private void QuitApplication()
    {
        Debug.Log("Application quitting...");
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}