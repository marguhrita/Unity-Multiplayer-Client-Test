
using UnityEngine;
using UnityEngine.UI;
using RiptideNetworking;
using RiptideNetworking.Utils;
using static NetworkManager;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager _singleton;

    public static UIManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(UIManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    [Header("Connect")]
    [SerializeField] private GameObject connectUI;
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TMP_InputField inputIP;

    private void Awake()
    {
        Singleton = this;
    }

    public void ConnectClicked()
    {
        usernameField.interactable = false;
        inputIP.interactable = false;
        connectUI.SetActive(false);

        if (inputIP.text.Length > 0)
        {
            NetworkManager.Singleton.setIp(inputIP.text);
        }
        

        NetworkManager.Singleton.Connect();
    }

    public void backToMain()
    {
        usernameField.interactable = true;
        connectUI.SetActive(true);
    }

    public void SendName()
    {
        Message message = Message.Create(MessageSendMode.reliable, (ushort) ClientToServerId.name);

        message.AddString(usernameField.text);

        NetworkManager.Singleton.Client.Send(message);
    }
}
