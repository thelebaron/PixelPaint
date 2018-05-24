using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Generate paint decals
/// </summary>
public class PainterScript : MonoBehaviour
{
    public static PainterScript Instance;
    public Color paintColor;

    /// <summary>
    /// A single paint decal to instantiate
    /// </summary>
    public Transform PaintPrefab;
    public Texture2D brushTexture;

    private int MinSplashs = 5;
    private int MaxSplashs = 15;
    private float SplashRange = 2f;

    private float MinScale = 0.25f;
    private float MaxScale = 2.5f;

    // DEBUG
    private bool mDrawDebug;
    private Vector3 mHitPoint;
    private List<Ray> mRaysDebug = new List<Ray>();

    void Awake()
    {
        if (Instance != null) Debug.LogError("More than one Painter has been instanciated in this scene!");
        Instance = this;

        if (PaintPrefab == null) Debug.LogError("Missing Paint decal prefab!");
    }

    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        // Check for a click
        bool paintonit = true;
        if (paintonit)
        {
            // Raycast
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // Paint!
                // Step back a little for a better effect (that's what "normal * x" is for)
                //Paint(hit.point + hit.normal * (SplashRange / 4f));


                
                var tex = hit.transform.GetComponent<Renderer>().material.mainTexture as Texture2D;

                
                // Find the u,v coordinate of the Texture
                Vector2 uv;
                uv.x = (hit.point.x - hit.collider.bounds.min.x) / hit.collider.bounds.size.x;
                uv.y = (hit.point.y - hit.collider.bounds.min.y) / hit.collider.bounds.size.y;
                // Paint it red
                tex.SetPixel((int)(-uv.x * tex.width), (int)(uv.y * tex.height), paintColor);
                tex.SetPixel((int)(-uv.x * tex.width), (int)(uv.y * tex.height) + 1, paintColor);
                tex.SetPixel((int)(-uv.x * tex.width) + 1, (int)(uv.y * tex.height), paintColor);
                tex.SetPixel((int)(-uv.x * tex.width), (int)(uv.y * tex.height) - 1, paintColor);
                tex.SetPixel((int)(-uv.x * tex.width) - 1, (int)(uv.y * tex.height), paintColor);
                tex.SetPixel((int)(-uv.x * tex.width) + 1, (int)(uv.y * tex.height) + 1, paintColor);
                tex.SetPixel((int)(-uv.x * tex.width) - 1, (int)(uv.y * tex.height) - 1, paintColor);
                tex.SetPixel((int)(-uv.x * tex.width) - 1, (int)(uv.y * tex.height) + 1, paintColor);
                tex.SetPixel((int)(-uv.x * tex.width) + 1, (int)(uv.y * tex.height) - 1, paintColor);
                tex.Apply();

                /*
                 
                             // Find the u,v coordinate of the Texture
                Vector2 uv;
                uv.x = (hit.point.x - hit.collider.bounds.min.x) / hit.collider.bounds.size.x;
                uv.y = (hit.point.y - hit.collider.bounds.min.y) / hit.collider.bounds.size.y;
                // Paint it red
                tex.SetPixel((int)(-uv.x * tex.width), (int)(uv.y * tex.height), paintColor);
                tex.SetPixel((int)(-uv.x * tex.width), (int)(uv.y * tex.height) + 1, paintColor);
                tex.SetPixel((int)(-uv.x * tex.width) + 1, (int)(uv.y * tex.height), paintColor);
                tex.SetPixel((int)(-uv.x * tex.width), (int)(uv.y * tex.height) - 1, paintColor);
                tex.SetPixel((int)(-uv.x * tex.width) - 1, (int)(uv.y * tex.height), paintColor);
                tex.SetPixel((int)(-uv.x * tex.width) + 1, (int)(uv.y * tex.height) + 1, paintColor);
                tex.SetPixel((int)(-uv.x * tex.width) - 1, (int)(uv.y * tex.height) - 1, paintColor);
                tex.SetPixel((int)(-uv.x * tex.width) - 1, (int)(uv.y * tex.height) + 1, paintColor);
                tex.SetPixel((int)(-uv.x * tex.width) + 1, (int)(uv.y * tex.height) - 1, paintColor);
                tex.Apply();
             */











                /*
                var blood = new Texture2D(2, 2, TextureFormat.ARGB32, false); //brushTexture;
                blood.SetPixels(brushTexture.GetPixels());
                blood.Apply();


                Vector2 pixelUV = hit.lightmapCoord;
                for (int y = 0; y < blood.height; y++)
                {
                    for (int x = 0; x < blood.width; x++)
                    {
                        Color color = paintColor;
                        tex.SetPixel(x, y, color);
                    }
                }
                tex.Apply();

    */
            }
        }
    }

    void Brush(Texture2D tex)
    {
        if (tex != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    // Find the u,v coordinate of the Texture
                    Vector2 uv;
                    uv.x = (hit.point.x - hit.collider.bounds.min.x) / hit.collider.bounds.size.x;
                    uv.y = (hit.point.y - hit.collider.bounds.min.y) / hit.collider.bounds.size.y;
                    // Paint it red
                    tex.SetPixel((int)(-uv.x * tex.width), (int)(uv.y * tex.height), paintColor);
                    tex.SetPixel((int)(-uv.x * tex.width), (int)(uv.y * tex.height) + 1, paintColor);
                    tex.SetPixel((int)(-uv.x * tex.width) + 1, (int)(uv.y * tex.height), paintColor);
                    tex.SetPixel((int)(-uv.x * tex.width), (int)(uv.y * tex.height) - 1, paintColor);
                    tex.SetPixel((int)(-uv.x * tex.width) - 1, (int)(uv.y * tex.height), paintColor);
                    tex.SetPixel((int)(-uv.x * tex.width) + 1, (int)(uv.y * tex.height) + 1, paintColor);
                    tex.SetPixel((int)(-uv.x * tex.width) - 1, (int)(uv.y * tex.height) - 1, paintColor);
                    tex.SetPixel((int)(-uv.x * tex.width) - 1, (int)(uv.y * tex.height) + 1, paintColor);
                    tex.SetPixel((int)(-uv.x * tex.width) + 1, (int)(uv.y * tex.height) - 1, paintColor);
                    tex.Apply();
                }
            
            if (Input.GetButton("Fire2"))
            {
                for (int i = 0; i < 128; i++)
                {
                    for (int j = 0; j < 128; j++)
                    {
                        tex.SetPixel(i, j, Color.white);
                    }
                }
                tex.Apply();
            }
        }
    }

    public void Paint(Vector3 location)
    {
        //DEBUG
        mHitPoint = location;
        mRaysDebug.Clear();
        mDrawDebug = true;

        int n = -1;

        int drops = Random.Range(MinSplashs, MaxSplashs);
        RaycastHit hit;

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
                var paintSplatter = GameObject.Instantiate(PaintPrefab,
                                                           hit.point,

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

        }
    }

    void OnDrawGizmos()
    {
        // DEBUG
        if (mDrawDebug)
        {
            Gizmos.DrawSphere(mHitPoint, 0.2f);
            foreach (var r in mRaysDebug)
            {
                Gizmos.DrawRay(r);
            }
        }
    }
}
