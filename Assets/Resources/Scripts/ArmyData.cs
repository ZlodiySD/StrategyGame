using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyData : IComparable<ArmyData>
{
    public ArmyData(UnitInfo unitInfo, int count, int ownerId)
    {
        this.unitInfo = unitInfo;
        this.count = count;
        this.ownerId = ownerId;
    }

    public ArmyData(UnitInfo unitInfo)
    {
        this.unitInfo = unitInfo;
    }

    public ArmyData(ArmyData data)
    {
        this.unitInfo = data.unitInfo;
        this.count = data.count;
        this.ownerId = data.ownerId;
    }
    public bool isOnCatle { get; set; }
    public int castleBonusAttack { get; set; }
    public int castleBonusDefence { get; set; }
    public int travelBonusAttack { get; set; }
    public int travelBonusDefence { get; set; }
    public UnitInfo unitInfo { get; set; }
    public int count { get; set; }
    public int ownerId { get; set; }
    public int health
    {
        get
        {
            int C = 0;
            if (isOnCatle)
                C = castleBonusDefence;
            else
                C = travelBonusDefence;
            return (unitInfo.defence + C) *count;
        }
    }

    public int strenght
    {
        get
        {
            int C = 0;
            if (isOnCatle)
                C = castleBonusAttack;
            else
                C = travelBonusAttack;
            return (unitInfo.attack + C) * count;
        }
    }

    private int unitTrainTime;
    public int UnitTrainTime
    {
        get
        {
            if (unitTrainTime != 0)
                return unitTrainTime;
            else
                return unitInfo.trainingTime;
        }
        set
        {
            unitTrainTime = value;
        }
    }

    public int CompareTo(ArmyData that)
    {
        if (that == null) return 1;
        if (this.unitInfo.initiative > that.unitInfo.initiative) return -1;
        if (this.unitInfo.initiative < that.unitInfo.initiative) return 1;
        return 0;
    }
}
