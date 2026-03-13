using UnityEngine;


public enum POIType
{
    loud,
    quiet,
    social,
    snack,
    none
}
[CreateAssetMenu(menuName = "POI data", fileName = "POI_")]
public class POI_Data : ScriptableObject 
{
    [Min(0)] public int id;

    [Min(0f)] public float baseWeight = 1f;

    [Min(0f)] public float minDwell = 2f;
    [Min(0f)] public float maxDwell = 10f;

    [Min(1)] public int capacity = 1;

    public POIType type;

    private void OnValidate()
    {
        if(maxDwell < minDwell) maxDwell = minDwell;
        if(capacity < 1) capacity = 1;
        if(baseWeight < 0f) baseWeight = 1f;
        if(id < 1) id = 1;
    }

    public float GetRandomDwell()
    {
        return Random.Range(minDwell, maxDwell);
    }
}
