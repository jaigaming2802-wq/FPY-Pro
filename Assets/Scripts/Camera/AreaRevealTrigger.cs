using UnityEngine;

public class AreaRevealTrigger : MonoBehaviour
{
    public enum TriggerType
    {
        Entry,
        Exit
    }

    [SerializeField] private TriggerType triggerType;
    [SerializeField] private AreaRevealManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (triggerType == TriggerType.Entry)
        {
            manager.EnterArea();
        }
        else
        {
            manager.CompleteArea();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (triggerType == TriggerType.Entry)
        {
            manager.CancelArea();
        }
    }
}