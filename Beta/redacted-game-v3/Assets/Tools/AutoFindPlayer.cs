using Cinemachine;
using UnityEngine;

public class AutoFindPlayer : MonoBehaviour
{
    void Start()
    {
        CinemachineVirtualCamera virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCamera.Follow = FindObjectOfType<PlayerMovement>().transform;
    }
}
