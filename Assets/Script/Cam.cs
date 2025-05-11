using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    [SerializeField] private Transform player;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        gameObject.transform.position = player.position;
    }
}
