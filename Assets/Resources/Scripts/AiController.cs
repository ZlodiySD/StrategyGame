using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{
    public Castle aiCastle;

    public int Id;

    public bool isPlayerFriendly;

    public int aggressionBehavior; // 

    public List<BuildingsData> buildingsData;
    public List<ArmyData> armyData;

    void Start()
    {
        aggressionBehavior = Random.Range(0, 51);

        if (Random.Range(0, 5) > 3)
        {
            isPlayerFriendly = true;
            gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            isPlayerFriendly = false;
        }
        aiCastle = GetComponent<Castle>();
        aiCastle.ownerId = Id;
    }

    public void OnTurnStart()
    {
        Debug.Log("AI on turn");
        StartCoroutine(ChoiceAction());
    }

    IEnumerator ChoiceAction()
    {
        yield return new WaitForEndOfFrame();
        if(aggressionBehavior + Random.Range(0,50) > 45)
        {
            if (ChoiceBuildingToBuy())
                StartCoroutine(ChoiceAction());
            else
            {
                MoveArmy();
                GameController.Insnatce.EndTurn();
            }
        }
        else
        {
            if (ChoiceUnitToBuy())
                StartCoroutine(ChoiceAction());
            else
            {
                MoveArmy();
                GameController.Insnatce.EndTurn();
            }
        }
    }

    public void MoveArmy()
    {
        if (aiCastle.ownAmryList.Count != 0)
        {
            Debug.Log("AI army info count:" + aiCastle.ownAmryList.Count);
            foreach (ArmyController army in aiCastle.ownAmryList.ToArray())
            {
                if (army == null)
                    continue;
                List<(int x,int y)> position =  GameController.Insnatce.ArmyMoveZone(army.gameObject);
                position.Shuffle();
                int i = 0;
                foreach((int x, int y) x in position)
                {
                    if (i >= position.Count)
                        break;
                    if (army.MoveArmyTo(position[i]))
                    {
                        break;
                    }
                }
            }
        }
        if (aiCastle.castleArmy.armyInfo.Count != 0)
        {
            aiCastle.MoveArmyFromCastle(ChoiceArmyToSend(aiCastle.castleArmy.armyInfo));
        }
    }

    public bool ChoiceBuildingToBuy()
    {
        buildingsData = aiCastle.buildingsInfo;
        buildingsData.Shuffle();

        foreach(BuildingsData building in buildingsData.ToArray())
        {
            if (building.upgrateCost < aiCastle.coinsCurrent)
            {
                aiCastle.buildings.ActionWithBuilding(building);
                return true;
            }
        }
        return false;
    }

    public bool ChoiceUnitToBuy()
    {
        armyData = aiCastle.avaliebleToTrainUnits;
        armyData.Shuffle();

        foreach (ArmyData unit in armyData.ToArray())
        {
            if (unit.unitInfo.coinsCost > aiCastle.coinsCurrent
                || unit.unitInfo.peopleCost > aiCastle.peoplesCurrent)
                break;
            else
            {
                aiCastle.BuyUnitWithDelay(unit.unitInfo, ChoiceUnitBuyCount(unit));
                return true;
            }
        }
        return false;
    }

    public int ChoiceUnitBuyCount(ArmyData unit)
    {
        int countToBuy = 0;
        int peopleRatio = aiCastle.peoplesCurrent / unit.unitInfo.peopleCost;
        int coinRatio = aiCastle.coinsCurrent / unit.unitInfo.coinsCost;

        if (peopleRatio < coinRatio)
        {
            countToBuy = Random.Range(1, peopleRatio + 1);
        }
        else
        {
            countToBuy = Random.Range(1, coinRatio + 1);
        }


        return countToBuy;
    }
    public List<ArmyData> ChoiceArmyToSend(List<ArmyData> data)
    {
        List<ArmyData> send = new List<ArmyData>();
        ArmyData temp;

        foreach (ArmyData army in data.ToArray())
        {
            temp = army;
            temp.count = Random.Range(1, army.count);
            send.Add(temp);
        }
        return send;
    }
}
