using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FishNet.Object;

public class PcDebugPanel : NetworkBehaviour
{
    [SerializeField] private GameObject Panel;

    [Header("looked at object")]
    [SerializeField] private Camera camera;
    [SerializeField] private TextMeshProUGUI lookedAtObjectText;

    [Header("monsterHealth")]
    [SerializeField] private MonsterHealth monsterHealth;
    [SerializeField] private TextMeshProUGUI monsterHealthText;

    [Header("CanKill")]
    [SerializeField] private PCabilities pcAbilities;
    [SerializeField] private TextMeshProUGUI canKillText;

    private bool panelOpened;


    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner)
        {
            gameObject.GetComponent<PcDebugPanel>().enabled = false;
        }
    }

    void Update()
    {
        CheckIfPanelOpened();
        if (panelOpened == true)
        {
            LookedAtObject();
            HP();
            CanKill();
        }
    }

    void CheckIfPanelOpened()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            Panel.SetActive(true);
            panelOpened = true;
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            Panel.SetActive(false);
            panelOpened = false;
        }
    }

    void LookedAtObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit))
        {
            lookedAtObjectText.text = hit.transform.name;
        }
    }

    void HP()
    {
        string _monsterHealthString = monsterHealth.health.Value.ToString();
        monsterHealthText.text = _monsterHealthString;
    }

    void CanKill()
    {
        string _canKillString = pcAbilities.canKill.ToString();
        canKillText.text = _canKillString;
    }
}
