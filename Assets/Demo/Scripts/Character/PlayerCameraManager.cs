using Unity.Cinemachine;
using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineCamera _thirdPersonCamera;
    [SerializeField]
    private CinemachineCamera _lockCamera;

    private CinemachineOrbitalFollow _orbitalFollow;

    private void Start()
    {
        _orbitalFollow = _thirdPersonCamera.GetComponent<CinemachineOrbitalFollow>();
        SwitchToThirdPersonCamera();
    }

    public void SwitchToThirdPersonCamera()
    {
        _orbitalFollow.HorizontalAxis.Value = transform.eulerAngles.y;
        _thirdPersonCamera.Priority.Value = 1;
        _lockCamera.Priority.Value = 0;
    }

    public void SwitchToLockCamera()
    {
        _thirdPersonCamera.Priority.Value = 0;
        _lockCamera.Priority.Value = 1;
    }
}
