using UnityEngine;
using System.Collections.Generic;

public interface ISickBeetsModel : IJsonModelNode
{
    /// <summary>
    /// The beet that we have selected to move, manupulate, etc.
    /// </summary>
    IBeetModel SelectedBeet { get; set; }

    /// <summary>
    /// Add a beet to model for tracking
    /// </summary>
    void AddBeet(IBeetModel beet);

    /// <summary>
    /// Removes a beet from model. Destroys any connections it's involved with.
    /// </summary>
    void RemoveBeet(IBeetModel beet);

    /// <summary>
    /// Adds a container to the model for tracking
    /// </summary>
    void AddContainer(IBeetContainerModel container);

    /// <summary>
    /// Removes a container from model. Destroys any connections it's involved with.
    /// </summary>
    void RemoveContainer(IBeetContainerModel container);

    /// <summary>
    /// Connects a beet to a container. Both must be present in the model before calling this method.
    /// Overwrites previous connections they may be involved in.
    /// </summary>
    void AssignBeetToContainer(IBeetModel beet, IBeetContainerModel container);

    /// <summary>
    /// Removes the connection between beet and a container. Both must be present in the model before calling this method.
    /// </summary>
    void UnassignBeetToContainer(IBeetModel beet, IBeetContainerModel container);

    /// <summary>
    /// Get the beet that has been assigned to the container
    /// </summary>
    IBeetModel GetBeetAssignment(IBeetContainerModel container);

    /// <summary>
    /// Gets the container assigned to this beet
    /// </summary>
    IBeetContainerModel GetContainerAssignment(IBeetModel beet);

    /// <summary>
    /// Fetch beet by view instance id
    /// </summary>
    IBeetModel GetBeetByID(int instanceID);

    /// <summary>
    /// Fetch view by instance id
    /// </summary>
    IBeetContainerModel GetContainerByID(int instanceID);

    /// <summary>
    /// Gets the first container with the specified function
    /// </summary>
    IBeetContainerModel GetContainerByFunction(BeetContainerFunction function);

    /// <summary>
    /// Gets all the containers with the specified function
    /// </summary>
    IEnumerable<IBeetContainerModel> GetContainersByFunction(BeetContainerFunction function);
}
