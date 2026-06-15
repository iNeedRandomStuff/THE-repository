using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class PCabilities : NetworkBehaviour
{
    //for raycasting
    [SerializeField] private Camera camera;
    [SerializeField] private float range;

    [Header("Audio")]
    public AudioSource source;
    public AudioClip Crack;

    // cosmonauts body parts
    private ScaleAndAllignment scaleAndAllignment;
    private GameObject rightArm;
    private GameObject leftArm;
    private GameObject head;

    [Header("abilityWaits")]
    [SerializeField] private float killCooldown;
    public float stalker;

    //abilityWaits private variables
    [HideInInspector]
    public bool canKill = false;
    public float currentStalkTimerState;

    [SerializeField] private MonsterHealth monsterHealth;

    public bool lookingAtPlayer;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner)
        {
            gameObject.GetComponent<PCabilities>().enabled = false;
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            KillCosmonaut();
        }
    }

    void Start()
    {
        InvokeRepeating(nameof(stalkTimer), 0f, 0.1f);
    }

    void KillCosmonaut()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.tag == "Player" && canKill == true)
            {
                scaleAndAllignment = hit.transform.gameObject.GetComponent<ScaleAndAllignment>();
                if(leftArm != null && rightArm != null && head != null)
                {
                    leftArm = scaleAndAllignment.LeftArm;
                    rightArm = scaleAndAllignment.RightArm;
                    head = scaleAndAllignment.Head;
                }
                canKill = false;
                //KillCosmonautServer(hit.transform.gameObject, leftArm, rightArm, head);
                KillCosmonautServer(hit.transform.gameObject);
                monsterHealth.ServerHealOnKill();
            }
        }
    }

    [ServerRpc]
    //void KillCosmonautServer(GameObject _cosmonaut, GameObject _leftArm, GameObject _rightArm, GameObject _Head)
    void KillCosmonautServer(GameObject _cosmonaut)
    {
        /*
        NetworkObject.Despawn(_leftArm);
        NetworkObject.Despawn(_rightArm);
        NetworkObject.Despawn(_Head);
        NetworkObject.Despawn(_cosmonaut);
        */
        source.PlayOneShot(Crack);
        currentStalkTimerState = 0;
        canKill = false;

        KillCosmonautObserver(_cosmonaut);

        //killCooldownFunction();
    }

    [ObserversRpc]
    void KillCosmonautObserver(GameObject _cosmonaut)
    {
        Animator animator = _cosmonaut.GetComponent<Animator>();
        animator.enabled = false;
        _cosmonaut.transform.SetParent(gameObject.transform);
        _cosmonaut.transform.position = gameObject.transform.position;
    }

    IEnumerator killCooldownFunction()
    {
        yield return new WaitForSeconds(killCooldown);
        canKill = true;
        // change this to work if waiting to kill doesnt go well
    }

    void stalkTimer()
    {
        print("Current currentStalkTimerState = " + currentStalkTimerState);
        if (currentStalkTimerState < stalker)
        {
            RaycastHit hit;
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit))
            {
                if(hit.transform.tag == "Player")
                {
                    currentStalkTimerState += 1f;
                    print(currentStalkTimerState);
                    lookingAtPlayer = true;
                }
                else
                {
                    lookingAtPlayer = false;
                }
            }
        }

        if(currentStalkTimerState >= stalker)
        {
            print("can kill");
            canKill = true;
        }
    }
}