using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{


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
}
