using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class ItemBehaviour : NetworkBehaviour
{
    public bool destroyOnUse = true;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color32(255, 0, 0, 196);
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color32(255, 0, 0, 196);
        Gizmos.DrawCube(transform.position, Vector3.one);
    }
}
