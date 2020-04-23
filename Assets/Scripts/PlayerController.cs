using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Handling
    public float rotationSpeed = 450; // 360 = 1 rev/sec
    public float walkSpeed = 5;
    public float runSpeed = 8;
    private float acceleration = 5;

    // System
    private Quaternion targetRotation;
    private Vector3 currentVelocityMod;
    
    // Components
    public Gun gun;
    private CharacterController controller;
    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ControlMouse();

        if (Input.GetButtonDown("Shoot")) {
            gun.Shoot();
        } else if (Input.GetButton("Shoot")) {
            gun.ShootContinuous();
        }
    }

    void ControlMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, camera.transform.position.y - transform.position.y));
        targetRotation = Quaternion.LookRotation(mousePos - new Vector3(transform.position.x, 0, transform.position.z));
        transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        currentVelocityMod = Vector3.MoveTowards(currentVelocityMod, input, acceleration * Time.deltaTime);
        Vector3 motion = currentVelocityMod;
        motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1) ? .7f : 1;
        motion *= (Input.GetButton("Run")) ? runSpeed : walkSpeed;
        motion += Vector3.up * -8;

        controller.Move(motion * Time.deltaTime);
    }

    void ControlWASD()
    {

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (input != Vector3.zero) {
            targetRotation = Quaternion.LookRotation(input);
            transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
        }

        Vector3 motion = input;
        motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1) ? .7f : 1;
        motion *= (Input.GetButton("Run")) ? runSpeed : walkSpeed;
        motion += Vector3.up * -8;

        controller.Move(motion * Time.deltaTime);
    }
}
