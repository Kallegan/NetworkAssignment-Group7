using Alteruna;
using UnityEngine;

public class PlayerActions : AttributesSync
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Alteruna.Avatar avatar;
    
    [SerializeField] private float coolDown = 0.5f;
    private bool canAttack = true;
    private float curCoolDown;

    private void Start()
    {
        curCoolDown = coolDown;
    }
    
    private void Update()
    {
        if (!avatar.IsMe)
            return;


        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            BroadcastRemoteMethod("Shoot");
            canAttack = false;
        }

        // Todo: clean up
        if (!canAttack)
        {
            curCoolDown -= Time.deltaTime;
            if (curCoolDown <= 0)
            {
                canAttack = true;
                curCoolDown = coolDown;
            }
        }
    }
    
    
    [SynchronizableMethod]
    private void Shoot()
    {
        GameObject proj = Instantiate(projectile, transform.position + transform.forward, transform.rotation);
        if (proj.TryGetComponent(out Projectile p))
            p.playerIndex = avatar.Possessor.Index;
    }
    
    [SynchronizableMethod]
    private void Deflect()
    {
        // 
    }
}
