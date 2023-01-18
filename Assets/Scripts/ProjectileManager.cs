using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using Alteruna.Trinity;
using UnityEngine;

public class ProjectileManager : AttributesSync
{
    [SerializeField ]private Alteruna.Avatar avatar;
    [SerializeField] private GameObject projectilePrefab;

    private Dictionary<int, GameObject> projectileDict;
    
    private void Start()
    {
        projectileDict = new Dictionary<int, GameObject>();
        
        Multiplayer.RegisterRemoteProcedure("SpawnProjectileRemote", SpawnProjectileRemote);
        Multiplayer.RegisterRemoteProcedure("OnPlayerDeflectProjectileRemote", OnPlayerDeflectProjectileRemote);
    }
    
    public void SpawnProjectileLocal(Vector3 spawnPos, Quaternion rotation)
    {
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, rotation);
        int id = projectile.GetInstanceID();
        projectileDict.Add(id, projectile);
        
        if (projectile.TryGetComponent(out Projectile proj))
            proj.localId = id;

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

        if (projectile.TryGetComponent(out Projectile proj))
            proj.localId = id;
        
        Debug.Log("SPAWN REMOTE, ID: " + id);
    }

    public void OnPlayerDeflectProjectile(int projectileId)
    {
        if (projectileDict.TryGetValue(projectileId, out var go))
        {
            var proj = go.GetComponent<Projectile>();
            proj.OnDeflect();
        }
        else
        {
            Debug.Log("LOCAL, PROJECTILE NOT FOUND IN DICT");
        }
        
        ProcedureParameters parameters = new ProcedureParameters();
        parameters.Set("projectileId", projectileId);
        Multiplayer.InvokeRemoteProcedure("OnPlayerDeflectProjectileRemote", UserId.All, parameters);
    }
    
    public void OnPlayerDeflectProjectileRemote(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        int projectileId = parameters.Get("projectileId", 0);
        
        if (projectileDict.TryGetValue(projectileId, out var go))
        {
            var proj = go.GetComponent<Projectile>();
            proj.OnDeflect();
        }
        else
        {
            Debug.Log("REMOTE, PROJECTILE NOT FOUND IN DICT");
        }
    }
}
