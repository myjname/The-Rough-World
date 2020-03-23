using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGen : MonoBehaviour {

    public Vector2Int MapSize = new Vector2Int (x: 10, y: 10); //размер карты

    public List<VoxelTile> TilePrefabs; //массив тайлов которые мы уже разместили на сцене
    private VoxelTile[, ] spawnedTiles; //масcив заспавненых тайлов

    public int seed = 2; //сид мира
    private int localseed; //локальный сид

    public GameObject Player;

    private void Start () {
        spawnedTiles = new VoxelTile[MapSize.x, MapSize.y]; //задаём размеры массиву заспавненых тайлов

        foreach (VoxelTile tilePrefab in TilePrefabs) {
            tilePrefab.CalculateSidesColor ();
        }

        int countBeforeAdding = TilePrefabs.Count;
        for (int i = 0; i < countBeforeAdding; i++) {
            VoxelTile clone;
            switch (TilePrefabs[i].Rotation) {
                case VoxelTile.RotationType.OnlyRotation:
                    break;

                case VoxelTile.RotationType.TwoRotations:
                    TilePrefabs[i].Weight /= 2;
                    if (TilePrefabs[i].Weight <= 0) TilePrefabs[i].Weight = 1;

                    clone = Instantiate (TilePrefabs[i], position : TilePrefabs[i].transform.position + Vector3.right, Quaternion.identity);
                    clone.Rotate90 ();
                    TilePrefabs.Add (clone);
                    break;

                case VoxelTile.RotationType.FourRotations:
                    TilePrefabs[i].Weight /= 4;
                    if (TilePrefabs[i].Weight <= 0) TilePrefabs[i].Weight = 1;

                    clone = Instantiate (TilePrefabs[i], position : TilePrefabs[i].transform.position + Vector3.right, Quaternion.identity);
                    clone.Rotate90 ();
                    TilePrefabs.Add (clone);

                    TilePrefabs.Add (clone);
                    clone = Instantiate (TilePrefabs[i], position : TilePrefabs[i].transform.position + Vector3.right * 2, Quaternion.identity);
                    clone.Rotate90 ();
                    clone.Rotate90 ();
                    TilePrefabs.Add (clone);

                    TilePrefabs.Add (clone);
                    clone = Instantiate (TilePrefabs[i], position : TilePrefabs[i].transform.position + Vector3.right * 3, Quaternion.identity);
                    clone.Rotate90 ();
                    clone.Rotate90 ();
                    clone.Rotate90 ();
                    TilePrefabs.Add (clone);
                    break;
                default:
                    throw new ArgumentOutOfRangeException ();
            }
        }

        localseed = seed;

        //StartCoroutine(routine: Generate());
        Generate ();

        Player.transform.position = new Vector3 (transform.position.x + MapSize.x / 2 * 6.4f, 4, transform.position.y + MapSize.y / 2 * 6.4f);

    }

    private void Update () {
        if (Input.GetKeyDown (KeyCode.G)) {
            StopAllCoroutines ();

            foreach (VoxelTile spawnedTile in spawnedTiles) {
                if (spawnedTile != null) Destroy (spawnedTile.gameObject);
            }

            localseed = seed;

            //StartCoroutine(routine: Generate());
            Generate ();
        }
    }

    //Функция генерации карты
    //public IEnumerator Generate()
    public void Generate () {
        for (int x = 1; x < MapSize.x - 1; x++) {
            for (int y = 1; y < MapSize.y - 1; y++) {
                //yield return new WaitForSeconds(0.001f);

                PlaseTail (x, y);
            }
        }
    }

    //Функция установки тайлов
    private void PlaseTail (int x, int y) {
        List<VoxelTile> availableTiles = new List<VoxelTile> (); //список тайлов которые мы можем поставить

        foreach (VoxelTile tilePrefab in TilePrefabs) {

            if (CanAppendTile (existingTile: spawnedTiles[x - 1, y], tileToAppend : tilePrefab, Vector3.left) &&
                CanAppendTile (existingTile: spawnedTiles[x + 1, y], tileToAppend : tilePrefab, Vector3.right) &&
                CanAppendTile (existingTile: spawnedTiles[x, y - 1], tileToAppend : tilePrefab, Vector3.back) &&
                CanAppendTile (existingTile: spawnedTiles[x, y + 1], tileToAppend : tilePrefab, Vector3.forward)) {
                availableTiles.Add (tilePrefab);
            }
        }
        if (availableTiles.Count == 0) return;

        VoxelTile selectedTiles = GetRandomTile (availableTiles); //выбираем рандомный тайл из списка доступных
        Vector3 position = new Vector3 (x, y : 0, z : y) * selectedTiles.VoxelSize * selectedTiles.TileSizexz;
        spawnedTiles[x, y] = Instantiate (selectedTiles, position, selectedTiles.transform.rotation); //устанавливаем выбраный тайл в нужнную клетку
        localseed += (localseed + localseed) * 7 / 3;
    }

    private VoxelTile GetRandomTile (List<VoxelTile> availableTiles) {
        List<float> chances = new List<float> ();

        for (int i = 0; i < availableTiles.Count; i++) {
            chances.Add (availableTiles[i].Weight);
        }

        Random.InitState (localseed);
        float value = Random.Range (0, chances.Sum ());
        float sum = 0;

        for (int i = 0; i < chances.Count; i++) {
            sum += chances[i];
            if (value < sum) {
                return availableTiles[i];
            }

        }

        return availableTiles[availableTiles.Count - 1];

    }

    //Функция, проверяющая можем ли мы поставить тайл
    private bool CanAppendTile (VoxelTile existingTile, VoxelTile tileToAppend, Vector3 direction) {
        if (existingTile == null) return true;

        if (direction == Vector3.right)
            return Enumerable.SequenceEqual (existingTile.ColorsRight, tileToAppend.ColorsLeft);

        else if (direction == Vector3.left)
            return Enumerable.SequenceEqual (existingTile.ColorsLeft, tileToAppend.ColorsRight);

        else if (direction == Vector3.forward)
            return Enumerable.SequenceEqual (existingTile.ColorsForward, tileToAppend.ColorsBack);

        else if (direction == Vector3.back)
            return Enumerable.SequenceEqual (existingTile.ColorsBack, tileToAppend.ColorsForward);

        else
            throw new ArgumentException (message: "Wrong direction value, shoud be Vector3.left/right/forward/back", nameof (direction));
    }

}