using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMovement : MonoBehaviour
{
    public float maxSpeed;
    public float timeToMax;
    public bool globalMovement;

    private float acceleration;
    private float speed;
    private Rigidbody rb;
    private Vector3 remainingInput;
    private InputHandler inputHandler;

    private Camera cam;
    private Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        speed = 0;
        rb = GetComponent<Rigidbody>();
        acceleration = maxSpeed / timeToMax;
        inputHandler = InputHandler.Instance;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        handleRotation();
        moving();
        quack();
    }

    // Seems not to work: i wanted to prevent the character from glitching through walls
    //private void OnCollisionStay(Collision collision)
    //{
    //    if(collision.gameObject.layer == 7)
    //    {
    //        rb.velocity = transform.forward * -0.5f;
    //    }
    //}

    private void handleRotation()
    {
        // Get MousePoint on Map
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(inputHandler.mouseposition);

        if (Physics.Raycast(ray, out hit, 500.0f, LayerMask.GetMask("Ground")))
        {
            target = hit.point;
        }

        // Rotate Player towards Mouse
        transform.LookAt(target);
        transform.localRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    private void moving()
    {
        // Get Player Input
        Vector3 input = new Vector3(inputHandler.horizontal, 0, inputHandler.vertical);

        if(input != Vector3.zero)
        {
            // Accelerate and save latest Input
            if (speed < maxSpeed) speed += acceleration * Time.deltaTime;
            remainingInput = input;
        } else
        {
            // Decelerate in the direction of latest saved Input
            if (speed > 0)
            {
                speed -= acceleration * Time.deltaTime;
                input = remainingInput;
            }
        }

        // Switching between two kinds of Movements
        if (globalMovement) input = Quaternion.Euler(0, 225, 0) * input;
        else input = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * input;

        // Slowing down before colliding with walls
        walldetection(input);

        // Move Player
        rb.MovePosition(transform.position + input * speed);
    }

    private void walldetection(Vector3 direction)
    {
        // Stop Player from going too fast for unity detection
        RaycastHit hit;
        if(Physics.Raycast(transform.position, direction, out hit, 0.5f))
        {
            if (hit.collider.gameObject.layer == 7) speed = 0.05f;
        }
    }

    private void quack()
    {
        if(inputHandler.fire != 0)
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
