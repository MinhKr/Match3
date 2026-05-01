using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    [SerializeField] private Image fill_img;
    [SerializeField] private TMP_Text loading_txt;

    [Header("Icon")]
    [SerializeField] private Image icon;
    private CanvasGroup light;

    Tweener T_Loading, T_Icon, T_Light;

    private void OnEnable()
    {
        Application.targetFrameRate = 60;
        //PlayerDataManager.SetClaimX5(false);

        AnimIcon();
        AnimLight();

        fill_img.fillAmount = 0;
        OnLoading();
    }

    private void OnLoading()
    {
        float cooldownTime = 3f;
        T_Loading = fill_img.DOFillAmount(1f, cooldownTime).SetEase(Ease.Linear).OnUpdate(() =>
        {
            loading_txt.text = $"Loading {(int)(fill_img.fillAmount * 100)}%";
        }).OnComplete(() =>
        {
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        });
    }

    private void AnimIcon()
    {
        T_Icon = icon.transform.DOScale(1.1f, 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    private void AnimLight()
    {
        //T_Light = light.DOFade(0.7f, 1f).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        T_Loading?.Kill();
        T_Icon?.Kill();
        T_Light?.Kill();
    }
}
