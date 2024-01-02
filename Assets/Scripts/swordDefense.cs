using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class swordDefense : MonoBehaviour
{
    Scene scene;
    public GameObject player;

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("npcWeap"))
        {
            player.GetComponent<playerDayak>().can += 2*player.GetComponent<playerDayak>().damage;
            player.gameObject.GetComponent<WeapHorseControl>().audio.PlayOneShot(player.gameObject.GetComponent<WeapHorseControl>().sesDizisi[1]);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.O))
            SceneManager.LoadScene(scene.buildIndex);
    }
}
