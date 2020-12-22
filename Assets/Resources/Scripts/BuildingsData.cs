using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsData
{
    public BuildingInfo building;

    public int upgrateCost { get => building.cost * currentLevel; }
    public int currentLevel = 1;

}
