using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerServer : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    public GameObject inputText;
    public GameObject socketClientObject;
    private SocketClient socketClient;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        socketClient = socketClientObject.GetComponent<SocketClient>();


    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.Interact) {
            socketClient.Send("abcd_aaaa112314088KK");
            // inputText.GetComponent<TextMeshProUGUI>().text
        }
    }
}
