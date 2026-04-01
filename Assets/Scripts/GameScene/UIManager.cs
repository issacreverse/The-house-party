using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIManager : MonoBehaviour
{
    [SerializeField] Canvas zoomCanvas;
    [SerializeField] Canvas dialogCanvas;
    //loading
    [SerializeField] Canvas loadingCanvas;
    [SerializeField] VideoPlayer videoPlayer;

    [SerializeField] Canvas userCanvas;

    [SerializeField] Text dialogText;
    [SerializeField] Text tagsLeftText;

    public static UIManager Instance;
    void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        zoomCanvas.gameObject.SetActive(false);

        GameManager.Instance.OnTagsLeftChanged += UI_UpdateTagsLeft;
        GameManager.Instance.InitialUIUpdate();
    }
    void OnDestory()
    {
        GameManager.Instance.OnTagsLeftChanged -= UI_UpdateTagsLeft;
    } 

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UI_ZoomScopeEnter()
    {
        zoomCanvas.gameObject.SetActive(true);
    }
    public void UI_ZoomScopeExit()
    {
        zoomCanvas.gameObject.SetActive(false);        
    }
    public void UI_dialogEnter(string dialog)
    {
        dialogCanvas.gameObject.SetActive(true);
        dialogText.text = "???: " + dialog;
    }
    public void UI_dialogExit()
    {
        dialogCanvas.gameObject.SetActive(false);
    }
    public void UI_UpdateTagsLeft()
    {
        tagsLeftText.text = "Tags Left: "+ GameManager.Instance.tagsLeft;
    }
    public void UI_ShowLoadingScreen()
    {
        loadingCanvas.gameObject.SetActive(true);
        videoPlayer.Play();
    }
    public void UI_HideLoadingScreen()
    {
        loadingCanvas.gameObject.SetActive(false);
        videoPlayer.Stop();
    }
    public void UI_ShowUserCanvas()
    {
        userCanvas.gameObject.SetActive(true);
    }
}
