using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilding
{
    void OnBuy();
    void OnTurnStart();
}


public class TownHall : IBuilding
{
    private BuildingsData buildingInfo;
    private Castle castle;
    private float tax = 0.1f;

    public TownHall(BuildingsData buildingInfo, Castle castle)
    {
        this.buildingInfo = buildingInfo;
        this.castle = castle;

    }
    
    public void OnBuy()
    {
        Debug.Log(buildingInfo.upgrateCost);
        if(buildingInfo.currentLevel >= buildingInfo.building.maxLevel)
        {
            Debug.LogError("Max level");
        }
        else if (castle.coinsCurrent >= buildingInfo.upgrateCost)
        {
            castle.coinsCurrent -= buildingInfo.upgrateCost;
            buildingInfo.currentLevel++;
        }
    }

    public void OnTurnStart()
    {
        if (GameController.Insnatce.currentTrun == 1)
            return;
        for (int i = 1; i < buildingInfo.currentLevel; i++)
            tax += 0.02f;
        float taxIncome = castle.peoplesCurrent * tax;
        castle.coinsCurrent += (int)taxIncome;
    }
}

public class House : IBuilding
{
    private BuildingsData buildingInfo;
    private Castle castle;
    private int bonusPerLvl = 200;

    public House(BuildingsData buildingInfo, Castle castle)
    {
        this.buildingInfo = buildingInfo;
        this.castle = castle;
        
        castle.poeplesLimit = buildingInfo.currentLevel * bonusPerLvl + GameController.Insnatce.startPeoplesLimit;
    }

    public void OnBuy()
    {
        Debug.Log(buildingInfo.upgrateCost);
        if (buildingInfo.currentLevel >= buildingInfo.building.maxLevel)
        {
            Debug.LogError("Max level");
        }
        else if (castle.coinsCurrent >= buildingInfo.upgrateCost)
        {
            castle.poeplesLimit += bonusPerLvl;

            castle.coinsCurrent -= buildingInfo.upgrateCost;
            buildingInfo.currentLevel++;
        }
    }

    public void OnTurnStart()
    {
        if(castle.peoplesCurrent <= castle.poeplesLimit)
            castle.peoplesCurrent += buildingInfo.currentLevel * 10;
        if (castle.peoplesCurrent > castle.poeplesLimit)
            castle.peoplesCurrent = castle.poeplesLimit;
    }
}

public class Wall : IBuilding
{
    private BuildingsData buildingInfo;
    private Castle castle;

    public Wall(BuildingsData buildingInfo, Castle castle)
    {
        this.buildingInfo = buildingInfo;
        this.castle = castle;
    }

    public void OnBuy()
    {
        if (buildingInfo.currentLevel >= buildingInfo.building.maxLevel)
        {
            Debug.LogError("Max level");
        }
        else if (castle.coinsCurrent >= buildingInfo.upgrateCost)
        {
            castle.coinsCurrent -= buildingInfo.upgrateCost;
            buildingInfo.currentLevel++;
            if (buildingInfo.currentLevel % 2 == 0)
            {
                castle.armyDefBonus++;
            }
        }
    }

    public void OnTurnStart()
    {

    }
}

public class Temple : IBuilding
{
    private BuildingsData buildingInfo;
    private Castle castle;

    public Temple(BuildingsData buildingInfo, Castle castle)
    {
        this.buildingInfo = buildingInfo;
        this.castle = castle;
    }

