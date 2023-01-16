using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using Alteruna.Trinity;
using UnityEngine;
using Avatar = UnityEngine.Avatar;

public class Projectile : MonoBehaviour
{
    public int playerIndex;
    private Vector3 direction;
    public float speed;
    
    private void Start()
    {
        direction = transform.forward;
        Debug.Log("Projectile player index: " + playerIndex);
    }

    void Update()
    {
        transform.position += direction * (Time.deltaTime * speed);
    }
    void OnBecameInvisible() 
    {
        Destroy(gameObject);
    }
}
