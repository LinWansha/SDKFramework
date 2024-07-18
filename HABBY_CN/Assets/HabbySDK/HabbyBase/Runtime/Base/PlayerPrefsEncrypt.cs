namespace Habby.Base
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using UnityEngine;
    using System.Collections;
    
public class PlayerPrefsEncrypt
{
    private static string sKEY = "ZTdkNTNmNDE2NTM3MWM0NDFhNTEzNzU1";
    private static string sIV = "4rZymEMfa/PpeJ89qY4gyA==";
    public static void SetInt(string key, int val)
    {
        PlayerPrefs.SetString(PlayerPrefsEncrypt.GetHash(key), PlayerPrefsEncrypt.Encrypt(val.ToString()));
    }
    public static int GetInt(string key, int defaultValue = 0)
    {
        string @string = PlayerPrefsEncrypt.GetString(key, defaultValue.ToString());
        int result = defaultValue;
        int.TryParse(@string, out result);
        return result;
    }
    public static void SetBool(string key, bool val)
    {
        PlayerPrefs.SetString(PlayerPrefsEncrypt.GetHash(key), PlayerPrefsEncrypt.Encrypt(val.ToString()));
    }
    public static bool GetBool(string key, bool defaultValue = false)
    {
        string @string = PlayerPrefsEncrypt.GetString(key, defaultValue.ToString());
        bool result = defaultValue;
        bool.TryParse(@string, out result);
        return result;
    }
    public static void SetUInt(string key, uint val)
    {
        PlayerPrefs.SetString(PlayerPrefsEncrypt.GetHash(key), PlayerPrefsEncrypt.Encrypt(val.ToString()));
    }
    public static uint GetUInt(string key, uint defaultValue = 0)
    {
        string @string = PlayerPrefsEncrypt.GetString(key, defaultValue.ToString());
        uint result = defaultValue;
        uint.TryParse(@string, out result);
        return result;
    }
    public static void SetLong(string key, long val)
    {
        PlayerPrefs.SetString(PlayerPrefsEncrypt.GetHash(key), PlayerPrefsEncrypt.Encrypt(val.ToString()));
    }
    public static long GetLong(string key, long defaultValue = 0L)
    {
        string @string = PlayerPrefsEncrypt.GetString(key, defaultValue.ToString());
        long result = defaultValue;
        long.TryParse(@string, out result);
        return result;
    }
    public static void SetULong(string key, ulong val)
    {
        PlayerPrefs.SetString(PlayerPrefsEncrypt.GetHash(key), PlayerPrefsEncrypt.Encrypt(val.ToString()));
    }
    public static ulong GetULong(string key, ulong defaultValue = 0L)
    {
        string @string = PlayerPrefsEncrypt.GetString(key, defaultValue.ToString());
        ulong result = defaultValue;
        ulong.TryParse(@string, out result);
        return result;
    }
    public static void SetFloat(string key, float val)
    {
        PlayerPrefs.SetString(PlayerPrefsEncrypt.GetHash(key), PlayerPrefsEncrypt.Encrypt(val.ToString()));
    }
    public static float GetFloat(string key, float defaultValue = 0f)
    {
        string @string = PlayerPrefsEncrypt.GetString(key, defaultValue.ToString());
        float result = defaultValue;
        float.TryParse(@string, out result);
        return result;
    }
    public static void SetString(string key, string val)
    {
        PlayerPrefs.SetString(PlayerPrefsEncrypt.GetHash(key), PlayerPrefsEncrypt.Encrypt(val));
    }
    public static string GetString(string key, string defaultValue = "")
    {
        string text = defaultValue;
        string @string = PlayerPrefs.GetString(PlayerPrefsEncrypt.GetHash(key), defaultValue.ToString());
        if (!text.Equals(@string))
        {
            text = PlayerPrefsEncrypt.Decrypt(@string);
        }
        return text;
    }
    public static bool HasKey(string key)
    {
        string hash = PlayerPrefsEncrypt.GetHash(key);
        return PlayerPrefs.HasKey(hash);
    }
    public static void DeleteKey(string key)
    {
        string hash = PlayerPrefsEncrypt.GetHash(key);
        PlayerPrefs.DeleteKey(hash);
    }
    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
    public static void Save()
    {
        PlayerPrefs.Save();
    }
    private static string Decrypt(string encString)
    {
        RijndaelManaged rijndaelManaged = new RijndaelManaged
        {
            Padding = PaddingMode.Zeros,
            Mode = CipherMode.CBC,
            KeySize = 128,
            BlockSize = 128
        };
        byte[] bytes = Encoding.UTF8.GetBytes(PlayerPrefsEncrypt.sKEY);
        byte[] rgbIV = Convert.FromBase64String(PlayerPrefsEncrypt.sIV);
        ICryptoTransform transform = rijndaelManaged.CreateDecryptor(bytes, rgbIV);
        byte[] array = Convert.FromBase64String(encString);
        byte[] array2 = new byte[array.Length];
        MemoryStream stream = new MemoryStream(array);
        CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Read);
        cryptoStream.Read(array2, 0, array2.Length);
        //Debugger.Log("Encoding.UTF8.GetString(array2).TrimEnd(new char[1] " + Encoding.UTF8.GetString(array2).TrimEnd(new char[1]));
        return Encoding.UTF8.GetString(array2).TrimEnd(new char[1]);
    }
    private static string Encrypt(string rawString)
    {
        RijndaelManaged rijndaelManaged = new RijndaelManaged
        {
            Padding = PaddingMode.Zeros,
            Mode = CipherMode.CBC,
            KeySize = 128,
            BlockSize = 128
        };
        byte[] bytes = Encoding.UTF8.GetBytes(PlayerPrefsEncrypt.sKEY);
        byte[] rgbIV = Convert.FromBase64String(PlayerPrefsEncrypt.sIV);
        ICryptoTransform transform = rijndaelManaged.CreateEncryptor(bytes, rgbIV);
        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
        byte[] bytes2 = Encoding.UTF8.GetBytes(rawString);
        cryptoStream.Write(bytes2, 0, bytes2.Length);
        cryptoStream.FlushFinalBlock();
        byte[] inArray = memoryStream.ToArray();
        return Convert.ToBase64String(inArray);
    }
    private static string GetHash(string key)
    {
        MD5 mD = new MD5CryptoServiceProvider();
        byte[] array = mD.ComputeHash(Encoding.UTF8.GetBytes(key));
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < array.Length; i++)
        {
            stringBuilder.Append(array[i].ToString("x2"));
        }
        return stringBuilder.ToString();
    }
    /// <summary>
    /// 用MD5加密字符串，可选择生成16位或者32位的加密字符串
    /// </summary>
    /// <param name="password">待加密的字符串</param>
    /// <param name="bit">位数，一般取值16 或 32</param>
    /// <returns>返回的加密后的字符串</returns>
    public static string MD5Encrypt(string password, int bit = 32)
    {
        MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
        byte[] hashedDataBytes;
        hashedDataBytes = md5Hasher.ComputeHash(Encoding.GetEncoding("gb2312").GetBytes(password));
        StringBuilder tmp = new StringBuilder();
        foreach (byte i in hashedDataBytes)
        {
            tmp.Append(i.ToString("x2"));
        }
        if (bit == 16)
            return tmp.ToString().Substring(8, 16);
        else
        if (bit == 32) return tmp.ToString();//默认情况
        else return string.Empty;
    }

    //  //默认密钥向量
    //  private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
    //  public static string keyss = "1234567z";
    //  /// <summary>
    //  /// DES加密字符串
    //  /// </summary>
    //  /// <param name="encryptString">待加密的字符串</param>
    //  /// <param name="encryptKey">加密密钥,要求为8位</param>
    //  /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
    //  public static string EncryptDES(string encryptString, string encryptKey)
    //  {
    //      try
    //      {
    //          byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
    //          byte[] rgbIV = Keys;
    //          byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
    //          DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
    //          MemoryStream mStream = new MemoryStream();
    //          CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
    //          cStream.Write(inputByteArray, 0, inputByteArray.Length);
    //          cStream.FlushFinalBlock();
    //          return Convert.ToBase64String(mStream.ToArray());
    //      }
    //      catch
    //      {
    //          return encryptString;
    //      }
    //  }/// <summary>
    //  /// DES解密字符串
    //  /// </summary>
    //  /// <param name="decryptString">待解密的字符串</param>
    //  /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
    //  /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
    //  public static string DecryptDES(string decryptString, string decryptKey)
    //  {
    //      try
    //      {
    //          byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
    //          byte[] rgbIV = Keys;
    //          byte[] inputByteArray = Convert.FromBase64String(decryptString);
    //          DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
    //          MemoryStream mStream = new MemoryStream();
    //          CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
    //          cStream.Write(inputByteArray, 0, inputByteArray.Length);
    //          cStream.FlushFinalBlock();
    //          return Encoding.UTF8.GetString(mStream.ToArray());
    //      }
    //      catch
    //      {
    //          return decryptString;
    //      }
    //  }
}

