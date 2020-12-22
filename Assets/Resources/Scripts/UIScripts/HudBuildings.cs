using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudBuildings : Hud
{
    [SerializeField]
    private GameObject
        buildingPrefab,
        buildingPrefabHolder;

    private List<BuildingPrefabContorller> buttonsList;

    Buildings buildings;

    public Button buttonSwitch;

    public override void OnOpen()
    {
        gameObject.SetActive(true);
        
    }

    public override void OnClose()
    {
        gameObject.SetActive(false);
    }

    public override void OnStart()
    {
        gameObject.SetActive(false);

        buttonSwitch.onClick.AddListener(() => { UiController.Instance.Open("hud_units"); OnClose(); });

        buttonsList = new List<BuildingPrefabContorller>();

        buildings = GameController.Insnatce.player.playerCastle.buildings;

        CreateHudElements();
    }

    public void CreateHudElements()
    {
        BuildingPrefabContorller temp;
        foreach(BuildingsData building in GameController.Insnatce.player.playerCastle.buildingsInfo)
        {
            temp = Instantiate(buildingPrefab, buildingPrefabHolder.transform).GetComponent<BuildingPrefabContorller>();
            temp.InitPrefab(building);
            buttonsList.Add(temp);
        }
    }

    public void UpdateHudBuildings()
    {
        buttonsList.ForEach(x => x.UpdatePrefabInfo());
    }

    public void ButtonClick(BuildingsData building)
    {
        Debug.Log("Button with name: "+ building.building.name);

        buildings.ActionWithBuilding(building);

        UpdateHudBuildings();
    }
}
