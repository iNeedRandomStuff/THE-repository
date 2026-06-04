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
    private Vector3 swayVector3;
    private PcVrCompatability compatabilityScript;

    void Start()
    {
        currentAmmoInMag = magCapacity;
        compatabilityScript = XROrigin.GetComponent<PcVrCompatability>();
        InvokeRepeating(nameof(randomSwayPerSec), 0f , swayWait);
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
            //currentAmmoInMagDisplayUI.text = _ammoInMag;
            PlayGunshotServerRpc(Impact, hitPoint, hitNormal);
            Recoil(Hand);
        }
    }

    void randomSwayPerSec()
    {
        //yield return new WaitForSeconds(swayWait);
        float randomnessFactor = Random.Range(-swayRange, swayRange);
        swayVector3 = new Vector3(randomnessFactor, randomnessFactor, 0f);
        Debug.Log(swayVector3);
        compatabilityScript.sway = swayVector3;
    }

    void Recoil(GameObject _Hand)
    {
        float randomnessFactor = Random.Range(0.5f, 1f);
        Quaternion currentRotation = Hand.transform.localRotation;
        Quaternion topRecoilTransform = Quaternion.Euler(recoilX * -randomnessFactor, recoilY * randomnessFactor, 0f);
        _Hand.transform.localRotation = Quaternion.Slerp(currentRotation, topRecoilTransform, smoothnes * Time.deltaTime) * currentRotation;
    }

    void Update()
    {
        //StartCoroutine(randomSwayPerSec());
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