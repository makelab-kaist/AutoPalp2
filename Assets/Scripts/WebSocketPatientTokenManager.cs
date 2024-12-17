using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using NativeWebSocket;

/// <summary>
/// Manages the WebSocket communication for resetting the AutoPalp system.
/// </summary>
public class WebSocketPatientTokenManager : MonoBehaviour
{
    /// <summary>
    /// WebSocket instance for communication with the Arduino.
    /// </summary>
    private WebSocket arduinoWebSocket;

    /// <summary>
    /// Reference to the TextMeshProUGUI element for displaying received data.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI displayText;

    [SerializeField]
    private Button sendButton;

    /// <summary>
    /// Event triggered when a message is received from the WebSocket.
    /// </summary>
    public event Action<string> OnMessageReceived;

    async void Start()
    {
        // Initialize the WebSocket connection.
        arduinoWebSocket = new WebSocket("ws://192.168.0.2:3000");

        // WebSocket connection opened.
        arduinoWebSocket.OnOpen += () => Debug.Log("WebSocket connection opened!");

        // WebSocket error handler.
        arduinoWebSocket.OnError += (error) => Debug.Log("WebSocket error: " + error);

        // WebSocket connection closed.
        arduinoWebSocket.OnClose += (code) => Debug.Log("WebSocket connection closed with code: " + code);

        // Message received from the WebSocket.
        arduinoWebSocket.OnMessage += (bytes) =>
        {
            string message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("Message received from Arduino: " + message);

            // Invoke the event for external message handling.
            OnMessageReceived?.Invoke(message);

            var patientData = JsonConvert.DeserializeObject<PatientData>(message);

            if (patientData.patientID.Length == 13)
            {
                DisplayPatientData(patientData); // Display the received data.
            }
        };

        displayText.text = "        Loading...";

        sendButton.onClick.AddListener(SendPatientDataMessage);

        Invoke(nameof(SendTokenMessage), 0.3f);

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
    /// Sends the "token" message to the WebSocket.
    /// </summary>
    private async void SendTokenMessage()
    {
        if (arduinoWebSocket.State == WebSocketState.Open)
        {
            string getTokenCommand = "token";
            await arduinoWebSocket.SendText(getTokenCommand);
            Debug.Log("Sent: " + getTokenCommand);

            Invoke(nameof(SendResetCommand), 0.1f);
        }
    }

    /// <summary>
    /// Sends the reset command to the WebSocket.
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
    /// Sends the "8001011234567" message to the WebSocket.
    /// </summary>
    private async void SendPatientDataMessage()
    {
        if (arduinoWebSocket.State == WebSocketState.Open)
        {
            string secondMessage = "8001011234567";
            await arduinoWebSocket.SendText(secondMessage);
            Debug.Log("Sent: " + secondMessage);
        }
    }

    /// <summary>
    /// Displays the received patient data on the TextMeshProUGUI element.
    /// </summary>
    /// <param name="patientData">The PatientData object received from the WebSocket.</param>
    private void DisplayPatientData(PatientData patientData)
    {
        try
        {
            displayText.text = $"Name: {patientData.name}\n" +
                            $"DOB: {patientData.dob:yyyy-MM-dd}\n" +
                            $"Height: {patientData.height} cm\n" +
                            $"Weight: {patientData.weight} kg\n" +
                            $"BMI: {patientData.bmi:F2}";
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to parse JSON data: " + ex.Message);
            displayText.text = "Loading...";
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
    /// Represents the patient data structure.
    /// </summary>
    private class PatientData
    {
        public string name { get; set; }
        public string patientID { get; set; }
        public DateTime dob { get; set; }
        public float height { get; set; }
        public float weight { get; set; }
        public float bmi { get; set; }
    }
}