using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudSendUnits : Hud
{
    public GameObject sendUnitPrefab;

    public GameObject holder;

    public List<ArmyData>
        castleArmy,
        sendArmy;

    public List<UnitSendPrefab>
        prefabsList;

    public Button
        buttonSend,
        buttonCancel;

    public override void OnStart()
    {
        gameObject.SetActive(false);
        prefabsList = new List<UnitSendPrefab>();
        buttonSend.onClick.AddListener(() => SendAmry());
        buttonCancel.onClick.AddListener(() => OnClose());
    }
    
    public override void OnOpen()
    {
        sendArmy = new List<ArmyData>();
        gameObject.SetActive(true);
        castleArmy = GameController.Insnatce.player.playerCastle.GetComponent<ArmyController>().armyInfo;

        if (prefabsList.Count > 0)
        {
            prefabsList.ForEach(x => Destroy(x.gameObject));
            prefabsList = new List<UnitSendPrefab>();
        }

        foreach (ArmyData army in castleArmy)
        {
            UnitSendPrefab prefab = Instantiate(sendUnitPrefab, holder.transform).GetComponent<UnitSendPrefab>();
            prefab.Initialize(army);
            prefabsList.Add(prefab);
        }
    }

    public override void OnClose()
    {
        gameObject.SetActive(false);
    }

    public void UpdateSendArmyList(UnitInfo data, int count)
    {
        if (count == 0)
            return;
        if (sendArmy.Count != 0 && sendArmy.Exists(x => x.unitInfo.name == data.name))
        {
            sendArmy.Find(x => x.unitInfo.name == data.name).count = count;
        }
        else
        {
            ArmyData army = new ArmyData(data, count, GameController.Insnatce.player.Id);
            sendArmy.Add(army);
        }
    }

    public void SendAmry()
    {
        foreach(UnitSendPrefab x in prefabsList)
        {
            if (x.armyToSendCount > 0)
            {
                if(x.armyToSendCount > int.Parse(x.unitCurrentCount.text))
                {
                    Debug.LogError("Army to send count incorrect");
                    return;
                }
                UpdateSendArmyList(x.unit, x.armyToSendCount);
            }
        }

        if (sendArmy.Count == 0)
        {
            Debug.LogError("SendArmy is null");
            return;
        }
        sendArmy.ForEach(x => Debug.Log(x.unitInfo + " " +x.count));
        OnClose();
        GameController.Insnatce.player.playerCastle.MoveArmyFromCastle(sendArmy);
    }
}
