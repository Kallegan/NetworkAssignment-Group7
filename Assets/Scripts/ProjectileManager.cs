using System.Collections;
using System.Collections.Generic;
using Alteruna;
using Alteruna.Trinity;
using UnityEngine;

public class ProjectileManager : AttributesSync
{
    [SerializeField ]private Alteruna.Avatar avatar;
    [SerializeField] private GameObject projectilePrefab;

    private Dictionary<int, GameObject> projectileDict = new Dictionary<int, GameObject>();
    
    private void Start()
    {
        Multiplayer.RegisterRemoteProcedure("SpawnProjectileRemote", SpawnProjectileRemote);
    }
    
    public void SpawnProjectileLocal(Vector3 spawnPos)
    {
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        int id = projectile.GetInstanceID();
        projectileDict.Add(id, projectile);
        
        ProcedureParameters parameters = new ProcedureParameters();
        
        parameters.Set("id", id);
        parameters.Set("spawnPosX", spawnPos.x);
        parameters.Set("spawnPosZ", spawnPos.z);
        
        Multiplayer.InvokeRemoteProcedure("SpawnProjectileRemote", UserId.All, parameters);
        
        Debug.Log("SPAWN LOCAL, ID: " + id);
    }
    
    void SpawnProjectileRemote(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        float posX = parameters.Get("spawnPosX", 0);
        float posZ = parameters.Get("spawnPosZ", 0);
        int id = parameters.Get("id", 0);

        Vector3 spawnPos = new Vector3(posX, 0, posZ);
        
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        projectileDict.Add(id, projectile);
        
        Debug.Log("SPAWN REMOTE, ID: " + id);
    }
}
