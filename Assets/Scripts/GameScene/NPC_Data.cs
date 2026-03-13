using UnityEngine;
using System;


public enum NPCType
{
    human,
    monster
}
public enum DialogCondition
{
    none,
}

[Serializable]
public class Dialog
{
    public string text = "New Text";
    public float weight = 1f;
    public DialogCondition condition = DialogCondition.none;
}

[CreateAssetMenu(menuName = "NPC data", fileName = "NPC_")]
public class NPC_Data : ScriptableObject 
{
    public POIType preference;

    public NPCType npcType;

    public string npcName;
    
    public Dialog[] dialogs = new Dialog[10];

}
