using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPC : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] Transform tpcCameraController;
    [SerializeField] Transform cameraTransform;
    [SerializeField] float sensX;
    [SerializeField] float sensY;
    [SerializeField] float smoothTime; 
    [SerializeField] Vector3 defaultOffset;

    [Header("Collision Settings")]
    [SerializeField] LayerMask collisionMask;
    [SerializeField] float collisionRadius;
    [SerializeField] float minDistanc;
    [SerializeField] float collisionOffset;
    [SerializeField] float collisionMultiplier;
    [SerializeField] Vector3 collisionDetectionOffset;
    

    private bool Clipping;
    RaycastHit RayHit;

    private float xRot, yRot;
    private Vector3 smoothVelocity;
    Actions inputs;

    void Awake()
    {
        inputs = new Actions();
    }

    void OnEnable()
    {
        inputs.Enable();  
    }

    void Update()
    {
        CameraRotation();
        CameraCollision();
    }

    void CameraRotation(){
        float mouseX = inputs.Player.Mouse.ReadValue<Vector2>().x;
        float mouseY = inputs.Player.Mouse.ReadValue<Vector2>().y;

        xRot -= mouseY * sensY * Time.deltaTime;
        yRot += mouseX * sensX * Time.deltaTime;
        xRot = Mathf.Clamp(xRot, -90, 90);

        tpcCameraController.rotation = Quaternion.Euler(xRot, yRot, 0);
    }

    void CameraCollision(){
        Vector3 desiredPosisiton = tpcCameraController.TransformPoint(defaultOffset);
    Clipping = Physics.SphereCast(tpcCameraController.position + collisionDetectionOffset, collisionRadius,(desiredPosisiton - tpcCameraController.position - collisionDetectionOffset).normalized, out RaycastHit hit, defaultOffset.magnitude, collisionMask);
        if(Clipping)
        {
            desiredPosisiton = tpcCameraController.position + (desiredPosisiton - tpcCameraController.position).normalized * (hit.distance - collisionOffset);
            RayHit = hit;

        }

        if(Vector3.Distance(cameraTransform.position, tpcCameraController.position) < minDistanc + .1f){
            if(Clipping){
                if(hit.normal == Vector3.up){
                    //cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, GetPointOnCircle(tpcCameraController.position, hit.point, minDistanc) + GroundOffset(), ref smoothVelocity, smoothTime);
                    cameraTransform.position = GetPointOnCircle(tpcCameraController.position, hit.point, minDistanc) + GroundOffset();
                }
                else{
                    cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, desiredPosisiton + GroundOffset(), ref smoothVelocity, smoothTime);
                }
            }
            else{
                cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, desiredPosisiton, ref smoothVelocity, smoothTime);
            }
        }
        else if (Vector3.Distance(cameraTransform.position, tpcCameraController.position) > minDistanc) {
            cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, desiredPosisiton + GroundOffset(), ref smoothVelocity, smoothTime);
        }


    }

    private Vector3 GroundOffset(){
        return RayHit.normal * collisionMultiplier;
    }

    private Vector3 GetPointOnCircle(Vector3 center, Vector3 poin, float radius){
        Vector3 direction = poin - center;

        direction.Normalize();

        return center + direction * radius;
    }



    void OnDrawGizmos()
    {

    }
}
