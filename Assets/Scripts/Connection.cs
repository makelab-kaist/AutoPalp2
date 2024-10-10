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

    // GameObject 참조 추가
    public GameObject targetObject;    // 활성화할 GameObject
    public Material targetMaterial;    // targetObject에 적용할 Material

    public GameObject targetObject2;   // 비활성화할 GameObject
    public Material targetMaterial2;   // targetObject2에 적용할 Material (필요시 사용)

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

        // 이벤트 구독
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

    // Arduino로 다시 메시지를 보내는 함수
    async void SendMessageToArduino()
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

    // 메시지를 처리하는 함수
    private void HandleMessage(string message)
    {
        if (message.Contains("data"))
        {
            // targetObject를 활성화하고 targetObject2를 비활성화
            SetActiveState(targetObject, true);
            SetActiveState(targetObject2, false);

            // Arduino에 다시 메시지 전송
            SendMessageToArduino();
        }
    }

    // GameObject의 활성화 상태를 변경하는 함수
    private void SetActiveState(GameObject obj, bool isActive)
    {
        if (obj != null)
        {
            obj.SetActive(isActive);   // true면 활성화, false면 비활성화
        }
    }
}