using MalbersAnimations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerDayak : MonoBehaviour
{
    public float can = 100;
    public float damage=5;
    public Image canBar;
    float oncekiCan;
    public GameObject npcDost;

    public void Start()
    {
        oncekiCan = can;
    }

    private void FixedUpdate()
    {
            GameObject majorObject = FindMainObject(transform);
            if (majorObject.name.Substring(0, 4) == "Hors")
            {
                npcDost.GetComponent<npcDost>().rideHorse(majorObject);
            }else
            {
                npcDost.GetComponent<npcDost>().offHorse();
            }
    }

    public GameObject FindMainObject(Transform childTransform)
    {
        if (childTransform.parent != null)
        {
            return FindMainObject(childTransform.parent);
        }
        else
        {
            return childTransform.gameObject;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("npcWeap"))
        {
            GameObject mainObject = FindMainObject(transform);
            GameObject npc= FindMainObject(collision.transform);
            if (npc.GetComponent<npc1>().vurusNpc)
            {
                can -= damage;
                canBar.fillAmount = (can/100);
                if (oncekiCan > can)
                {
                    GetComponent<Animator>().SetTrigger("damage");
                    GetComponent<WeapHorseControl>().audio.PlayOneShot(GetComponent<WeapHorseControl>().sesDizisi[2]);
                    if (mainObject != null && (mainObject.name == "Horse1" || mainObject.name == "Horse2" || mainObject.name == "Horse3"))
                    {
                        mainObject.GetComponent<horseDayak>().horseDamage();
                    }
                    Debug.Log("player darbe");
                }
                oncekiCan = can;
                if (can <=0)
                {
                    can = 0;
                    GetComponent<WeapHorseControl>().audio.PlayOneShot(GetComponent<WeapHorseControl>().sesDizisi[4]);
                    transform.gameObject.layer = 0;
                    
                    if (mainObject != null && (mainObject.name == "Horse1" || mainObject.name == "Horse2" || mainObject.name == "Horse3"))
                    {
                        transform.SetParent(null);
                        mainObject.GetComponent<horseDayak>().horseDeath();
                    }

                    GetComponent<Animator>().SetTrigger("death");
                    StartCoroutine(DestroyScript(mainObject));
                }
            }
        }
    }

    IEnumerator DestroyScript(GameObject mainObject)
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(GetComponent<WeapHorseControl>());
        Destroy(GetComponent<MalbersInput>());
        if(mainObject!=null && mainObject.name.Substring(0, 5)=="Horse")
        mainObject.GetComponent<horseDayak>().destroyHorseScripts();
    }
}
