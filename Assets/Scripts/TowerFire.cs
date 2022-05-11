using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerFire : MonoBehaviour
{
    public float Radius, FireDelay, Damage;
    public LayerMask EnemyLayer;
    public Transform BulletPrefab;
    public float TurningSpeed;
    [Min(1)]
    public int MaxTargetCount = 1;

    private float timeToFire;
    private Transform gun, firePoint;
    private Transform[] enemyTargets;

    void Start()
    {
        timeToFire = FireDelay;
        gun = transform.GetChild(0);
        firePoint = gun.GetChild(0);
        enemyTargets = new Transform[MaxTargetCount];
    }

    void Update()
    {
        for (int i = 0; i < enemyTargets.Length; i++)
            if (IsTargetUseless(enemyTargets[i]))
                enemyTargets[i] = ClosestEnemy();

        if (timeToFire >= 0)
            timeToFire -= Time.deltaTime;
        else
            Fire();

        if (MaxTargetCount == 1 && enemyTargets[0])
            EnemyTraking(enemyTargets[0].position);
    }

    private void EnemyTraking(Vector3 lookAt)
    {
        lookAt.y = gun.position.y;
        gun.rotation = Quaternion.Slerp(gun.rotation, Quaternion.LookRotation(lookAt - gun.position), Time.deltaTime * TurningSpeed);
    }

    private bool IsTargetUseless(Transform targ)
    {
        return !targ || Vector3.Distance(transform.position, targ.position) > Radius
            || !targ.GetComponent<CapsuleCollider>().enabled;
    }

    private Transform ClosestEnemy()
    {
        List<Collider> enemyColliders = new List<Collider>(Physics.OverlapSphere(transform.position, Radius, EnemyLayer));
        for (int i = 0; i < enemyColliders.Count;)
        {
            if (!enemyTargets.Contains(enemyColliders[i].transform)) i++;
            else enemyColliders.RemoveAt(i);
        }
        if (enemyColliders.Count > 0)
        {
            int minId = 0;
            float min = Vector3.Distance(transform.position, enemyColliders[0].transform.position);
            for (int i = 1; i < enemyColliders.Count; i++)
            {
                float distance = Vector3.Distance(transform.position, enemyColliders[i].transform.position);
                if (distance < min)
                {
                    min = distance;
                    minId = i;
                }
            }
            return enemyColliders[minId].transform;
        }
        return null;
    }

    private void SpawnBullet(int targetId)
    {
        Transform bullet = Instantiate(BulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<FlyBullet>().Target = enemyTargets[targetId].GetChild(0);
        bullet.GetComponent<FlyBullet>().Damage = Damage;
    }

    private void Fire()
    {
        bool haveTarget = false;
        for (int i = 0; i < enemyTargets.Length; i++)
            if (enemyTargets[i])
            {
                haveTarget = true;
                SpawnBullet(i);
            }

        if (haveTarget)
        {
            timeToFire = FireDelay;
            var audio = transform.GetChild(0).GetComponent<AudioSource>();
            if (audio) audio.PlayOneShot(audio.clip);
        }
    }
}