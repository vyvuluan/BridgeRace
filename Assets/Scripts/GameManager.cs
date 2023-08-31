using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private List<MaterialColor> materialColors;
    private Dictionary<BrickColor, Material> colorDic = new();


    void Start()
    {
        Instance = this;
        colorDic = materialColors.ToDictionary(n => n.Color, n => n.Material);
    }

    public Material GetMaterial(BrickColor color) => colorDic[color];


}
