using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightController
{
    private ArmyController
        atckArmyController,
        defArmyCOntroller;

    private List<ArmyData>
        atckArmyList,
        defArmyList,
        attackOrderList;

    private int 
        defArmyAtckBonus,
        defArmyDefBonus;

    public void InitiateFight(ArmyController atckArmy, ArmyController defArmy)
    {
        attackOrderList = new List<ArmyData>();

        if (atckArmy == null || defArmy == null)
        {
            Debug.LogError("Army is null");
            return;
        }

        atckArmyController = atckArmy;
        defArmyCOntroller = defArmy;

        defArmyAtckBonus = defArmyCOntroller.armyAttackBonus;
        defArmyDefBonus = defArmyCOntroller.armyDefBonus;

        atckArmyList = atckArmyController.armyInfo;
        defArmyList = defArmyCOntroller.armyInfo;

        atckArmyList.Sort();
        defArmyList.Sort();

        if (atckArmyList.Count == 0 || defArmyList.Count == 0)
        {
            Debug.LogError("Army is empty");
            if(defArmyList.Count == 0)
            {
                defArmy.Die(atckArmy);
            }
            return;
        }

        CreateAttackOrder();
    }

    private void CreateAttackOrder()
    {
        List<ArmyData>
            tempAtckList = new List<ArmyData>(),
            tempDefList = new List<ArmyData>(),
            temp = new List<ArmyData>();
        
        tempAtckList = atckArmyList;
        tempDefList = defArmyList;

        int max = 0;
        atckArmyList.ForEach(x =>
        {
            if (x.unitInfo.speed > max)
                max = x.unitInfo.speed;
        });

        temp = atckArmyList.FindAll(x => x.unitInfo.speed == max);

        ArmyData tmp = temp[UnityEngine.Random.Range(0, temp.Count)];

        tempAtckList.Remove(tmp);
        attackOrderList.Add(tmp);

        while (tempAtckList.Count > 0 || tempDefList.Count > 0)
        { 
            if(tempDefList.Count > 0 && (tempAtckList.Count == 0 || tempAtckList[0].unitInfo.initiative < tempDefList[0].unitInfo.initiative))
            {
                attackOrderList.Add(tempDefList[0]);
                tempDefList.RemoveAt(0);
            }
            else
            {
                attackOrderList.Add(tempAtckList[0]);
                tempAtckList.RemoveAt(0);
            }
        }
        //attackOrderList.ForEach(x => Debug.Log("Unit name: " + x.unitInfo.name + " || Initiative: " + x.unitInfo.initiative + " || Owner: " + x.ownerId + " || Count: " + x.count));


        StartFight();
    }

    private void StartFight()
    {
        Debug.Log("||||||||||||START FIGHT||||||||||||");

       List <ArmyData> temp = new List<ArmyData>();
        ArmyData tempArmy;

        foreach(ArmyData attacker in attackOrderList.ToArray())
        {
            if (attacker.count <= 0)
                continue;

            temp = attackOrderList.FindAll(defender => defender.ownerId != attacker.ownerId);
            if (temp.Count == 0)
                break;
            tempArmy = temp.Find(x => x.health == temp.Min(min => min.health));

            Attack(attacker, tempArmy, false, out ArmyData atck, out ArmyData def);

            if (def.count < 0)
            {
                attackOrderList.Remove(tempArmy);
                Debug.Log("LEFT UNITS: " + attackOrderList.Count + " DEAD UNIT name: " + tempArmy.unitInfo.name + " owner: " + tempArmy.ownerId);
            }
            else
            {
                attackOrderList[attackOrderList.FindIndex(x => x.Equals(tempArmy))] = def;
                if (attacker.unitInfo.isReciveConterAttack)
                {
                    Attack(def, atck, true, out ArmyData atck2, out ArmyData def2);
                    if (def2.count < 0)
                    {
                        attackOrderList.Remove(def2);
                        Debug.Log("LEFT UNITS: " + attackOrderList.Count + " DEAD UNIT name: " + def2.unitInfo.name + " owner: " + def2.ownerId);
                    }
                    else
                    {
                        attackOrderList[attackOrderList.FindIndex(x => x.Equals(attacker))] = def2;
                    }
                }
            }
        }

        if (attackOrderList.FindAll(x => x.ownerId == atckArmyController.ownerId).Count == 0)
        {
            Debug.Log("DEFENDERS WIN");
            atckArmyController.Die(defArmyCOntroller);
            defArmyCOntroller.UpdateArmyInfo(attackOrderList);
        }
        else if (attackOrderList.FindAll(x => x.ownerId == defArmyCOntroller.ownerId).Count == 0)
        {
            Debug.Log("ATTACKERS WIN");
            defArmyCOntroller.Die(atckArmyController);
            atckArmyController.UpdateArmyInfo(attackOrderList);
        }
        else if(attackOrderList.Count == 0)
        {
            Debug.Log("DRAW");
            atckArmyController.Die(defArmyCOntroller);
            defArmyCOntroller.Die(atckArmyController);
        }
        else
            StartFight();

        //attackOrderList.ForEach(x => Debug.Log("Unit name: " + x.unitInfo.name + " Initiative: " + x.unitInfo.initiative + " Owner: " + x.ownerId));
    }

    private void Attack(ArmyData attacker, ArmyData attackSubject, bool isCounterAttack ,out ArmyData atck, out ArmyData def)
    {

        atck = attacker;
        def = attackSubject;

        float attackValue = atck.strenght;

        if (isCounterAttack)
        {
            Debug.Log("COUNTERATTACK");

             attackValue *= 0.75f;
        }

        Debug.Log(atck.unitInfo.name + " " + atck.ownerId + " attacks " + def.unitInfo.name + " " + def.ownerId + " start count: " + def.count);
        Debug.Log("Def health: " + def.health + " Unit defence " + def.unitInfo.defence + " Attack: " + (int)attackValue);

        def.count -= Mathf.Abs((def.health - (int)attackValue) / (def.unitInfo.defence + def.castleBonusDefence));

        Debug.Log("Unit count after fight: " + def.count);
    }
}

