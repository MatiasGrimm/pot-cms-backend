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
        GetLocation = 4,
        ModifyLocation = 8,

        All = GetInventory | UpdateInventory | GetLocation | ModifyLocation
    }
}