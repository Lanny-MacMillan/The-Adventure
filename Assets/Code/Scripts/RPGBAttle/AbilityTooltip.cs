using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// hover an ability button to see what it does
public class AbilityTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject tooltip;
    [SerializeField] private Text tooltipText;
    [TextArea(2, 4)]
    [SerializeField] private string description;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltip == null || tooltipText == null)
        {
            return;
        }
        tooltipText.text = description;
        tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Hide();
    }

    // the button hides when clicked, so pointer-exit never fires
    private void OnDisable()
    {
        Hide();
    }

    private void Hide()
    {
        if (tooltip != null)
        {
            tooltip.SetActive(false);
        }
    }
}
