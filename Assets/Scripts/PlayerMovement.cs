using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Actions inputActions;
    float _walkingSpeed, _sprintSpeed, _jumpHeight, _health, _groundDistance;

    private float currentSpeed;
    
    RaycastHit hit;
    Rigidbody rb;

    [SerializeField] PlayerData playerData;

    private Vector2 inputDirection;
    private Vector3 movementDirection;

    [SerializeField] Transform groundCheck, orientation, mainCamera;
    [SerializeField] LayerMask ground;


    public bool Grounded(){
        Ray gRay = new Ray(groundCheck.position, Vector3.down);
        if(Physics.Raycast(gRay, out hit, _groundDistance, ground)){
            return true;
        }
        else{
            return false;
        }

    }
    void Awake()
    {
        inputActions = new Actions();
        rb = GetComponent<Rigidbody>();

        _walkingSpeed = playerData.walkingSpeed;
        _sprintSpeed = playerData.sprintSpeed;
        _jumpHeight = playerData.jumpHeight;
        _health = playerData.health;
        _groundDistance = playerData.groundDistance;
    }

    void Start()
    {
        hit = new RaycastHit();
        currentSpeed = _walkingSpeed;  
        Cursor.lockState = CursorLockMode.Locked;
    }
    void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Sprint.started += Sprint;
        inputActions.Player.Sprint.canceled += Sprint; 
        inputActions.Player.Jump.performed  += Jump;

    }

    private void Update() {
        CalculateMovement();

        orientation.rotation = Quaternion.Euler(orientation.eulerAngles.x, mainCamera.eulerAngles.y, orientation.eulerAngles.z);  

        if(inputDirection.magnitude > 0){
            this.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, orientation.eulerAngles.y, transform.eulerAngles.z), 10 * Time.deltaTime);
        }
        RaycastHit raycastHit = new RaycastHit();
        Physics.Raycast(groundCheck.position, Vector3.down, out raycastHit);
    }

    void CalculateMovement(){
        inputDirection = inputActions.Player.Movement.ReadValue<Vector2>();
        movementDirection = orientation.right * inputDirection.x + orientation.forward * inputDirection.y;
    }

    void Sprint(InputAction.CallbackContext context){
        if(context.started){
            currentSpeed = _sprintSpeed;
        }
        else if(context.canceled){
            currentSpeed = _walkingSpeed;
        }
    }

    void Jump(InputAction.CallbackContext context){
        if(!Grounded())return;
        rb.AddForce(Vector3.up * _jumpHeight , ForceMode.Impulse);
    }


    private void FixedUpdate() {
        Movement();
    }

    void Movement(){
        rb.velocity = new Vector3(movementDirection.x * currentSpeed * 100 * Time.fixedDeltaTime, rb.velocity.y -2f, movementDirection.z * currentSpeed * 100 * Time.fixedDeltaTime);
    }
}