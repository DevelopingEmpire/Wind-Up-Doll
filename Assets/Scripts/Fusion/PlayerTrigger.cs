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
    NetworkTransform _rootBox;
    bool _isTele = false;
    bool _isPushBox = false;
    Rigidbody _pushedBody;
    Vector3 _pushDirection;

    public override void FixedUpdateNetwork()
    {
        if (_isTele)
        {
            _root.TeleportToPosition(GameManager.instance.targetPosition.position);
            _isTele = false;
        }

        if (_isPushBox && _pushedBody != null)
        {
            _pushedBody.velocity = _pushDirection * pushPower;
            _isPushBox = false; 
            _rootBox.FixedUpdateNetwork();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "MoveBox")
        {
            _pushedBody = hit.collider.attachedRigidbody;
            _rootBox = hit.gameObject.GetComponent<NetworkTransform>();
            // �ڽ��� Rigidbody�� ���ų�, Kinematic�̸� ����
            if (_pushedBody == null || _pushedBody.isKinematic)
            {
                return;
            }
            // �ڽ��� �о�� ���� ���
            _pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            _isPushBox = true; // ���� �����ؾ� ���� ��Ÿ���� �÷��� ����
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TeleBox"))
        {
            _isTele = true;
        }
    }

    
}

