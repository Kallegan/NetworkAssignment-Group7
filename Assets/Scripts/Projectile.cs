using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using Alteruna.Trinity;
using UnityEngine;
using Avatar = UnityEngine.Avatar;

public class Projectile : AttributesSync
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

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;
        
        GameObject other = collision.gameObject;
        // Gör vad vi vill ex, Player.Damage();

        BroadcastRemoteMethod("Destroy");
    }

    [SynchronizableMethod]
    void Destroy()
    {
        Destroy(gameObject);
    }
    
    void OnBecameInvisible() 
    {
        Destroy(gameObject);
    }
}
