using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;
using static NetworkManager;

public class ShootGun : MonoBehaviour
{

    [SerializeField] Player player;
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
    


    private bool canShoot = true;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Shoot();
            sendShootMessage();
        }
    }


    void Shoot()
    {

        if (!canShoot)
        {
            return;
        }

        canShoot = false;

        Invoke("resetShootCooldown", cooldown);


        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit,1 << ~gameObject.layer)) //Performs raycast on camera direction and outputs to "hit"
        {

            damageTarget(hit);

            TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));


        }
        else
        {
            TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(trail, cam.transform.forward * 100, Vector3.zero, false));

        }

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

    public void sendShootMessage()
    {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.playerShot);

        message.AddUShort(player.id);

        Singleton.Client.Send(message);
    }
}

