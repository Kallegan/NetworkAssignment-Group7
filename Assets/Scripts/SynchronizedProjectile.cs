using Alteruna;
using UnityEngine;
using Avatar = Alteruna.Avatar;
using Vector3 = UnityEngine.Vector3;

public class SynchronizedProjectile : Synchronizable
{
    public Spawner spawner;
    public Avatar avatar;
    public int ownerIndex;

    private Vector3 direction;
    private Vector3 _oldDirection;

    [SerializeField] private float speed = 5.0f;
    private float _oldSpeed;

    public int damage = 1;

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
        if (avatar.Possessor.Index == ownerIndex)
            spawner.Despawn(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var damageable = collision.gameObject.GetComponentInChildren<DamageableComponent>();
        if (damageable)
            damageable.OnHit(1, direction);

        DestroySelf();
    }
}
