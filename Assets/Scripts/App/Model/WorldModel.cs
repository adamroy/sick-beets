﻿using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

// Contains data about world (beets, placements, selection, etc)
[Serializable]
public class WorldModel : IJsonModelNode
{
    public BeetModel SelectedBeet { get; set; }

    [SerializeField]
    private List<BeetModel> beets;

    [SerializeField]
    private List<BeetContainerModel> containers;

    [SerializeField]
    private CameraDestination cameraPosition;

    private SerializableDictionary<BeetContainerModel, BeetModel> assignments; // Maps containers to beets
    private SerializableDictionary<string, float> environmentVariables; // Maps names to values

    public WorldModel()
    {
        Clear();
    }

    public void Clear()
    {
        SelectedBeet = null;
        beets = new List<BeetModel>();
        containers = new List<BeetContainerModel>();
        cameraPosition = CameraDestination.Nursery;
        assignments = new SerializableDictionary<BeetContainerModel, BeetModel>(containers, beets);
        environmentVariables = new SerializableDictionary<string, float>();
    }

    #region public data methods

    public void AddBeet(BeetModel beet)
    {
        beets.Add(beet);
    }

    public void RemoveBeet(BeetModel beet)
    {
        // Remove assignments
        assignments.RemoveValue(beet);

        beets.Remove(beet);
    }

    public void AddContainer(BeetContainerModel container)
    {
        containers.Add(container);
    }

    public void RemoveContainer(BeetContainerModel container)
    {
        // Remove assignments
        assignments.Remove(container);

        containers.Remove(container);
    }

    public void AssignBeetToContainer(BeetModel beet, BeetContainerModel container)
    {
        assignments.RemoveValue(beet);
        assignments.Remove(container);
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

    public BeetContainerModel GetContainerByName(string name)
    {
        return containers.FirstOrDefault(container => container.Name == name);
    }

    public BeetContainerModel GetContainerByFunction(BeetContainerFunction function)
    {
        return containers.FirstOrDefault(container => container.Function == function);
    }

    public IEnumerable<BeetContainerModel> GetAllContainersByFunction(BeetContainerFunction function)
    {
        return containers.FindAll(container => container.Function == function);
    }

    public List<KeyValuePair<BeetContainerModel, BeetModel>> GetAllAssignements()
    {
        return assignments.ToList();
    }

    public void SetEnvironmentValue(EnvironmentVariable variable, float value)
    {
        if (environmentVariables.ContainsKey(variable.name))
            environmentVariables.Remove(variable.name);
        environmentVariables[variable.name] = value;
    }

    public float GetEnvironmentValue(EnvironmentVariable variable)
    {
        // Set a default value 
        if (!environmentVariables.ContainsKey(variable.name))
            SetEnvironmentValue(variable, 0.5f);

        return environmentVariables[variable.name];
    }

    public void SetCameraDestination(CameraDestination dest)
    {
        cameraPosition = dest;
    }

    public CameraDestination GetCameraDestination()
    {
        return cameraPosition;
    }

    #endregion

    #region IJsonModelNode

    public void BeforeSerializing()
    {

    }

    public void AfterDeserializing()
    {
        // Deserializing creates new lists out of serialized ones, so we need 
        // to reinitialize managed dictionaries so that it has references to the actual lists
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
