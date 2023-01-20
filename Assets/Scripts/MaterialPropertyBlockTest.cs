using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MaterialPropertyBlockTest : MonoBehaviour
{
    public Color DefaultColor, Color1, Color2;
    private float Speed = 1, Offset;

    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;

    private bool isMarkedForDeletion = false;

    void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        _renderer.GetPropertyBlock(_propBlock);

        if (isMarkedForDeletion)
        {
            _renderer.GetPropertyBlock(_propBlock);
            _propBlock.SetColor("_Color", Color.Lerp(Color1, Color2, (Mathf.Sin(Time.time * Speed + Offset) + 1) / 2f));
        }
        else
            _propBlock.SetColor("_Color", DefaultColor);

        _renderer.SetPropertyBlock(_propBlock);


    }

    public void MarkForDeletion()
    {
        isMarkedForDeletion = true;
    }
}