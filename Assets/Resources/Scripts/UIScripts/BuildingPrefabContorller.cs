using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPrefabContorller: MonoBehaviour
{
    [HideInInspector]
    public BuildingsData buildingInfo;

    [SerializeField]
    private Text 
        text,
        price,
        currentLevel;

    [SerializeField]
    private Button button;

    [SerializeField]
    private Image image;

    public void InitPrefab(BuildingsData building)
    {
        buildingInfo = building;

        if (buildingInfo.building.image != null)
        {
            //image.sprite = buildingInfo.image;
        }
        
        button.onClick.AddListener(() =>{ GetComponentInParent<HudBuildings>().ButtonClick(buildingInfo); });

        UpdatePrefabInfo();
    }

    public void UpdatePrefabInfo()
    {
        text.text = buildingInfo.building.name;

        price.text = buildingInfo.upgrateCost.ToString();

        currentLevel.text = buildingInfo.currentLevel.ToString();
    }
}
