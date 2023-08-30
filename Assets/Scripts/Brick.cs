using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private BrickColor color;
    [SerializeField] private Vector2Int index;
    [SerializeField] private MeshRenderer meshRenderer;

    public Vector2Int Index { get => index; set => index = value; }
    public BrickColor Color { get => color; set => color = value; }

    public void SetMaterial(Material material)
    {
        meshRenderer.material = material;
    }
    public void OnDespawn()
    {
        gameObject.SetActive(false);
        Invoke(nameof(OnInit), 4f);
    }
    public void OnInit()
    {
        gameObject.SetActive(true);
    }
}
