using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UI_Buttons_KeyAssignments : MonoBehaviour
{
    public KeyCode[] keys = { KeyCode.R };
    public float ignoreInputTime = 0.3f;

    Button button;
    float enabledAt;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    void OnEnable()
    {
        enabledAt = Time.unscaledTime;
    }

    void Update()
    {
        if (!button.interactable) return;
        if (Time.unscaledTime - enabledAt < ignoreInputTime) return;

        foreach (KeyCode key in keys)
        {
            if (Input.GetKeyDown(key))
            {
                Press();
                break;
            }
        }
    }

    void Press()
    {
        if (EventSystem.current != null)
            ExecuteEvents.Execute(gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        else
            button.onClick.Invoke();
    }
}