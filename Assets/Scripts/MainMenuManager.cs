using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using RiptideNetworking;
using static NetworkManager;

public class MainMenuManager : MonoBehaviour
{

    private static MainMenuManager _singleton;

    public static MainMenuManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(MainMenuManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject multiplayerMenu;
    [SerializeField] private GameObject networkManager;

    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_InputField ipField;

    public void loadSingleplayer()
    {
        Player player;
        
        SceneManager.LoadScene("Level1");

        player = Instantiate(GameLogic.Singleton.LocalPlayerPrefab, new Vector3(0,1,0) , Quaternion.identity).GetComponent<Player>();

        //UIManager.Singleton.loadPlayer(player.gameObject);

        player.isLocal = true;

        DontDestroyOnLoad(player);

    }

    public void loadMultiplayerMenu()
    {
        startMenu.SetActive(false);
        multiplayerMenu.SetActive(true);

        
        //DontDestroyOnLoad(networkManager);
    }

    public void ConnectClicked()
    {

        if (ipField.text.Length > 0)
        {
            NetworkManager.Singleton.setIp(ipField.text);
        }

        
        NetworkManager.Singleton.Connect();


        DontDestroyOnLoad(networkManager);

        SceneManager.LoadScene("Level1");
    }

    public void backToStartMenu()
    {
        startMenu.SetActive(true);
        multiplayerMenu.SetActive(false);
    }

    public void SendName()
    {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.name);

        message.AddString(username.text);

        Debug.Log("Sending name");

        NetworkManager.Singleton.Client.Send(message);
    }

    public void backToMain()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
