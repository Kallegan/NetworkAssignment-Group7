using Alteruna;
using Alteruna.Trinity;
using UnityEngine;
using Avatar = UnityEngine.Avatar;
using Vector3 = UnityEngine.Vector3;

public class Projectile : AttributesSync
{
    public int playerIndex;
    public int localId;

    [SynchronizableField] public float speed = 50.0f;
    [SynchronizableField] public float dirX = 0.0f;
    [SynchronizableField] public float dirY = 0.0f;
    [SynchronizableField] public float dirZ = 0.0f;
    public Vector3 direction;
    
    private void Start()
    {
        Debug.Log("PROJECTILE CREATED");
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

    // RPC 
    public void OnDeflect(Vector3 newDirection)
    {
        dirX = newDirection.x;
        dirY = newDirection.y;
        dirZ = newDirection.z;

        direction.x = dirX;
        direction.y = dirY;
        direction.z = dirZ;
        
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
