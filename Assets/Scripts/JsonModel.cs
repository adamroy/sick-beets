using System;
using UnityEngine;
using System.IO;

public interface JsonModel
{
    // Prepare data to be serialized
    void BeforeSerializing();

    // Use data to set up object and create children
    void AfterDeserializing();

    // Get all children that need to be serialized/deserialized
    JsonModel[] GetChildren();
}

public class JsonWriter : StreamWriter
{
    public JsonWriter(MemoryStream s) : base(s) { }

    public void WriteObject(JsonModel o)
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

    public void ReadObject(JsonModel o)
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



