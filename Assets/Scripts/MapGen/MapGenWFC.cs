using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenWFC : MonoBehaviour
{
    public Vector2Int MapSize = new Vector2Int(x: 10, y: 10); //размер карты

    public VoxelTile tileFirst1;
    public VoxelTile tileFirst2;

    public List<VoxelTile> TilePrefabs; //массив тайлов которые мы уже разместили на сцене и будем спавнить
    private VoxelTile[,] spawnedTiles; //масcив заспавненых тайлов

    private Queue<Vector2Int> recalcPossibleTilesQueue = new Queue<Vector2Int>();
    private List<VoxelTile>[,] possibleTiles;

    //public GameObject Player;

    private void Start()
    {
        spawnedTiles = new VoxelTile[MapSize.x, MapSize.y]; //задаём размеры массиву заспавненых тайлов

        foreach (VoxelTile tilePrefab in TilePrefabs)
        {
            tilePrefab.CalculateSidesColor();
        }

        int countBeforeAdding = TilePrefabs.Count;
        for (int i = 0; i < countBeforeAdding; i++)
        {
            VoxelTile clone;
            switch (TilePrefabs[i].Rotation)
            {
                case VoxelTile.RotationType.OnlyRotation:
                    break;

                case VoxelTile.RotationType.TwoRotations:
                    TilePrefabs[i].Weight /= 2;
                    if (TilePrefabs[i].Weight <= 0) TilePrefabs[i].Weight = 0;

                    clone = Instantiate(TilePrefabs[i], position: TilePrefabs[i].transform.position + Vector3.right, Quaternion.identity);
                    clone.Rotate90();
                    TilePrefabs.Add(clone);
                    break;

                case VoxelTile.RotationType.FourRotations:
                    TilePrefabs[i].Weight /= 4;
                    if (TilePrefabs[i].Weight <= 0) TilePrefabs[i].Weight = 0;

                    clone = Instantiate(TilePrefabs[i], position: TilePrefabs[i].transform.position + Vector3.right, Quaternion.identity);
                    clone.Rotate90();
                    TilePrefabs.Add(clone);

                    TilePrefabs.Add(clone);
                    clone = Instantiate(TilePrefabs[i], position: TilePrefabs[i].transform.position + Vector3.right * 2, Quaternion.identity);
                    clone.Rotate90();
                    clone.Rotate90();
                    TilePrefabs.Add(clone);

                    TilePrefabs.Add(clone);
                    clone = Instantiate(TilePrefabs[i], position: TilePrefabs[i].transform.position + Vector3.right * 3, Quaternion.identity);
                    clone.Rotate90();
                    clone.Rotate90();
                    clone.Rotate90();
                    TilePrefabs.Add(clone);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        //localseed = seed;

        //StartCoroutine(routine: Generate());
        Generate();

        //Player.transform.position = new Vector3(transform.position.x + (MapSize.x / 2 + 1) * 6.4f, 4, transform.position.y + (MapSize.y - 2) * 6.4f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            foreach (VoxelTile spawnedTile in spawnedTiles)
            {
                if (spawnedTile != null) Destroy(spawnedTile.gameObject);
            }
            Generate();
        }
    }

    public void Generate()
    {
        possibleTiles = new List<VoxelTile>[MapSize.x, MapSize.y];

        int maxAttempts = 10;
        int attempts = 0;
        while (attempts++ < maxAttempts)
        {
            for (int x = 0; x < MapSize.x; x++)
            {
                for (int y = 0; y < MapSize.y; y++)
                {
                    possibleTiles[x, y] = new List<VoxelTile>(TilePrefabs);
                }
            }
            GenerateFirstTiles();

            VoxelTile tileInCenter = GetRandomTile(TilePrefabs);// берём рандомный тайл 
            possibleTiles[MapSize.x / 2, MapSize.y / 2] = new List<VoxelTile> { tileInCenter };//ставим его в центр

            recalcPossibleTilesQueue.Clear();
            EnqueueNeigboursToRecalc(new Vector2Int(MapSize.x / 2, MapSize.y / 2));

            bool success = GenerateAllPossibleTiles();

            if (success) break;
        }

        PlaseAllTiles();
    }

    private void GenerateFirstTiles()
    {
        for (int i = 1; i < MapSize.y - 1; i++)
        {
            possibleTiles[MapSize.x / 2 + 1, i] = new List<VoxelTile> { tileFirst1 };
            //if (i == MapSize.x / 2 + 2) possibleTiles[MapSize.x / 2 + 1, i] = new List<VoxelTile> { tileFirst2 };
        }
    }

    private bool GenerateAllPossibleTiles()
    {
        int backtracks = 0;

        int maxIterations = MapSize.x * MapSize.y;
        int iterations = 0;
        while (iterations++ < maxIterations)
        {
            int maxInnerIterations = 500;
            int innerIterations = 0;
            while (recalcPossibleTilesQueue.Count > 0 && innerIterations++ < maxInnerIterations)
            {
                Vector2Int position = recalcPossibleTilesQueue.Dequeue();
                if (position.x == 0 ||
                    position.y == 0 ||
                    position.x == MapSize.x - 1 ||
                    position.y == MapSize.y - 1)
                {
                    continue;
                }

                List<VoxelTile> possibleTilesHere = possibleTiles[position.x, position.y];

                int countRemoved = possibleTilesHere.RemoveAll(match: t => !IsTilePossible(t, position));

                if (countRemoved > 0) EnqueueNeigboursToRecalc(position);

                if (possibleTilesHere.Count == 0)
                {
                    // Зашли в тупик, в этих координатах невозможен ни один тайл. Попробуем ещё раз разрешим все тайлы
                    // в этих и соседних координатах и посмотрим устаканится ли всё
                    possibleTilesHere.AddRange(TilePrefabs);
                    possibleTiles[position.x + 1, position.y] = new List<VoxelTile>(TilePrefabs);
                    possibleTiles[position.x - 1, position.y] = new List<VoxelTile>(TilePrefabs);
                    possibleTiles[position.x, position.y + 1] = new List<VoxelTile>(TilePrefabs);
                    possibleTiles[position.x, position.y - 1] = new List<VoxelTile>(TilePrefabs);

                    EnqueueNeigboursToRecalc(position);

                    backtracks++;
                }
            }
            if (innerIterations == maxInnerIterations) break;

            List<VoxelTile> maxCountTile = possibleTiles[1, 1];
            Vector2Int maxCountTilePosition = new Vector2Int(x: 1, y: 1);

            for (int x = 1; x < MapSize.x - 1; x++)
            {
                for (int y = 1; y < MapSize.y - 1; y++)
                {
                    if (possibleTiles[x, y].Count > maxCountTile.Count)
                    {
                        maxCountTile = possibleTiles[x, y];
                        maxCountTilePosition = new Vector2Int(x, y);
                    }
                }
            }

            if (maxCountTile.Count == 1)
            {
                Debug.Log(message: $"Generated for {iterations} iterations, with {backtracks} backtracks");
                return true;
            }

            VoxelTile tileToCollapse = GetRandomTile(maxCountTile);
            possibleTiles[maxCountTilePosition.x, maxCountTilePosition.y] = new List<VoxelTile> { tileToCollapse };
            EnqueueNeigboursToRecalc(maxCountTilePosition);
        }

        Debug.Log(message: $"Failed, run out of iterations with {backtracks} backtracks");
        return false;
    }

    private bool IsTilePossible(VoxelTile tile, Vector2Int position)
    {
        bool isAllRightTileImpossible = possibleTiles[position.x - 1, position.y]
            .All(rightTile => !CanAppendTile(existingTile: tile, tileToAppend: rightTile, Vector3.right));
        if (isAllRightTileImpossible) return false;

        bool isAllLeftTileImpossible = possibleTiles[position.x + 1, position.y]
            .All(leftTile => !CanAppendTile(existingTile: tile, tileToAppend: leftTile, Vector3.left));
        if (isAllLeftTileImpossible) return false;

        bool isAllForwardTileImpossible = possibleTiles[position.x, position.y - 1]
            .All(forwardTile => !CanAppendTile(existingTile: tile, tileToAppend: forwardTile, Vector3.forward));
        if (isAllForwardTileImpossible) return false;

        bool isAllBackTileImpossible = possibleTiles[position.x, position.y + 1]
            .All(backTile => !CanAppendTile(existingTile: tile, tileToAppend: backTile, Vector3.back));
        if (isAllBackTileImpossible) return false;

        return true;
    }

    private void PlaseAllTiles()
    {
        for (int x = 1; x < MapSize.x - 1; x++)
        {
            for (int y = 1; y < MapSize.y - 1; y++)
            {
                PlaceTail(x, y);
            }
        }
    }

    private void EnqueueNeigboursToRecalc(Vector2Int position)
    {
        recalcPossibleTilesQueue.Enqueue(new Vector2Int(x: position.x + 1, position.y));
        recalcPossibleTilesQueue.Enqueue(new Vector2Int(x: position.x - 1, position.y));
        recalcPossibleTilesQueue.Enqueue(new Vector2Int(position.x, y: position.y + 1));
        recalcPossibleTilesQueue.Enqueue(new Vector2Int(position.x, y: position.y - 1));
    }

    //Функция установки тайлов
    private void PlaceTail(int x, int y)
    {
        if (possibleTiles[x, y].Count == 0) return;

        VoxelTile selectedTiles = GetRandomTile(possibleTiles[x , y]); //выбираем рандомный тайл из списка доступных
        Vector3 position = new Vector3(x, y: 0, z: y) * selectedTiles.VoxelSize * selectedTiles.TileSizexz;//задаём координаты
        spawnedTiles[x, y] = Instantiate(selectedTiles, position, selectedTiles.transform.rotation); //устанавливаем выбраный тайл в нужнную клетку
    }

    private VoxelTile GetRandomTile(List<VoxelTile> availableTiles)
    {
        List<float> chances = new List<float>();

        for (int i = 0; i < availableTiles.Count; i++)
        {
            chances.Add(availableTiles[i].Weight);
        }

        float value = Random.Range(0, chances.Sum());
        float sum = 0;

        for (int i = 0; i < chances.Count; i++)
        {
            sum += chances[i];
            if (value < sum)
            {
                return availableTiles[i];
            }

        }

        return availableTiles[availableTiles.Count - 1];

    }
    
    //Функция, проверяющая можем ли мы поставить тайл
    private bool CanAppendTile(VoxelTile existingTile, VoxelTile tileToAppend, Vector3 direction)
    {
        if (existingTile == null) return true;

        if (direction == Vector3.right)
            return Enumerable.SequenceEqual(existingTile.ColorsRight, tileToAppend.ColorsLeft);

        else if (direction == Vector3.left)
            return Enumerable.SequenceEqual(existingTile.ColorsLeft, tileToAppend.ColorsRight);

        else if (direction == Vector3.forward)
            return Enumerable.SequenceEqual(existingTile.ColorsForward, tileToAppend.ColorsBack);

        else if (direction == Vector3.back)
            return Enumerable.SequenceEqual(existingTile.ColorsBack, tileToAppend.ColorsForward);

        else
            throw new ArgumentException(message: "Wrong direction value, shoud be Vector3.left/right/forward/back", nameof(direction));
    }
}
