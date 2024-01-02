using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeapHorseControl : MonoBehaviour
{
    bool isStafe=false;
    public bool vurusPlayer = false;

    float attackIndex;
    Animator anim;

    Collider playerWeapColl;
    public GameObject handWeapon;
    public GameObject backWeapon;
    public GameObject defenseCol;

    public AudioSource audio;
    public AudioClip[] sesDizisi;

    void Start()
    {
        anim = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        playerWeapColl = handWeapon.GetComponent<Collider>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            isStafe = !isStafe;

        if (isStafe)
        {
            anim.SetBool("strafeMoving", true);
        }
        else
        {
            anim.SetBool("strafeMoving", false);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && isStafe)
        {
            playerWeapColl.enabled=true;
            attackIndex = Random.Range(0, 3);
            anim.SetFloat("attackIndex", attackIndex);
            anim.SetTrigger("saldir");
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            vurusPlayer = true;
            StartCoroutine(vurusKapat());
        }

        if (Input.GetKey(KeyCode.Mouse1) && isStafe)
        {
            anim.SetBool("defense", true);
            defenseCol.GetComponent<BoxCollider>().enabled=true;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        }
        else
        {
            anim.SetBool("defense", false);
            defenseCol.GetComponent<BoxCollider>().enabled = false;
            GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionY;
        }

        if (Input.GetKey(KeyCode.L))
            Time.timeScale = 0f;
        if (Input.GetKey(KeyCode.K))
            Time.timeScale = 1f;
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

    IEnumerator vurusKapat()
    {
        yield return new WaitForSeconds(1.5f);
        vurusPlayer = false;
        playerWeapColl.enabled = false;
        GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionY;
    }
}
