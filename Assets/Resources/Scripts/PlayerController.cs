using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Castle playerCastle;

    public int Id;

    void Start()
    {
        Id = 1;
        playerCastle = GetComponent<Castle>();
        playerCastle.ownerId = Id;
    }
}
