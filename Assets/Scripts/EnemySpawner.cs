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

    private void Start()
    {
        SpawnEnemy();
    }
    public void SpawnEnemy()
    {
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
    public void SpawnPlayer()
    {
        Player player = SimplePool.Spawn(playerPrefabs, startPoint[^1].position, playerPrefabs.transform.rotation).GetComponent<Player>();
        player.SetMaterial(GameManager.Instance.GetMaterial(BrickColor.Blue));
        player.SetStage(stages);
        player.Color = BrickColor.Blue;
        player.SetJoystick(joystick);
        player.transform.SetParent(startPoint[^1]);
    }
}
