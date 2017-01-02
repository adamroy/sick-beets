using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;

// Let's us use a dictionary like normal in the model but be able to serialize it
// Doesn't support equivalent values
[Serializable]
public class SerializableDictionary<TKey, TValue> : IJsonModelNode, IDictionary<TKey, TValue>
{
    private Dictionary<TKey, TValue> dictionary;

    // For some reason this wasn't serializing. Revisit later maybe
    /*
    [Serializable]
    public class IndexPair
    {
        public int Key;
        public int Value;
    }
    
    [SerializeField]
    private List<IndexPair> dictionaryBacking;
    */

    [SerializeField]
    private List<int> keyIndices;
    [SerializeField]
    private List<int> valueIndices;

    // We may need to store the key/value pairs if they are not externally managed
    [SerializeField]
    private List<TKey> internalKeySource;
    [SerializeField]
    private List<TValue> internalValueSource;

    // The key/value sources we are using. May either refer to internal sources or external sources.
    private List<TKey> keySource;
    private List<TValue> valueSource;

    public bool IsKeyManaged { get { return internalKeySource != null; } }
    public bool IsValueManaged { get { return internalValueSource != null; } }

    #region constructors

    public SerializableDictionary() : this(null, null)
    {
        keySource = internalKeySource = new List<TKey>();
        valueSource = internalValueSource = new List<TValue>();
    }

    public SerializableDictionary(List<TKey> inKeySource, List<TValue> inValueSource)
    {
        if (keySource == null)
        {
            if (inKeySource != null)
            {
                keySource = inKeySource;
            }
            else
            {
                internalKeySource = keySource = new List<TKey>();
            }
        }
        if(valueSource == null)
        {
            if (inValueSource != null)
            {
                valueSource = inValueSource;
            }
            else
            {
                internalValueSource = valueSource = new List<TValue>();
            }
        }

        dictionary = new Dictionary<TKey, TValue>();
        keyIndices = new List<int>();
        valueIndices = new List<int>();
    }

    #endregion

    public bool ContainsValue(TValue value)
    {
        return dictionary.ContainsValue(value);
    }

    public bool RemoveValue(TValue value)
    {
        if (ContainsValue(value))
        {
            var key = this.First(kvp => kvp.Value.Equals(value)).Key;
            return Remove(key);
        }
        else
            return false;
    }

    #region IDictionary<TKey, TValue>

    public ICollection<TKey> Keys
    {
        get
        {
            return dictionary.Keys;
        }
    }

    public ICollection<TValue> Values
    {
        get
        {
            return dictionary.Values;
        }
    }

    public int Count
    {
        get
        {
            return dictionary.Count;
        }
    }

    public bool IsReadOnly
    {
        get
        {
            return false;
        }
    }

    public TValue this[TKey key]
    {
        get
        {
            return dictionary[key];
        }

        set
        {
            Add(key, value);
        }
    }

    public void Add(TKey key, TValue value)
    {
        dictionary.Add(key, value);
    }

    public bool ContainsKey(TKey key)
    {
        return dictionary.ContainsKey(key);
    }

    public bool Remove(TKey key)
    {
        return dictionary.Remove(key);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return dictionary.TryGetValue(key, out value);
    }

    public void Add(System.Collections.Generic.KeyValuePair<TKey, TValue> item)
    {
        dictionary.Add(item.Key, item.Value);
    }

    public void Clear()
    {
        dictionary.Clear();
    }

    public bool Contains(System.Collections.Generic.KeyValuePair<TKey, TValue> item)
    {
        return dictionary.ContainsKey(item.Key) && dictionary[item.Key].Equals(item.Value);
    }

    public void CopyTo(System.Collections.Generic.KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        foreach (var kvp in dictionary)
        {
            array[arrayIndex] = kvp;
            arrayIndex++;
        }
    }

    public bool Remove(System.Collections.Generic.KeyValuePair<TKey, TValue> item)
    {
        if (Contains(item))
            return Remove(item.Key);
        else
            return false;
    }

    public IEnumerator<System.Collections.Generic.KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return dictionary.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return dictionary.GetEnumerator();
    }

    #endregion

    #region IJsonModelNode

    public void BeforeSerializing()
    {
        keyIndices.Clear();
        valueIndices.Clear();
        
        if(IsKeyManaged)
        {
            internalKeySource.Clear();
        }
        if(IsValueManaged)
        { 
            internalValueSource.Clear();
            // Reserve space
            for (int i = 0; i < dictionary.Count; i++)
                internalValueSource.Add(default(TValue));
        }

        int index = 0;
        
        foreach (var kvp in dictionary)
        {
            int keyIndex = -1;
            if (IsKeyManaged == false) 
            {
                // When external managing keys, use the index of the actual source (no changing order)
                keyIndex = keySource.IndexOf(kvp.Key);
                keyIndices.Add(keyIndex);
            }
            else
            {
                // When internally managing, add keys in order of enumeration
                internalKeySource.Add(kvp.Key);
                keyIndex = index++;
                keyIndices.Add(keyIndex);
            }

            if (IsValueManaged == false)
            {
                // When externally managing, use the index of the actual source (no changing order)
                int valueIndex = valueSource.IndexOf(kvp.Value);
                valueIndices.Add(valueIndex);
            }
            else
            {
                // When internally managing values, add them in order of the keys
                internalValueSource[keyIndex] = kvp.Value;
                valueIndices.Add(keyIndex);
            }
        }
    }

    public void AfterDeserializing()
    {
        dictionary.Clear();
        
        // Fetch our saved values if they exist
        if (internalKeySource != null && internalKeySource.Count > 0)
            keySource = internalKeySource;
        else
            internalKeySource = null;
        if (internalValueSource != null && internalValueSource.Count > 0)
            valueSource = internalValueSource;
        else
            internalValueSource = null;

        for (int i = 0; i < keyIndices.Count; i++)
        {
            int keyIndex = keyIndices[i];
            int valueIndex = valueIndices[i];

            if (keyIndex < keySource.Count && valueIndex < valueSource.Count && keyIndex >= 0 && valueIndex >= 0)
            {
                dictionary.Add(keySource[keyIndex], valueSource[valueIndex]);
            }
            else
            {
                throw new Exception("SerializableDictionary error: attempting to access indices outside source size.");
            }
        }
    }

    public IEnumerable<IJsonModelNode> GetChildren()
    {
        return null;
    }

    #endregion
}
