using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 3f;
    public float rotateSpeed = 10f;

    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    public GameObject player;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();

        player = GameObject.Find("PlayerCharacter");
    }

    private void FixedUpdate()
    {
        Move();

        playerAnimator.SetFloat("Move_GoBack", playerInput.Move_GoBack);
        playerAnimator.SetFloat("Move_LeftRight", playerInput.Move_LeftRight);
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
