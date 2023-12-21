using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyArrowPlayer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image button;
    [SerializeField] private Image background;
    [SerializeField] private Color onPressedButton;
    [SerializeField] private Color onPressedBackground;
    [SerializeField] private KeyCode key;
    [SerializeField] private UnityEvent onPressedEvents;
    private Color startColorButton;
    private Color startColorBackground;
    private bool pointerIsDown;

    void Start()
    {
        startColorButton = button.color;
        startColorBackground = background.color;
    }

    void Update()
    {
        if (pointerIsDown)
        {
            onPressedEvents?.Invoke();
        }
        else
        {
            if (Input.GetKeyDown(key))
            {
                ChangeColorOnPointerDown();
            }
            if (Input.GetKeyUp(key))
            {
                ChangeColorOnPointerUp();
            }
        }
    }
    public void ChangeColorOnPointerDown()
    {
        button.color = onPressedButton;
        background.color = onPressedBackground;
    }

    public void ChangeColorOnPointerUp()
    {
        button.color = startColorButton;
        background.color = startColorBackground;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerIsDown = true;
        ChangeColorOnPointerDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerIsDown = false;
        ChangeColorOnPointerUp();
    }
}
