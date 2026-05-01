using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
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

    public PopupWin Popupwin;
    public PopupLose PopupLose;
    public PopupBackGame popupBackGame;
    public PopupPause PopupPause;
    public PopupTut PopupTut;
    //public RemainingShowAds remainingShowAds;
    public RectTransform PanelDimItemMerge;

    [Header("Anim")]
    [SerializeField] private RectTransform ContentTop;
    [SerializeField] private RectTransform ContentBottom;
    Tweener T_MoveContentTop, T_MoveContentBottom, T_MoveBackContentTop, T_MoveBackContentBottom;

    private void OnEnable()
    {
        btnClose.onClick.AddListener(OnBackToHome);
        btnSetting.onClick.AddListener(OnOpenPopupSetting);

        ActiveAnim();

        InitCoinAndLevel();
    }

    private void Start()
    {
        if (PlayerDataManager.GetCompletedTut())
        {
            PopupTut.gameObject.SetActive(false);
        }
        else
        {
            PopupTut.gameObject.SetActive(true);
        }
    }

    #region Anim Content
    public void ActiveAnim()
    {
        AnimContentTop();
        AnimContenBottom();
    }

    public void AnimContentTop()
    {
        Vector3 _contentTop = ContentTop.anchoredPosition;
        _contentTop.y += 450f;

        ContentTop.anchoredPosition = _contentTop;

        T_MoveContentTop = ContentTop.DOAnchorPosY(0f, 0.3f).SetEase(Ease.Linear);
    }

    public void AnimContenBottom()
    {
        Vector3 _contentBottom = ContentBottom.anchoredPosition;
        _contentBottom.y -= 350f;

        ContentBottom.anchoredPosition = _contentBottom;

        T_MoveContentBottom = ContentBottom.DOAnchorPosY(150f, 0.3f).SetEase(Ease.Linear);
    }

    public void AnimMoveBack()
    {
        AnimBackContentTop();
        AnimBackContenBottom();
    }

    public void AnimBackContentTop()
    {
        T_MoveBackContentTop = ContentTop.DOAnchorPosY(500f, 0.3f).SetEase(Ease.Linear);
    }

    public void AnimBackContenBottom()
    {
        T_MoveBackContentBottom = ContentBottom.DOAnchorPosY(-600f, 0.3f).SetEase(Ease.Linear);
    }

    #endregion

    public void InitCoinAndLevel()
    {
        coinText.text = Helper.FormatCurrency(PlayerDataManager.GetCoin());
        levelText.text = $"Level {PlayerDataManager.GetCurrentLevel()}";
    }

    private void OnBackToHome()
    {
        //if (AdsHandle.instance.canShowInterBack)
        //{
        //    AdManager.instance.ShowInter(null, null, "ShowInter");
        //}

        //SoundManager.Instance.PlayFxSound(SoundManager.Instance.buttonclick);
        AnimMoveBack();
        GameManager.instance.StartDontInGame();
        popupBackGame.gameObject.SetActive(true);
    }

    private void OnOpenPopupSetting()
    {
        //SoundManager.Instance.PlayFxSound(SoundManager.Instance.buttonclick);
        AnimMoveBack();
        GameManager.instance.StartDontInGame();
        PopupPause.gameObject.SetActive(true);
    }

    public void OnOpenPopupWin()
    {
        AnimMoveBack();
        GameManager.instance.StartDontInGame();
        Popupwin.gameObject.SetActive(true);
    }

    public void OnOpenPopupLose()
    {
        GameManager.instance.StartDontInGame();
        PopupLose.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        T_MoveContentTop?.Kill();
        T_MoveBackContentBottom?.Kill();
        T_MoveBackContentTop?.Kill();
        T_MoveContentBottom?.Kill();


        btnClose.onClick.RemoveListener(OnBackToHome);
        btnSetting.onClick.RemoveListener(OnOpenPopupSetting);
    }
}
