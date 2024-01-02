using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followRiidePoint : MonoBehaviour
{
    public Transform ridePoint;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = ridePoint.position;
    }
}
