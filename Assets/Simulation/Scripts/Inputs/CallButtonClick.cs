using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CallButtonClick : MonoBehaviour
{
    [SerializeField] private KeyCode key;
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
       if (Input.GetKeyDown(key))
        {
            button.onClick.Invoke();
        }
    }
}
