using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isPlayerInRange = false;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
