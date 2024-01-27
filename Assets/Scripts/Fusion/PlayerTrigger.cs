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
    [SerializeField] private float pushPower = 2.0f; // 박스를 밀 때의 힘
    bool _isTele = false;
    bool _isPushBox = false;


    public override void FixedUpdateNetwork() // RPC 쓰는게 아니라 이동은 FixedUpdateNetwork에서 처리
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
            // 박스에 Rigidbody가 없거나, Kinematic이면 무시
            if (body == null || body.isKinematic)
            {
                return;
            }
            // 박스를 밀어내는 방향 계산
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

            // 박스에 힘을 추가하여 밀어냄
            body.velocity = pushDir * pushPower;
        }
    }
}

