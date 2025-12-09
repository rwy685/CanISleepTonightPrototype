
public struct StressChangedEvent // 스트레스 수치변경
{
    public float Value;
    public float Max;
}

public struct FatigueChangedEvent // 피로도 수치변경
{
    public float Value;
    public float Max;
}

public struct ActionTimerUpdatedEvent // 액션 타이머 업데이트
{
    public int Day;
    public float RemainingSeconds;
}

public struct InteractionPromptEvent // 상호작용 프롬프트
{
    public string Text;
    public bool Visible;
}
