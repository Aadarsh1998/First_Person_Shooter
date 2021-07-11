using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float running;
    public float gravityModifier;
    public float jumpSpeed;
    public CharacterController CharacterController;
    private Vector3 MoveInput;
    public Transform camTransform;
    public float mouseSensitivity;
    public bool invertX, invertY;
    public bool isGrounded, canDoubleJump;
    public LayerMask whatIsGround;
    public Transform groundCheckPoint;

    void Start()
    {
        
    }

    
    void Update()
    {
        float ystore = MoveInput.y;
        
        //MoveInput.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        //MoveInput.z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        Vector3 horiMove = transform.right * Input.GetAxis("Horizontal");
        Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");

        MoveInput = horiMove + vertMove;
        MoveInput = MoveInput * moveSpeed;
        //MoveInput.Normalize();

        if(Input.GetKey(KeyCode.LeftShift))
        {
            MoveInput = MoveInput * running;
        }
        else
        {
            MoveInput = MoveInput * moveSpeed;
        }

        MoveInput.y = ystore;
        MoveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;
        
        if(CharacterController.isGrounded)
        {
            MoveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
        }

        isGrounded = Physics.OverlapSphere(groundCheckPoint.position, 0.25f, whatIsGround).Length > 0;
        //Handle Jump Movement

        //if(isGrounded)
        //{
        //    canDoubleJump = false;
        //}
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            MoveInput.y = jumpSpeed;
            canDoubleJump = true;
        }
        else if(canDoubleJump && Input.GetKeyDown(KeyCode.Space))
        {
            
            MoveInput.y = jumpSpeed;
            canDoubleJump = false;
        }

        CharacterController.Move(MoveInput * Time.deltaTime);

        //Control the Camera Rotation
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y") * mouseSensitivity);

        if(invertX)
        {
            mouseInput.x = -mouseInput.x;
        }
        if(invertY)
        {
            mouseInput.y = -mouseInput.y;
        }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);

        //cam rotation
        camTransform.rotation = Quaternion.Euler(camTransform.rotation.eulerAngles + new Vector3(-mouseInput.y,0,0));
    }
}
