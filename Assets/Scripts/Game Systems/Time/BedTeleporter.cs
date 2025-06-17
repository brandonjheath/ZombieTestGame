using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class BedTeleporter : MonoBehaviour
{
    public Transform bedSpawnPoint;
    private CharacterController cc;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        TimeManager.OnDayStart += TeleportToBed;
    }

    private void OnDisable()
    {
        TimeManager.OnDayStart -= TeleportToBed;
    }

    private void TeleportToBed()
    {
        if (cc != null)
        {
            cc.enabled = false;
            transform.SetPositionAndRotation(bedSpawnPoint.position, bedSpawnPoint.rotation);
            StartCoroutine(ReenableControllerNextFrame());
        }
        else
        {
            transform.SetPositionAndRotation(bedSpawnPoint.position, bedSpawnPoint.rotation);
        }
    }

    private IEnumerator ReenableControllerNextFrame()
    {
        // Wait one frame (or you can use WaitForSeconds(0.01f) if you prefer)
        yield return null;
        cc.enabled = true;
    }
}