public class PlayerPrefsEncryptBase
{
    private byte[] bytes_key = new byte[] { 104, 77, 211, 46, 22, 23, 49, 62, 228, 115, 215, 20, 227, 146, 123, 190, 9, 249, 114, 218, 156, 48, 227, 83, 72, 87, 61, 130, 203, 122, 124, 102 };
    private byte[] bytes_iv = new byte[] { 213, 15, 172, 203, 65, 236, 96, 215, 197, 32, 96, 219, 243, 189, 17, 34 };
    public PlayerPrefsEncryptBase(byte[] _key, byte[] _iv)
    {
        bytes_key = _key;
        bytes_iv = _iv;
    }
    public void SetInt(string key, int val)
    {
        PlayerPrefs.SetString(GetHash(key), Encrypt(val.ToString()));
    }
    public int GetInt(string key, int defaultValue = 0)
    {
        string @string = GetString(key, defaultValue.ToString());
        int result = defaultValue;
        int.TryParse(@string, out result);
        return result;
    }
    public void SetBool(string key, bool val)
    {
        PlayerPrefs.SetString(GetHash(key), Encrypt(val.ToString()));
    }
    public bool GetBool(string key, bool defaultValue = false)
    {
        string @string = GetString(key, defaultValue.ToString());
        bool result = defaultValue;
        bool.TryParse(@string, out result);
        return result;
    }
    public void SetUInt(string key, uint val)
    {
        PlayerPrefs.SetString(GetHash(key), Encrypt(val.ToString()));
    }
    public uint GetUInt(string key, uint defaultValue = 0)
    {
        string @string = GetString(key, defaultValue.ToString());
        uint result = defaultValue;
        uint.TryParse(@string, out result);
        return result;
    }
    public void SetLong(string key, long val)
    {
        PlayerPrefs.SetString(GetHash(key), Encrypt(val.ToString()));
    }
    public long GetLong(string key, long defaultValue = 0L)
    {
        string @string = GetString(key, defaultValue.ToString());
        long result = defaultValue;
        long.TryParse(@string, out result);
        return result;
    }
    public void SetULong(string key, ulong val)
    {
        PlayerPrefs.SetString(GetHash(key), Encrypt(val.ToString()));
    }
    public ulong GetULong(string key, ulong defaultValue = 0L)
    {
        string @string = GetString(key, defaultValue.ToString());
        ulong result = defaultValue;
        ulong.TryParse(@string, out result);
        return result;
    }
    public void SetFloat(string key, float val)
    {
        PlayerPrefs.SetString(GetHash(key), Encrypt(val.ToString()));
    }
    public float GetFloat(string key, float defaultValue = 0f)
    {
        string @string = GetString(key, defaultValue.ToString());
        float result = defaultValue;
        float.TryParse(@string, out result);
        return result;
    }
    public void SetString(string key, string val)
    {
        PlayerPrefs.SetString(GetHash(key), Encrypt(val));
    }
    public string GetString(string key, string defaultValue = "")
    {
        string text = defaultValue;
        string @string = PlayerPrefs.GetString(GetHash(key), defaultValue.ToString());
        if (!text.Equals(@string))
        {
            text = Decrypt(@string);
        }
        return text;
    }
    public bool HasKey(string key)
    {
        string hash = GetHash(key);
        return PlayerPrefs.HasKey(hash);
    }
    public void DeleteKey(string key)
    {
        string hash = GetHash(key);
        PlayerPrefs.DeleteKey(hash);
    }
    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
    public void Save()
    {
        PlayerPrefs.Save();
    }
    public string Decrypt(string encString)
    {
        RijndaelManaged rijndaelManaged = new RijndaelManaged
        {
            Padding = PaddingMode.Zeros,
            Mode = CipherMode.CBC,
            KeySize = 128,
            BlockSize = 128
        };
        byte[] bytes = bytes_key;//Encoding.UTF8.GetBytes(sKEY);
        byte[] rgbIV = bytes_iv;//Convert.FromBase64String(sIV);
        ICryptoTransform transform = rijndaelManaged.CreateDecryptor(bytes, rgbIV);
        byte[] array = Convert.FromBase64String(encString);
        byte[] array2 = new byte[array.Length];
        MemoryStream stream = new MemoryStream(array);
        CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Read);
        cryptoStream.Read(array2, 0, array2.Length);
        //Debugger.Log("Encoding.UTF8.GetString(array2).TrimEnd(new char[1] " + Encoding.UTF8.GetString(array2).TrimEnd(new char[1]));
        return Encoding.UTF8.GetString(array2).TrimEnd(new char[1]);
    }
    public string Encrypt(string rawString)
    {
        RijndaelManaged rijndaelManaged = new RijndaelManaged
        {
            Padding = PaddingMode.Zeros,
            Mode = CipherMode.CBC,
            KeySize = 128,
            BlockSize = 128
        };
        byte[] bytes = bytes_key;//Encoding.UTF8.GetBytes(sKEY);
        byte[] rgbIV = bytes_iv;//Convert.FromBase64String(sIV);
        ICryptoTransform transform = rijndaelManaged.CreateEncryptor(bytes, rgbIV);
        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
        byte[] bytes2 = Encoding.UTF8.GetBytes(rawString);
        cryptoStream.Write(bytes2, 0, bytes2.Length);
        cryptoStream.FlushFinalBlock();
        byte[] inArray = memoryStream.ToArray();
        return Convert.ToBase64String(inArray);
    }
    private string GetHash(string key)
    {
        MD5 mD = new MD5CryptoServiceProvider();
        byte[] array = mD.ComputeHash(Encoding.UTF8.GetBytes(key));
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < array.Length; i++)
        {
            stringBuilder.Append(array[i].ToString("x2"));
        }
        return stringBuilder.ToString();
    }
    /// <summary>
    /// 用MD5加密字符串，可选择生成16位或者32位的加密字符串
    /// </summary>
    /// <param name="password">待加密的字符串</param>
    /// <param name="bit">位数，一般取值16 或 32</param>
    /// <returns>返回的加密后的字符串</returns>
    public string MD5Encrypt(string password, int bit = 32)
    {
        MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
        byte[] hashedDataBytes;
        hashedDataBytes = md5Hasher.ComputeHash(Encoding.GetEncoding("gb2312").GetBytes(password));
        StringBuilder tmp = new StringBuilder();
        foreach (byte i in hashedDataBytes)
        {
            tmp.Append(i.ToString("x2"));
        }
        if (bit == 16)
            return tmp.ToString().Substring(8, 16);
        else
        if (bit == 32) return tmp.ToString();//默认情况
        else return string.Empty;
    }
}
}