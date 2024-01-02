using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcDayak : MonoBehaviour
{
    int damageCount = 0;
    public int can=5;
    public GameObject player;
    public Collider deadNpcBody;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("playerWeap") && player.GetComponent<WeapHorseControl>().vurusPlayer)
        {
            Debug.Log("npc darbe");
            GetComponent<Animator>().SetTrigger("damage");
            player.gameObject.GetComponent<WeapHorseControl>().audio.PlayOneShot(player.gameObject.GetComponent<WeapHorseControl>().sesDizisi[3]);
            damageCount++;
            if (damageCount >= can)
            {
                transform.gameObject.layer = 0;
                GetComponent<Animator>().SetTrigger("death");
                player.gameObject.GetComponent<WeapHorseControl>().audio.PlayOneShot(player.gameObject.GetComponent<WeapHorseControl>().sesDizisi[5]);
                GetComponent<npc1>().enabled = false;
                GetComponent<CapsuleCollider>().enabled = false;
                Destroy(GetComponent<Rigidbody>());
                deadNpcBody.enabled = true;
            }
            Debug.Log("damageCount:" + damageCount);
        }
            }
 }
 