using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 3f;
    public float rotateSpeed = 10f;

    private PlayerInput playerInput;
    private PlayerInteraction playerInteraction;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;



    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInteraction = GetComponent<PlayerInteraction>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();

        playerAnimator.SetFloat("Move_GoBack", playerInput.Move_GoBack);
        playerAnimator.SetFloat("Move_LeftRight", playerInput.Move_LeftRight);

        if (playerInteraction.currentObject != null && playerInteraction.isHoldingObject)
        {
            playerInteraction.currentObject.transform.position = transform.position;
        }
    }

    private void Move()
    {
        Vector3 dir = new(playerInput.Move_LeftRight, 0, playerInput.Move_GoBack);

        if (!(playerInput.Move_GoBack == 0 && playerInput.Move_LeftRight == 0))
        {
            playerRigidbody.MovePosition(playerRigidbody.position + moveSpeed * Time.deltaTime * dir);
            playerRigidbody.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotateSpeed);
        }
    }
}
