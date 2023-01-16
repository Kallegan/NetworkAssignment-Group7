using Alteruna;
using Alteruna.Trinity;
using UnityEngine;
using Avatar = UnityEngine.Avatar;
using Vector3 = UnityEngine.Vector3;

public class Projectile : AttributesSync
{
    public int playerIndex;

    public float speed = 50.0f;
    
    public Vector3 direction;
    
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
    public void OnDeflect(float x, float y, float z)
    {
        direction.x = x;
        direction.y = y;
        direction.z = z;
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
