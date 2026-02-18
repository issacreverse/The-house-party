using UnityEngine;

public class POI : MonoBehaviour
{
    [SerializeField] public POI_Data data;
    [SerializeField] private Transform[] slots;
    
    public bool[] slotState;

    void Awake()
    {
        slotState = new bool[slots.Length];
        if(data.capacity > slots.Length)
            Debug.Log("Capacity is bigger than slot length");
    }

    //슬롯을 배정한다. 
    //실패하면 Vector3.zero를 반환한다.
    public Vector3 AssignSlot(NPC npc)
    {
        if(data.capacity <= GetCrowdCount())
            return Vector3.zero;
        for(int i=0; i<slotState.Length; i++)
        {
            if(slotState[i] == false)
            {
                slotState[i] = true;
                npc.currentPOISlot = i;
                Debug.Log(data.id + " assigned and slot is " + i);
                return slots[i].position; 
            }
        }
        return Vector3.zero;  
    }
    public int GetCrowdCount()
    {
        int count = 0;
        for(int i=0; i<slotState.Length; i++)
        {
            if(slotState[i] == true)
            {
                count++;
            }
        }
        return count;
    }
    public void FreeSlot(int n)
    {
        slotState[n] = false;
        Debug.Log("Free slot " + n);
    }
}
