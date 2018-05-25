using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class act : MonoBehaviour
{
    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        // duplicate the original texture and assign to the material

        Texture2D mainTexInstance = Instantiate(rend.material.mainTexture) as Texture2D;
        Texture2D paintTexInstance = Instantiate(rend.material.GetTexture("_PaintMap") as Texture2D) as Texture2D;

        var basetex = new Texture2D(mainTexInstance.width, mainTexInstance.height, TextureFormat.ARGB32, false);
        basetex.filterMode = FilterMode.Point;
        basetex.SetPixels(mainTexInstance.GetPixels());
        basetex.Apply();

        var painttex = new Texture2D(paintTexInstance.width, paintTexInstance.height, TextureFormat.ARGB32, false);
        painttex.filterMode = FilterMode.Point;
        painttex.SetPixels(paintTexInstance.GetPixels());
        painttex.Apply();



        rend.material.mainTexture = basetex;
        rend.material.SetTexture("_PaintMap", painttex);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
