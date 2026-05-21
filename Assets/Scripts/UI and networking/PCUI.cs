using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PCUI : MonoBehaviour
{
    [Header("Circle inside crosshair")]
    [SerializeField] private GameObject redCircle;
    [SerializeField] private Image circleBorder;

    [Header("crosshair border")]
    [SerializeField] private PCabilities pcAbilitiesScript;
    [SerializeField] private Color canKillColor;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color stalkingColor;

    [Header("vignette")]
    [SerializeField] private MonsterHealth monsterHealthScirpt;
    [SerializeField] private Volume post;
    private Vignette vignette;

    void Start()
    {
        post.profile.TryGet(out vignette);
    }

    void Update()
    {
        centeralCircleScale();
        crosshairBorderTimer();
        HealthToVignette();
    }

    void centeralCircleScale()
    {
        float _stalkTimer = pcAbilitiesScript.currentStalkTimerState / pcAbilitiesScript.stalker * 0.15f;
        redCircle.transform.localScale = new Vector3(_stalkTimer, _stalkTimer, _stalkTimer);
    }

    void crosshairBorderTimer()
    {
        if (pcAbilitiesScript.canKill == true)
        {
            circleBorder.color = canKillColor;
        }
        else
        {
            circleBorder.color = defaultColor;
        }

        if (pcAbilitiesScript.lookingAtPlayer == true && pcAbilitiesScript.canKill == false)
        {
            circleBorder.color = stalkingColor;
        }
        else
        {
            if(pcAbilitiesScript.canKill == false)
                circleBorder.color = defaultColor;
        }
    }

    void HealthToVignette()
    {
        vignette.intensity.value = (monsterHealthScirpt.health.Value - 100f) * -1f / 100f;
    }
}
