using UnityEngine;
using UnityEngine.UI;

public class DashIndicator : MonoBehaviour
{
    [HideInInspector] public Player player;

    [SerializeField] private Image background;
    [SerializeField] private Slider slider;

    float timePassed;

    bool isStarted = false;
    public bool IsStarted => isStarted;

    public void ResetTimer()
    {
        player.PlayerDash.AddDash();

        SetSliderState(false);
    }

    public void StartTimer(float cooldown)
    {
        SetSliderState(true);

        slider.maxValue = cooldown;
    }

    private void Start()
    {
        SetSliderState(false);
    }

    private void Update()
    {
        if (slider.enabled)
        {
            timePassed += Time.deltaTime;
            slider.value = timePassed;

            if (slider.value >= slider.maxValue)
                ResetTimer();
        }
    }

    private void SetSliderState(bool newState)
    {
        isStarted = newState;

        background.enabled = !newState;

        timePassed = 0;
        slider.value = 0;

        slider.enabled = newState;
    }
}