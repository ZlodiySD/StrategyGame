using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void OnDestroy()
    {
        SceneManager.LoadScene(0);

        Debug.Log("Oh no you lose");
    }
}
