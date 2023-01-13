using Alteruna;
using UnityEngine;

public class Attack : AttributesSync
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Alteruna.Avatar avatar;
    private void Update()
    {
        if (!avatar.IsMe)
            return;
        if (Input.GetMouseButtonDown(0))
            BroadcastRemoteMethod("Shoot");
    }
    
    [SynchronizableMethod]
    private void Shoot()
    {
        Instantiate(projectile, transform.position + transform.forward, transform.rotation);
    }
}
