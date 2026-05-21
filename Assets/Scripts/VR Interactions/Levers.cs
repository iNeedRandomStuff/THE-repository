using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class Levers : NetworkBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip electricalSpark;

    [SerializeField] private GameObject otherLever;
    [SerializeField] private int waitingTime;
    [SerializeField] private Animator animator;

    public Color green;
    public Color yellow;
    public Color red;

    private bool leverActiveLocal;
    private Levers otherLeverScript;
    private readonly SyncVar<bool> leverActive = new SyncVar<bool>();
    public readonly SyncVar<bool> BothleversPulled = new SyncVar<bool>();

    public bool X;

    private Coroutine routine;

    private void Start()
    {
        otherLeverScript = otherLever.GetComponent<Levers>();
    }

    public void Function()
    {
        ActivateServerRpc(green, yellow, red);
    }

    private IEnumerator LeverRoutine()
    {
        leverActive.Value = true;

        yield return new WaitForSeconds(waitingTime);

        if (otherLeverScript.leverActive.Value == true)
        {
            Debug.Log("Levers activated!");
        }
        else
        {
            leverActive.Value = false;
            ChangeColorObserver(red);
            animator.SetBool("leverUpOrDown", false);
            print("levers deactivated");
        }
        routine = null;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ActivateServerRpc(Color green, Color yellow, Color red)
    {
        if (routine != null)
            return;

        routine = StartCoroutine(LeverRoutine());

        ChangeColorObserver(yellow);
        animator.SetBool("leverUpOrDown", true);

        if (leverActive.Value == true && otherLeverScript.leverActive.Value == true)
        {
            ChangeColorObserver(green);
            BothleversPulled.Value = true;
            otherLeverScript.BothleversPulled.Value = true;
        }
    }

    void Update()
    {
        if(X)
            BothleversPulled.Value = true;
    }

    [ObserversRpc]
    public void ChangeColorObserver(Color color)
    {
        gameObject.GetComponent<Renderer>().material.color = color;
        otherLever.GetComponent<Renderer>().material.color = color;
        audioSource.PlayOneShot(electricalSpark);
    }
}