using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class NurserySettingsPanelModel : MonoBehaviour, IJsonModelNode
{
    public List<SliderModel> sliders;

    public void AfterDeserializing() { }

    public void BeforeSerializing() { }

    public IEnumerable<IJsonModelNode> GetChildren()
    {
        return sliders.Cast<IJsonModelNode>();
    }
}
