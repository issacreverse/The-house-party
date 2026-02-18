using UnityEngine;
using System.Collections.Generic;

public class POIManager : MonoBehaviour
{
    public static POIManager Instance {get; private set;}

    [SerializeField] private Transform POIs;

    public static List<POI> list = new List<POI>();

    [SerializeField] private int selectAmount = 2; //표본 몇 개 뽑을 건지
    public bool samePOIEnabled = true;  //같은 곳 또 배정받을 수 있는지 
    [SerializeField] private float crowdPenalty = 0.3f;  //사람 한 명 붐빌 때마다 패널티 
    [SerializeField] private float thresholdScore = 0.9f; //임계값. 못 넘으면 해당 자리에 계속 머문다 

    void Awake()
    {
        if(Instance == null)
            Instance = this;

        CollectPOIs();

        Screen.fullScreenMode = FullScreenMode.Windowed;
        Screen.fullScreen = false;
    }
    
    void CollectPOIs()
    {
        list.Clear();

        for(int i=0; i < POIs.childCount; i++)
        {
            list.Add(POIs.GetChild(i).gameObject.GetComponent<POI>());
        }
    }
    public POI FindPOIWithId(int id)
    {
        POI poi = list.Find(n => n.data.id == id);
        return poi;
    }

    //뽑은 POI의 점수가 임계값 미달이거나 
    //뽑은 POI의 슬롯이 다 찼을 경우 
    //Vector3.zero를 반환한다. 
    //슬롯이 다 찼을 경우 차선 POI를 찾아서 슬롯을 배정해야되지만 
    //귀찮다 
    public Vector3 GetNextDestination(NPC npc)
    {
        List<POI> candidateList = new List<POI>();

        if(!samePOIEnabled)
        {
            POI currentPOI = FindPOIWithId(npc.currentPOIId);
            candidateList.Add(currentPOI);
        }
        while(candidateList.Count < selectAmount + 1)
        {
            POI select = list[Random.Range(0, list.Count)];
            
            if(!candidateList.Contains(select))
            {
                candidateList.Add(select);
            }
        }
        Debug.Log("CandidateList made: " + candidateList.Count);

        float maxScore = 0f;
        POI maxScorePOI = candidateList[0];
        foreach(POI p in candidateList)
        {
            Debug.Log("maxScore: " + maxScore);
            Debug.Log("maxScorePOI:" + maxScorePOI.data.id);

            float score = CalculateScore(p, npc.data.preference);
            Debug.Log(p.data.id + "'s score: " + score);
            if(score >= maxScore)
            {
                maxScore = score;
                maxScorePOI = p;
            }
        }

        if(maxScore >= thresholdScore)
        {
            Debug.Log("maxScore >= thresholdScore");
            
            
            //slot 결정해주기
            Vector3 slot = maxScorePOI.AssignSlot(npc);

            if(slot != null)
            {
                npc.currentPOIId = maxScorePOI.data.id;
                npc.givenDwellTime = Random.Range(maxScorePOI.data.minDwell, maxScorePOI.data.maxDwell);
                Debug.Log("DwellTime is: "+ npc.givenDwellTime);
                return slot;
            }
                
            else
                return Vector3.zero;
        }
        else
        {
            Debug.Log("maxScore <= thresholdScore");
            return Vector3.zero;
        }

        //npc의 현재 스팟을 제외한 2개의 스팟을 고른다
        //점수를 계산한다
        //해당 점수가 고정임계값을 넘겼을 경우, 해당 POI의 transform을 NPC에게 넘겨준다(반환 값으로)
        //추가적으로 npc의 currentspotID를 넘겨주는 POI의 ID로 바꾼다. 
        //NPC는 해당 함수의 반환값을 다음 setDestination 값으로 지정하면 끝
    }
    float CalculateScore(POI p, POIType preference)
    {
        float score = p.data.baseWeight
                    * (p.data.type == preference ? 1.2f : 1.0f)
                    * (1.0f - (crowdPenalty * p.GetCrowdCount()))
                    * Random.Range(0.8f, 1.2f);
                    //distancePenalty

        return score;
    }
}
