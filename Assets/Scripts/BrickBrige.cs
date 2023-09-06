using UnityEngine;

public class BrickBrige : MonoBehaviour
{
    [SerializeField] private BrickColor color;
    [SerializeField] private MeshRenderer meshRenderer;

    public BrickColor Color { get => color; set => color = value; }

    public void SetMaterial(Material material)
    {
        meshRenderer.material = material;
    }

}
