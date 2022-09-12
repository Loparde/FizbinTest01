using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2ndPrsn : MonoBehaviour
{
    public float distance = 5.0f; 
    private Vector3 direction;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        direction = new Vector3(1, 1, 1);
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        cam.transform.position = transform.position + direction * distance;
        cam.transform.LookAt(transform.position);
    }
}
