using UnityEngine;

public class DeflectArea : MonoBehaviour
{ 
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent(out Projectile proj))
            return;

        transform.parent.GetComponent<PlayerActions>().deflectable = proj;
    }

    private void OnTriggerExit(Collider other)
    {
        transform.parent.GetComponent<PlayerActions>().deflectable = null;
    }
}
