using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private Image health_stats, stamina_stats;

    public void Display_HealthStats(float health) {
        health_stats.fillAmount = health/100f;
    }
    public void Display_StaminaStats(float stamina) {
        stamina_stats.fillAmount = stamina/100f;
    }
}
