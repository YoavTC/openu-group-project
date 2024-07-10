using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject hoverElement;
    [SerializeField] private Color buttonHoverTextColour;
    private Color originalButtonTextColour;
    private TMP_Text textDisplay;
    
    public UnityEvent<Transform> onMouseHoverEnter;
    public UnityEvent<Transform> onMouseHoverExit;

    private void Start()
    {
        textDisplay = transform.GetChild(0).GetComponent<TMP_Text>();
        originalButtonTextColour = textDisplay.color;
        OnPointerExit(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onMouseHoverEnter?.Invoke(transform);
        
        hoverElement.SetActive(true);
        textDisplay.color = buttonHoverTextColour;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        onMouseHoverExit?.Invoke(transform);
        
        hoverElement.SetActive(false);
        textDisplay.color = originalButtonTextColour;
    }
}
