using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour
{
    public bool isTalkable;
    public bool moveEnabled;
    public bool isDoneTalking;
    public bool isMoving;
    public bool navigationPaused;

    [SerializeField] private float duration = 1.2f;

    [SerializeField] private NPC_Data data;
    public int currentPOIId;
    public float givenDwellTime = 2f;
    public float currentTime = 0f;
    public bool isTimeRunning = true;

    void Start()
    {
        isTalkable = false;
        moveEnabled = true;
        isDoneTalking = true;
        isMoving = false;
        navigationPaused = false;
    }

    void Update()
    {
        //if(moveEnabled)
        //  navigation... and movement...

        if(isTimeRunning)
        {
            currentTime += Time.deltaTime;
        }

        if(moveEnabled && currentTime >= givenDwellTime && !isMoving)
        {
            //navigation시도
            //성공하면 
            //isMoving = true;
            //isTimeRunning = false;
            //도착하면 
            //isMoving false;
            //isTimeRunning = true;
            currentTime = 0f;
        }

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
        //moveEnabled = false;
        //isMoving이였다면 navagation 중지, navigationPaused = true;

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
            yield return null;
        }

        transform.rotation = endRot;

        //Actual Conversation
        Debug.Log("BlahBlahBlah");
        yield return new WaitForSeconds(3f);
        isDoneTalking = true;
    }
    public IEnumerator NPCAfterConversation()
    {
        //원래대로 회전
        //navigationPaused = true라면 재개하고 false로 바꾼다.
        Debug.Log("Walked Away");
        yield return null;
    }
}
