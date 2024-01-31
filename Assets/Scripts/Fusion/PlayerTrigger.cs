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
    [SerializeField] private float pushPower = 2.0f; // �ڽ��� �� ���� ��
    [SerializeField] NetworkTransform _rootBox;
    bool _isTele = false;
    bool _isPushBox = false;
    [SerializeField] public Rigidbody _pushedBody;
    [Networked]
    public Vector3 PushDirection { get; set; }
    private float _lastHitTime = 0f;
    private float _hitInterval = 0.1f; // 0.1�� �������� OnControllerColliderHit ȣ�� ���

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
            
            _pushedBody.AddForce(PushDirection * pushPower); // ���Ⱑ �� ������ �ִ� 
            _isPushBox = false;
            //_rootBox.FixedUpdateNetwork();
            _rootBox.TeleportToPosition(GameManager.instance.targetPosition.position);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (Time.time - _lastHitTime > _hitInterval)
        {
            _lastHitTime = Time.time; // ������ ȣ�� �ð��� ���� �ð����� ������Ʈ

            if (hit.gameObject.tag == "MoveBox")
            {
                _pushedBody = hit.collider.attachedRigidbody;
                _rootBox = hit.gameObject.GetComponent<NetworkTransform>();
                // �ڽ��� Rigidbody�� ���ų�, Kinematic�̸� ����
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

                _isPushBox = true; // ���� �����ؾ� ���� ��Ÿ���� �÷��� ����
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