using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush : MonoBehaviour
{
    /// <summary>
    /// Debug stuff
    /// </summary>
    public Renderer editorRenderer;
    public Texture2D editorTexture2D;
    public int MinSplash = 5;
    public int MaxSplash = 115;
    public int PixelRange = 50; 
    private float SplashRange = 2f;

    private float MinScale = 0.25f;
    private float MaxScale = 2.5f;







    public int resolution = 512;
    Texture2D whiteMap;
    public float brushSize;
    public Texture2D brushTexture;
    Vector2 stored;
    public static Dictionary<Collider, RenderTexture> paintTextures = new Dictionary<Collider, RenderTexture>();
    void Start()
    {
        CreateClearTexture();// clear white texture to draw on
    }

    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * 20f, Color.magenta);
        RaycastHit hit;

        // uncomment for mouse painting
        //if (Physics.Raycast(transform.position, Vector3.down, out hit))
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) /*&& Input.GetKeyDown(KeyCode.Space)*/) 
        {
            /// debug ray
            Debug.DrawRay(hit.point, (hit.normal * 5f), Color.magenta);

            if (stored != hit.lightmapCoord) // stop drawing on the same point
            {
                Paint(hit.point, hit);
                stored = hit.lightmapCoord;
            }

            Collider coll = hit.collider;
            return;
            //old code for rendertexture
            if (coll != null)
            {
                if (!paintTextures.ContainsKey(coll)) // if there is already paint on the material, add to that
                {
                    Renderer rend = hit.transform.GetComponent<Renderer>();
                    paintTextures.Add(coll, getWhiteRT());
                    rend.material.SetTexture("_PaintMap", paintTextures[coll]);


                }
                if (stored != hit.lightmapCoord) // stop drawing on the same point
                {

                    ///

                    
                   
                    stored = hit.lightmapCoord;
                    Vector2 pixelUV = hit.lightmapCoord;
                    pixelUV.y *= resolution;
                    pixelUV.x *= resolution;

                    ///
                    //editorTexture2D.SetPixel((int)pixelUV.x, (int)pixelUV.y, Color.red);
                    //editorTexture2D.Apply();
                    
                    //old//DrawTexture(paintTextures[coll], pixelUV.x, pixelUV.y);
                    
                }
            }
        }
    }

    void DrawTexture(RenderTexture rt, float posX, float posY)
    {
        RenderTexture.active = rt; // activate rendertexture for drawtexture;
        GL.PushMatrix();                       // save matrixes
        GL.LoadPixelMatrix(0, resolution, resolution, 0);      // setup matrix for correct size

        // draw brushtexture
        Graphics.DrawTexture(new Rect(posX - brushTexture.width / brushSize, (rt.height - posY) - brushTexture.height / brushSize, brushTexture.width / (brushSize * 0.5f), brushTexture.height / (brushSize * 0.5f)), brushTexture);
        GL.PopMatrix();
        RenderTexture.active = null;// turn off rendertexture


    }

    RenderTexture getWhiteRT()
    {
        RenderTexture rt = new RenderTexture(resolution, resolution, 32);
        Graphics.Blit(whiteMap, rt);
        return rt;
    }

    [AddComponentMenu("clear")]
    void CreateClearTexture()
    {
        whiteMap = new Texture2D(1, 1);
        whiteMap.SetPixel(0, 0, Color.white);
        whiteMap.Apply();
    }


    public void Paint(Vector3 location, RaycastHit hit)
    {
        int drops = Random.Range(MinSplash, MaxSplash);
        int n = -1;



        editorRenderer = hit.transform.GetComponent<Renderer>();
        editorTexture2D = editorRenderer.material.GetTexture("_PaintMap") as Texture2D;
        resolution = editorTexture2D.height;

        stored = hit.lightmapCoord;
        Vector2 pixelUV2 = hit.lightmapCoord;
        pixelUV2.y *= resolution;
        pixelUV2.x *= resolution;


        while (n <= drops)
        {
            n++;

            float chancex = UnityEngine.Random.Range((float)0, (float)1);
            float chancey = UnityEngine.Random.Range((float)0, (float)1);

            int randomX = UnityEngine.Random.Range(-PixelRange, PixelRange);
            int randomY = UnityEngine.Random.Range(-PixelRange, PixelRange);
            //pixelUV2.x += randomX;
            //pixelUV2.y += randomY;
            if (chancex > 0.8)
                randomX *= randomX * randomX;
            if (chancey > 0.8)
                randomY *= randomY * randomX;

            editorTexture2D.SetPixel((int)(pixelUV2.x + randomX), (int)(pixelUV2.y + randomY), Color.red);
        }

        
        editorTexture2D.Apply();

        
        /*
        // Generate multiple decals in once
        while (n <= drops)
        {
            n++;

            // Get a random direction (beween -n and n for each vector component)
            var fwd = transform.TransformDirection(Random.onUnitSphere * SplashRange);

            mRaysDebug.Add(new Ray(location, fwd));

            // Raycast around the position to splash everwhere we can
            if (Physics.Raycast(location, fwd, out hit, SplashRange))
            {
                // Create a splash if we found a surface
                var paintSplatter = GameObject.Instantiate(PaintPrefab,hit.point,
                                                           // Rotation from the original sprite to the normal
                                                           // Prefab are currently oriented to z+ so we use the opposite
                                                           Quaternion.FromToRotation(Vector3.back, hit.normal)
                                                           ) as Transform;

                // Random scale
                var scaler = Random.Range(MinScale, MaxScale);

                paintSplatter.localScale = new Vector3(
                    paintSplatter.localScale.x * scaler,
                    paintSplatter.localScale.y * scaler,
                    paintSplatter.localScale.z
                );

                // Random rotation effect
                var rater = Random.Range(0, 359);
                paintSplatter.transform.RotateAround(hit.point, hit.normal, rater);


                // TODO: What do we do here? We kill them after some sec?
                Destroy(paintSplatter.gameObject, 25);
            }

        }*/
    }
}