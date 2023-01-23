using System;
using Alteruna;
using Alteruna.Trinity;
using UnityEngine;

public class PlayerActions : AttributesSync
{
    // TODO: add charge up on projectiles
    
    private Alteruna.Avatar avatar;
    [SerializeField]private Spawner spawner;
    
    // Attack
    [SerializeField] private float attackCoolDown = 0.5f;
    private bool canAttack = true;
    private float curAttackCoolDown;
    
    // Deflect
    [SerializeField] private BoxCollider deflectArea;
    public SynchronizedProjectile curDeflectable = null;
    
    [SerializeField] private float deflectCoolDown = 0.5f;
    private float curDeflectCoolDown;
    private bool canDeflect = true;

    
    private void Awake()
    {
        spawner = FindObjectOfType<Spawner>();
    }

    private void Start()
    {
        curAttackCoolDown = attackCoolDown;
        curDeflectCoolDown = deflectCoolDown;
        avatar = gameObject.GetComponentInParent(typeof(Alteruna.Avatar)) as Alteruna.Avatar;
    }
    
    private void Update()
    {
        if (!avatar.IsMe)
            return;

        if (Input.GetMouseButtonDown(0))
            if (canAttack)
                Shoot();
       
        if (Input.GetMouseButton(1))
        {
            if (TryDeflect())
                OnDeflect();
        }
        
        if (!canAttack)
        {
            curAttackCoolDown -= Time.deltaTime;
            if (curAttackCoolDown <= 0)
            {
                canAttack = true;
                curAttackCoolDown = attackCoolDown;
            }
        }
        
        if (!canDeflect)
        {
            curDeflectCoolDown -= Time.deltaTime;
            if (curDeflectCoolDown <= 0)
            {
                canDeflect = true;
                curDeflectCoolDown = deflectCoolDown;
            }
        }
    }

    bool TryDeflect()
    {
        if (canDeflect && curDeflectable)
            return true;
        
        canDeflect = false;
        return false;
    }

    void OnDeflect()
    {
        Vector3 direction = transform.parent.forward;
        curDeflectable.OnDeflect(direction.normalized);
        curDeflectable = null;
    }
    private void Shoot()
    {
        Vector3 spawnPosition = transform.position + (transform.forward * 2f);
        Quaternion spawnRotation = transform.rotation;
        
        GameObject proj = spawner.Spawn(0, spawnPosition, spawnRotation);
        
        if (!proj.TryGetComponent(out SynchronizedProjectile p)) 
            return;
       
        UInt16 playerIndex = avatar.Possessor.Index;
        
        ProcedureParameters parameters = new ProcedureParameters();
        parameters.Set("playerIndex", (UInt16)Multiplayer.Instance.Me.Index);
        Multiplayer.InvokeRemoteProcedure("RemoteGetOwnerIndex", UserId.All, parameters);
        
        canAttack = false;
    }
}
