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

    #region source management

    private void TryAddManagedKey(TKey key)
    {
        if (internalKeySource == null) return;

        if (!internalKeySource.Contains(key))
            internalKeySource.Add(key);
    }

    private void TryRemoveManagedKey(TKey key)
    {
        if (internalKeySource == null) return;

        internalKeySource.Remove(key);
    }

    private void ClearManagedKeys()
    {
        if (internalKeySource == null) return;

        internalKeySource.Clear();
    }

    private void TryAddManagedValue(TValue value)
    {
        if (internalValueSource == null) return;

        if (!internalValueSource.Contains(value))
            internalValueSource.Add(value);
    }

    private void TryRemoveManagedValue(TValue value)
    {
        if (internalValueSource == null) return;

        internalValueSource.Remove(value);
    }

    private void ClearManagedValues()
    {
        if (internalValueSource == null) return;

        internalValueSource.Clear();
    }

    #endregion

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
        TryAddManagedKey(key);
        TryAddManagedValue(value);
    }

    public bool ContainsKey(TKey key)
    {
        return dictionary.ContainsKey(key);
    }

    public bool Remove(TKey key)
    {
        TryRemoveManagedKey(key);
        if (dictionary.ContainsKey(key))
            TryRemoveManagedValue(dictionary[key]);
        return dictionary.Remove(key);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return dictionary.TryGetValue(key, out value);
    }

    public void Add(System.Collections.Generic.KeyValuePair<TKey, TValue> item)
    {
        TryAddManagedKey(item.Key);
        TryAddManagedValue(item.Value);
        dictionary.Add(item.Key, item.Value);
    }

    public void Clear()
    {
        ClearManagedKeys();
        ClearManagedValues();
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
        foreach (var kvp in dictionary)
        {
            keyIndices.Add(keySource.IndexOf(kvp.Key));
            valueIndices.Add(valueSource.IndexOf(kvp.Value));
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

            if (keyIndex < keySource.Count && valueIndex < valueSource.Count)
            {
                dictionary.Add(keySource[keyIndex], valueSource[valueIndex]);
            }
            else
            {
                throw new Exception("SerializableDictionary error, saved indices exceed source length.");
            }
        }
    }

    public IEnumerable<IJsonModelNode> GetChildren()
    {
        return null;
    }

    #endregion
}
