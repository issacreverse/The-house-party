using UnityEngine;
using System.Collections.Generic;

public class POIManager : MonoBehaviour
{
    public static POIManager Instance {get; private set;}

    [SerializeField] private Transform POIs;

    public static List<POI> list = new List<POI>();

    void Awake()
    {
        if(Instance == null)
            Instance = this;

        CollectPOIs();
    }
    
    void CollectPOIs()
    {
        list.Clear();

        for(int i=0; i < POIs.childCount; i++)
        {
            list.Add(POIs.GetChild(i).gameObject.GetComponent<POI>());
        }
    }

    public Vector3 GetNextDestination(NPC npc)
    {
        //npc의 현재 스팟을 제외한 2개의 스팟을 고른다
        //점수를 계산한다
        //해당 점수가 고정임계값을 넘겼을 경우, 해당 POI의 transform을 NPC에게 넘겨준다(반환 값으로)
        //추가적으로 npc의 currentspotID를 넘겨주는 POI의 ID로 바꾼다. 
        //NPC는 해당 함수의 반환값을 다음 setDestination 값으로 지정하면 끝
        return Vector3.zero;
    }
}
