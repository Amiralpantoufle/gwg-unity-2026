using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Base_QueueDisplayer : MonoBehaviour
{
    [SerializeField] private Image fillerGauge;
    [SerializeField] private TextMeshProUGUI txt_QAmount;
    [SerializeField] private TextMeshProUGUI txt_QTime;

    private Coroutine timerCoroutine;
    private int prodAmount;
    private float prodTime;

    public void Start_NewQueue(int amount, float time)
    {
        gameObject.SetActive(true);

        prodAmount = amount;
        prodTime = time;
        txt_QAmount.text = amount.ToString();
        txt_QTime.text = GetBuildTime(time);

        //Gauge
        fillerGauge.fillAmount = 0;
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        timerCoroutine = StartCoroutine(ProductionTimer(time));

    }

    private void End_ActiveQueue()
    {
        prodAmount--;

        if(prodAmount > 0)
        {
            Start_NewQueue(prodAmount, prodTime);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private string GetBuildTime(float seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(Mathf.Max(0, seconds));

        if (time.TotalDays >= 1)
            return $"{time.Days}j{time.Hours:00}h";

        if (time.TotalHours >= 1)
            return $"{time.Hours}h{time.Minutes:00}";

        if (time.TotalMinutes >= 1)
            return $"{time.Minutes:00}m{time.Seconds:00}s";

        return $"{time.Seconds:00}s";
    }
    private IEnumerator ProductionTimer(float totalTime)
    {
        float remainingTime = totalTime;

        while (remainingTime > 0f)
        {
            // Pourcentage restant : 1 -> 0
            fillerGauge.fillAmount = 1f - (remainingTime / totalTime);

            txt_QTime.text = GetBuildTime(remainingTime);

            yield return null;

            remainingTime -= Time.deltaTime;
        }

        // On force les valeurs finales
        fillerGauge.fillAmount = 1f;
        txt_QTime.text = "0m";

        End_ActiveQueue();

        timerCoroutine = null;
    }
}
