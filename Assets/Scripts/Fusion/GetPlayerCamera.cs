using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Fusion;
using StarterAssets;

public class GetPlayerCamera : MonoBehaviour
{
    [SerializeField] Transform playerCameraRoot;
    private void Start()
    {
        NetworkObject thisObject = GetComponent<NetworkObject>();

        if (thisObject.HasStateAuthority)
        {
            GameObject playerCamera = GameObject.Find("PlayerFollowCamera");
            playerCamera.GetComponent<PlayerCamera>().target = playerCameraRoot.gameObject;

            //virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = playerCameraRoot;

            playerCamera.GetComponent<PlayerCamera>().enabled = true;

            GetComponent<Player>().enabled = true;
        }
    }
}
