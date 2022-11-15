using System;

namespace PotShop.API.Models.Entities.Enums
{
    [Flags]
    public enum Access
    {
        // Numbers shouldn't be changed at all!
        // ONLY ADD TO THE BOTTOM OF THE LIST :)
        None = 0,
        GetInventory = 1,
        UpdateInventory = 2,
        GetDisabledProducts = 4,
        ModifyLocation = 8,

        All = GetInventory | UpdateInventory | GetDisabledProducts | ModifyLocation
    }
}