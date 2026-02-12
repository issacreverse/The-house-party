using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] Canvas zoomCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        zoomCanvas.gameObject.SetActive(false);
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
}
