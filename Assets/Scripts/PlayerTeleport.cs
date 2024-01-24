using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Globalization;
using StarterAssets;
using Cinemachine;

public class PlayerTeleport : NetworkBehaviour
{
    [SerializeField] NetworkTransform _root;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");
        if (other.CompareTag("TeleBox"))
        {
            // 모든 클라이언트에서 플레이어 이동 처리를 위해 RPC 호출
            _root.TeleportToPosition(GameManager.instance.targetPosition.position);
        }
        else
        {
            Debug.Log("Not TeleBox");
        }
    }
    
}
