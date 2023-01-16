using System;
using Alteruna;
using UnityEngine;

public class PlayerActions : AttributesSync
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Alteruna.Avatar avatar;
    
    // Attack
    [SerializeField] private float attackCoolDown = 0.5f;
    private bool canAttack = true;
    private float curAttackCoolDown;
    
    
    // Deflect
    [SerializeField] private float deflectRange = 2f;
    [SerializeField] private float deflectRadius = 5f;
    [SerializeField] private float deflectCoolDown = 0.5f;
    private float curDeflectCoolDown;
    private bool canDeflect = true;
    
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
            OnAction();
        
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
        if (canDeflect)
        {
            if (Physics.SphereCast(transform.position, deflectRadius, transform.forward, out var hit, deflectRange))
            {
                if (hit.collider.gameObject.TryGetComponent(out Projectile p))
                {
                    Deflect(p);
                    return;
                }
            }
        }
        
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
        proj.BroadcastRemoteMethod("OnDeflect", transform.forward);
    }
}
