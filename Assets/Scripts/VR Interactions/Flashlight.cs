using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class Flashlight : NetworkBehaviour
{
    [SerializeField] private Light flashlight;
    [SerializeField] private float targetValue;

    public AudioSource audioSource;
    public AudioClip flashlightClip;

    private readonly SyncVar<float> lightIntensety = new SyncVar<float>();
    private readonly SyncVar<bool> setOn = new SyncVar<bool>(); 

    public void Function()
    {
        FlashlighServer(targetValue);
    }

    [ServerRpc(RequireOwnership = false)]
    public void FlashlighServer(float _targetValue)
    {
        setOn.Value = !setOn.Value;
        PlaySoundObserversRpc();
        if (setOn.Value == true)
        {
            lightIntensety.Value = _targetValue;
        }
        else
        {
            lightIntensety.Value = 0;
        }
    }

    [ObserversRpc(BufferLast = false)]
    private void PlaySoundObserversRpc()
    {
        audioSource.PlayOneShot(flashlightClip);
    }

    void Update()
    {
        flashlight.intensity = lightIntensety.Value;
    }
}
