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
            // 박스에 Rigidbody가 없거나, Kinematic이면 무시
            if (_pushedBody == null || _pushedBody.isKinematic)
            {
                return;
            }
            // 박스를 밀어내는 방향 계산
            _pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            _isPushBox = true; // 힘을 적용해야 함을 나타내는 플래그 설정
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

