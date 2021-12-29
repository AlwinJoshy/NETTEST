using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
public class ImageExtractor : MonoBehaviour, IImageExtract
{

    [SerializeField] RawImage imageTex;
    byte[] imageDataWriteArray = new byte[32768];
    int writeIndex = 0;

    Texture2D tex;


    void Awake()
    {
        tex = new Texture2D(128, 128, TextureFormat.RGB565, false);
    }

    public void ExtractImage(byte[] data)
    {
        int a = 0;
        int dataID = BitwiseRead.ReadInt(data, ref a);
        int packetID = BitwiseRead.ReadInt(data, ref a);
        int packetCount = BitwiseRead.ReadInt(data, ref a);
        int readLength = BitwiseRead.ReadInt(data, ref a);

        BitwiseRead.ReadIntoArray(data, a, ref imageDataWriteArray, ref packetID, readLength);

        /*
        if(packetID + 1 == packetCount)
        {
            writeIndex = 0;
        }
        */

        //CustomLog.Log(writeIndex + "|" + '\n');

        //CustomLog.Log("Got It");

        tex.LoadRawTextureData(imageDataWriteArray);
        tex.Apply();
        imageTex.texture = tex;



        //ImageConversion.EncodeArrayToJPG(imageDataWriteArray, UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_UNorm, 256, 256, 256 * 4, 75);


    }

}
