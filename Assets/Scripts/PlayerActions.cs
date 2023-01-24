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

    public delegate void ShootDelegate();
    public event ShootDelegate OnShoot;

    public delegate void DeflectDelegate();
    public event DeflectDelegate OnTryDeflect;
    
    private void Awake()
    {
        spawner = FindObjectOfType<Spawner>();
    }

    private void Start()
    {
        curAttackCoolDown = attackCoolDown;
        curDeflectCoolDown = deflectCoolDown;
        avatar = gameObject.GetComponentInParent(typeof(Alteruna.Avatar)) as Alteruna.Avatar;

        Multiplayer.RegisterRemoteProcedure("ShootRemote", ShootRemote);
        Multiplayer.RegisterRemoteProcedure("DeflectRemote", DeflectRemote);
    }
    
    private void Update()
    {
        if (!avatar.IsMe)
            return;

        if (Input.GetMouseButtonDown(0))
            if (canAttack)
                Shoot();
        
        if (Input.GetMouseButton(1) && canDeflect)
        {
            if (CheckDeflectable())
                OnDeflectSuccess();
            else
                OnDeflectMiss();
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
    
    bool CheckDeflectable()
    {
        if (curDeflectable)
            return true;
        
        return false;
    }

    void OnDeflectSuccess()
    {
        OnTryDeflect?.Invoke(); //This is kind of borked
        Multiplayer.InvokeRemoteProcedure("DeflectRemote", UserId.All);
        
        Vector3 direction = transform.parent.forward;
        curDeflectable.OnDeflect(direction.normalized);
        curDeflectable = null;
        
        canDeflect = false;
    }
    
    void OnDeflectMiss()
    {

        OnTryDeflect?.Invoke(); //This is kind of borked
        Multiplayer.InvokeRemoteProcedure("DeflectRemote", UserId.All);
        
        canDeflect = false;
    }
    
    private void Shoot()
    {
        OnShoot?.Invoke();
        Multiplayer.InvokeRemoteProcedure("ShootRemote", UserId.All);
        
        GameObject proj = spawner.Spawn(0, transform.position + transform.forward * 2f, transform.rotation);
        
        canAttack = false;
    }

    private void ShootRemote(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        OnShoot?.Invoke();
    }

    private void DeflectRemote(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        OnTryDeflect?.Invoke();
    }
}
