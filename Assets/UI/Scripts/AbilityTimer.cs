using UnityEngine;

public class AbilityTimer : MonoBehaviour
{
    public delegate void AbilityTimerEvent(float cooldown);
    public AbilityTimerEvent OnTimerStarted;
    public AbilityTimerEvent OnTimerTimeout;

    private float cooldown = 0f;

    public bool IsStarted { get; private set; }
    public float TimePassed { get; private set; }

    private void Update()
    {
        if (!IsStarted)
            return;

        TimePassed += Time.deltaTime;

        if (TimePassed >= cooldown)
            Timeout();
    }

    public void StartTimer(float cooldown)
    {
        this.cooldown = cooldown;
        TimePassed = 0;

        IsStarted = true;
        OnTimerStarted?.Invoke(cooldown);
    }

    private void Timeout()
    {
        IsStarted = false;
        OnTimerTimeout?.Invoke(cooldown);
    }
}
