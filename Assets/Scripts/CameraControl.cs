using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //Ввод переменных
    public Transform Player;//игровой персонаж
    private Vector3 offset;//растояние от игрока до камеры
    private Vector3 moveCamera;//координаты правильной установки камеры

    public float speedZoom = 2f;

    void Start()
    {
        CameraStart();
        transform.position = Player.transform.position + moveCamera;
        offset = transform.position - Player.transform.position;//расстояние от игрока до камеры
    }

    void Update()
    {
        transform.position = Player.transform.position + offset;

        transform.LookAt(Player);

        CameraZoom();
        
    }

    //Метод для установки камеры в правильное положение
    private void CameraStart()
    {
        moveCamera.z = Player.transform.position.z - 7f;
        moveCamera.y = Player.transform.position.y + 7f;
        //transform.rotation = Quaternion.Euler(45f, 0f, 0f);
    }

    //Метод зума
    private void CameraZoom()
    {
        
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && offset.y >= 3.5f && offset.z <= -3.5f)//приближение
        {
            offset.y += -(Input.GetAxis("Mouse ScrollWheel") * speedZoom);
            offset.z -= -(Input.GetAxis("Mouse ScrollWheel") * speedZoom);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && offset.y <= 10 && offset.z >= -10)//отдаление
        {
            offset.y -= (Input.GetAxis("Mouse ScrollWheel") * speedZoom);
            offset.z += (Input.GetAxis("Mouse ScrollWheel") * speedZoom);
        }

    }
}
