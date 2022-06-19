using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;
using static NetworkManager;

public class ShootGun : MonoBehaviour
{

    Player player;
    [SerializeField] GameObject cam;

    [Header("Weapon Stats")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float fireRate = 10f;
    [SerializeField] private float cooldown= 0.1f;

    [Header("Trails")]
    [SerializeField] private TrailRenderer BulletTrail;
    [SerializeField] private Transform BulletSpawnPoint;
    [SerializeField] private ParticleSystem ImpactParticleSystem;
    [SerializeField] public float BulletSpeed = 100;
    [SerializeField] private LayerMask whatIsMap;

    


    private bool canShoot = true;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {

            if (!canShoot)
            {
                return;
            }

            if (player != null)
            {
                sendShootMessage();
            }

            Shoot(cam.transform, BulletSpawnPoint);

            canShoot = false;
            Invoke("resetShootCooldown", cooldown);
        }


    }


    void Shoot(Transform viewTransform, Transform bulletOriginTransform)
    {
       
        RaycastHit hit;

        if (Physics.Raycast(viewTransform.position, viewTransform.forward, out hit,whatIsMap)) //Performs raycast on camera direction and outputs to "hit"
        {

            Debug.Log("Hit object");

            damageTarget(hit);//need to check this

            TrailRenderer trail = Instantiate(BulletTrail, bulletOriginTransform.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));

        }
        else
        {
            TrailRenderer trail = Instantiate(BulletTrail, bulletOriginTransform.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(trail, viewTransform.forward * 100, Vector3.zero, false));

        }
    }

    public void createTrail(Transform startTransform, Vector3 endPoint)
    {
        

        TrailRenderer trail = Instantiate(BulletTrail, startTransform.position, Quaternion.identity);


        StartCoroutine(SpawnTrail(trail, startTransform.forward, Vector3.zero, true));


    }


    //Magic code that creates a trail from the spawn position to the impact point
    public IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, bool MadeImpact)
    {

        Vector3 startPosition = Trail.transform.position;
        float distance = Vector3.Distance(Trail.transform.position, HitPoint);//distance between where the trail was instantiated and where the 
        float remainingDistance = distance;



        while (remainingDistance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (remainingDistance / distance));

            remainingDistance -= BulletSpeed * Time.deltaTime;

            yield return null;
        }

        //Animator.SetBool("IsShooting", false);

        Trail.transform.position = HitPoint;
        if (MadeImpact)
        {
            Instantiate(ImpactParticleSystem, HitPoint, Quaternion.LookRotation(HitNormal));
        }

        Destroy(Trail.gameObject, Trail.time);
    }


    void resetShootCooldown()
    {
        canShoot = true;
    }

    private void damageTarget(RaycastHit hit)
    {
        Target target = hit.transform.GetComponent<Target>();

        if (target != null)
        {
            target.takeDamage(damage);
        }

    }

    public TrailRenderer getTrail()
    {
        return Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);
    }


    

    public void sendShootMessage()
    {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.playerShot);

        message.AddUShort(player.id);

        Singleton.Client.Send(message);
    }
}

