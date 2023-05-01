using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkView : View
{
    public PerkUICell[] cells;

    public void FillPerkCells()
    {
        var perks = PerkManager.instance.GeneratePerkSelection(cells.Length);
        for (int i = 0; i < perks.Count; ++i)
        {
            cells[i].selectedPerk = perks[i];
            cells[i].Setup();
        }
    }
}
