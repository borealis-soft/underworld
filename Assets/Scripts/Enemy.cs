using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public delegate void Handler(int cost);
    public event Handler OnDeath;
    public event Handler OnPass;

    public Transform[] Points;
    public bool Mooving = true;
    public float Speed, RotateSpeed;
    public float MaxHP;
    public int PlayerDamage;
    public Side Side_ = Side.None;
    public Image HPImage;

    [SerializeField]
    private int enemyCost;

    private Transform currentPoint;
    private Vector3 direction;
    private int currentPointID;
    public float HP { get; set; }

    void Start()
    {
        currentPointID = 0;
        currentPoint = Points[currentPointID];
        HP = MaxHP;
    }

    void Update()
    {
        if (Mooving)
            Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet") && HP > 0)
        {
            Destroy(other.gameObject);
            HP -= other.GetComponent<FlyBullet>().Damage;
            HPImage.fillAmount = HP / MaxHP;
            if (HP <= 0)
            {
                if (GameMode.Singleton == null || GameMode.Singleton.gameMod == GameMode.GameMods.SingleGame || 
                    PlayerResourses.Singleton.side.Value != Side_)
                    OnDeath(enemyCost);
                Animator animator = GetComponent<Animator>();
                if (!animator)
                    Destroy(gameObject);
                else
                    animator.SetBool("IsDead", true);
            }
        }
    }

    private void Move()
    {
        direction = currentPoint.position - transform.position;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, RotateSpeed * Time.deltaTime, 0);
        transform.rotation = Quaternion.LookRotation(newDirection);

        transform.position = Vector3.MoveTowards(transform.position, currentPoint.position, Speed * Time.deltaTime);
        if (transform.position == currentPoint.position)
        {
            currentPointID++;
            if (currentPointID >= Points.Length)
            {
                Destroy(gameObject);
                if (GameMode.Singleton == null || GameMode.Singleton.gameMod == GameMode.GameMods.SingleGame || 
                    PlayerResourses.Singleton.side.Value != Side_)
                    OnPass(PlayerDamage);
            }
            else currentPoint = Points[currentPointID];
        }
    }
}