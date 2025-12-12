using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class BlinkVignetteController : MonoBehaviour
{
    [Header("PostProcess References")]
    public Volume volume;                // Global Volume
    public BlinkSettings settings;       // 기획자 SO

    private Vignette vignette;
    private ColorAdjustments colorAdjust;

    private void Awake()
    {
        // Vignette 가져오기
        if (!volume.profile.TryGet(out vignette))
            Debug.LogError("[BlinkVignette] Vignette Override가 Profile에 없습니다.");

        // ColorAdjustments 가져오기
        if (!volume.profile.TryGet(out colorAdjust))
            Debug.LogError("[BlinkVignette] ColorAdjustments Override가 Profile에 없습니다.");
    }


    // --------------------------
    // Helper Getter / Setter
    // --------------------------

    private float GetIntensity() => vignette.intensity.value;
    private void SetIntensity(float v) => vignette.intensity.value = v;

    private float GetExposure() => colorAdjust.postExposure.value;
    private void SetExposure(float v) => colorAdjust.postExposure.value = v;


    // --------------------------
    // 단일 깜빡임
    // --------------------------

    public void PlayBlinkOnce(float speed)
    {
        DOTween.Sequence()
            .Append(DOTween.To(GetIntensity, SetIntensity, settings.maxVignette * 0.6f, speed))
            .Append(DOTween.To(GetIntensity, SetIntensity, 0f, speed));
    }


    // --------------------------
    // 잠들기 연출 전체
    // --------------------------

    public void PlaySleepAnimation()
    {
        Sequence seq = DOTween.Sequence();

        // 1) 깜빡임 반복
        for (int i = 0; i < settings.blinkCount; i++)
        {
            seq.Append(DOTween.To(GetIntensity, SetIntensity, settings.maxVignette * 0.6f, settings.blinkSpeed));
            seq.Append(DOTween.To(GetIntensity, SetIntensity, 0f, settings.blinkSpeed));
            seq.AppendInterval(settings.blinkInterval);
        }

        // 2) 부드럽게 눈 감기 (Vignette 강하게)
        seq.Append(
            DOTween.To(GetIntensity, SetIntensity, settings.maxVignette, settings.finalCloseSpeed)
            .SetEase(Ease.InOutQuad)
        );

        // 3) 완전 암전 (Exposure 조절 — 선택)
        if (settings.useExposureDarkening)
        {
            seq.Append(
                DOTween.To(GetExposure, SetExposure, settings.finalExposure, settings.exposureDarkenSpeed)
            );
        }

        // NOTE: GameManager에서 Animation 종료 후 페이즈 전환 처리
    }


    // --------------------------
    // 깨어나기 연출
    // --------------------------

    public void PlayWakeUpAnimation()
    {
        Sequence seq = DOTween.Sequence();

        // Exposure를 다시 0으로 복구 (만약 사용중이라면)
        if (settings.useExposureDarkening)
            seq.Append(DOTween.To(GetExposure, SetExposure, 0f, settings.wakeOpenSpeed));

        // Vignette도 자연스럽게 제거
        seq.Append(
            DOTween.To(GetIntensity, SetIntensity, 0f, settings.wakeOpenSpeed)
            .SetEase(Ease.OutQuad)
        );
    }
}


