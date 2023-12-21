using UnityEngine;
using UnityEngine.EventSystems;

public class CursorHoverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private CustomCursor customCursor;
    private bool isPointerDown = false;
    private bool isPointerHoverUI = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Display the cursor while hovering
        SetCursor();
        isPointerHoverUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isPointerDown)
        {
            RestoreDefault();
        }
        isPointerHoverUI = false;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isPointerHoverUI)
        {
            RestoreDefault();
        }
        isPointerDown = false;
    }

    public void RestoreDefault()
    {
        // Restore the default cursor
        if (customCursor)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    private void OnDisable()
    {
        RestoreDefault();
    }

    public void SetCustomCursor(CustomCursor customCursor)
    {
        this.customCursor = customCursor;
    }

    private void SetCursor()
    {
        if (customCursor)
        {
            Cursor.SetCursor(customCursor.texture, customCursor.hotspot, CursorMode.Auto);
        }
    }

}
