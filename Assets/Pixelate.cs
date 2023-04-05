using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class Pixelate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [SerializeField]
    private Material postprocessMaterial;

    [Range(1, 32)] public int intRange = 1;
    [Range(1, 16)] public int iterations = 1;
    //method which is automatically called by unity after the camera is done rendering
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //RenderTexture r = RenderTexture.GetTemporary( //get a hold of a temporary texture
        //    source.width, source.height, 0, source.format //no need for depth which is why the third parameter is 0, source format is for matching the camera settings.
        // );
        int width = source.width / intRange;
        int height = source.height / intRange;
        RenderTexture[] textures = new RenderTexture[16];
        RenderTextureFormat format = source.format;

        RenderTexture  currentDest = RenderTexture.GetTemporary(width, height, 0, format);
        currentDest.filterMode = FilterMode.Point;
        //Graphics.Blit(source, destination);//Copies source texture into destination render texture with a shader
        //Graphics.Blit(source, r);//Blit to a temporary destination
        //Graphics.Blit(r, destination);//Blit to the destination
        //RenderTexture.ReleaseTemporary(r);//To make it available for reuse, release it by invoking 

        Graphics.Blit(source, currentDest);
        RenderTexture currentSource = currentDest;
        currentSource.filterMode = FilterMode.Point;
        Graphics.Blit(currentSource, destination);
        RenderTexture.ReleaseTemporary(currentSource);

        for (int i = 1; i < iterations; i++)
        {
            width /= 2;
            height /= 2;
            currentDest = textures[i] = RenderTexture.GetTemporary(width, height, 0, format);
            if (height < 2)
            {
                break;
            }
            currentDest =
                RenderTexture.GetTemporary(width, height, 0, format);
            Graphics.Blit(currentSource, currentDest);
            RenderTexture.ReleaseTemporary(currentSource);
            currentSource = currentDest;
        }


        for (int i = 1; i < iterations; i++)
        {
            Graphics.Blit(currentSource, currentDest);
            //			RenderTexture.ReleaseTemporary(currentSource);
            currentSource = currentDest;

        }

        for (int i = 1; i >= 0; i--)
        {
            textures[i].filterMode = FilterMode.Point;
            textures[i].antiAliasing = 16;
            currentDest = textures[i];
            textures[i] = null;
            Graphics.Blit(currentSource, currentDest);
            RenderTexture.ReleaseTemporary(currentSource);
            currentSource = currentDest;
        }
    }
}
