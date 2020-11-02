using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public CinemachineVirtualCamera mainCam;
    // Start is called before the first frame update
    void Start()
    {
        //mainCam = GameObject.FindWithTag("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        //mainCam.LookAt = transform;
        //mainCam.Follow = transform;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        transform.position = new Vector3(
            transform.position.x + x * moveSpeed * Time.deltaTime, 
            transform.position.y, 
            transform.position.z + z * moveSpeed * Time.deltaTime);
    }
}
