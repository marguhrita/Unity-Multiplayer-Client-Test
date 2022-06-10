
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

    private void Awake()
    {
        Singleton = this;
    }


    [SerializeField] private GameObject connectUI; //menu canvas

    [Header("Connect")]
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TMP_InputField inputIP;

    [Header("Pause")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button resume;
    [SerializeField] private Button leave;


    #region startmenu
    public void ConnectClicked()
    {

        setInactive();

        if (inputIP.text.Length > 0)
        {
            NetworkManager.Singleton.setIp(inputIP.text);
        }
        

        NetworkManager.Singleton.Connect();
    }

    private void setInactive()
    {

        usernameField.interactable = false;
        inputIP.interactable = false;
        connectUI.SetActive(false);


    }

    public void backToMain()
    {
        usernameField.interactable = true;
        inputIP.interactable = true;
        connectUI.SetActive(true);
    }

    public void SendName()
    {
        Message message = Message.Create(MessageSendMode.reliable, (ushort) ClientToServerId.name);

        message.AddString(usernameField.text);

        NetworkManager.Singleton.Client.Send(message);
    }

    #endregion

    #region pausemenu

    private void showPauseMenu()
    {
        pauseMenu.SetActive(true);

    }
    
    private void hidePauseMenu()
    {
        pauseMenu.SetActive(false);
    }




    #endregion

}
