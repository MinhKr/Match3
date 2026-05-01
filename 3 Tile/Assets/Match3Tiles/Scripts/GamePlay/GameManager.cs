using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public UiController UiController;

    public bool isInGame = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartIsInGame()
    {
        StartCoroutine(IE_DelayIsInGame());
    }

    IEnumerator IE_DelayIsInGame()
    {
        yield return null;
        isInGame = true;
    }
}
