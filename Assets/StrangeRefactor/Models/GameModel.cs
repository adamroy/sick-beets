using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

[Serializable]
public class GameModel : IJsonModelNode
{
    public bool SuccessfulyLoaded { get; set; }

    [SerializeField]
    public long Time;

    [SerializeField]
    private List<BeetModel> beets;

    [SerializeField]
    private List<BeetContainerModel> containers;

    private SerializableDictionary<BeetContainerModel, BeetModel> assignments;
    private SerializableDictionary<EnvironmentVariable, float> environmentVariables;

    public GameModel()
    {
        SuccessfulyLoaded = false;
        beets = new List<BeetModel>();
        containers = new List<BeetContainerModel>();
        assignments = new SerializableDictionary<BeetContainerModel, BeetModel>(containers, beets);
        environmentVariables = new SerializableDictionary<EnvironmentVariable, float>();
    }

    public BeetModel SelectedBeet { get; set; }

    public void AddBeet(BeetModel beet)
    {
        beets.Add(beet);
    }

    public void RemoveBeet(BeetModel beet)
    {
        // Remove assignments
        if (assignments.ContainsValue(beet))
            assignments.Remove(assignments.First(kvp => kvp.Value == beet).Key);

        beets.Remove(beet);
    }
    
    public void AddContainer(BeetContainerModel container)
    {
        containers.Add(container);
    }

    public void RemoveContainer(BeetContainerModel container)
    {
        // Remove assignments
        if (assignments.ContainsKey(container))
            assignments.Remove(container);

        containers.Remove(container);
    }

    public void AssignBeetToContainer(BeetModel beet, BeetContainerModel container)
    {
        if (assignments.ContainsValue(beet))
            assignments.Remove(assignments.First(kvp => kvp.Value == beet).Key);
        assignments[container] = beet;
    }
    
    public void UnassignBeetToContainer(BeetModel beet, BeetContainerModel container)
    {
        if (assignments.ContainsKey(container))
            assignments.Remove(container);
    }

    public BeetModel GetBeetAssignment(BeetContainerModel container)
    {
        if (assignments.ContainsKey(container))
            return assignments[container];
        else
            return null;
    }

    public BeetContainerModel GetContainerByAssignment(BeetModel beet)
    {
        if (assignments.ContainsValue(beet))
            return assignments.First(kvp => kvp.Value == beet).Key;
        else
            return null;
    }

    public BeetModel GetBeetByID(int instanceID)
    {
        return beets.FirstOrDefault(beet => beet.InstanceID == instanceID);
    }

    public BeetContainerModel GetContainerByID(int instanceID)
    {
        return containers.FirstOrDefault(container => container.InstanceID == instanceID);
    }

    public BeetContainerModel GetContainerByFunction(BeetContainerFunction function)
    {
        return containers.FirstOrDefault(container => container.Function == function);
    }

    public IEnumerable<BeetContainerModel> GetContainersByFunction(BeetContainerFunction function)
    {
        return containers.FindAll(container => container.Function == function);
    }

    public List<KeyValuePair<BeetContainerModel, BeetModel>> GetAllAssignements()
    {
        return assignments.ToList();
    }

    public float GetEnvironmentValue(EnvironmentVariable variable)
    {
        if (environmentVariables.ContainsKey(variable))
            return environmentVariables[variable];
        else
            return 0f;
    }

#region IJsonModelNode

    public void BeforeSerializing()
    {

    }

    public void AfterDeserializing()
    {
        // Deserializing creates new lists out of serialized ones, so we need 
        // to reinitialize dictionaries so that it has references to the actual lists
        assignments = new SerializableDictionary<BeetContainerModel, BeetModel>(containers, beets);
    }
    
    public IEnumerable<IJsonModelNode> GetChildren()
    {
        // Since the dictionaries need to process before and after serialization, pass off as children
        List<IJsonModelNode> children = new List<IJsonModelNode>();
        children.Add(assignments);
        children.Add(environmentVariables);
        return children;
    }

#endregion
}

