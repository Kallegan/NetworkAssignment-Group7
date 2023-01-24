using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialPropertyBlockPlasma : MonoBehaviour
{
    public Color startColor, EndColor, defaultColor;  

    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;
    
    private bool projectileDeflected = false;

    [SerializeField] private float colorFlashTimer = 0.2f;
    private float currentColorFlashTimer;
    


    void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();
        _propBlock.SetColor("_Color", defaultColor);
        _renderer.SetPropertyBlock(_propBlock);
    }

    void Update()
    {
        _renderer.GetPropertyBlock(_propBlock);

        if (projectileDeflected)
            _propBlock.SetColor("_Color", Color.Lerp(startColor, EndColor, (Mathf.Sin(Time.time * 20f + 0) + 1) / 2f));  

        

        if(currentColorFlashTimer <= 0)
        {
            _propBlock.SetColor("_Color", defaultColor);
            projectileDeflected = false;
            currentColorFlashTimer = colorFlashTimer;
        }

        currentColorFlashTimer -= Time.deltaTime;
        _renderer.SetPropertyBlock(_propBlock);
    }

    public void SuccessDeflect()
    {
        projectileDeflected = true;
    }

    public void ResetShield()
    {
        projectileDeflected = false;
    }

}
