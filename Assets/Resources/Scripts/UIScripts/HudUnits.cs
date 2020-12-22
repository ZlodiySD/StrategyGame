using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudUnits : Hud
{
    public Button buttonSwitch;

    private List<UnitInfo> units;

    List<ArmyData> avaliebleToTrainUnit;
    Castle castle;

    public GameObject 
        buyPrefab,
        holder;

    public override void OnStart()
    {
        buttonSwitch.onClick.AddListener(() => { UiController.Instance.Open("hud_buildings"); OnClose(); });

        castle = GameController.Insnatce.player.playerCastle;
        units = GameController.Insnatce.unitsInfo;
    }

    public override void OnOpen()
    {
        gameObject.SetActive(true);

        UnitBuyPrefab temp;
        avaliebleToTrainUnit = castle.avaliebleToTrainUnits;
        foreach (ArmyData uniy in avaliebleToTrainUnit)
        {
            temp = Instantiate(buyPrefab, holder.transform).GetComponent<UnitBuyPrefab>();
            temp.Init(this, uniy.unitInfo);
        }
    }

    public override void OnClose()
    {
        gameObject.SetActive(false);

        foreach (var obj in holder.GetComponentsInChildren<UnitBuyPrefab>())
        {
            Destroy(obj.gameObject);
        }
    }
    public void AttemptToBuy(UnitInfo info, int count)
    {
        if(count > 0)
        {
            castle.BuyUnitWithDelay(info,count);
        }
    }
}
