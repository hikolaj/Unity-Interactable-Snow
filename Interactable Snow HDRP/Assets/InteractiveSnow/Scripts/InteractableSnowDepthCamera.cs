using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[RequireComponent(typeof(Camera))]
public class InteractableSnowDepthCamera : MonoBehaviour
{
    public Camera DepthCamera;
    public Shader temporalTestShader;
    public RenderTexture rt;
    public Material mat;
    /*
    void Start()
    {
        DepthCamera = GetComponent<Camera>();
        rt = new RenderTexture(Screen.width, Screen.height, 0);
        temporalMaterial = new Material(temporalTestShader);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        mat.SetTexture("something", sometex);
        RenderTexture temp = RenderTexture.GetTemporary(...);
        Blit(A, temp, mat); // Add A to "something", save to temp
        Blit(temp, B); // Copy temp to some result texture B
        rt.Release(temp);
    }
    */

}
