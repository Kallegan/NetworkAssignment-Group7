using UnityEngine;

public class DeflectArea : MonoBehaviour
{
    [SerializeField] private PlayerActions player;
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter: " +other);
        
        if (!other.gameObject.TryGetComponent(out Projectile proj))
            return;

        // if (!player.deflectable)
            player.deflectable = proj;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit: " +other);
        player.deflectable = null;
    }
}
