using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public Vector3 mouseposition;
    public float fire;

    #region Singleton
    public static InputHandler Instance { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance != null && Instance != this) Destroy(Instance);
        else Instance = this;
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        fire = Input.GetAxis("Fire1");
        mouseposition = Input.mousePosition;
    }
}
