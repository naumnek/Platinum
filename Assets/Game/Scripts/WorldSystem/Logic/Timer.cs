using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private int min = 0;
    public float seconds = 0f;
    public int minutes = 0;
    public int hours = 0;
    public int days = 0;
    public int weeks = 0;
    public int months = 0;
    public int yaers = 0;
    // Start is called before the first frame update

    private void AllTime()
    {
        minutes = min;
        hours = minutes / 60;
        days = hours / 24;
        weeks = days / 7;
        months = days / 30;
        yaers = months / 12;
    }
    // Update is called once per frame
    void Update()
    {
        seconds += Time.deltaTime; /* Вычитаем из 10 время кадра (оно в миллисекундах) */
        if (seconds >= 60f) /* Время вышло пишем */
        {
            seconds = 0f; /* запускает опять таймер на 60,чтобы повторялось бесконечно */
            min += 1;
            if(min >= 60)
            {
                AllTime();
            }
        }
    }
}
