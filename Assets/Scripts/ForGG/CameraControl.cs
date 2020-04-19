using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private GameObject persGG; //игровой персонаж
    private Vector3 offset; //растояние от игрока до камеры

    public float speedZoom = 2f;

    void Start ()
    {
        persGG = GameObject.FindGameObjectWithTag("Player");

        transform.position = persGG.transform.position + new Vector3(0f, 7f, -4f); //установка камеры в правильное поожение
        offset = transform.position - persGG.transform.position; //расстояние от игрока до камеры
    }

    void Update ()
    {
        CameraMove();

        CameraZoom();
    }

    private void CameraMove()
    {
        transform.position = persGG.transform.position + offset;
        transform.LookAt(persGG.transform);
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