using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotBox : MonoBehaviour
{
    public Vector2Int id;
    public Strip slotStrip;

    // We don't need this now, but if we want to have a functionality
    // where we want to change individual slot's display item, then
    // this will be useful to check the results.
    public string currentlyShownItem = "";

    private void Start()
    {
        // Set the current item shown when the strip stops spinning
        Strip.SpinComplete += () => { SetCurrentItem(); };

        // Check if proper strips are assigned to slots
        if(id != slotStrip.id)
        {
            Debug.LogError("Mismatched strip to slot added");
            return;
        }
        else
        {
            // print("Associated strip found!");
        }
    }

    /// <summary>
    /// Set the current item on slot based on Strip's position
    /// </summary>
    public void SetCurrentItem()
    {
        currentlyShownItem = slotStrip.GetStoppedOnItem();
    }
}
