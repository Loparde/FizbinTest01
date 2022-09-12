using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMovement : MonoBehaviour
{
    public float maxSpeed;
    public float acceleration;

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
    }

    // Update is called once per frame
    void Update()
    {
        handleRotation();
        moving();
    }

    private void handleRotation()
    {
        // Get MousePoint on Map
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 500.0f))
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
            speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * acceleration);
            //if (speed < maxSpeed)
            //{
            //    speed = speed + acceleration;
            //}
            remainingInput = input;
        } else
        {
            speed = Mathf.Lerp(speed, 0, Time.deltaTime * acceleration);
            //if (speed > 0)
            //{
            //    speed = speed - acceleration;
            //}
            input = remainingInput;
        }
        input = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * input;
        rb.MovePosition(transform.position + input * speed);
    }
}
