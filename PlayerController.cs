using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivityX = 1f;
    [SerializeField] float mouseSensitivityY = 1f;
    float xRotation; 
    float yRotation; 
    Vector2 currentDirVelocity = Vector2.zero; 

    
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground1;  
    bool isGrounded;


    [SerializeField] bool lockCursor = true; 

    [SerializeField] float playerSpeed = 1f; 
    CharacterController controller = null;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f; 
   
    [SerializeField] float gravity = -13.0f; 
    Vector2 currentDir = Vector2.zero; 
    public bool isSprinting = false;

    float velocityY = 0.0f; 
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>(); 
        if (lockCursor)
        {
            Cursor.visible = false; 
            Cursor.lockState = CursorLockMode.Locked;
            
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateMouseLook();
        UpdateMovement();
        
        
    }
    void UpdateMouseLook() {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivityY;

        yRotation += mouseX; 
        xRotation -= mouseY; 
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); 

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
         
    

    }
    void UpdateMovement() {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, ground1);
        velocityY += gravity * 2f * Time.deltaTime;
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize(); 
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime); 


        if(Input.GetKey(KeyCode.LeftShift)) {
            isSprinting = true;
        } else {
            isSprinting = false; 
        }

        Vector3 velocity = (transform.forward * targetDir.y + transform.right * targetDir.x) * playerSpeed + Vector3.up * velocityY;
        if(isSprinting) {
            velocity *= 1.5f;  
        }
        controller.Move(velocity * Time.deltaTime); 
        if(isGrounded! && controller.velocity.y < -1f)
        {
            velocityY = -8f;
        }
    }
}
