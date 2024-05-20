using System;
using System.IO;
using UnityEngine;

public class JsonHelper
{
    public static T[] GetJsonArray<T>(string json)
    {
        if (json == null) return null;

        string newJson = "{ \"array\": " + json + "}";

        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    public static T[] GetJsonArrayFromJsonFile<T>(string jsonFileName)
    {
        if (jsonFileName == null) return null;

        // Read the JSON File
        string jsonData = File.ReadAllText(jsonFileName);

        return GetJsonArray<T>(jsonData);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}