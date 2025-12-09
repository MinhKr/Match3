using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiGamePlay : MonoBehaviour
{
    [SerializeField] private Button btnClose;
    [SerializeField] private Button btnSetting;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text levelText;

    public ListItemPicked listItemPicked;
    public ListItem listItem;

   /* public PopupWin Popupwin;
    public PopupLose PopupLose;
    public PopupBackGame popupBackGame;
    public PopupPause PopupPause;
    public PopupTut PopupTut;
    public RemainingShowAds remainingShowAds;*/
    public RectTransform PanelDimItemMerge;
}
