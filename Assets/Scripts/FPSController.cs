using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))] //this is a great thing to show people as it shows them how to make sure components will set up on objects.
public class FPSController : MonoBehaviour
{
    private float _xRotation;
    private CharacterController _characterController;

    #region Mouse look
    [SerializeField] private float _mouseSensitivity = 200f;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _xCameraBounds = 60f;
    #endregion

    #region Smoothing
    private Vector2 _currentMouseDelta;
    private Vector2 _currentMouseVelocity;
    [SerializeField] private float _smoothTime = .1f;
    #endregion

    #region Movement
    public Vector3 Velocity;
    [SerializeField] public float JumpStrength = 7.0F;
    [SerializeField] public float WalkSpeed = 5.0F;
    #endregion

    public void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Update()
    {
        Movement();
        Rotation();
    }

    private void Movement() //had a call with Mason where he helped me figure out all the movement issues
    {
        Vector3 moveVector;

        moveVector = transform.forward * Input.GetAxisRaw("Vertical");
        moveVector += transform.right * Input.GetAxisRaw("Horizontal");
        moveVector.Normalize();
        moveVector *= WalkSpeed;
        Velocity.x = moveVector.x;
        Velocity.z = moveVector.z;
        Velocity.y -= 0.1F;

        Vector3 fudge = Vector3.zero;

        if (!_characterController.isGrounded)
        {
            fudge = Vector3.down * 0.01F;
        }

        _characterController.Move(Velocity * Time.deltaTime + fudge);

        if (_characterController.isGrounded)
        {
            Velocity.y = 0;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Velocity.y = JumpStrength;
            }
        }
    }

    private void Rotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;
        Vector2 targetDelta = new Vector2(mouseX, mouseY);
        _currentMouseDelta = Vector2.SmoothDamp(_currentMouseDelta, targetDelta, ref _currentMouseVelocity, _smoothTime);
        _xRotation -= _currentMouseDelta.y;
        _xRotation = Mathf.Clamp(_xRotation, -_xCameraBounds, _xCameraBounds);
        transform.Rotate(Vector3.up * _currentMouseDelta.x);
        _camera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }
}