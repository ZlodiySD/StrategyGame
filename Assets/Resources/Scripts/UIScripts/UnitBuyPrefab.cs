using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitBuyPrefab : MonoBehaviour
{
    public Text
        coinsCost,
        peopleCost,
        countSelected,
        unitName;

    public Button 
        buttonInc,
        buttonDec,
        buttonBuy;
    private int count = 0;
    

    public void Init(HudUnits hud, UnitInfo info)
    {
    
        buttonInc.onClick.AddListener(() => UpdateText(true));
        buttonDec.onClick.AddListener(() => UpdateText(false));
        buttonBuy.onClick.AddListener(() => hud.AttemptToBuy(info, count));

        unitName.text = info.unitName;
        peopleCost.text =  info.peopleCost.ToString();
        coinsCost.text = info.coinsCost.ToString();
    }

    private void UpdateText(bool isInc)
    {
        if (isInc && count < 99)
        {
            count++;
            countSelected.text = count.ToString();
        }
        else if (!isInc && count > 0)
        {
            count--;
            countSelected.text = count.ToString();
        }
    }
}
