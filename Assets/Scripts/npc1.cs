using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class npc1 : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject handWeapon, backWeapon;
    public Collider weapColl;
    public bool vurusNpc=false;

    public LayerMask ground, player;
    public float sightRange, attackRange;
    public float inHorseAttackRange;
    float realAttackRange;
    public bool playerInSightRange, playerInAttackRange;
    public bool destinationPointSet, alreadyAttacked;
    public float walkPointRange;

    public Vector3 destinationPoint;
    [SerializeField] Transform _player2,horse;
    Transform _player;
    public float timeBetweenAttack;
    Animator npcAnim;

    void Awake()
    {
        npcAnim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange,player);
        playerInAttackRange = Physics.CheckSphere(transform.position, realAttackRange, player);

        if (playerInSightRange && playerInAttackRange && _player2.CompareTag("Player") ) Attack();
        else if (playerInSightRange) Chase();
        else Patroling();
    }

    private void FixedUpdate()
    {
        horse = _player2.gameObject.GetComponent<playerDayak>().FindMainObject(_player2).transform;
        if (_player2.gameObject.GetComponent<playerDayak>().FindMainObject(_player2).name == "Horse1" ||
            _player2.gameObject.GetComponent<playerDayak>().FindMainObject(_player2).name == "Horse2" ||
            _player2.gameObject.GetComponent<playerDayak>().FindMainObject(_player2).name == "Horse3")
        {
            _player = horse;
            realAttackRange = inHorseAttackRange;
        }
        else
        {
            _player = _player2;
            realAttackRange = attackRange;
        }

        if (_player2.GetComponent<Animator>().GetBool("Mount"))
        {
            _player.gameObject.GetComponent<WeapHorseControl>().audio.PlayOneShot(_player.gameObject.GetComponent<WeapHorseControl>().sesDizisi[0]);
            horse.gameObject.layer = 7;
        }
        else
            horse.gameObject.layer = 6;

    }

    void Patroling()
    {
        npcAnim.SetBool("inEnemy", false);
        if (!destinationPointSet) SearchWalkPoint();
        else
        {
            agent.SetDestination(destinationPoint);
        }
        Vector3 distanceDestination = transform.position - destinationPoint;
        if (distanceDestination.magnitude < 1.0f) destinationPointSet = false;
    }

    void Chase()
    {
        npcAnim.SetBool("inEnemy", true);
        agent.SetDestination(_player.position);
    }

    void Attack()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(_player);
        if (!alreadyAttacked)
        {
            vurusNpc = true;
            weapColl.enabled = true;
            npcAnim.SetBool("attack",true);
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

    void SearchWalkPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        destinationPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(destinationPoint, -transform.forward, 2.0f,ground))
        {
            destinationPointSet = true;
        }
    }

    public void equip()
    {
        handWeapon.SetActive(true);
        backWeapon.SetActive(false);
    }

    public void unequip()
    {
        handWeapon.SetActive(false);
        backWeapon.SetActive(true);
    }
}
