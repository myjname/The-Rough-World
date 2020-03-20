using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //Ввод переменных
    public Transform Player; //игровой персонаж
    private Vector3 offset; //растояние от игрока до камеры

    public float speedZoom = 2f;

    void Start ()
    {
        CameraStart ();
    }

    void Update ()
    {
        transform.position = Player.transform.position + offset;

        transform.LookAt (Player);

        CameraZoom ();
    }

    //Метод для установки камеры в правильное положение
    private void CameraStart ()
    {
        transform.position = Player.transform.position + new Vector3(0f, 7f, -4f); //установка камеры в правильное поожение
        offset = transform.position - Player.transform.position; //расстояние от игрока до камеры
    }

    //Метод зума
    private void CameraZoom ()
    {

        if (Input.GetAxis ("Mouse ScrollWheel") > 0 && offset.y >= 5f && offset.z <= -2f) //приближение
        {
            offset.y += -0.2f * speedZoom;
            offset.z -= -0.2f * speedZoom;
        }

        if (Input.GetAxis ("Mouse ScrollWheel") < 0 && offset.y <= 9 && offset.z >= -6) //отдаление
        {
            offset.y += 0.2f * speedZoom;
            offset.z -= 0.2f * speedZoom;
        }

    }
}