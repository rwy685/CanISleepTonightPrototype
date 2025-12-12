using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UISleepVignette : MonoBehaviour
{
    [SerializeField] private Image vignetteImage;
    [SerializeField] private float duration = 1.5f;

    private void Awake()
    {
        //시작 시 투명
        var color = vignetteImage.color;
        color.a = 0;
        vignetteImage.color = color;
        gameObject.SetActive(false);
    }

    public void PlayVignette(float duration)
    {
        gameObject.SetActive(true);

        vignetteImage.DOFade(1f, duration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                //완전히 어두워진 후 GameManager가 다음 Phase로 넘어가도록 메시지 보내기
                GameManager.Instance.StartCoroutine(
                    GameManager.Instance.TransitionToSettlementAfterSleep()
                    );
            });
    }
}
