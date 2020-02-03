using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPath : MonoBehaviour
{
    public Camera Camera;
    public Shader DrawShader;

    private RenderTexture _splatMap;
    private Material _snowMaterial;
    private Material _drawMaterial;
    private RaycastHit _hit;
    [Range(1, 500)]
    public float BrushSize;
    [Range(0, 1)]
    public float BrushStrength;
    // Start is called before the first frame update
    void Start()
    {
        _drawMaterial = new Material(DrawShader);
        _drawMaterial.SetVector("_Color", Color.red);

        _snowMaterial = GetComponent<MeshRenderer>().material;
        _splatMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        _snowMaterial.SetTexture("_Splat", _splatMap);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out _hit))
            {
                Debug.Log(_hit.textureCoord);
                _drawMaterial.SetVector("_Coordinate", new Vector4(_hit.textureCoord.x, _hit.textureCoord.y, 0, 0));
                _drawMaterial.SetFloat("_Strength", BrushStrength);
                _drawMaterial.SetFloat("_Size", BrushSize);
                RenderTexture temp = RenderTexture.GetTemporary(_splatMap.width, _splatMap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(_splatMap, temp);
                Graphics.Blit(temp, _splatMap, _drawMaterial);
                RenderTexture.ReleaseTemporary(temp);
            } 
        }
    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, 256, 256), _splatMap, ScaleMode.ScaleToFit, false, 1);
    }
}
