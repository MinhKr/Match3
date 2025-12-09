using UnityEngine;

public class UiController : MonoBehaviour
{
    public UiGamePlay uiGameplay;

    public void ProcessWinLose(E_Result type)
    {
        switch (type)
        {
            case E_Result.Processing:
                break;
            case E_Result.Lose:
                //uiGameplay.OnOpenPopupLose();
                Debug.Log("You Lose!");
                break;
            case E_Result.Win:
                //uiGameplay.OnOpenPopupWin();
                Debug.Log("You Win!");
                break;
        }
    }
}
