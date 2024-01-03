using Fusion;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    private CharacterController _cc;
    public float playerSpeed = 4f;

    private Vector3 _velocity;
    private bool jumpPressed;

    public float jumpForce = 5f;
    public float gravityValue = -9.81f;

    public Camera _camera;

    void Awake()
    {
        _cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        }
    }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            _camera = Camera.main;
            _camera.GetComponent<FirstPersonCamera>().target = transform;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority == false) return;
        if (_cc.isGrounded)
        {
            _velocity = new Vector3(0, -1, 0);
        }

        var cameraRotationY = Quaternion.Euler(0, _camera.transform.rotation.eulerAngles.y, 0);
        Vector3 move = cameraRotationY * new Vector3(Input.GetAxis("Horizontal"), 0, 
            Input.GetAxis("Vertical")) * Runner.DeltaTime * playerSpeed;

        _velocity.y += gravityValue * Runner.DeltaTime;
        if(jumpPressed && _cc.isGrounded)
        {
            _velocity.y += jumpForce;
        }

        _cc.Move(move + _velocity * Runner.DeltaTime);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        jumpPressed = false;
    }
}