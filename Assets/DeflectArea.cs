using UnityEngine;

public class DeflectArea : MonoBehaviour
{
    [SerializeField] private PlayerActions player;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent(out Projectile proj))
            return;

        player.deflectable = proj;
    }

    private void OnTriggerExit(Collider other)
    {
        player.deflectable = null;
    }
}
