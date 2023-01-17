using System;
using Alteruna;
using UnityEngine;

public class PlayerActions : AttributesSync
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Alteruna.Avatar avatar;

    private ProjectileManager projectileManager;
    
    // Attack
    [SerializeField] private float attackCoolDown = 0.5f;
    private bool canAttack = true;
    private float curAttackCoolDown;
    
    
    // Deflect
    [Range(0, 10)]
    [SerializeField] private float deflectRange = 2f;
    [Range(0, 5)]
    [SerializeField] private float deflectRadius = 1f;
    [SerializeField] private float deflectCoolDown = 0.5f;
    
    private float curDeflectCoolDown;
    private bool canDeflect = true;
    public Projectile deflectable = null;
    [SerializeField] private BoxCollider deflectArea;

    private void Awake()
    {
        projectileManager = FindObjectOfType<ProjectileManager>();
    }

    private void Start()
    {
        curAttackCoolDown = attackCoolDown;
        curDeflectCoolDown = deflectCoolDown;
    }
    
    private void Update()
    {
        if (!avatar.IsMe)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            projectileManager.SpawnProjectileLocal(transform.position + transform.forward);
        }
            //OnAction();
        if (Input.GetMouseButtonDown(1))
        {
            if (deflectable)
            {
                Deflect(deflectable);
                deflectable = null;
            }
                
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
    private void OnAction()
    {
        if (canAttack)
            BroadcastRemoteMethod("Shoot");
    }
    
    
    [SynchronizableMethod]
    private void Shoot()
    {
        GameObject proj = Instantiate(projectile, transform.position + transform.forward, transform.rotation);
        if (proj.TryGetComponent(out Projectile p))
            p.playerIndex = avatar.Possessor.Index;
    }
    
    private void Deflect(Projectile proj)
    {
        // debug when sending and re.. 
        float x = transform.forward.x;
        float y = transform.forward.y;
        float z = transform.forward.z;
        
        proj.BroadcastRemoteMethod("OnDeflect", x, y, z);
        //proj.BroadcastRemoteMethod("Destroy");
    }
}
