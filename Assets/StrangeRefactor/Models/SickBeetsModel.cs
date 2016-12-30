using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

[Serializable]
public class SickBeetsModel : IJsonModelNode
{
    [SerializeField]
    private List<BeetModel> beets;

    [SerializeField]
    private List<BeetContainerModel> containers;
    
    [SerializeField]
    private List<Assignment> assignmentsList;

    [Serializable]
    class Assignment
    {
        public int beetIndex;
        public int containerIndex;
    }

    private Dictionary<BeetContainerModel, BeetModel> assignments;
    
    public SickBeetsModel()
    {
        beets = new List<BeetModel>();
        containers = new List<BeetContainerModel>();
        assignments = new Dictionary<BeetContainerModel, BeetModel>();
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

    public void BeforeSerializing()
    {
        assignmentsList = new List<Assignment>();
        foreach (var kvp in assignments)
        {
            assignmentsList.Add(new Assignment() { containerIndex = containers.IndexOf(kvp.Key), beetIndex = beets.IndexOf(kvp.Value) });
        }
    }

    public void AfterDeserializing()
    {
        assignments = new Dictionary<BeetContainerModel, BeetModel>();
        foreach (var kvp in assignmentsList)
        {
            var container = containers[kvp.containerIndex];
            var beet = beets[kvp.beetIndex];
            assignments[container] = beet;
        }
    }

    public IEnumerable<IJsonModelNode> GetChildren()
    {
        return null;
    }
}

