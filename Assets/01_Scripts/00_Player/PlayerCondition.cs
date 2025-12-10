using System;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    [SerializeField] private float stress;
    [SerializeField] private float fatigue;
    private float maxValue = 100;

    public float Stress => stress;
    public float Fatigue => fatigue;

    //private UIManager uiManager;

    private void Start()
    {
        //uiManager = UIManager.Instance;
        
        // 시작 시 UI값 초기 반영
        //uiManager.UpdateStressUI(stress, maxValue);
        //uiManager.UpdateFatigueUI(fatigue, maxValue);

        EventBus.Publish(new StressChangedEvent { Value = stress, Max = maxValue });
        EventBus.Publish(new FatigueChangedEvent { Value = fatigue, Max = maxValue });
    }

    //private void Update()
    //{
    //    // 디버깅용
    //    if (Input.GetKeyDown(KeyCode.Alpha1))
    //        AddStress(10);
    //    else if (Input.GetKeyDown(KeyCode.Alpha2))
    //        AddFatigue(30);
    //}

    public void AddStress(float value)
    {
        stress = Mathf.Clamp(stress + value, 0, 100);

        EventBus.Publish(new StressChangedEvent
        {
            Value = stress,
            Max = maxValue
        });

    }

    public void AddFatigue(float value)
    {
        fatigue = Mathf.Clamp(fatigue + value, 0, 100);
        EventBus.Publish(new FatigueChangedEvent
        {
            Value = fatigue,
            Max = maxValue
        });
    }
}
