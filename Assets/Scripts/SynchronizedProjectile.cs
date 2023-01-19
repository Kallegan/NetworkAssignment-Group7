using Alteruna;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class SynchronizedProjectile : Synchronizable
{
    public Spawner spawner;
    public int playerIndex;

    private Vector3 direction;
    private Vector3 _oldDirection;
    
    [SerializeField] private float speed = 5.0f;
    private float _oldSpeed;

    private void Start()
    {
        direction = transform.forward;
    }
    
    private void Update()
    {
        if (_oldDirection != direction | _oldSpeed != speed)
        {
            Commit();
            _oldDirection = direction;
            _oldSpeed = speed;
        }
        transform.position += direction * (speed * Time.deltaTime);
        SyncUpdate();
    }
    
    public override void AssembleData(Writer writer, byte LOD = 100)
    {
        writer.Write(direction);
        writer.Write(speed);
    }

    public override void DisassembleData(Reader reader, byte LOD = 100)
    {
        direction = reader.ReadVector3();
        _oldDirection = direction;

        speed = reader.ReadFloat();
        _oldSpeed = speed;
    }
    
    public void OnDeflect(Vector3 newDirection)
    {
        direction = newDirection;
        speed *= 1.1f;
    }

    private void DestroySelf()
    {
        spawner.Despawn(gameObject);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        DestroySelf();
    }
}
