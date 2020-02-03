using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWithCollider : MonoBehaviour
{
    public Shader DrawShader;
    public GameObject Terrain;

    private RaycastHit _raycastGround;
    private int _layerMask;
    private RenderTexture _splatMap;
    private Material _snowMaterial;
    private Material _drawMaterial;

    [Range(1, 500)]
    public float BrushSize;
    [Range(0, 1)]
    public float BrushStrength;


    // Start is called before the first frame update
    void Start()
    {
        _layerMask = LayerMask.GetMask("Ground");
        _drawMaterial = new Material(DrawShader);        

        _snowMaterial = Terrain.GetComponent<MeshRenderer>().material;
        _splatMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        _snowMaterial.SetTexture("_Splat", _splatMap);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionStay(Collision collision)
    {
        if(collision.collider != null)
        {
            
            foreach(ContactPoint contact in collision.contacts)
            {
               

                if(Physics.Raycast(transform.position, Vector3.down, out _raycastGround, 1f, _layerMask))
                {
                    
                    
                    _drawMaterial.SetVector("_Coordinate", new Vector4(_raycastGround.textureCoord.x, _raycastGround.textureCoord.y, 0, 0));                    
                    _drawMaterial.SetFloat("_Strength", BrushStrength);
                    _drawMaterial.SetFloat("_Size", BrushSize);
                    RenderTexture temp = RenderTexture.GetTemporary(_splatMap.width, _splatMap.height, 0, RenderTextureFormat.ARGBFloat);
                    Graphics.Blit(_splatMap, temp);
                    Graphics.Blit(temp, _splatMap, _drawMaterial);
                    RenderTexture.ReleaseTemporary(temp);
                    
                    
                }
                
            }

    
            
            
        }
    }

   
}
