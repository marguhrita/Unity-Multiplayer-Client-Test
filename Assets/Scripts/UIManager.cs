
using UnityEngine;


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

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            showPauseMenu();
        }
    }

    private PlayerMovement playerMovement;
    private PlayerCamera playerCamera;

    [Header("Pause")]
    [SerializeField] private GameObject pauseMenu;

    [Header("Death")]
    [SerializeField] private GameObject deathMenu;

    

    public void loadPlayer(GameObject player)
    {

        //Debug.Log(player.transform);

        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
            playerCamera = player.GetComponent<PlayerCamera>();
        }

    }


    #region pausemenu

    public void showPauseMenu()
    {
        Debug.Log("Showing pause menu");
        playerMovement.enabled = false;
        playerCamera.enableCursor();
        playerCamera.enabled = false;
        pauseMenu.SetActive(true);
        
    }

    public void hidePauseMenu()
    {
        Debug.Log("hiding pause menu");
        pauseMenu.SetActive(false);
        playerMovement.enabled = true;
        playerCamera.enabled = true;
        playerCamera.disableCursor();
    }

    public void showDeathMenu()
    {
        Debug.Log("Showing death menu");
        playerMovement.enabled = false;
        playerCamera.enableCursor();
        playerCamera.enabled = false;
        deathMenu.SetActive(true);
    }

    public void hideDeathMenu()
    {
        Debug.Log("Hiding pause menu");
        deathMenu.SetActive(false);
        playerMovement.enabled = true;
        playerCamera.enabled = true;
        playerCamera.disableCursor();
    }

    public void backToMain()
    {
        MainMenuManager.Singleton.backToMain();
    }

    

    #endregion

}
