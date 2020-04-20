using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float speedZoom = 2f;
    public float speedRotate = 0.1f;
    public Vector3 CamCoord = new Vector3(0, 20, 0);

    private GameObject persGG; //игровой персонаж
    private Vector3 offset; //растояние от игрока до камеры

    void Start ()
    {
        persGG = GameObject.FindGameObjectWithTag("Player");

        transform.position = persGG.transform.position + CamCoord; //установка камеры в правильное поожение
        offset = transform.position - persGG.transform.position; //расстояние от игрока до камеры
    }

    void Update ()
    {
        CameraMove();

        //CameraZoom();

        if (Input.GetKey(KeyCode.Q)) CameraRotate(1);
        if (Input.GetKey(KeyCode.E)) CameraRotate(-1);
    }

    private void CameraMove()
    {
        transform.position = persGG.transform.position + offset;
        transform.LookAt(persGG.transform);
    }

    private void CameraRotate(int direction)
    {
        transform.RotateAround(persGG.transform.position, Vector3.up, direction * speedRotate);
        offset = transform.position - persGG.transform.position;
    }

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