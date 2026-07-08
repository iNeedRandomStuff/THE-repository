using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using TMPro;

public class Pistol : NetworkBehaviour
{
    [SerializeField] private Transform gunBarrel;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip gunshotClip;

    [Header("ammo")]
    [SerializeField] private float magAmount;
    [SerializeField] private float magCapacity;
    public float currentAmmoInMag;
    [SerializeField] private TextMeshPro currentAmmoInMagDisplay3D;
    //[SerializeField] private TextMeshProUGUI currentAmmoInMagDisplayUI;

    [Header("Muzzle Flash")]
    [SerializeField] private Light muzzleflash;
    [SerializeField] private float targetValue;
    [SerializeField] private float flashDuration;
    [SerializeField] private GameObject Impact;

    private RaycastHit hit;
    private Vector3 hitPoint;
    private Vector3 hitNormal;

    [Header("lazer")]
    public float distance;
    [SerializeField] private LineRenderer lr;

    [Header("recoil and sway")]
    public GameObject Hand;
    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float smoothnes;
    [SerializeField] private float swayRange;
    [SerializeField] private float swayWait;
    [SerializeField] private GameObject XROrigin;
    [SerializeField] private float timeToCenter;
    private Vector3 swayVector3;
    private PcVrCompatability compatabilityScript;
    private HandFollow handFollowScript;
    //private Quaternion prevRecoilValue = Quaternion.Euler(1, 1, 1);
    private Quaternion topRecoilTransform;
    
    private float additionalrecoilX;

    private float additionalrecoilY;

    private int timesinceshot = 0;

    private float randomnessFactorX;
    private float randomnessFactorY;

    private int timesshot = 1;

    void Start()
    {
        currentAmmoInMag = magCapacity;
        compatabilityScript = XROrigin.GetComponent<PcVrCompatability>();
        handFollowScript = Hand.GetComponent<HandFollow>();
        InvokeRepeating(nameof(randomSwayPerSec), 0f , swayWait);
        handFollowScript.timeToReset = timeToCenter;
    }

    private void FixedUpdate()
    {
        handFollowScript.time = handFollowScript.time + 1; 
        if (handFollowScript.time > 30)
        {
            timesshot = 0;
            handFollowScript.offset = Quaternion.Euler(0,0,0);
        }
    }


    public void Function()
    {
        Ray ray = new Ray(gunBarrel.position, gunBarrel.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && currentAmmoInMag >= 1f)
        {
            hitPoint = hit.point;
            hitNormal = hit.normal;
            if (hit.transform.tag == "monster")
            {
                MonsterHealth monsterHealth = hit.transform.GetComponent<MonsterHealth>();
                monsterHealth.Health();
            }
            else
            {

            }


            currentAmmoInMag -= 1f;
            string _ammoInMag = currentAmmoInMag.ToString();
            currentAmmoInMagDisplay3D.text = _ammoInMag;
            PlayGunshotServerRpc(Impact, hitPoint, hitNormal);
            Recoil(Hand);
            handFollowScript.time = 0;
        }
    }

    void randomSwayPerSec()
    {
        float randomnessFactor = Random.Range(-swayRange, swayRange);
        swayVector3 = new Vector3(randomnessFactor, randomnessFactor, 0f);
        compatabilityScript.sway = swayVector3;
    }

    void Recoil(GameObject _Hand)
    {
        if (handFollowScript.time <= 60)
        {
            randomnessFactorX = Random.Range(0.8f, 1f);
            randomnessFactorY = Random.Range(0.8f, 1f);
            topRecoilTransform = Quaternion.Euler(recoilX * -randomnessFactorX * timesshot, recoilY  +randomnessFactorY * (2 * timesshot), 0f);
            handFollowScript.offset = topRecoilTransform;
            handFollowScript.time = 0f;
            timesshot = timesshot + 1;
            if (timesshot > 3)
            {
                timesshot = 3;
            }
        }
        else
        {
            randomnessFactorX = Random.Range(0.1f, 1f);
            randomnessFactorY = Random.Range(0.1f, 1f);
            topRecoilTransform = Quaternion.Euler(recoilX * -randomnessFactorX, recoilY  +randomnessFactorY, 0f);
            handFollowScript.offset = topRecoilTransform;
            handFollowScript.time = 0f;
            timesshot = 1;
        }
    }

    void Update()
    {
        Ray ray = new Ray(gunBarrel.position, gunBarrel.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            lr.SetPosition(0, ray.origin);
            lr.SetPosition(1, hit.point);
        }
        else
        {
            lr.SetPosition(0, ray.origin);
            lr.SetPosition(1, ray.origin + ray.direction * 100f);
        }
    }

    [ServerRpc]
    public void PlayGunshotServerRpc(GameObject _impact, Vector3 _hitPoint, Vector3 _hitNormal)
    {
        PlayGunshotObserversRpc();
        playMuzzleflashObserver();
        playImpactObserver(_impact, _hitPoint, _hitNormal);
    }

    [ObserversRpc(BufferLast = false)]
    private void PlayGunshotObserversRpc()
    {
        audioSource.PlayOneShot(gunshotClip);
    }

    [ObserversRpc(BufferLast = false)]
    private void playMuzzleflashObserver()
    {
        StartCoroutine(MuzzleFlashRoutine());
    }

    private IEnumerator MuzzleFlashRoutine()
    {
        muzzleflash.intensity = targetValue;

        yield return new WaitForSeconds(flashDuration);

        muzzleflash.intensity = 0f;
    }

    [ObserversRpc(BufferLast = false)]
    private void playImpactObserver(GameObject _impact, Vector3 _hitPoint, Vector3 _hitNormal)
    {
        GameObject _spawned = Instantiate(_impact, _hitPoint, Quaternion.LookRotation(_hitNormal));
        _spawned.transform.parent = hit.transform;
        ServerManager.Spawn(_spawned);
    }
}