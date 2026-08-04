using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsPanelDrag : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] private RectTransform optionsPanel;
    [SerializeField] private Canvas canvas;

    private Vector2 pointerOffset;

    //Grabs the header with the mouse and calculates pointerOffset
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(optionsPanel == null || canvas == null)
            return;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(optionsPanel, eventData.position, eventData.pressEventCamera, out pointerOffset);
    }

    //Once the header is grabbed, moves the Panel by changing optionsPanel.anchoredPosition, calculating the difference between localPointerPos and pointerOffset
    public void OnDrag(PointerEventData eventData)
    {
        if(optionsPanel == null || canvas == null)
            return;

        RectTransform canvasRect = canvas.transform as RectTransform;

        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, eventData.pressEventCamera, out var localPointerPos))
        {
            optionsPanel.anchoredPosition = localPointerPos - pointerOffset;
        }
    }
}
