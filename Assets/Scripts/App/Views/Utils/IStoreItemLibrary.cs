  using UnityEngine;
using System.Collections.Generic;

public interface IStoreItemLibrary
{
    IEnumerable<StoreItem> StoreItems { get; }
}