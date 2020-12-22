using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hud : MonoBehaviour, IUi
{
    public string Name;
    
    public virtual void OnStart()
    {

    }

    public virtual void OnOpen()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnClose()
    {
        gameObject.SetActive(false);
    }
}
