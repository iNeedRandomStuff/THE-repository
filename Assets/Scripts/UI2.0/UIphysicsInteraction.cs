using UnityEngine.UI;
using UnityEngine;

public class UIphysicsInteraction : MonoBehaviour
{
    [SerializeField] private Button targetButton;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("IndexFinger") && targetButton != null && targetButton.interactable)
        {
            targetButton.onClick.Invoke();
        }
    }
}