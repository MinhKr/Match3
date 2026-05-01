using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiHome : MonoBehaviour
{
    public static UiHome Instance { get; private set; }

    [Header("Data Home")]
    public DataHome dataHome;

    [SerializeField] private Button btn_Setting;

    //[Header("Panel Lives")]
    //[SerializeField] private TMP_Text amoutLives_txt;
    //[SerializeField] private TMP_Text timeRemaingAddLives_txt;
    //[SerializeField] private Button btn_AddLive;

    public int MAX_TURNS = 5;
    private float RECOVERY_TIME = 30;


    //public int playerTurns;
    //[SerializeField] private DateTime lastPlayTime;

    [Header("Panel Coin")]
    [SerializeField] public Image IconCoin;
    [SerializeField] private TMP_Text AmoutCoin_txt;

    [Header("Play")]
    [SerializeField] private Button btn_Play;

    [Header("Level")]
    [SerializeField] private TMP_Text Level_txt;

    //[Header("popup")]
    //public PopupSetting popupSetting;
    //public RemainingShowAds remainingShowAds;
    //public AnimCoin AnimCoin;

    [Header("Anim")]
    [SerializeField] private RectTransform ContentTop;
    private Vector2 contentTopOriginPos;
    private float contentTopHideY;
    [SerializeField] private RectTransform ContentBottom;
    private Vector2 contentBottomOriginPos;
    private float contentBottomHideY;
    Tweener T_MoveContentTop, T_MoveContentBottom, T_MoveBackContentTop, T_MoveBackContentBottom;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        CachePosition();
    }

    private void OnEnable()
    {
        btn_Play.onClick.AddListener(OnPlayGame);
        //btn_Setting.onClick.AddListener(OnOpenSetting);
        //btn_AddLive.onClick.AddListener(OnAddLive);

        ActiveAnim();
        //InitLevelText();

        //initBtnAds();
        //InitCoin();

        //InitClaimX5();
    }


    private void Update()
    {
        UpdateCountdownText();
        CheckAFKAds();
    }

    private void InitClaimX5()
    {
        //if (PlayerDataManager.GetIsClaimX5())
        //{
        //    AnimCoin.gameObject.SetActive(true);
        //}
        //else
        //{
        //    AnimCoin.gameObject.SetActive(false);
        //}
    }

    public void CheckAFKAds()
    {
        //if (Input.GetMouseButton(0) && AdsHandle.instance.CanShowInterAFK)
        //{
        //    /*UiController.PanelAds.gameObject.SetActive(false);*/
        //    remainingShowAds.gameObject.SetActive(false);
        //}

        //if (Input.GetMouseButton(0) && !AdsHandle.instance.Detected /*&& !isOpenShop)*/)
        //{
        //    AdsHandle.instance.Detected = true;
        //    AdsHandle.instance.ShowInterAFK();
        //}
    }


    private void OnAddLive()
    {
        //SoundManager.Instance.PlayFxSound(SoundManager.Instance.buttonclick);

        //AdManager.instance.ShowReward(() =>
        //{
        //    playerTurns++;
        //    PlayerDataManager.SetAmoutTurn(playerTurns);
        //    amoutLives_txt.text = playerTurns.ToString();
        //    initBtnAds();
        //}, () =>
        //{
        //    GameManager.instance.UiController.PopupNoInternet.gameObject.SetActive(true);
        //}, "ShowReward");
    }

    public void initBtnAds()
    {
        Debug.Log("init");

        //btn_AddLive.gameObject.SetActive(PlayerDataManager.GetAmoutTurn() == 0);
    }

    public void InitCoin()
    {
        //AmoutCoin_txt.text = Helper.FormatCurrency(PlayerDataManager.GetCoin());
    }

    private void InitLevelText()
    {
        //Level_txt.text = $"Level {PlayerDataManager.GetCurrentLevel()}";
    }

    public void ActiveAnim()
    {
        AnimContentTop();
        AnimContenBottom();
    }

    private void CachePosition()
    {
        RectTransform rtTop = ContentTop;
        RectTransform rtBottom = ContentBottom;

        //Top Content
        contentTopOriginPos = rtTop.anchoredPosition;
        contentTopHideY = contentTopOriginPos.y + rtTop.rect.height;

        //Bottom Content
        contentBottomOriginPos = rtBottom.anchoredPosition;
        contentBottomHideY = -rtBottom.rect.height;
    }

    public void AnimContentTop()
    {
        Vector2 startPos = contentTopOriginPos;
        startPos.y = contentTopHideY;

        ContentTop.anchoredPosition = startPos;

        T_MoveContentTop = ContentTop.DOAnchorPos(contentTopOriginPos, 0.3f).SetEase(Ease.Linear);
    }

    public void AnimContenBottom()
    {
        Vector2 startPos = contentBottomOriginPos;
        startPos.y = contentBottomHideY;

        ContentBottom.anchoredPosition = startPos;

        T_MoveContentBottom = ContentBottom.DOAnchorPos(contentBottomOriginPos, 0.3f).SetEase(Ease.Linear);
    }

    public void AnimMoveBack()
    {
        AnimBackContentTop();
        AnimBackContenBottom();
    }

    public void AnimBackContentTop()
    {
        Vector2 targetPos = contentTopOriginPos;
        targetPos.y = contentTopHideY;

        T_MoveBackContentTop = ContentTop.DOAnchorPos(targetPos, 0.3f).SetEase(Ease.Linear);
    }

    public void AnimBackContenBottom()
    {
        Vector2 targetPos = contentBottomOriginPos;
        targetPos.y = contentBottomHideY;

        T_MoveBackContentBottom = ContentBottom.DOAnchorPos(targetPos, 0.3f).SetEase(Ease.Linear);
    }

    private void Start()
    {
        LoadGameState();

        //SoundManager.Instance.PlayBGM(SoundManager.Instance.BGM);
    }

    void LoadGameState()
    {
        //playerTurns = PlayerDataManager.GetAmoutTurn();
        //amoutLives_txt.text = playerTurns.ToString();
        //long ticks = Convert.ToInt64(PlayerDataManager.GetLastPlayTimeHome());
        //lastPlayTime = new DateTime(ticks);
    }

    void UpdateCountdownText()
    {
        //if (playerTurns < MAX_TURNS)
        //{
        //    TimeSpan timePassed = DateTime.Now.Subtract(lastPlayTime);

        //    double minutesPassed = timePassed.TotalMinutes;
        //    float remainingTime = RECOVERY_TIME - (float)(minutesPassed % RECOVERY_TIME);

        //    // Hi?n th? th?i gian còn l?i cho vi?c h?i tim
        //    TimeSpan timeSpan = TimeSpan.FromMinutes(remainingTime);
        //    timeRemaingAddLives_txt.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

        //    // Ki?m tra và c?ng l??t ch?i n?u ?ã ?? th?i gian
        //    if (minutesPassed >= RECOVERY_TIME)
        //    {
        //        int additionalTurns = (int)(minutesPassed / RECOVERY_TIME);
        //        playerTurns = Mathf.Min(MAX_TURNS, playerTurns + additionalTurns);
        //        amoutLives_txt.text = playerTurns.ToString();

        //        if (playerTurns == MAX_TURNS)
        //        {
        //            // Khi ng??i ch?i ??t t?i ?a l??t ch?i, reset th?i gian h?i l?i thành 29'59"
        //            lastPlayTime = DateTime.Now;
        //        }
        //        else
        //        {
        //            // C?p nh?t lastPlayTime ?? gi? l?i ph?n th?i gian ch? còn l?i
        //            double remainingMinutes = minutesPassed % RECOVERY_TIME;
        //            lastPlayTime = DateTime.Now.Subtract(TimeSpan.FromMinutes(remainingMinutes));
        //        }

        //        SaveGameState();
        //        //PlayerDataManager.SetLastPlayHome(lastPlayTime.Ticks.ToString());
        //    }
        //}
        //else
        //{
        //    // N?u ng??i ch?i ?ã ??y l??t ch?i, hi?n th? "Full"
        //    timeRemaingAddLives_txt.text = "Full";
        //}

        //// C?p nh?t l??t ch?i
        //amoutLives_txt.text = playerTurns.ToString();
    }
    private void OnOpenSetting()
    {
        //SoundManager.Instance.PlayFxSound(SoundManager.Instance.buttonclick);
        //AnimMoveBack();
        //popupSetting.gameObject.SetActive(true);
    }

    private void OnPlayGame()
    {
        //SoundManager.Instance.PlayFxSound(SoundManager.Instance.buttonclick);

        //if (playerTurns > 0)
        //{
        //    SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
        //}
        SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
    }

    void SaveGameState()
    {
        //PlayerDataManager.SetAmoutTurn(playerTurns);
        /* PlayerDataManager.SetLastPlayHome(DateTime.Now.Ticks.ToString());*/
    }

    private void OnDisable()
    {
        DOTween.KillAll();
        T_MoveContentTop?.Kill();
        T_MoveBackContentBottom?.Kill();
        T_MoveBackContentTop?.Kill();
        T_MoveContentBottom?.Kill();

        btn_Play.onClick.RemoveListener(OnPlayGame);
    }
}