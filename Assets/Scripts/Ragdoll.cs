using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class Ragdoll : MonoBehaviour
{
    Actions actions;
    [SerializeField] Transform _ragdoll;

    [SerializeField] Rigidbody playersRigidbody;

    [SerializeField] RigBuilder rigBuilder;
    [SerializeField] Animator animator;
    [SerializeField] Collider[] playersColliders;

    [SerializeField] private Rigidbody[] rigidbodies;
    [SerializeField] private Collider[] colliders;

    private bool ragdollEnabled = true;

    void Awake()
    {
      actions = new Actions();
    }

    void OnEnable()
    {
        actions.Player.Debug1.performed += Toggle;
        actions.Enable();
    }

    void OnDisable()
    {
        actions.Player.Debug1.performed -= Toggle;
        actions.Disable();  
    }
    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();

        Toggle(new InputAction.CallbackContext());
    }

    private void Toggle(InputAction.CallbackContext context){
        if(ragdollEnabled){
            foreach(Rigidbody rigidbody in rigidbodies){
                rigidbody.isKinematic = true;
                rigidbody.useGravity = false;
                rigidbody.detectCollisions = false;
            }
            foreach(Collider collider in colliders){
                collider.enabled = false;
            }
            foreach(Collider collider1 in playersColliders){
                collider1.enabled = true;
            }
            playersRigidbody.isKinematic = false;
            playersRigidbody.detectCollisions = true;
            rigBuilder.enabled = true;
            animator.enabled = true;
            ragdollEnabled = false;
        }
        else{
            foreach(Rigidbody rigidbody in rigidbodies){
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
                rigidbody.detectCollisions = true;
            }
            foreach(Collider collider in colliders){
                collider.enabled = true;
            }
            foreach(Collider collider1 in playersColliders){
                collider1.enabled = false;
            }
            playersRigidbody.isKinematic = true;
            playersRigidbody.detectCollisions = false;
            rigBuilder.enabled = false;
            animator.enabled = false;
            ragdollEnabled = true;
        }
    }

}
