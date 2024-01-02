using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class weaponController : MonoBehaviour
{
    bool isStafe=false;
    bool canAttack = true;
    public bool vurusPlayer = false;

    float attackIndex;
    Animator anim;

    Collider playerWeapColl;
    public GameObject handWeapon;
    public GameObject backWeapon;
    public GameObject defenseCol;

    void Start()
    {
        anim = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        playerWeapColl = handWeapon.GetComponent<Collider>();
    }

    void Update()
    {
        anim.SetBool("iS", isStafe);

        if (Input.GetKeyDown(KeyCode.X))
            isStafe = !isStafe;

        if (isStafe)
        {
            GetComponent<playerController>().hareketTipi = playerController.moveType.Strafe;
        }
        else
        {
            GetComponent<playerController>().hareketTipi = playerController.moveType.Directional;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && isStafe && canAttack)
        {
            playerWeapColl.enabled=true;
            attackIndex = Random.Range(0, 3);
            anim.SetFloat("attackIndex", attackIndex);
            anim.SetTrigger("saldir");
            vurusPlayer = true;
            StartCoroutine(vurusKapat());
        }

        if (Input.GetKey(KeyCode.Mouse1) && isStafe)
        {
            anim.SetBool("defense", true);
            defenseCol.GetComponent<BoxCollider>().enabled=true;
        }else
        {
            anim.SetBool("defense", false);
            defenseCol.GetComponent<BoxCollider>().enabled = false;
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
    }
}
