
using UnityEngine;
using RiptideNetworking;
using RiptideNetworking.Utils;
using System;

public class NetworkManager : MonoBehaviour
{
    public enum ServerToClientId : ushort
    {
        playerSpawned = 1,
        playerPositions,
    }
    public enum ClientToServerId : ushort
    {
        name = 1,
        playerPosition,
    }

    private static NetworkManager _singleton;



    public static NetworkManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    public Client Client { get; private set; }

    [SerializeField] private string ip;
    [SerializeField] private ushort port;

    private void Awake()
    {
        Singleton = this;
    }

    public void setIp(string ip)
    {
        this.ip = ip;
    }

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false); //sends riptide error messages to unity


        Client = new Client(); // new client instance
        Client.Connected += DidConnect; //subscribes 
        Client.ConnectionFailed += FailedToConnect;
        Client.ClientDisconnected += playerLeft;
        Client.Disconnected += DidDisconnect;

    }

    private void FixedUpdate()
    {
        Client.Tick();
    }

    private void OnApplicationQuit()
    {
        Client.Disconnect();
    }

    public void Connect()
    {
        Debug.Log($"Connecting to {ip}");
        Client.Connect($"{ip}:{port}");

    }

    private void DidConnect(object sender, EventArgs e)
    {
        UIManager.Singleton.SendName();
    }

    private void FailedToConnect(object sender, EventArgs e)
    {
        UIManager.Singleton.backToMain();
    }

    private void playerLeft(object sender, ClientDisconnectedEventArgs e)
    {
        Destroy(Player.list[e.Id].gameObject);
    }

    private void DidDisconnect(object sender, EventArgs e)
    {
        UIManager.Singleton.backToMain();
    }

}
