using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Alteruna;
using Alteruna.Trinity;
using UnityEngine;
using Avatar = UnityEngine.Avatar;
using Vector3 = UnityEngine.Vector3;

public class Projectile : AttributesSync
{
    public int playerIndex;

    public float speed = 50.0f;
    
    //TODO: only send on deflect/if changed, to reduce overhead
    [SynchronizableField]public Vector3 direction;
    
    private void Start()
    {
        direction = transform.forward;
    }

    void Update()
    {
        transform.position += direction * (20 * Time.deltaTime);
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
    public void OnDeflect(Vector3 fromDirection)
    {
        direction = fromDirection;
        speed *= 1.1f;
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
