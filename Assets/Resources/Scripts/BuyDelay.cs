using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyDelay : MonoBehaviour
{
    Castle castle;
    ArmyData unit;
    int time;

    public BuyDelay(Castle castle, ArmyData unit, int count)
    {
        Debug.Log("BuyDelay");
        this.castle = castle;
        this.unit = unit;
        this.unit.count = count;
        time = unit.UnitTrainTime;
    }

    public bool OnTurn()
    {
        time--;
        if (time > 0)
        {
            Debug.Log("Trying to train unit time:  " + time);
            return false;
        }
        else
        {
            castle.castleArmy.UpdateArmyInfo(unit);
            Debug.Log("unit.unitInfo.name " + unit.unitInfo.name + " " + unit.count);
            return true;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Debug.Log("Time " + time);
        }
    }
}
