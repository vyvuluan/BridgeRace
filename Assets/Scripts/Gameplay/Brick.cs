﻿using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private BrickColor color;
    [SerializeField] private MeshRenderer meshRenderer;

    public BrickColor Color { get => color; set => color = value; }

    public void SetMaterial(Material material)
    {
        meshRenderer.material = material;
    }
    public void OnDespawn()
    {
        gameObject.SetActive(false);
        Invoke(nameof(OnInit), 7f);
    }
    public void OnInit()
    {
        gameObject.SetActive(true);
    }
}
