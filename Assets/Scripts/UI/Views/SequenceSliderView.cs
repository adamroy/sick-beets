using UnityEngine;
using System.Collections.Generic;

public class SequenceSliderView : SliderView
{
    private int selectionSize;
    private int marginSize;

    public void DisplaySequence(List<Base> sequence, int selectionSize, int marginSize)
    {
        this.selectionSize = selectionSize;
    }

    public void ClearSequence()
    {

    }

    public List<Base> GetLeftMargin()
    {
        return null;
    }

    public List<Base> GetRightMargin()
    {
        return null;
    }

    public List<Base> GetSelectedSequence()
    {
        return null;
    }
}
