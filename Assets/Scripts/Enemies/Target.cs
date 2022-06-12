
using UnityEngine;
using RiptideNetworking;
using static NetworkManager;

public abstract class Target : MonoBehaviour
{

    [Header("Stats")]
    [SerializeField] protected float health = 100f;
    [SerializeField] protected float speed = 0f;

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public virtual void takeDamage(float damage)
    {
        
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.damage);


        message.AddUShort(player.id);
        message.AddFloat(damage);

        Singleton.Client.Send(message);

    }

    [MessageHandler((ushort)ServerToClientId.playerHealth)]
    private void takeDamage(Message message)
    {
        if (player.id == message.GetUShort())
        {
            health -= message.GetFloat();
        }

        if (health < 0)
        {
            Destroy(gameObject);
        }
    }

    
}
