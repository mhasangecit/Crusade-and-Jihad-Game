using MalbersAnimations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class horseDayak : MonoBehaviour
{
    public Transform player;
    Animator anim;
    int damageIndex=0;
    public int horseCan=30;

    void Start()
    {
        anim = GetComponent<Animator>();  
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("npcWeap"))
        {
            horseDamage();
        }
        if (damageIndex == horseCan)
        {
            horseDeath();
        }
    }

    public void horseDamage()
    {
        anim.SetInteger("Mode", 3001);
        anim.SetInteger("ModeStatus", 1);
        damageIndex++;
    }

    public void horseDeath()
    {
        player.SetParent(null);
        anim.SetInteger("State", 10);
        anim.SetInteger("ModeStatus", -2);
        destroyHorseScripts();
    }

    public void destroyHorseScripts()
    {
        Destroy(GetComponent<MalbersInput>());
    }
}
