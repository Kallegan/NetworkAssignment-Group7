using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;

public class ProjectileMoveMartin : Synchronizable
{
    public Vector3 SynchronizedPosition;
    private Vector3 _oldSynchronizedPosition;
        
    void Update()
    {
        transform.position += transform.forward * (Time.deltaTime * 20);
        SynchronizedPosition = transform.position;
        
        // If the value of our float has changed, sync it with the other players in our playroom.
        if (SynchronizedPosition != _oldSynchronizedPosition)
        {
            // Store the updated value
            _oldSynchronizedPosition = SynchronizedPosition;

            // Tell Alteruna that we want to commit our data.
            Commit();
        }

        // Update the Synchronizable
        base.SyncUpdate();
        
    }

    public override void DisassembleData(Reader reader, byte LOD)
    {
        // Set our data to the updated value we have recieved from another player.
        SynchronizedPosition = reader.ReadVector3();

        // Save the new data as our old data, otherwise we will immediatly think it changed again.
        _oldSynchronizedPosition = SynchronizedPosition;
    }

    public override void AssembleData(Writer writer, byte LOD)
    {
        // Write our data so that it can be sent to the other players in our playroom.
        writer.Write(SynchronizedPosition);
    }
    
    void OnBecameInvisible() 
    {
        Destroy(gameObject);
    }
}
