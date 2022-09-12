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
    private Camera cam;
    private Vector3 target;
    private Rigidbody rb;
    private Vector3 remainingInput;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        speed = 0;
        rb = GetComponent<Rigidbody>();
        acceleration = maxSpeed / timeToMax;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        handleRotation();
        moving();
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
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

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
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        Vector3 input = new Vector3(hor, 0, ver);

        if(hor != 0 || ver != 0)
        {
            if (speed < maxSpeed) speed += acceleration * Time.deltaTime;
            remainingInput = input;
        } else
        {
            if (speed > 0)
            {
                speed -= acceleration * Time.deltaTime;
                input = remainingInput;
            }
        }
        if (globalMovement) input = Quaternion.Euler(0, 225, 0) * input;
        else input = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * input;
        walldetection(input);
        rb.MovePosition(transform.position + input * speed);
    }

    private void walldetection(Vector3 direction)
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, direction, out hit, 0.5f))
        {
            if (hit.collider.gameObject.layer == 7) speed = 0.05f; // acceleration * Time.deltaTime;
        }
    }
}
