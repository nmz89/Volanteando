using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class UI_LocalizedText : MonoBehaviour
{
    public string key;
    TMP_Text label;

    void Awake() => label = GetComponent<TMP_Text>();
    void OnEnable() { UI_Language.Changed += Refresh; Refresh(); }
    void OnDisable() => UI_Language.Changed -= Refresh;
    void Refresh() => label.text = UI_Language.Get(key);
}