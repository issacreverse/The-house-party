using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour
{
    public bool isTalkable;
    public bool moveEnabled;
    public bool isDoneTalking;

    [SerializeField] private float duration = 0.3f;

    void Start()
    {
        isTalkable = false;
        moveEnabled = true;
        isDoneTalking = true;
    }

    void Update()
    {
        //if(moveEnabled)
        //  navigation... and movement...
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            isTalkable = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            isTalkable = false;
        }
    }
    public IEnumerator StartTalking(Transform target)
    {
        //Look At Player
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.0001f) yield break;

        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.LookRotation(dir);

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);
        }

        transform.rotation = endRot;

        //Actual Conversation
        Debug.Log("BlahBlahBlah");
        yield return new WaitForSeconds(3f);
        isDoneTalking = true;
    }
    public IEnumerator NPCAfterConversation()
    {
        Debug.Log("Walked Away");
        yield return null;
    }
}
