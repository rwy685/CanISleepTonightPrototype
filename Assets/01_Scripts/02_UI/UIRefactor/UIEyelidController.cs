using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIEyelidController : MonoBehaviour
{
    [SerializeField] private RectTransform topLid;
    [SerializeField] private RectTransform bottomLid;

    // 화면 절반 정도를 덮는 양
    private float eyeCloseSize;

    private void Awake()
    {
        eyeCloseSize = Screen.height * 0.5f;

        // 초기 위치: 완전히 열림 상태 (Height = 0)
        topLid.sizeDelta = new Vector2(topLid.sizeDelta.x, 0);
        bottomLid.sizeDelta = new Vector2(bottomLid.sizeDelta.x, 0);
    }

    // 단일 눈 감기 애니메이션
    public Tween CloseEyes(float duration)
    {
        Sequence seq = DOTween.Sequence();

        seq.Join(topLid.DOSizeDelta(
            new Vector2(topLid.sizeDelta.x, eyeCloseSize),
            duration));

        seq.Join(bottomLid.DOSizeDelta(
            new Vector2(bottomLid.sizeDelta.x, eyeCloseSize),
            duration));

        return seq;
    }

    public Tween OpenEyes(float duration)
    {
        Sequence seq = DOTween.Sequence();

        seq.Join(topLid.DOSizeDelta(
            new Vector2(topLid.sizeDelta.x, 0),
            duration));

        seq.Join(bottomLid.DOSizeDelta(
            new Vector2(bottomLid.sizeDelta.x, 0),
            duration));

        return seq;
    }


    // 깜빡임 (눈 감기 → 눈 뜨기)
    public void Blink(int count = 1, float speed = 0.15f)
    {
        Sequence blinkSeq = DOTween.Sequence();

        for (int i = 0; i < count; i++)
        {
            blinkSeq.Append(CloseEyes(speed))
                    .Append(OpenEyes(speed));
        }
    }

    // “잠들기” 연출: 깜빡임 → 서서히 감기 → 완전히 닫힘
    public void PlaySleepAnimation()
    {
        float blinkSpeed = 0.25f;      // 깜빡임 속도(너무 빠르지 않게)
        float finalCloseSpeed = 1.2f;  // 완전히 감길 때 느리게

        Sequence seq = DOTween.Sequence();

        // 1) 첫 번째 깜빡임
        seq.Append(CloseEyes(blinkSpeed));
        seq.Append(OpenEyes(blinkSpeed));
        seq.AppendInterval(0.15f);

        // 2) 두 번째 깜빡임
        seq.Append(CloseEyes(blinkSpeed));
        seq.Append(OpenEyes(blinkSpeed));
        seq.AppendInterval(0.25f);

        // 3) 잠드는 듯이 서서히 감김
        seq.Append(CloseEyes(finalCloseSpeed).SetEase(Ease.InOutQuad));

        // 4) 완료 콜백
        seq.OnComplete(() =>
        {
            GameManager.Instance.StartCoroutine(
                GameManager.Instance.TransitionToSettlementAfterSleep()
            );
        });
    }


    // “잠에서 깸” 연출: 천천히 열린 후 깜빡 한 번
    public void PlayWakeUpAnimation()
    {
        float wakeOpenSpeed = 1.0f;
        float blinkSpeed = 0.18f;

        Sequence seq = DOTween.Sequence();

        // 1) 눈이 천천히 열림
        seq.Append(OpenEyes(wakeOpenSpeed).SetEase(Ease.OutQuad));

        // 2) 짧게 깜빡임
        seq.Append(CloseEyes(blinkSpeed));
        seq.Append(OpenEyes(blinkSpeed));
    }

}

