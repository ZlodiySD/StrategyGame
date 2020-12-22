using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public int
        coinsCurrent,
        poeplesLimit;

    [SerializeField]
    public int peoplesCurrent;

    public int ownerId;

    [HideInInspector]
    public (int xcord, int ycord) location;


    [HideInInspector]
    public ArmyController castleArmy;

    public List<ArmyController> ownAmryList;
    public List<ArmyData> avaliebleToTrainUnits;
    public List<BuildingsData> buildingsInfo;

    public Buildings buildings;

    public int
        armyDefBonus,
        armyAttackBonus,
        armyTravelDefBonus,
        armyTravelAttackBonus;

    private BuyDelay buyDelay;
    private List<BuyDelay> buyDelayList;

    private void Start()
    {
        if (gameObject.TryGetComponent(out PlayerController p))
        {
            ownerId = p.Id;
        }
        else
            ownerId = Random.Range(2, 1000);

        coinsCurrent = GameController.Insnatce.startCoins;
        peoplesCurrent = GameController.Insnatce.startPeoples;

        buildingsInfo = new List<BuildingsData>();
        avaliebleToTrainUnits = new List<ArmyData>();
        castleArmy = GetComponent<ArmyController>();
        buyDelayList = new List<BuyDelay>();
        buildings = new Buildings(this);
        foreach (BuildingInfo info in GameController.Insnatce.buildingsInfo)
        {
            BuildingsData temp = new BuildingsData();
            temp.building = info;
            buildingsInfo.Add(temp);
        }
    }

    private void OnDestroy()
    {
        foreach (ArmyController army in ownAmryList.ToArray())
        {
            if (army == null)
                continue;
            else
                Destroy(army.gameObject);
        }
    }

    public void CastleDestroy()
    {
        Destroy(gameObject);
    }

    public void OnTurnStart()
    {
        ownAmryList.ForEach(x => x.OnTurnStart());

        buildings.OnTurnStart();

        BuyDelayOnTurn();

        if (gameObject.TryGetComponent(out AiController ai))
        {
            Debug.Log("OnTurnStart ai castle");
            ai.OnTurnStart();
        }
    }

    public bool MoveArmyFromCastle(List<ArmyData> data)
    {
        List<(int x, int y)> ps = GameController.Insnatce.ArmyMoveZone(gameObject, true);
        ps.Shuffle();
        (int x, int y) position = (0, 0);
        foreach ((int x, int y) x in ps)
        {
            if (GameController.Insnatce.PositionAvailabilityCheck(x))
            {
                position = x;
                break;
            }
        }

        if (position.x != 0 || position.y != 0)
        {
            Debug.Log("Data count " + data.Count);
            castleArmy.ArmySplit(data);
            ownAmryList.Add(GameController.Insnatce.CreateAmry(data, position, this));
            return true;
        }
        else
        {
            Debug.LogError("Cannot place army");
            return false;
        }
    }

    public void BuyUnitWithDelay(UnitInfo info, int count)
    {
        if (info.peopleCost * count > peoplesCurrent || info.coinsCost * count > coinsCurrent)
        {
            Debug.LogError("Not enought");
            return;
        }

        peoplesCurrent -= info.peopleCost * count;
        coinsCurrent -= info.coinsCost * count;

        buyDelay = new BuyDelay(this, new ArmyData(info), count);
        buyDelayList.Add(buyDelay);
    }

    public void BuyDelayOnTurn()
    {
        if (buyDelayList.Count == 0)
            return;

        foreach (BuyDelay delay in buyDelayList.ToArray())
        {
            if (delay.OnTurn())
            {
                buyDelayList.Remove(delay);
            }
        }
    }

    public void UpdateArmyListy(ArmyController army)
    {
        ownAmryList.Remove(army);
    }
}