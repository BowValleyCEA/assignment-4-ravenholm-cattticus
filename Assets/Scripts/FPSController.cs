using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))] //this is a great thing to show people as it shows them how to make sure components will set up on objects.
public class FPSController : MonoBehaviour
{
    private float _xRotation;
    private Vector3 _moveVector;
    private Vector3 _jumpVector;
    private float gravity;
    private CharacterController _controller;

    [SerializeField] private float mouseSensitivity = 200f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Camera camera;
    [SerializeField] private float xCameraBounds = 60f;

    
    
    
    #region Smoothing code
    private Vector2 _currentMouseDelta;
    private Vector2 _currentMouseVelocity;
    [SerializeField] private float smoothTime = .1f;
    
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Rotation();
    }

    private void Movement()
    {
        bool wasOnGround = _controller.isGrounded; 
        bool isActuallyOnGround = false; 

       
      
        _moveVector = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal"); //easier to explain after by using the forward and right vectors
        _moveVector.Normalize();
        _moveVector *= speed;
        _moveVector.y -= gravity;
        _moveVector *= Time.deltaTime;
       
        gravity += 9.81f;
        if (_controller.isGrounded)
        {
            gravity = 0f;
            _moveVector.y = -0.01f; //forces character controller to recognize player is on the ground (shoutout mason)
        }

        _controller.Move(_moveVector );
        
        isActuallyOnGround = wasOnGround || _controller.isGrounded;

        if (Input.GetKeyDown(KeyCode.Space) && isActuallyOnGround)
        {
            _moveVector.y += 6;
        }


    }

    private void Jump()
    {
       
    }
    private void Rotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        Vector2 targetDelta = new Vector2(mouseX, mouseY);
        _currentMouseDelta = Vector2.SmoothDamp(_currentMouseDelta, targetDelta, ref _currentMouseVelocity, smoothTime);
        _xRotation -= _currentMouseDelta.y;
        _xRotation = Mathf.Clamp(_xRotation, -xCameraBounds, xCameraBounds);
        transform.Rotate(Vector3.up * _currentMouseDelta.x);
        camera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }

    private void LateUpdate()
    {

        
    }
}
