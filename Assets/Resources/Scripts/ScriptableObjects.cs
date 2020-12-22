using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "ScriptableObjects/Building")]
public class BuildingInfo : ScriptableObject
{
    public int cost;
    public int maxLevel;
    public string buidingName;
    public Sprite image;
}

[CreateAssetMenu(fileName = "New Unit", menuName = "ScriptableObjects/Unit")]
public class UnitInfo : ScriptableObject
{
    public int coinsCost;
    public int peopleCost;
    public string unitName;
    public Sprite image;

    public int attack;
    public int initiative;
    public int defence;
    public int speed;
    public int trainingTime;

    public bool isReciveConterAttack;
}
