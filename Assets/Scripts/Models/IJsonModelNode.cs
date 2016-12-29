using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public interface IJsonModelNode
{
    // Prepare data to be serialized
    // Save things that will let you instantiate any children you need to
    void BeforeSerializing();

    // Use data to set up object and create children
    // Create children from data saved in BeforeSerializing(), in same order
    void AfterDeserializing();

    // Get all children that need to be serialized/deserialized
    // Return null if this is a leaf node
    // Important! Children must brought up in the same order before serialization and after deserialization
    // This is because that order is saved in the Json string and needs to be maintained for proper deserialization
    IEnumerable<IJsonModelNode> GetChildren();
}

public class JsonWriter : StreamWriter
{
    public JsonWriter(MemoryStream s) : base(s) { }

    // Writes this object and all its children recursively to the stream
    public void WriteObject(IJsonModelNode o)
    {
        o.BeforeSerializing();
        string json = JsonUtility.ToJson(o);
        int length = json.Length;
        Write(length.ToString("D10"));
        Write(json);
        Flush();

        var children = o.GetChildren();
        if (children != null)
        {
            foreach (var child in children)
                WriteObject(child);
        }
    }

    public override string ToString()
    {
        return Convert.ToBase64String(((MemoryStream)BaseStream).ToArray());
    }
}

public class JsonReader : StreamReader
{
    private char[] buff;

    public JsonReader(MemoryStream s) : base(s)
    {
        buff = new char[10];
    }

    // Reads this object and all its children recursively from the stream
    public void ReadObject(IJsonModelNode o)
    {
        Read(buff, 0, 10);
        int length = int.Parse(new string(buff));

        char[] jsonBuff = new char[length];
        Read(jsonBuff, 0, length);
        string json = new string(jsonBuff);
        JsonUtility.FromJsonOverwrite(json, o);
        o.AfterDeserializing();

        var children = o.GetChildren();
        if (children != null)
        {
            foreach (var child in children)
                ReadObject(child);
        }
    }
}

public static class JsonSavingUtility
{
    public static void Save(string key, IJsonModelNode root)
    {
        MemoryStream stream = new MemoryStream();
        JsonWriter writer = new JsonWriter(stream);
        writer.WriteObject(root);
        string data = Convert.ToBase64String(stream.ToArray());
        PlayerPrefs.SetString(key, data);
    }

    public static void Load(string key, IJsonModelNode root)
    {
        if (PlayerPrefs.HasKey(key))
        {
            var dataString = PlayerPrefs.GetString(key);
            var dataBuffer = Convert.FromBase64String(dataString);
            var dataStream = new MemoryStream(dataBuffer);
            JsonReader reader = new JsonReader(dataStream);
            reader.ReadObject(root);
        }
    }
}