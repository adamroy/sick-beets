using UnityEngine;
using System.Collections;
using System.Linq;
using strange.extensions.command.impl;

public class UnlockStoreItemCommand : Command
{
    [Inject]
    public IStoreItemLibrary storeItemLibrary { get; set; }

    [Inject]
    public GameModel model { get; set; }

    [Inject]
    public StoreItemUnlockedSignal storeItemUnlockedSignal { get; set; }

    public override void Execute()
    {
        var unlockedItem = storeItemLibrary.StoreItems.FirstOrDefault(item => item.UnlockingSequence.bases.SequenceEqual(model.Research.GetResearchSequence()));

        if (unlockedItem != null && !model.Store.IsUnlocked(unlockedItem))
        {
            model.Store.UnlockItem(unlockedItem);
            storeItemUnlockedSignal.Dispatch(unlockedItem);
        }

        // Resets the research so it's ready for next round
        model.Research.Clear();
    }
}
