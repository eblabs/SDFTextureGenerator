using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class PointsToSDF : MonoBehaviour
{
    //
    public RenderTexture outputTexture;
    Simplex.Procedures.MaskToSdfTextureProcedure _procedure;


    public void PointsToSDFEditor()
    {
        // test
        var sourceTexture = CreateRandomTexture(512, 512, 10);

        // setup
        _procedure?.Release();
        _procedure = new Simplex.Procedures.MaskToSdfTextureProcedure();

        // send to compute
        var _sourceTexture = sourceTexture;// null;
        var _sourceValueThreshold = 0.5f; // [SerializeField, Range(0f, 1f)] 
        var _sourceChannel = Simplex.Procedures.MaskToSdfTextureProcedure.TextureScalar.A;
        var _downSampling = Simplex.Procedures.MaskToSdfTextureProcedure.DownSampling.None;
        var _precision = Simplex.Procedures.MaskToSdfTextureProcedure.Precision._32;
        bool _addBorders = false;
        bool _showSource = false;

        _procedure.Update(_sourceTexture, _sourceValueThreshold, _sourceChannel, _downSampling, _precision, _addBorders, _showSource);

        // blit
        Graphics.Blit(_procedure.sdfTexture, outputTexture);
    }

    Texture2D CreateRandomTexture(int width, int height, int whitePixelCount)
    {
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBAFloat, false);
        Color[] pixels = new Color[width * height];

        // Initialize all pixels to black
        var black = new Color(0f, 0f, 0f, 0f);
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = black;
        }

        // Set random pixels to white
        var white = new Color(1f, 1f, 1f, 1f);
        for (int i = 0; i < whitePixelCount; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, pixels.Length);
            pixels[randomIndex] = white;
        }

        texture.SetPixels(pixels);
        texture.Apply();

        return texture;
    }

    /*
      Texture2D ConvertRenderTextureToTexture2D(RenderTexture rt)
      {
          // Create a new Texture2D with the same dimensions as the RenderTexture
          Texture2D texture2D = new Texture2D(rt.width, rt.height, TextureFormat.RGBAFloat, false);

          // Set the RenderTexture as the active RenderTexture
          RenderTexture.active = rt;

          // Read the pixels from the RenderTexture into the Texture2D
          texture2D.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);

          texture2D.Apply();

          // Reset the active RenderTexture
          RenderTexture.active = null;

          return texture2D;
      }
      */
}



#if UNITY_EDITOR

[CustomEditor(typeof(PointsToSDF))]
public class PointsToSDF_Editor : Editor
{
    public override void OnInspectorGUI()
    {

        if (GUILayout.Button("Points to SDF"))
        {

            foreach (var t in targets)
            {
                var component = (PointsToSDF)t;
                component.PointsToSDFEditor();
            }
        }

        //
        DrawDefaultInspector();
    }
}

#endif