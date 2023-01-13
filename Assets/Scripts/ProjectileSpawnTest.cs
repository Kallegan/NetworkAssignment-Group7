using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;

public class ProjectileSpawnTest : AttributesSync
{
    [SynchronizableMethod]
    private void Create()
    {
        Instantiate(gameObject, transform.position, transform.rotation);
    }
}
