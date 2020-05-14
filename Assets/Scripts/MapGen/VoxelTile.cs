using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelTile : MonoBehaviour {
    public float VoxelSize = 0.1f; //размер вокселя 
    public int TileSizexz = 64; //размер сторон меша по ширине
    public int TileSizey = 6; //размер высоты меша

    [Range (0, 10000)] public int Weight = 50;

    public RotationType Rotation;

    public enum RotationType {
        OnlyRotation,
        TwoRotations,
        FourRotations
    }

    //Массив цветов для каждой стороны
    [HideInInspector] public int[] ColorsRight;
    [HideInInspector] public int[] ColorsForward;
    [HideInInspector] public int[] ColorsLeft;
    [HideInInspector] public int[] ColorsBack;

    public void CalculateSidesColor () {
        ColorsRight = new int[TileSizexz * TileSizey];
        ColorsForward = new int[TileSizexz * TileSizey];
        ColorsLeft = new int[TileSizexz * TileSizey];
        ColorsBack = new int[TileSizexz * TileSizey];

        for (int i = 0; i < TileSizey; i++) {
            for (int j = 0; j < TileSizexz; j++) {
                ColorsRight[i * TileSizexz + j] = GetVoxelColor (verticalLayer: i, horizontalOffset: j, Vector3.right);
                ColorsForward[i * TileSizexz + j] = GetVoxelColor (verticalLayer: i, horizontalOffset: j, Vector3.forward);
                ColorsLeft[i * TileSizexz + j] = GetVoxelColor (verticalLayer: i, horizontalOffset: j, Vector3.left);
                ColorsBack[i * TileSizexz + j] = GetVoxelColor (verticalLayer: i, horizontalOffset: j, Vector3.back);
            }
        }
    }

    public void Rotate90 () {
        transform.Rotate (xAngle: 0, yAngle: 90, zAngle: 0);

        int[] colorsRightNew = new int[TileSizexz * TileSizey];
        int[] colorsForwardNew = new int[TileSizexz * TileSizey];
        int[] colorsLeftNew = new int[TileSizexz * TileSizey];
        int[] colorsBackNew = new int[TileSizexz * TileSizey];

        for (int layer = 0; layer < TileSizey; layer++) {
            for (int offset = 0; offset < TileSizexz; offset++) {
                colorsRightNew[layer * TileSizexz + offset] = ColorsForward[layer * TileSizexz + TileSizexz - offset - 1];
                colorsForwardNew[layer * TileSizexz + offset] = ColorsLeft[layer * TileSizexz + offset];
                colorsLeftNew[layer * TileSizexz + offset] = ColorsBack[layer * TileSizexz + TileSizexz - offset - 1];
                colorsBackNew[layer * TileSizexz + offset] = ColorsRight[layer * TileSizexz + offset];
            }
        }

        ColorsRight = colorsRightNew;
        ColorsForward = colorsForwardNew;
        ColorsLeft = colorsLeftNew;
        ColorsBack = colorsBackNew;
    }

    //Метод воровства цвета у "вокселя"
    private int GetVoxelColor (int verticalLayer, int horizontalOffset, Vector3 direction)
    {
        MeshCollider meshColider = GetComponent<MeshCollider>(); //кешируем меш колайдер, чтобы делать рейкаст (запуск луча)

        float vox = VoxelSize;
        float half = VoxelSize / 2; //для удобства

        //В зависимости от стороны меша запускаем луч
        Vector3 rayStart;
        if (direction == Vector3.right) {
            rayStart = meshColider.bounds.min +
                new Vector3 (x: -half, y : 0, z : half + horizontalOffset * vox);
        } else if (direction == Vector3.forward) {
            rayStart = meshColider.bounds.min +
                new Vector3 (x: half + horizontalOffset * vox, y: 0, z: -half);
        } else if (direction == Vector3.left) {
            rayStart = meshColider.bounds.max +
                new Vector3 (x: half, y: 0, z: -half - (TileSizexz - horizontalOffset - 1) * vox);
        } else if (direction == Vector3.back) {
            rayStart = meshColider.bounds.max +
                new Vector3 (x: -half - (TileSizexz - horizontalOffset - 1) * vox, y : 0, z : half);
        } else {
            throw new ArgumentException (message: "Wrong direction value, shoud be Vector3.left/right/forward/back", nameof (direction));
        }

        rayStart.y = meshColider.bounds.min.y + half + verticalLayer * vox;

        //Debug.DrawRay(rayStart, dir: direction * 0.1f, Color.blue, duration: 2);//дебаг луча, запускаем луч из старта в направлении 0,0,1 на 2 секунды

        //Запускаем луч и проверяем ударился ли он
        if (Physics.Raycast (new Ray (origin: rayStart, direction), out RaycastHit hit, vox)) {
            Mesh mesh = meshColider.sharedMesh; //забираем меш из меш колайдера

            int hitTriangleVertex = mesh.triangles[hit.triangleIndex * 3]; //воруем индекс вершин треугольника в который попали
            int colorIndex = (int) (mesh.uv[hitTriangleVertex].x * 256); //номер цвета из UV
            return colorIndex;
        }
        return -1;
    }
}