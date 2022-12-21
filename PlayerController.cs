using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] Transform playerCamera = null;
    [SerializeField] Transform playerArm = null;
    [SerializeField] float mouseSensitivity = 1f;
    float cameraPitch = 0.0f; 
    [SerializeField] bool lockCursor = true; 
    [SerializeField] float playerSpeed = 1f; 
    CharacterController controller = null;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f; 
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f; 
    [SerializeField] float gravity = -13.0f; 
    Vector2 currentDir = Vector2.zero; 
    Vector2 currentDirVelocity = Vector2.zero; 
    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero; 

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
    void Update()
    {
        UpdateMouseLook();
        UpdateMovement();
        
        
    }
    void UpdateMouseLook() {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); 

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
        cameraPitch -= currentMouseDelta.y * mouseSensitivity; 

        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f); 

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    

    }
    void UpdateMovement() {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize(); 
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime); 

        if(controller.isGrounded) {
            velocityY = 0.0f; 
        }
        velocityY += gravity * Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.LeftShift)) {
            playerSpeed *= 1.5f; 
        }
        Vector3 velocity = (transform.forward * targetDir.y + transform.right * targetDir.x) * playerSpeed + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime); 
    }
}
