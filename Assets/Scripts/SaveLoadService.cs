using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class SaveLoadService
{
    public static void Save<T>(T data, string fileName)
    {
        string path = GetPath(fileName);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    public static bool TryLoad<T>(string fileName, out T data) where T : class
    {
        string path = GetPath(fileName);
        data = null;

        if (!File.Exists(path))
            return false;

        try
        {
            string json = File.ReadAllText(path);

            if (string.IsNullOrEmpty(json))
                return false;

            data = JsonUtility.FromJson<T>(json);

            if (data == null)
                return false;

            return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[SaveLoad] File corrupted: {fileName}\n{e.Message}");
            return false;
        }
    }

    public static T LoadOrCreate<T>(string fileName) where T : class, new()
    {
        if (TryLoad(fileName, out T data))
            return data;

        // fallback
        T newData = new T();
        Save(newData, fileName);
        return newData;
    }

    public static void Delete(string fileName)
    {
        string path = GetPath(fileName);

        if (File.Exists(path))
            File.Delete(path);
    }

    private static string GetPath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }
}
