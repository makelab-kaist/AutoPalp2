using UnityEngine;
using UnityEngine.UI; // Needed to use UI elements
using TMPro; // Add this to use TextMeshPro

public class ButtonHandler : MonoBehaviour
{
    public Connection connection; // Reference to the Connection script
    public Button sendButton; // Reference to the button in the UI
    public TextMeshPro textMeshPro; // Reference to the TextMeshProUGUI component
    public string messageToSend; // String variable to set the message

    void Start()
    {
        if (sendButton != null)
        {
            sendButton.onClick.AddListener(OnSendButtonClick);
        }

        // Subscribe to the OnMessageReceived event
        if (connection != null)
        {
            connection.OnMessageReceived += OnMessageReceived;
        }
    }

    void OnSendButtonClick()
    {
        if (connection != null)
        {
            connection.SendText(messageToSend); // Use the string variable for the message
        }
    }

    void OnMessageReceived(string message)
    {
        // Update the TextMeshProUGUI text
        if (textMeshPro != null)
        {
            textMeshPro.text = "Force: " + message;
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the OnMessageReceived event to avoid memory leaks
        if (connection != null)
        {
            connection.OnMessageReceived -= OnMessageReceived;
        }
    }
}
