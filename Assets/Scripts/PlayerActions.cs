using System;
using Alteruna;
using UnityEngine;

public class PlayerActions : AttributesSync
{
    [SerializeField] private Alteruna.Avatar avatar;
    [SerializeField]private Spawner spawner;
    
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
    public SynchronizedProjectile deflectable = null;
    [SerializeField] private BoxCollider deflectArea;

    private void Awake()
    {
        spawner = FindObjectOfType<Spawner>();
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
            Shoot();
       
        if (Input.GetMouseButton(1))
        {
            if (deflectable)
            {
                //projectileManager.OnPlayerDeflectProjectile(deflectable.localId);
                Vector3 direction = transform.position + transform.forward;
                deflectable.OnDeflect(direction);
                //Deflect(deflectable);
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
    
    private void Shoot()
    {
        GameObject proj = spawner.Spawn(0, transform.position + transform.forward, transform.rotation);
        if (proj.TryGetComponent(out SynchronizedProjectile p))
        {
            p.playerIndex = avatar.Possessor.Index;
            p.spawner = spawner;
        }
    }
}
