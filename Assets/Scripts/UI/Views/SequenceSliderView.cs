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

    public List<Base> GetSelectedSequence()
    {
        return null;
    }
}
