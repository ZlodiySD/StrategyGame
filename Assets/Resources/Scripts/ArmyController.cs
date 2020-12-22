using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArmyController : MonoBehaviour
{
    #region Variables
    [SerializeField]
    public int ownerId;

    [HideInInspector]
    public int amrySpeed;

    [HideInInspector]
    public List<ArmyData> armyInfo;
    private List<UnitInfo> unitInfoList;

    private Grid mainGrid;

    public bool 
        isMovedThisTurn,
        isOnCastle;

    [HideInInspector]
    public int
        armyDefBonus,
        armyAttackBonus;

    public int armyTravelDefBonus { get => homeCastle.armyTravelDefBonus; }

    public int armyTravelAttackBonus { get => homeCastle.armyTravelAttackBonus; }

    public Castle homeCastle;
    public AiController controller;

    public int carriedCoins;
    public int CarriedCoins
    {
        get
        {
            return carriedCoins;
        }
        set
        {
            Debug.Log("carriedCoins: " + carriedCoins + " value: " + value);
            carriedCoins = value;
        }
    }
    #endregion
    
    private void Start()
    {
        unitInfoList = GameController.Insnatce.unitsInfo;

        if (TryGetComponent(out Castle C))
        {
            armyInfo = new List<ArmyData>();

            UpdateCatleArmyInfo();
            isOnCastle = true;
            isMovedThisTurn = true;
            ownerId = C.ownerId;

            armyDefBonus = C.armyDefBonus;
            armyAttackBonus = C.armyAttackBonus;
        }
        mainGrid = GameController.Insnatce.grid;
    }

    public void OnTurnStart()
    {
        if (!isOnCastle)
        {
            isMovedThisTurn = false;
        }
    }

    public void InitArmy(List<ArmyData> army, Castle castle)
    {
        mainGrid = GameController.Insnatce.grid;
        armyInfo = army;
        ownerId = castle.ownerId;
        isMovedThisTurn = true;
        homeCastle = castle;

        if (homeCastle.TryGetComponent(out AiController ai))
            controller = ai;

        if (ownerId == 1)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if (controller.isPlayerFriendly)
            gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        else
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;

        FindObjectOfType<Castle>();

        CalculateArmySpeed();

        army.ForEach(x => Debug.Log(x.unitInfo.name + " " + x.count));
    }

    //public void UpdateArmyInfo(UnitInfo unitInfo, int count)
    //{
    //    if (armyInfo.Count != 0 && armyInfo.Exists(x => x.unitInfo.name == unitInfo.name))
    //    {
    //        armyInfo.Find(x => x.unitInfo.name == unitInfo.name).count += count;
    //    }
    //    else
    //        armyInfo.Add(new ArmyData(unitInfo, count, ownerId));
    //    CalculateArmySpeed();
    //}

    public void UpdateArmyInfo(ArmyData data)
    {
        data.ownerId = ownerId;
        if (armyInfo.Count != 0 && armyInfo.Exists(x => x.unitInfo.name == data.unitInfo.name))
        {
            armyInfo.Find(x => x.unitInfo.name == data.unitInfo.name).count += data.count;
        }
        else
            armyInfo.Add(data);
        CalculateArmySpeed();
    }

    public void UpdateArmyInfo(List<ArmyData> armyDatas, bool isClearArmyData = true)
    {
        if (isClearArmyData)
            armyInfo = new List<ArmyData>();

        foreach (ArmyData amry in armyDatas)
        {
            UpdateArmyInfo(amry);
        }
    }

    public void ArmySplit(List<ArmyData> data)
    {
        foreach (ArmyData army in armyInfo.ToArray())
        {
            if (army.count <= 0)
                continue;
            if(data.Exists(x => x.unitInfo.name == army.unitInfo.name))
            {
                army.count -= data.Find(x => x.unitInfo.name == army.unitInfo.name).count;
                //armyInfo.Find(x => x == army).count = army.count;
            }

            if (army.count <= 0)
            {
                armyInfo.Remove(army);
            }
        }
    }

    public void CalculateArmySpeed()
    {
        if(armyInfo.Count > 0)
            amrySpeed = armyInfo.Min(x => x.unitInfo.speed);
        else
        {
            amrySpeed = 0;
            Debug.Log("Army speed is 0, is something wrong?");
        }

        Debug.Log("Army speed by CalculateArmySpeed: " + amrySpeed);
    }

    public void OnTurnEnd()
    {
        if (!isOnCastle)
            isMovedThisTurn = false;
    }

    public void UpdateCatleArmyInfo()
    {
        armyInfo.ForEach(x => x.castleBonusAttack = armyAttackBonus);
        armyInfo.ForEach(x => x.castleBonusDefence = armyDefBonus);
        armyInfo.ForEach(x => x.travelBonusAttack = armyTravelAttackBonus);
        armyInfo.ForEach(x => x.travelBonusDefence = armyTravelDefBonus);
    }

    public bool MoveArmyTo((int x, int y)x)
    {
        return MoveArmyTo( mainGrid.GetWorldPosition(x.x, x.y));
    }

    public bool MoveArmyTo(Vector3 position)
    {
        if (isMovedThisTurn || isOnCastle)
        {
            return true;
        }

        GameObject pointObject = mainGrid.GetValue(position);

        if (pointObject == null)
        {
            Debug.Log("Move to empty point");
        }
        else if(pointObject.TryGetComponent(out Castle castle) && castle.ownerId == ownerId)
        {
            if(castle.ownerId == ownerId)
            {
                //homeCastle.castleArmy.UpdateArmyInfo(armyInfo);
                Debug.Log(armyInfo.Count);
                homeCastle.castleArmy.UpdateArmyInfo(armyInfo, false);
                homeCastle.coinsCurrent += CarriedCoins;
                homeCastle.UpdateArmyListy(this);
                Destroy(gameObject);
                return true;
            }
            else if (homeCastle.TryGetComponent(out AiController aicc) && aicc.isPlayerFriendly)
            {
                return true;
            }
            else
            {
                Debug.Log("Move to enemy Base");
                UpdateCatleArmyInfo();
                GameController.Insnatce.fightController.InitiateFight(this, pointObject.GetComponent<ArmyController>());
            }
        }
        else if (pointObject.TryGetComponent(out ArmyController otherArmy))
        {
            if (otherArmy.ownerId == ownerId)
            {
                Debug.Log("Move to own Army");
                return false;
            }
            else if (otherArmy.ownerId != ownerId)
            {
                if(otherArmy.TryGetComponent(out AiController aic))
                {
                    if (ownerId == 1 && aic.isPlayerFriendly)
                        return false;
                    else if (controller != null && controller.isPlayerFriendly)
                        return false;
                }
                Debug.Log("Move to enemy Army");
                UpdateCatleArmyInfo();
                GameController.Insnatce.fightController.InitiateFight(this, otherArmy);
            }
        }

        isMovedThisTurn = true;

        mainGrid.MoveTo(position, gameObject);
        return true;
    }

    public void Die(ArmyController armyKiller)
    {
        Debug.Log("Army owned by:" + ownerId + " died");
        if (isOnCastle)
        {
            if (TryGetComponent(out Castle c))
            {
                if(c.coinsCurrent > 0)
                    armyKiller.CarriedCoins += c.coinsCurrent / 2;
                c.CastleDestroy();
            }
        }
        else
            Destroy(gameObject);
    }
}
