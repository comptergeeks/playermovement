using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera playerCameraValue; 

    [SerializeField] Transform playerCamera = null;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] float mouseSensitivity = 3.5f;
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
    [SerializeField] float springSpeed; 
    Vector2 currentMouseDelta;
    Vector2 currentMouseDeltaVelocity;
    Vector2 currentDir = Vector2.zero; 
    public bool isSprinting = false;
    float cameraCap; 
    float velocityY = 0.0f; 
    public float t = 0.5f;
    public float maxFOV;
    public float startFOV; 
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
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
 
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
 
        cameraCap -= currentMouseDelta.y * mouseSensitivity;
 
        cameraCap = Mathf.Clamp(cameraCap, -90.0f, 90.0f);
 
        playerCamera.localEulerAngles = Vector3.right * cameraCap;
 
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
         
    

    }
    void UpdateMovement() {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, ground1);
        velocityY += gravity * 2f * Time.deltaTime;
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize(); 
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime); 


        if(Input.GetKey(KeyCode.LeftShift)) {
            isSprinting = true;
            playerCameraValue.fieldOfView = Mathf.Lerp(playerCameraValue.fieldOfView, maxFOV, t);
        } else {
            isSprinting = false; 
            playerCameraValue.fieldOfView = Mathf.Lerp(playerCameraValue.fieldOfView, startFOV, t);
        }

        Vector3 velocity = (transform.forward * targetDir.y + transform.right * targetDir.x) * playerSpeed + Vector3.up * velocityY;
        if(isSprinting) {
            velocity *= springSpeed;  
        }
        controller.Move(velocity * Time.deltaTime); 
        if(isGrounded! && controller.velocity.y < -1f)
        {
            velocityY = -8f;
        }
        if(Input.GetKey(KeyCode.Space) && isGrounded) {
               velocityY += 6f;  
        }
    }
}