    public void OnBuy()
    {
        if (buildingInfo.currentLevel >= buildingInfo.building.maxLevel)
        {
            Debug.LogError("Max level");
        }
        else if (castle.coinsCurrent >= buildingInfo.upgrateCost)
        {
            castle.coinsCurrent -= buildingInfo.upgrateCost;
            buildingInfo.currentLevel++;
            if (buildingInfo.currentLevel % 3 == 0)
            {
                castle.armyTravelAttackBonus++;
            }
            if (buildingInfo.currentLevel % 2 == 0)
            {
                castle.armyAttackBonus++;
            }
        }
    }
    public void CheckPermissionAndBonuses()
    {
        ArmyData data = new ArmyData(null, 0, castle.ownerId); ;
        if (buildingInfo.currentLevel == 1)
        {
            if (!castle.avaliebleToTrainUnits.Exists(x => x.unitInfo.unitName == "Recrut"))
            {
                data.unitInfo = GameController.Insnatce.unitsInfo.Find(x => x.unitName == "Recrut");
                castle.avaliebleToTrainUnits.Add(data);
            }
        }
    }

    public void OnTurnStart()
    {
        CheckPermissionAndBonuses();
    }
}

public class Barracks : IBuilding
{
    private BuildingsData buildingInfo;
    private Castle castle;

    public Barracks(BuildingsData buildingInfo, Castle castle)
    {
        this.buildingInfo = buildingInfo;
        this.castle = castle;
        CheckPermissionAndBonuses();
    }

    public void OnBuy()
    {
        if (buildingInfo.currentLevel >= buildingInfo.building.maxLevel)
        {
            Debug.LogError("Max level");
        }
        else if (castle.coinsCurrent >= buildingInfo.upgrateCost)
        {
            castle.coinsCurrent -= buildingInfo.upgrateCost;
            buildingInfo.currentLevel++;
            CheckPermissionAndBonuses();
        }
    }

    public void CheckPermissionAndBonuses()
    {
        ArmyData data = new ArmyData(null, 0, castle.ownerId); ;
        if (buildingInfo.currentLevel == 1)
        {
            if (!castle.avaliebleToTrainUnits.Exists(x => x.unitInfo.unitName == "Archer"))
            {
                data.unitInfo = GameController.Insnatce.unitsInfo.Find(x => x.unitName == "Archer");
                castle.avaliebleToTrainUnits.Add(data);
            }
        }
        if (buildingInfo.currentLevel == 2 || buildingInfo.currentLevel == 3)
        {
            data.unitInfo = GameController.Insnatce.unitsInfo.Find(x => x.unitName == "Archer");
            data.UnitTrainTime--;
            castle.avaliebleToTrainUnits[castle.avaliebleToTrainUnits.FindIndex(x => x.unitInfo.unitName == "Archer")] = data;
        }
        if (buildingInfo.currentLevel == 4)
        {
            data.unitInfo = GameController.Insnatce.unitsInfo.Find(x => x.unitName == "Footman");
            castle.avaliebleToTrainUnits.Add(data);
        }
        if (buildingInfo.currentLevel == 5 || buildingInfo.currentLevel == 6)
        {
            data.unitInfo = GameController.Insnatce.unitsInfo.Find(x => x.unitName == "Footman");
            data.UnitTrainTime--;
            castle.avaliebleToTrainUnits[castle.avaliebleToTrainUnits.FindIndex(x => x.unitInfo.unitName == "Footman")] = data;
        }
        if (buildingInfo.currentLevel == 7)
        {
            data.unitInfo = GameController.Insnatce.unitsInfo.Find(x => x.unitName == "Rider");
            castle.avaliebleToTrainUnits.Add(data);
        }
        if (buildingInfo.currentLevel == 8 || buildingInfo.currentLevel == 9 || buildingInfo.currentLevel == 10)
        {
            data.unitInfo = GameController.Insnatce.unitsInfo.Find(x => x.unitName == "Rider");
            data.UnitTrainTime--;
            castle.avaliebleToTrainUnits[castle.avaliebleToTrainUnits.FindIndex(x => x.unitInfo.unitName == "Rider")] = data;
        }

    }

    public void OnTurnStart()
    {
        if(buildingInfo.currentLevel!= buildingInfo.building.maxLevel)
            CheckPermissionAndBonuses();
    }
}