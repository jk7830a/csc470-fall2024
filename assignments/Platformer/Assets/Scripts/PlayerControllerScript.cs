using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code inspired from gamesplusjames on YouTube: https://www.youtube.com/watch?v=RZPPLxVooSM&list=PLiyfvmtjWC_V_H-VMGGAZi7n5E0gyhc37&index=1

public class PlayerScript : MonoBehaviour
{
    public CharacterController CC;

    float moveSpeed = 7.5f;
    float jumpForce = 20; 

    private Vector3 moveDirection;
    float gravityScale = 5;

    public Animator anim;
    public Transform pivot;
    float rotateSpeed = 25;

    public GameObject playerModel;

    public GameManagerScript gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, moveDirection.y, Input.GetAxis("Vertical") * moveSpeed); 

        float yStore = moveDirection.y;
        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        moveDirection = moveDirection.normalized * moveSpeed;
        moveDirection.y = yStore;

        if(CC.isGrounded)
        {
            moveDirection.y = 0f;
            if(Input.GetKeyDown(KeyCode.Space))
            {
                moveDirection.y = jumpForce;
            }
        }

        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
        CC.Move(moveDirection * Time.deltaTime);

        //move the player in different directions based on camera look direction
        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical")!= 0)
        {
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }

        anim.SetBool("isGrounded", CC.isGrounded);
        anim.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal")));
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("carrot"))
        {
            gameManager.CollectCarrot();

            Destroy(other.gameObject);
        }

        if(other.CompareTag("raddish"))
        {
            gameManager.CollectRadish();
            Destroy(other.gameObject);
        }
        else if(other.CompareTag("skunk") || other.CompareTag("Fox"))
        {
            gameManager.GameOverByPredator();
        }

    }
}
