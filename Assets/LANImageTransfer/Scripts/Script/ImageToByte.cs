using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ImageToByte : MonoBehaviour
{
    private RenderTexture renderTexture, compressTex;
    [SerializeField] private RawImage screenRenderImageUI;
    [SerializeField] Material imageManageMat;

    [SerializeField] ByteArrayUnityEvent OnBytesReady;

    [SerializeField] int byteLength;

    void Start()
    {
        renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
        compressTex = new RenderTexture(Screen.width / 2, Screen.height / 2, 0);
        screenRenderImageUI.texture = compressTex;
        
    }

    void FixedUpdate()
    {
        StartCoroutine(CaptureSCreen());
    }
    IEnumerator CaptureSCreen()
    {
        yield return new WaitForEndOfFrame();
        ScreenCapture.CaptureScreenshotIntoRenderTexture(renderTexture);
        Graphics.Blit(renderTexture, compressTex, imageManageMat);
        AsyncGPUReadback.Request(compressTex, 0, TextureFormat.RGB24, ReadbackCompleted);
    }

    void ReadbackCompleted(AsyncGPUReadbackRequest request)
    {
        // Render texture no longer needed, it has been read back.
        ///DestroyImmediate(renderTexture);

        using (var imageBytes = request.GetData<byte>())
        {
            byte[] imageData = imageBytes.ToArray();
            byteLength = imageData.Length;
            OnBytesReady.Invoke(imageData);
        }
    }
}
