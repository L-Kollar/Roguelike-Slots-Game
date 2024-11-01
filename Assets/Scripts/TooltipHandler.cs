using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject tooltipPrefab;

    private string title = "";
    private string text = "";

    private GameObject tooltipBuffer;
    private GameObject canvas;
    private void Start()
    {
        canvas = GameObject.Find("ShopContainer");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipBuffer = Instantiate(tooltipPrefab, canvas.transform);

        tooltipBuffer.transform.position = transform.position;

        tooltipBuffer.transform.GetChild(0).GetComponent<TMP_Text>().text = title;
        tooltipBuffer.transform.GetChild(1).GetComponent<TMP_Text>().text = text;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(tooltipBuffer);
    }

    public void SetText(string title, string text)
    {
        this.title = title;
        this.text = text;
    }
}
