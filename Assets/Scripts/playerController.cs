using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
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
    public KeyCode sprintButton = KeyCode.LeftShift, walkButton = KeyCode.C;

    public enum moveType
    {
        Directional,
        Strafe
    };
    public moveType hareketTipi;

    void Start()
    {
        anim = GetComponent<Animator>();
        mainCam=Camera.main;
        normalFov = mainCam.fieldOfView;
    }

    private void LateUpdate()
    {
        Movement();
        
    }

    void Movement()
    {
        if (hareketTipi == moveType.Directional)
        {
            inputMove();
            inputRotate();
            
            stickDirection = new Vector3(inputX, 0f, inputY);

            if (Input.GetKey(sprintButton))
            {
                mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, sprintFov, Time.deltaTime * 2);
                maxSpeed = 2f;
                inputX = 2 * Input.GetAxis("Horizontal");
                inputY = 2 * Input.GetAxis("Vertical");
            }
            else if (Input.GetKey(walkButton))
            {
                mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, normalFov, Time.deltaTime * 2);
                maxSpeed = 0.2f;
                inputX = Input.GetAxis("Horizontal");
                inputY = Input.GetAxis("Vertical");
            }
            else
            {
                mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, normalFov, Time.deltaTime * 2);
                maxSpeed = 1f;
                inputX = Input.GetAxis("Horizontal");
                inputY = Input.GetAxis("Vertical");
            }
        }


        if (hareketTipi == moveType.Strafe)
        {
            inputX = Input.GetAxis("Horizontal");
            inputY = Input.GetAxis("Vertical");

            anim.SetFloat("iX", inputX, damp, Time.deltaTime * 10);
            anim.SetFloat("iY", inputY, damp, Time.deltaTime * 10);

            bool hareketEdiyor = inputX != 0 || inputY != 0;

            if (hareketEdiyor)
            {
                float yawCamera = mainCam.transform.rotation.eulerAngles.y;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0f), strafeTurnSpeed * Time.fixedDeltaTime);
                anim.SetBool("strafeMoving", true);
            }else{
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
