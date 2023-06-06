using UnityEngine;

public class TeleportComponent : MonoBehaviour
{
    [SerializeField] private Transform _destination;

    public void Teleport(GameObject targetToTeleport)
    {
        targetToTeleport.transform.position = _destination.position;
    }
}
