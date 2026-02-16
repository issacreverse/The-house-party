using UnityEngine;

public enum NPCType
{
    human,
    monster
}

[CreateAssetMenu(menuName = "NPC data", fileName = "NPC_")]
public class NPC_Data : ScriptableObject 
{
    public POIType preference;

    public NPCType npcType;

    //dialog...
}
