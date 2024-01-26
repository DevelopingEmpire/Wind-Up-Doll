using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Globalization;
using StarterAssets;
using Cinemachine;

public class PlayerTrigger : NetworkBehaviour
{
    [SerializeField] NetworkTransform _root;
    bool _isTele = false;

    public override void FixedUpdateNetwork() // RPC ���°� �ƴ϶� �̵��� FixedUpdateNetwork���� ó��
    {
        if (_isTele)
        {
            _root.TeleportToPosition(GameManager.instance.targetPosition.position);
            _isTele = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");
        if (other.CompareTag("TeleBox"))
        {
            _isTele = true;
        }
        else
        {
            Debug.Log("Not TeleBox");
            Debug.Log(other.gameObject);
        }
    }

}
