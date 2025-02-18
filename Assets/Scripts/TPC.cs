using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPC : MonoBehaviour
{
    [SerializeField] Transform tpcCameraController;
    [SerializeField] float sensX, sensY, xRot, yRot, mouseX, mouseY;

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
        mouseX = inputs.Player.Mouse.ReadValue<Vector2>().x;
        mouseY = inputs.Player.Mouse.ReadValue<Vector2>().y;

        xRot -= mouseY * sensY * Time.deltaTime;
        yRot += mouseX * sensX * Time.deltaTime;

        xRot = Mathf.Clamp(xRot, -90, 90);

        tpcCameraController.rotation = Quaternion.Euler(xRot, yRot, 0);


    }
}
