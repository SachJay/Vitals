using UnityEngine;
using UnityEngine.UI;

public class AttackIndicator : MonoBehaviour
{
    [HideInInspector]
    public Player player;

    [SerializeField]
    Image background;

    [SerializeField]
    Slider slider;

    float timePassed;

    bool isStarted = false;
    public bool IsStarted => isStarted;

    private void Start()
    {
        SetSlider(false);
    }

    private void Update()
    {
        if (slider.enabled)
        {
            timePassed += Time.deltaTime;
            slider.value = timePassed;

            if (slider.value >= slider.maxValue)
            {
                player.AddAttack();

                SetSlider(false);
            }
        }
    }

    public void StartTimer(float cooldown)
    {
        SetSlider(true);

        slider.maxValue = cooldown;
    }

    private void SetSlider(bool newState)
    {
        isStarted = newState;

        background.enabled = !newState;

        timePassed = 0;
        slider.value = 0;

        slider.enabled = newState;
    }
}
