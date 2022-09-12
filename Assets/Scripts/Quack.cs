using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Quack : MonoBehaviour
{
    public GameObject echo;

    private InputHandler inputHandler;
    private float distance;
    private float buildUpTime;
    private float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        inputHandler = InputHandler.Instance;
        distance = 1;
        buildUpTime = 0.05f;
        cooldown = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
       if(cooldown > 0) cooldown -= Time.deltaTime;
    }

    public void quack()
    {
        if (inputHandler.fire != 0 && cooldown < 0)
        {
            GetComponent<AudioSource>().Play();
            StartCoroutine(Echo());
            cooldown = 1.5f;
        }
    }

    private IEnumerator Echo()
    {
        // Create echos infront of player
        GameObject[] echos = new GameObject[3];
    
        for(int i = 0; i < echos.Length; i++)
        {
            yield return new WaitForSeconds(buildUpTime);
            echos[i] = Instantiate(echo);
            echos[i].transform.position = transform.position + transform.forward * distance * (i + 1) + Vector3.up * 0.5f;
            echos[i].transform.localScale = transform.localScale * (i + 1);
            echos[i].transform.rotation = transform.rotation * Quaternion.Euler(0, 90, 0);
        }

        // Destroy echos after short time
        yield return new WaitForSeconds(0.2f);

        for(int i = 0; i < echos.Length; i++)
        {
            yield return new WaitForSeconds(buildUpTime);
            Destroy(echos[i]);
        }

        yield break;
    }
}
