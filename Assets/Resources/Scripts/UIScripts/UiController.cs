using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{

    [SerializeField]
    public List<Hud> listHud;

    public string Name;

    [SerializeField]
    private Button 
        sendArmy,
        endTurn;

    [SerializeField]
    private Text
        coinsCount,
        peopleCount;

    private static UiController instance;

    public static UiController Instance { get => instance; set => instance = value; }

    public GameObject blackSubstrate;

    private void LateUpdate()
    {
        UpdateHud();
    }

    public void InitInterface()
    {
        instance = this;

        StartCoroutine(InitHud());

        sendArmy.onClick.AddListener(() => { Open("hud_send_units"); ; });
        endTurn.onClick.AddListener(() => { GameController.Insnatce.EndTurn(); });
    }

    public void TurnStart(int x)
    {
        if(x == 0)
        {
            blackSubstrate.SetActive(false);
        }
        else
            blackSubstrate.SetActive(true);
    }

    public void UpdateHud()
    {
        if (GameController.Insnatce.player != null && GameController.Insnatce.player.playerCastle != null)
        {
            coinsCount.text = GameController.Insnatce.player.playerCastle.coinsCurrent.ToString();
            peopleCount.text = GameController.Insnatce.player.playerCastle.peoplesCurrent.ToString() + "/" + GameController.Insnatce.player.playerCastle.poeplesLimit.ToString();
        }
    }

    IEnumerator InitHud()
    {
        yield return new WaitForEndOfFrame();
        listHud.ForEach(x => x.OnStart());
        UpdateHud();
        Open("hud_buildings");
    }

    public void Open(string name)
    {
        listHud.Find(x => x.Name == name).OnOpen();
    }

    public void ShowNotification(string notificationText)
    {
        Debug.Log("ShowNotification with text: " + notificationText);
    }
}
