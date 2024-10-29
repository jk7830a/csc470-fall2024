using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code inspired from gamesplusjames on YouTube: https://www.youtube.com/watch?v=RZPPLxVooSM&list=PLiyfvmtjWC_V_H-VMGGAZi7n5E0gyhc37&index=11

public class CameraController : MonoBehaviour
{
    public Transform target;

    Vector3 offset;

    //public bool useOffsetValues;

    float rotateSpeed = 5;

    public Transform pivot;

    // Start is called before the first frame update
    void Start()
    {
        offset = target.position - transform.position;

        pivot.transform.position = target.transform.position;
        //pivot.transform.parent = target.transform;
        pivot.transform.parent = null;

        //to hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        pivot.transform.position = target.transform.position;

        //get the x position of the mouse and rotate the target
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        pivot.Rotate(0, horizontal, 0);

        //get the y position of the mouse and rotate the pivot
        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;
        pivot.Rotate(-vertical, 0, 0);

        //limit up/down camera rotation
        if(pivot.rotation.eulerAngles.x > 45f && pivot.rotation.eulerAngles.x < 180f)
        {
            pivot.rotation = Quaternion.Euler(45f, 0, 0);
        }

        if(pivot.rotation.eulerAngles.x > 180f && pivot.rotation.eulerAngles.x < 315f)
        {
            pivot.rotation = Quaternion.Euler(315f, 0, 0);
        }

        //move the camera based on the current rotation of the target and the original offset
        float desiredYAngle = pivot.eulerAngles.y;
        float desiredXAngle = pivot.eulerAngles.x;

        Quaternion rotation  = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
        transform.position = target.position - (rotation * offset); 

        //transform.position = target.position - offset;

        if(transform.position.y < target.position.y)
        {
            transform.position = new Vector3(transform.position.x, target.position.y - 0.5f, transform.position.z);
        }

        transform.LookAt(target);
    }
}
