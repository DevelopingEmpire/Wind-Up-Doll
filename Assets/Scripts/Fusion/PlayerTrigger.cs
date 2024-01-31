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
    [SerializeField] NetworkCharacterControllerPrototype characterControllerPrototype;
    [SerializeField] NetworkTransform _root;
    [SerializeField] private float pushPower = 2.0f; // 박스를 밀 때의 힘
    [SerializeField] NetworkTransform _rootBox;
    bool _isTele = false;
    bool _isPushBox = false;
    [SerializeField] public Rigidbody _pushedBody;
    [Networked]
    public Vector3 PushDirection { get; set; }
    private float _lastHitTime = 0f;
    private float _hitInterval = 0.1f; // 0.1초 간격으로 OnControllerColliderHit 호출 허용

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (_isTele)
        {
            characterControllerPrototype.TeleportToPosition(GameManager.instance.targetPosition.position);
            _isTele = false;
        }

        if (_isPushBox && _pushedBody != null && _rootBox != null)
        {
            
            _pushedBody.AddForce(PushDirection * pushPower); // 여기가 좀 문제가 있다 
            _isPushBox = false;
            //_rootBox.FixedUpdateNetwork();
            _rootBox.TeleportToPosition(GameManager.instance.targetPosition.position);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (Time.time - _lastHitTime > _hitInterval)
        {
            _lastHitTime = Time.time; // 마지막 호출 시간을 현재 시간으로 업데이트

            if (hit.gameObject.tag == "MoveBox")
            {
                _pushedBody = hit.collider.attachedRigidbody;
                _rootBox = hit.gameObject.GetComponent<NetworkTransform>();
                // 박스에 Rigidbody가 없거나, Kinematic이면 무시
                if (_pushedBody == null || _pushedBody.isKinematic)
                {
                    return;
                }


                // Determine the dominant direction between x and z
                if (Mathf.Abs(hit.moveDirection.x) > Mathf.Abs(hit.moveDirection.z))
                {
                    // If x is more dominant, set z to 0
                    PushDirection = new Vector3(hit.moveDirection.x, 0, 0).normalized;
                }
                else
                {
                    // If z is more dominant or equal, set x to 0
                    PushDirection = new Vector3(0, 0, hit.moveDirection.z).normalized;
                }

                _isPushBox = true; // 힘을 적용해야 함을 나타내는 플래그 설정
            }
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