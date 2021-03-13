using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowPhysicsSimulation : MonoBehaviour
{
    [HideInInspector] public ComputeShader calculateEngine;

    public Texture cameraInput;

    public Material snowMaterial;

    public float recoveryTime;

    public RenderTexture[] RTexture;
    private int rtextureId;

    public Texture SnowTexture
    {
        get { return RTexture[rtextureId]; }
    }

    public Texture SnowBuffer
    {
        get { return RTexture[(rtextureId + 1) % 2]; }
    }

    private int physicsSimulationId;

    void Start()//Awake
    {

        physicsSimulationId = calculateEngine.FindKernel("SnowPhysicsUpdate");
        int overwrite = calculateEngine.FindKernel("SnowFlashInput");
        
        RTexture = new RenderTexture[2];
        for(int i = 0; i < RTexture.Length; i++)//++i
        {
            RTexture[i] = new RenderTexture(cameraInput.width, cameraInput.height, 24);
            RTexture[i].format = RenderTextureFormat.RFloat;
            RTexture[i].wrapMode = TextureWrapMode.Repeat;
            RTexture[i].filterMode = FilterMode.Point;
            RTexture[i].enableRandomWrite = true;
            RTexture[i].Create();
        }

        calculateEngine.SetTexture(overwrite, "Input", cameraInput);
        calculateEngine.SetTexture(overwrite, "PreviousState", SnowBuffer);
        calculateEngine.SetTexture(overwrite, "Result", SnowTexture);
        calculateEngine.Dispatch(overwrite, SnowTexture.width / 8, SnowTexture.height / 8, 1);

        calculateEngine.SetFloat("Width", SnowTexture.width);
        calculateEngine.SetFloat("Height", SnowTexture.height);
        calculateEngine.SetTexture(physicsSimulationId, "Input", cameraInput);
    }

    void FixedUpdate()
    {
        calculateEngine.SetFloat("RecoveryTime", recoveryTime);
        calculateEngine.SetFloat("ElapsedTime", Time.fixedDeltaTime);
        calculateEngine.SetTexture(physicsSimulationId, "PreviousState", SnowBuffer);
        calculateEngine.SetTexture(physicsSimulationId, "Result", SnowTexture);
        calculateEngine.Dispatch(physicsSimulationId, SnowTexture.width / 8, SnowTexture.height / 8, 1);
        
        snowMaterial.SetTexture("_SnowMask", SnowTexture);
        rtextureId = (rtextureId + 1) % 2;
    }
}
