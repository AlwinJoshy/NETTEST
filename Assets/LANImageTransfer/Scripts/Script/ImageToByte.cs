using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ImageToByte : MonoBehaviour
{
    public RenderTexture renderTexture, compressTex, textTex;
    [SerializeField] private RawImage screenRenderImageUI;
    [SerializeField] Material imageManageMat;

    [SerializeField] ByteArrayUnityEvent OnBytesReady;

    [SerializeField] string byteLength;

    void Start()
    {
        renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
        compressTex = new RenderTexture(Screen.width / 4, Screen.height / 4, 0);
        screenRenderImageUI.texture = compressTex;

    }

    float nextUpdate = 0;

    void FixedUpdate()
    {
        if (Time.time > nextUpdate)
        {
            StartCoroutine(CaptureSCreen());
            nextUpdate = Time.time + 1 / 5;
        }
    }
    IEnumerator CaptureSCreen()
    {
        yield return new WaitForEndOfFrame();
        ScreenCapture.CaptureScreenshotIntoRenderTexture(renderTexture);
        Graphics.Blit(renderTexture, compressTex, imageManageMat);
        AsyncGPUReadback.Request(compressTex, 0, TextureFormat.RGB565, ReadbackCompleted);
    }

    void ReadbackCompleted(AsyncGPUReadbackRequest request)
    {
        // Render texture no longer needed, it has been read back.
        ///DestroyImmediate(renderTexture);

        using (var imageBytes = request.GetData<byte>())
        {
            byte[] imageData = imageBytes.ToArray();
            byte[] comData = Ziper.Compress(imageData);
            byteLength = imageData.Length + " | " + comData.Length;
            OnBytesReady.Invoke(comData);
        }
    }
}
