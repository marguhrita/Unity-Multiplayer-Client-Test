
using UnityEngine;
using TMPro;
using RiptideNetworking;
using static NetworkManager;

public abstract class Target : MonoBehaviour
{

    [SerializeField] protected TMP_Text healthText;

    [Header("Stats")]
    [SerializeField] protected float health;
    [SerializeField] protected float speed;

    


    public Player player;
    

    protected virtual void Start()
    {
        player = GetComponent<Player>();
        
    }

    public virtual void setHealth(float health)
    {
        //Debug.Log("Setting health to " + health);


        if (healthText!= null)
        {
            healthText.text = health.ToString();
        }
        
        
        this.health = health;
    }

    public float getHealth()
    {
        return health;
    }

    public virtual void takeDamage(float damage)
    {
        Debug.Log($"Damaging player {player.id} with {damage} damage");

        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.damage);

        message.AddUShort(player.id);

        message.AddFloat(damage);

        NetworkManager.Singleton.Client.Send(message);

    }
}
