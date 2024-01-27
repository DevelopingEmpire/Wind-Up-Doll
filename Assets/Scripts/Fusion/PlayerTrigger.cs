using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Globalization;
using StarterAssets;
using Cinemachine;
using static Fusion.NetworkCharacterController;

public class PlayerTrigger : NetworkBehaviour
{
    [SerializeField] NetworkTransform _root;
    [SerializeField] private float pushPower = 2.0f; // �ڽ��� �� ���� ��
    bool _isTele = false;
    bool _isPushBox = false;


    public override void FixedUpdateNetwork() // RPC ���°� �ƴ϶� �̵��� FixedUpdateNetwork���� ó��
    {
        if (_isTele)
        {
            _root.TeleportToPosition(GameManager.instance.targetPosition.position);
            _isTele = false;
        }
        if (_isPushBox)
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TeleBox"))
        {
            _isTele = true;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "MoveBox")
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            // �ڽ��� Rigidbody�� ���ų�, Kinematic�̸� ����
            if (body == null || body.isKinematic)
            {
                return;
            }
            // �ڽ��� �о�� ���� ���
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

            // �ڽ��� ���� �߰��Ͽ� �о
            body.velocity = pushDir * pushPower;
        }
    }
}

