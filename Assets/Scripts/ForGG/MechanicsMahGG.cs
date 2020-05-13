using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicsMahGG : MonoBehaviour {
    //Основные параметры
    public float speedMove; //скорость персонажа
    public float jumpPower; //сила прыжка

    //Параметры геймплея для персонажа
    private float gravityForces; //гравитация персонажа
    private Vector3 moveVector; //направление движения персонажа

    //Ссылки на компоненты
    private CharacterController ch_controller;
    private Animator ch_animator;

    private int speedRun;

    void Start ()
    {
        ch_controller = GetComponent<CharacterController>();
        ch_animator = GetComponent<Animator>();
    }

    void Update ()
    {
        CharacterMove();
        GamingGravity();
    }

    //Метод перемещения персонажа
    private void CharacterMove()
    {
        //Перемещение по поверхности
        if (ch_controller.isGrounded)
        {
            if (Input.GetKey(KeyCode.LeftShift)) speedRun = 5;
            else speedRun = 1;

            moveVector = Vector3.zero;
            moveVector.x = Input.GetAxis ("Horizontal") * speedMove * speedRun;
            moveVector.z = Input.GetAxis ("Vertical") * speedMove * speedRun;

            //анимация передвижения персонажа
            if (moveVector.x != 0 || moveVector.z != 0) ch_animator.SetBool ("Move", true);
            else ch_animator.SetBool ("Move", false);

            //Поворот персонажа в сторону направления перемещения
            if (Vector3.Angle(Vector3.forward, moveVector) > 1 || Vector3.Angle(Vector3.forward, moveVector) == 0)
            {
                Vector3 direct = Vector3.RotateTowards (transform.forward, moveVector, speedMove, 0.0f);
                transform.rotation = Quaternion.LookRotation (direct);
            }
        }

        moveVector.y = gravityForces; //расчёт гравитации вополнять после поворота
        ch_controller.Move (moveVector * Time.deltaTime); //метод передвижения по направлению

    }

    //Метод гравитации
    private void GamingGravity () {
        if (ch_controller.isGrounded) gravityForces -= 20f * Time.deltaTime;
        else gravityForces = -1f;
        if (Input.GetKeyDown (KeyCode.Space) && ch_controller.isGrounded) gravityForces = jumpPower;
    }

}