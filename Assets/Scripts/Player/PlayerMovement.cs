using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RiptideNetworking;
using RiptideNetworking.Utils;
using static NetworkManager;

public class PlayerMovement : MonoBehaviour
{
    
    [Header("Movement")]
    public float moveSpeed;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded; 
    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump = true;

    public Transform orientation;
    public Camera cam;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private Player player;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        player = GetComponent<Player>();
    }

    private void Update()
    {
        myInput();

        //raycast to check if player is grounded
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        Debug.Log($"Grounded is currently {grounded}");

        if (grounded)
        {
            //Debug.Log(grounded);
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        movePlayer();
        sendPosition();
    }

    void myInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Space) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(resetJump), jumpCooldown);
        }

    }

    private void movePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10 * airMultiplier, ForceMode.Force);
        }
        
    }

    private void speedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit velocity
        Vector3 limitedVel = flatVel.normalized * moveSpeed;
        rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void resetJump()
    {
        readyToJump = true;
    }
  
    #region messages

    private void sendPosition()
    {
        Message message = Message.Create(MessageSendMode.unreliable, ClientToServerId.playerPosition);

       // Debug.Log("Sending positions " + transform.position);

        message.AddUShort(player.id);
        message.AddVector3(transform.position);
        message.AddVector3(cam.transform.position);
        Singleton.Client.Send(message);


    }




    #endregion

}