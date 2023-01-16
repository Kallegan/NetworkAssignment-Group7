using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using Alteruna.Trinity;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    void Update()
    {
        transform.position += transform.forward * (Time.deltaTime * 20);
    }
    void OnBecameInvisible() 
    {
        Destroy(gameObject);
    }
}
