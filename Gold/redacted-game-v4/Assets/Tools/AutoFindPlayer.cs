using Cinemachine;
using UnityEngine;

public class AutoFindPlayer : MonoBehaviour
{
    [SerializeField] private PlayerSettingsScriptableObject playerSettings;
    void Start()
    {
        CinemachineVirtualCamera virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCamera.Follow = FindObjectOfType<PlayerMovement>().transform;
    }
}
