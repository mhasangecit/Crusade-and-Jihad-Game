using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class npcDost : MonoBehaviour
{
    public bool enemyInSightRange;
    public bool enemyInAttackRange;
    public float sightRange, attackRange;
    public LayerMask Npc, ground;
    NavMeshAgent agent;
    public Transform player;
    public float maxDostMesafe;
    float mesafe;
    Animator npcAnim;
    public Collider weapColl;
    bool vurusNpc = false;
    bool alreadyAttacked;
    public float timeBetweenAttack;
    int damageCount = 0;
    public int can = 50;
    public GameObject handWeapon, backWeapon;
    public GameObject[] targetObjects;
    public Transform playerHorseBack;

    public float rideSpeed;
    public float horseDistance;
    bool inHorse;
    GameObject horse;
    bool inHorsePos = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        npcAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (inHorsePos)
        {
            transform.position = playerHorseBack.position;
            transform.rotation = playerHorseBack.rotation;
        }else
        {

        }
    }

    void FixedUpdate()
    { 
        enemyInSightRange = Physics.CheckSphere(transform.position, sightRange, Npc);
        enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, Npc);

        mesafe = Vector3.Distance(transform.position, player.position);

        if (enemyInSightRange && enemyInAttackRange && !inHorse) Attack();
        else if (enemyInSightRange && !inHorse) Chase();
        else if (mesafe > maxDostMesafe && !inHorse) Patroling();
        else if(player.GetComponent<Rigidbody>().velocity.magnitude>0) npcAnim.SetBool("idle", false);
        else  npcAnim.SetBool("idle", true);
    }

    void Patroling()
    {
        npcAnim.SetBool("idle", false);
        agent.SetDestination(player.position);
    }

    void Chase()
    {
        npcAnim.SetBool("inEnemy", true);
        npcAnim.SetBool("idle", false);

        foreach (GameObject targetObject in targetObjects)
        {
                float distanceToTarget = Vector3.Distance(transform.position, targetObject.transform.position);
                if (distanceToTarget <= sightRange)
                {
                    agent.SetDestination(targetObject.transform.position);
                }
        }
    }

    void Attack()
    {
        npcAnim.SetBool("idle", false);
        agent.SetDestination(transform.position);
        //transform.LookAt(_player);
        if (!alreadyAttacked)
        {
            vurusNpc = true;
            weapColl.enabled = true;
            npcAnim.SetBool("attack", true);
            alreadyAttacked = true;
            Invoke(nameof(resetAttack), timeBetweenAttack);
        }
    }

    void resetAttack()
    {
        alreadyAttacked = false;
        vurusNpc = false;
        weapColl.enabled = false;
        npcAnim.SetBool("attack", false);
    }

    void equip()
    {
        backWeapon.SetActive(false);
        handWeapon.SetActive(true);
    }
    void unequip()
    {
        backWeapon.SetActive(true);
        handWeapon.SetActive(false);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("npcWeap"))
        {
            Debug.Log("dost darbe");
            GetComponent<Animator>().SetTrigger("damage");
            damageCount++;
            if (damageCount >= can)
            {
                GetComponent<Animator>().SetTrigger("death");
                GetComponent<npcDost>().enabled = false;
                GetComponent<CapsuleCollider>().enabled = false;
                Destroy(GetComponent<Rigidbody>());
            }
        }
    }

    public void rideHorse(GameObject majorObject)
    {
        horse=player.gameObject.GetComponent<playerDayak>().FindMainObject(player);

        if(!inHorse)
        agent.SetDestination(horse.transform.position);
        
        if (Vector3.Distance(transform.position, horse.transform.position) < horseDistance)
        {
            Vector3 targetPosition = transform.position - transform.forward * 2.0f;
            transform.position= Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * rideSpeed);
            npcAnim.SetBool("horseRide", true);
            npcAnim.SetBool("idle", false);
            print("horseRide anim");
            inHorse = true;
        }

        if (inHorse)
        {
            npcAnim.SetBool("inEnemy", false);
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            inHorsePos = true;
            agent.SetDestination(transform.position);
        }else
        {
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<CapsuleCollider>().enabled = true;
            inHorsePos = false;
        }
    }

    public void offHorse()
    {
        npcAnim.SetBool("horseRide", false);
        npcAnim.SetBool("idle", true);
        GetComponent<NavMeshAgent>().enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;
        inHorsePos = false;
        transform.position = transform.position;
        agent.SetDestination(player.position);
    }
}
