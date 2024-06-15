using UnityEngine;
using UnityEngine.UI;

public class AttackIndicator : MonoBehaviour
{
    [HideInInspector] public Player player;

    [SerializeField] private Image background;
    [SerializeField] private Slider slider;
    
    private float timePassed;

    public bool IsStarted { get; private set; }

    public void ResetTimer()
    {
        player.PlayerAttack.AddAttack();

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
        IsStarted = newState;

        background.enabled = !newState;

        timePassed = 0;
        slider.value = 0;

        slider.enabled = newState;
    }
}
