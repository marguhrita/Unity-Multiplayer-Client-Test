
using UnityEngine;
using RiptideNetworking;
using static NetworkManager;

public abstract class Target : MonoBehaviour
{

    [Header("Stats")]
    [SerializeField] public float health;
    [SerializeField] protected float speed;

    private Player player;

    protected virtual void Start()
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


    
}
