using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    Vector3 move;

   //Animations Variables
   [SerializeField] internal Animator animator;
    internal string currentState;
    bool haveSword = false;
    bool comboPossible;
    int comboStep;
    [SerializeField] public GameObject drawSword;
    [SerializeField] public GameObject unDrawSword;

    private void Start()
    {
        //controller = gameObject.AddComponent<CharacterController>();
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;

        animator.SetFloat("speed", Mathf.Abs(move.x));

        animator.SetBool("ground", controller.isGrounded);

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        animator.SetFloat("speed.y", playerVelocity.y);

        move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.SetTrigger("jump");
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}