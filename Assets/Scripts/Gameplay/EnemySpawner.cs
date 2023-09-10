using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MaterialColor
{
    [SerializeField] private BrickColor color;
    [SerializeField] private Material material;

    public Material Material { get => material; set => material = value; }
    public BrickColor Color { get => color; set => color = value; }
}
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefabs;
    [SerializeField] private GameObject playerPrefabs;
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private List<Stage> stages;
    [SerializeField] private List<Transform> startPoint;
    [SerializeField] private List<MaterialColor> materials;
    [SerializeField] private List<Enemy> enemies;
    private int quantity = 3;
    private BrickColor enemyColor;
    private System.Action<Transform> onSetPlayer;


    private void Start()
    {
        SpawnEnemy();
        SpawnPlayer(enemyColor);
    }
    public void Init(BrickColor brickColor, System.Action<Transform> onSetPlayer)
    {
        this.enemyColor = brickColor;
        this.onSetPlayer = onSetPlayer;
    }
    public void SpawnEnemy()
    {
        RemoveByColor(enemyColor);
        for (int i = 0; i < quantity; i++)
        {
            Enemy enemy = SimplePool.Spawn(enemyPrefabs, startPoint[i].position, enemyPrefabs.transform.rotation).GetComponent<Enemy>();
            int indexMaterial = Random.Range(0, materials.Count);
            enemy.SetMaterial(materials[indexMaterial].Material);
            enemy.SetStage(stages);
            enemy.Color = materials[indexMaterial].Color;
            enemy.transform.SetParent(startPoint[i]);
            materials.RemoveAt(indexMaterial);
            enemies.Add(enemy);
        }

    }
    public void SpawnPlayer(BrickColor color)
    {
        Player player = SimplePool.Spawn(playerPrefabs, startPoint[^1].position, playerPrefabs.transform.rotation).GetComponent<Player>();
        player.SetMaterial(GameManager.Instance.GetMaterial(color));
        player.SetStage(stages);
        player.Color = color;
        player.SetJoystick(joystick);
        player.transform.SetParent(startPoint[^1]);
        onSetPlayer?.Invoke(player.transform);
    }
    public void RemoveByColor(BrickColor color)
    {
        foreach (var item in materials)
        {
            if (item.Color == color)
            {
                materials.Remove(item);
                return;
            }
        }
    }
}
