using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;
using static NetworkManager;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();
    public ushort id { get; private set; }
    public bool isLocal;

    private string username;


    [SerializeField] private Target target;
    [SerializeField] private Camera cam;


    private void Awake()
    {
        target = GetComponent<Target>();
    }

    private void OnDestroy()
    {
        list.Remove(id);
    }

  


    //spawns the player
    public static void Spawn(ushort id, string username, Vector3 position)
    {
        Player player;
        if (id == Singleton.Client.Id) //checks if the player is local
        {
            player = Instantiate(GameLogic.Singleton.LocalPlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.isLocal = true;
        }
        else
        {
            player = Instantiate(GameLogic.Singleton.PlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
        }

        player.name = $"Player {id} ({(string.IsNullOrEmpty(username) ? "Guest" : username)})";
        player.id = id;
        player.username = username;

        list.Add(id, player);
    }

    private void movePlayer(Vector3 position)
    {
        if (!isLocal)
        {
            transform.position = position;
        }
    }

    private void changeCamRotation(Quaternion rotation)
    {
        if (!isLocal)
        {
            cam.transform.rotation = rotation;
        }
    }

    private void killPlayer()
    {
        Debug.Log("You died");
    }


    #region messages


    [MessageHandler((ushort)ServerToClientId.playerSpawned)]
    private static void spawnPlayer(Message message)
    {
        Spawn(message.GetUShort(), message.GetString(), message.GetVector3());
    }


    [MessageHandler((ushort)ServerToClientId.playerPositions)]
    private static void updateOtherPlayerPositions(Message message)
    {
        Debug.Log("Recieved player positons");

        Player player = list[message.GetUShort()];
        
        player.movePlayer(message.GetVector3());
        player.changeCamRotation(message.GetQuaternion());
    }

    
    [MessageHandler((ushort)ServerToClientId.playerHealth)]
    private static void playerHealthMessage(Message message)
    {

        Debug.Log("Recieved player health");


        Player player = list[message.GetUShort()];

        player.target.health = message.GetFloat();

        if (player.target.health < 0)
        {
            Debug.Log($"Player with id:{player.id} died");
        }
    }
    
    #endregion



}
