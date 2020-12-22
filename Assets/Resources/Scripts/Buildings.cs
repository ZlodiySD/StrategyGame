using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Buildings
{
    private Castle castle;

    public enum ACTION {BUY, STARTTURN }

    public Buildings(Castle castle)
    {
        this.castle = castle;
    }

    public void ActionWithBuilding(BuildingsData buildingInfo , ACTION action = ACTION.BUY)
    {
        
        switch (buildingInfo.building.buidingName)
        {
            case "TownHall":
                {
                    TownHall building = new TownHall(buildingInfo, castle);
                    if(action == ACTION.BUY)
                        building.OnBuy();
                    else if (action == ACTION.STARTTURN)
                        building.OnTurnStart();
                    break;
                }
            case "House":
                {
                    House building = new House(buildingInfo, castle);
                    if (action == ACTION.BUY)
                        building.OnBuy();
                    else if (action == ACTION.STARTTURN)
                        building.OnTurnStart();
                    break;
                }
            case "Wall":
                {
                    Wall building = new Wall(buildingInfo, castle);
                    if (action == ACTION.BUY)
                        building.OnBuy();
                    else if (action == ACTION.STARTTURN)
                        building.OnTurnStart();
                    break;
                }
            case "Temple":
                {
                    Temple building = new Temple(buildingInfo, castle);
                    if (action == ACTION.BUY)
                        building.OnBuy();
                    else if (action == ACTION.STARTTURN)
                        building.OnTurnStart();
                    break;
                }
            case "Barracks":
                {
                    Barracks building = new Barracks(buildingInfo, castle);
                    if (action == ACTION.BUY)
                        building.OnBuy();
                    else if (action == ACTION.STARTTURN)
                        building.OnTurnStart();
                    break;
                }
        }
    }

    public void OnTurnStart()
    {
        foreach(BuildingsData buildingInfo in castle.buildingsInfo)
        {
            ActionWithBuilding(buildingInfo, ACTION.STARTTURN);
        }
    }
}
