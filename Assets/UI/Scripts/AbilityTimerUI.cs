using UnityEngine;
using UnityEngine.UI;

public class AbilityTimerUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AbilityTimer abilityTimer;

    [Header("Components")]
    [SerializeField] private Image background;
    [SerializeField] private Slider slider;

    private void Awake()
    {
        background.enabled = true;
        slider.enabled = false;
    }

    private void Start()
    {
        abilityTimer.OnTimerStarted += AbilityTimer_OnTimerStarted;
        abilityTimer.OnTimerTimeout += AbilityTimer_OnTimerTimeout;
    }

    private void Update()
    {
        if (!abilityTimer.IsStarted)
            return;

        slider.value = abilityTimer.TimePassed;
    }

    private void AbilityTimer_OnTimerStarted(float cooldown)
    {
        slider.maxValue = cooldown;
        slider.value = 0;
        background.enabled = false;
        slider.enabled = true;
    }

    private void AbilityTimer_OnTimerTimeout(float _)
    {
        background.enabled = true;
        slider.enabled = false;
    }
}
