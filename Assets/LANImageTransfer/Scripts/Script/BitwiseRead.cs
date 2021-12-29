using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BitwiseRead
{
    static public int ReadInt(byte[] data, ref int index)
    {
        int x = index;
        index += 4;
        return (int)(((data[x]) << 24) | ((data[x + 1] & 0xff) << 16) | ((data[x + 2] & 0xff) << 8) | ((data[x + 3] & 0xff)));
    }

    static public char ReadChar(byte[] data, ref int index)
    {
        int x = index;
        index += 1;
        return (char)((data[x] & 0xff));
    }

    static public void ReadIntoArray(byte[] readArray, int readIndex, ref byte[] writeArray, ref int writeIndex, int writeLength)
    {
        try
        {
            for (int i = 0; i < writeLength; i++)
            {
                writeArray[writeIndex + i] = readArray[readIndex + i];
            }
            writeIndex += writeLength;
        }
        catch (System.Exception)
        {

            CustomLog.Log(writeArray.Length.ToString());
        }

    }
}
