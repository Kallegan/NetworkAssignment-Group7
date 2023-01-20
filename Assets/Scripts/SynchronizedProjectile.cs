using System;
using Alteruna;
using UnityEngine;
using Avatar = Alteruna.Avatar;
using Vector3 = UnityEngine.Vector3;

public class SynchronizedProjectile : Synchronizable
{
    private Spawner _spawner;
    private UInt16 _ownerIndex;

    private Vector3 _direction;
    private Vector3 _oldDirection;

    [SerializeField] private float _speed = 5.0f;
    private float _oldSpeed;
    
    public int damage = 1;

    public void Start()
    {
        Debug.Log("Proj was spawned");
    }

    [SynchronizableMethod]
    public void Init(UInt16 fromIndex)
    {
        _direction = transform.forward;
        _ownerIndex = fromIndex;
        _spawner = FindObjectOfType<Spawner>();
    }
    
    private void Update()
    {
        /*
        if (_oldDirection != _direction | _oldSpeed != _speed)
        {
            _oldDirection = _direction;
            _oldSpeed = _speed;
            Commit();
        }
        
        transform.position += _direction * (_speed * Time.deltaTime);
        SyncUpdate();
        */
    }
    
    public override void AssembleData(Writer writer, byte LOD = 100)
    {
        writer.Write(_direction);
        writer.Write(_speed);
    }

    public override void DisassembleData(Reader reader, byte LOD = 100)
    {
        _direction = reader.ReadVector3();
        _oldDirection = _direction;

        _speed = reader.ReadFloat();
        _oldSpeed = _speed;
    }

    public void OnDeflect(Vector3 newDirection)
    {
        _direction = newDirection;
        _speed *= 1.1f;
    }

    private void DestroySelf()
    {
        if (Multiplayer.Me.Index == _ownerIndex)
            _spawner.Despawn(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var damageable = collision.gameObject.GetComponentInChildren<DamageableComponent>();
        if (damageable)
            damageable.OnHit(1, _direction);

        DestroySelf();
    }
}
