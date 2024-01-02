using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plyContWithHorse : MonoBehaviour
{
    float inputX, inputY;
    float maxSpeed=1f;
    float normalFov;
    Vector3 stickDirection;
    Camera mainCam;
    Animator anim;

    [Header("publicler")]
    public float damp;
    public float sprintFov;
    public Transform model;
    [Range(1, 20)] public float rotationSpeed;
    [Range(1, 20)] public float strafeTurnSpeed;

    public enum moveType
    {
        Directional,
        Strafe
    };
    public moveType hareketTipi;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
    }

    void Movement()
    {
        if (hareketTipi == moveType.Strafe)
        {
            inputX = Input.GetAxis("Horizontal");
            inputY = Input.GetAxis("Vertical");

            bool hareketEdiyor = inputX != 0 || inputY != 0;

            if (hareketEdiyor)
            {
                anim.SetBool("strafeMoving", true);
            }
            else{
                anim.SetBool("strafeMoving", false);
            }
        }
    }

    void inputMove()
    {
        anim.SetFloat("speed", Vector3.ClampMagnitude(stickDirection, maxSpeed).magnitude, damp, 10 * Time.deltaTime);
    }

    void inputRotate()
    {
        Vector3 rotOfset = mainCam.transform.TransformDirection(stickDirection);
        rotOfset.y = 0;
        model.forward = Vector3.Slerp(model.forward, rotOfset, Time.deltaTime * rotationSpeed);
    }
}
