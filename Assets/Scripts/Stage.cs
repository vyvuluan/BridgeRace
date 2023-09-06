using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private Vector2Int size;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private List<Vector2Int> listCheckRandom = new();
    [SerializeField] private List<Material> brickColors;
    [SerializeField] private List<Brick> bricks = new();
    [SerializeField] private List<Brige> briges;
    private Brick[,] brickTable;
    private void Start()
    {
        SpawnBrick();
        RandomColorBrick();
    }
    private void SpawnBrick()
    {
        brickTable = new Brick[size.x, size.y];
        //bricks = new();
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Vector3 pos = new(i * 1.5f, brickPrefab.transform.position.y, j * -1.0f);
                Brick brick = SimplePool.Spawn(brickPrefab, pos, brickPrefab.transform.rotation).GetComponent<Brick>();
                brick.Index = new(i, j);
                bricks.Add(brick);
                brickTable[i, j] = brick;
                listCheckRandom.Add(brick.Index);
                //brick.transform.SetParent(transform);
            }

        }
    }
    private void RandomColorBrick()
    {
        int quantityOfType = (size.x * size.y) / brickColors.Count;
        for (int i = 0; i < brickColors.Count; i++)
        {
            for (int j = 0; j < quantityOfType; j++)
            {
                int index = Random.Range(0, listCheckRandom.Count);
                Vector2Int indexTable = listCheckRandom[index];
                brickTable[indexTable.x, indexTable.y].SetMaterial(brickColors[i]);
                brickTable[indexTable.x, indexTable.y].Color = (BrickColor)(i + 1);
                listCheckRandom.RemoveAt(index);
            }
        }
    }
    public Brige GetBriges(int index) => briges[index];
    public int RandomBrige() => Random.Range(0, briges.Count);
}
