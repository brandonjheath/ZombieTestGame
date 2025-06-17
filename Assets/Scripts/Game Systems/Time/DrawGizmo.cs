using UnityEngine;

public class DrawSpawnGizmo : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}
