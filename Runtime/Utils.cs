using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

public class Utils
{
    public static Dictionary<string, string> StringToDict(string input)
    {
        var dict = new Dictionary<string, string>();
        if (string.IsNullOrEmpty(input)) return dict;
		
        var pairs = input.Split(',');
        foreach (var pair in pairs)
        {
            var parts = pair.Split('=');
            string key = parts[0].Trim();
            string value = parts[1].Trim();
            dict[key] = value;
        }
        return dict;
    }
    
    public static string ComputeSha256Hash(string rawData)
    {
        using var sha256 = SHA256.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(rawData);
        byte[] hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
	
    static readonly uint[] Table = GenerateTable();

    public static uint ComputeCRC(string input)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        uint crc = 0xFFFFFFFF;

        foreach (byte b in bytes)
        {
            byte index = (byte)((crc ^ b) & 0xFF);
            crc = (crc >> 8) ^ Table[index];
        }

        return ~crc;
    }

    private static uint[] GenerateTable()
    {
        uint[] retTable = new uint[256];
        const uint polynomial = 0xEDB88320;

        for (uint i = 0; i < retTable.Length; i++)
        {
            uint c = i;
            for (int j = 0; j < 8; j++)
                c = (c & 1) != 0 ? (polynomial ^ (c >> 1)) : (c >> 1);
            retTable[i] = c;
        }

        return retTable;
    }
    
    public static Dictionary<string, object> DecodeJwt(string token)
    {
        string[] parts = token.Split('.');
        string payload = parts[1];
        payload = PadBase64(payload); // Ensure padding is correct
        byte[] bytes = Convert.FromBase64String(Base64UrlDecode(payload));
        string json = Encoding.UTF8.GetString(bytes);

        return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
    }

    private static string Base64UrlDecode(string input)
    {
        return input.Replace('-', '+').Replace('_', '/');
    }

    private static string PadBase64(string input)
    {
        switch (input.Length % 4)
        {
            case 2: return input + "==";
            case 3: return input + "=";
            default: return input;
        }
    }
}