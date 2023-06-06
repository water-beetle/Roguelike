using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerContorller : MonoBehaviour
{
    private Animator playerAnimator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;
    private PlayerInputAction playerInputActions;

    [SerializeField] private float speed;
    private Vector2 playerDir;
    private Vector2 mousePos;


    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerInputActions = new PlayerInputAction();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable();
    }

    private void Update()
    {
        playerDir = playerInputActions.Player.Movement.ReadValue<Vector2>().normalized;
        mousePos = getMousePos();
        AnimatePlayer();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }



    private void MovePlayer()
    {
        rigidBody.velocity = playerDir * speed;

    }

    private void AnimatePlayer()
    {
        float relativePosx;

        relativePosx = mousePos.x - transform.position.x;
        spriteRenderer.flipX = relativePosx < 0;

        playerAnimator.SetBool(Settings.isRun, playerDir.magnitude > 0);
    }

    private Vector2 getMousePos()
    {
        Vector3 mousePos = playerInputActions.Player.MousePos.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }






}
