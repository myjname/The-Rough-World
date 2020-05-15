using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMechanicsManGG : MonoBehaviour
{
    public float MoveSpeed = 4;
    public float Gravity = 2;
    public float RunSpeed = 1.5f;

    private Vector3 MoveDirection;

    private Animator animator;
    private CharacterController ChController;

    private Vector2 CenterOfScreen;
    private Vector2 FirstVector;

    private void Start()
    {
        Gravity = 0 - Gravity;

        animator = GetComponent<Animator>();
        ChController = GetComponent<CharacterController>();

        CenterOfScreen = new Vector2(Screen.width / 2, Screen.height / 2);
        FirstVector = new Vector2(Screen.width / 2, Screen.height);
    }

    private void Update()
    {
        LockAtMouse();
        CharacterMove();
    }

    private void CharacterMove()
    {
        float localRunSpeed;

        if (Input.GetKey(KeyCode.LeftShift)) localRunSpeed = RunSpeed;
        else localRunSpeed = 1;

        MoveDirection = Vector3.zero;

        MoveDirection.x = Input.GetAxis("Horizontal") * MoveSpeed * localRunSpeed;
        MoveDirection.z = Input.GetAxis("Vertical") * MoveSpeed * localRunSpeed;
        MoveDirection.y = Gravity;

        if (Math.Abs(MoveDirection.x) > 0 && Math.Abs(MoveDirection.z) > 0)
        {
            MoveDirection.x *= (float)(Math.Pow(2, 0.5) / 2);
            MoveDirection.z *= (float)(Math.Pow(2, 0.5) / 2);
        }

        if (MoveDirection.x != 0 || MoveDirection.z != 0) animator.SetBool("Move", true);
        else animator.SetBool("Move", false);

        ChController.Move(MoveDirection * Time.deltaTime);
    }
    private void LockAtMouse()
    {
        Vector2 mousePos = Input.mousePosition;
        float rotationAngle = Vector2.Angle(FirstVector - CenterOfScreen, mousePos - CenterOfScreen);
        
        if (mousePos.x < CenterOfScreen.x) rotationAngle = Math.Abs(180 - rotationAngle) + 180;

        transform.eulerAngles = new Vector3(0, rotationAngle, 0);
    }
}
