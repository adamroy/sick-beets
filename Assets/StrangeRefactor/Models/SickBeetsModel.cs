using System.Collections.Generic;
using System;
using System.Linq;


public class SickBeetsModel : ISickBeetsModel
{
    private List<IBeetModel> beets;
    private List<IBeetContainerModel> containers;
    private Dictionary<IBeetContainerModel, IBeetModel> assignments;
    
    public SickBeetsModel()
    {
        beets = new List<IBeetModel>();
        containers = new List<IBeetContainerModel>();
        assignments = new Dictionary<IBeetContainerModel, IBeetModel>();
    }

    public IBeetModel SelectedBeet { get; set; }

    public void AddBeet(IBeetModel beet)
    {
        beets.Add(beet);
    }

    public void RemoveBeet(IBeetModel beet)
    {
        // Remove assignments
        if (assignments.ContainsValue(beet))
            assignments.Remove(assignments.First(kvp => kvp.Value == beet).Key);

        beets.Remove(beet);
    }
    
    public void AddContainer(IBeetContainerModel container)
    {
        containers.Add(container);
    }

    public void RemoveContainer(IBeetContainerModel container)
    {
        // Remove assignments
        if (assignments.ContainsKey(container))
            assignments.Remove(container);

        containers.Remove(container);
    }

    public void AssignBeetToContainer(IBeetModel beet, IBeetContainerModel container)
    {
        if (assignments.ContainsValue(beet))
            assignments.Remove(assignments.First(kvp => kvp.Value == beet).Key);
        assignments[container] = beet;
    }
    
    public void UnassignBeetToContainer(IBeetModel beet, IBeetContainerModel container)
    {
        if (assignments.ContainsKey(container))
            assignments.Remove(container);
    }

    public IBeetModel GetBeetAssignment(IBeetContainerModel container)
    {
        if (assignments.ContainsKey(container))
            return assignments[container];
        else
            return null;
    }

    public IBeetContainerModel GetContainerAssignment(IBeetModel beet)
    {
        if (assignments.ContainsValue(beet))
            return assignments.First(kvp => kvp.Value == beet).Key;
        else
            return null;
    }

    public IBeetModel GetBeetByID(int instanceID)
    {
        return beets.FirstOrDefault(beet => beet.InstanceID == instanceID);
    }

    public IBeetContainerModel GetContainerByID(int instanceID)
    {
        return containers.FirstOrDefault(container => container.InstanceID == instanceID);
    }

    public IBeetContainerModel GetContainerByFunction(BeetContainerFunction function)
    {
        return containers.FirstOrDefault(container => container.function == function);
    }

    public IEnumerable<IBeetContainerModel> GetContainersByFunction(BeetContainerFunction function)
    {
        return containers.FindAll(container => container.function == function);
    }
}

