using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: 可以针对Base64做一次优化
public class LiteralXor
{
#if UNITY_EDITOR
    public static void enc(string key)
    {
        System.Random rand = new System.Random();
        int number = rand.Next();
        char min = Char.MaxValue;
        int[] data = new int[key.Length];
        

        for (int i = 0; i < key.Length; ++i)
        {
            if (key[i] < min) min = key[i];
        }

        for (int i = 0; i < key.Length; ++i)
        {
            int r = key[i] - min;
            r = ((r << 16) & 0x7FFFFFFF) + ((rand.Next() * 16807) & 0xFFFF);
            r = r ^ number;
            data[i] = r;
        }

        Debug.Log("number = " + number);
        Debug.Log("min = '" + min + "'");
        Debug.Log("{" + string.Join(",", data) + "}");
    }
#endif

    public static string dec(int number, char min, int[] data)
    {
        char[] result = new char[data.Length];

        for(int i= 0; i< data.Length; ++i)
        {
            int r = data[i] ^ number;
            r = (r >> 16) & 0x7FFF;
            result[i] = Convert.ToChar(r + min);
        }
        
        return new string(result);
    }
}
