using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerContorller : MonoBehaviour
{
    private Rigidbody2D rgbd;
    private PlayerInputAction playerInputActions;
    private float moveSpeed = 5f;

    private void Awake()
    {

        playerInputActions = new PlayerInputAction();
        playerInputActions.Player.Enable();

        rgbd = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        rgbd.velocity = (Vector3)(inputVector * moveSpeed);
    }


}
