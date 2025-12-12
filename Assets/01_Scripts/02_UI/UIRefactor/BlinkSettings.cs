using UnityEngine;

[CreateAssetMenu(menuName = "Game/Blink Settings")]
public class BlinkSettings : ScriptableObject
{
    [Header("Blink")]
    public int blinkCount = 2;
    public float blinkSpeed = 0.15f;
    public float blinkInterval = 0.1f;

    [Header("Vignette Closing")]
    public float maxVignette = 1.0f;
    public float finalCloseSpeed = 1.0f;

    [Header("Exposure Darken (완전 암전 효과)")]
    public bool useExposureDarkening = true;
    public float finalExposure = -5.0f;
    public float exposureDarkenSpeed = 0.4f;

    [Header("Wake Up")]
    public float wakeOpenSpeed = 1.0f;
}

