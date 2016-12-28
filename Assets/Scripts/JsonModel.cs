using UnityEngine;
using System.Collections;
using System.IO;

public interface JsonModel
{
    void Serialize(StreamWriter writer);
    void Deserialize(StreamReader reader);
}
