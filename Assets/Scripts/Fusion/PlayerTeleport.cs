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
            // ��� Ŭ���̾�Ʈ���� �÷��̾� �̵� ó���� ���� RPC ȣ��
            _root.TeleportToPosition(GameManager.instance.targetPosition.position);
        }
        else
        {
            Debug.Log("Not TeleBox");
        }
    }
    
}
