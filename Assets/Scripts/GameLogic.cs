using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    private static GameLogic _singleton;

    public static GameLogic Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(GameLogic)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    public GameObject LocalPlayerPrefab => localPlayerPrefab;
    public GameObject PlayerPrefab => playerPrefab;

    [Header("Prefabs")]
    [SerializeField] private GameObject localPlayerPrefab;
    [SerializeField] private GameObject playerPrefab;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Awake()
    {
        Singleton = this;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += sceneLoaded; // subscribes "sceneLoaded" to new scene loading
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= sceneLoaded;// unsubscribes "sceneLoaded" to new scene loading
    }

    private void sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level1" & MainMenuManager.Singleton.isSingleplayer)
        {
            Player player;

            player = Instantiate(Singleton.LocalPlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity).GetComponent<Player>();

            player.isLocal = true;

            UIManager.Singleton.loadPlayer(player.gameObject);
        }
    }
}
