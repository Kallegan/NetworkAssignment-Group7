using UnityEngine;

public class DeflectArea : MonoBehaviour
{
    [SerializeField] private PlayerActions player;
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter: " +other);
        
        if (!other.gameObject.TryGetComponent(out SynchronizedProjectile proj))
            return;

        // if (!player.deflectable)
        //Debug.Log("PLAYER CUR DEFLECTABLE ID:" + proj.localId);

            player.curDeflectable = proj;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit: " +other);
        player.curDeflectable = null;
    }
}
