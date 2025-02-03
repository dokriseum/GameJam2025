using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Text))]
[RequireComponent(typeof(LayoutElement))]
[ExecuteInEditMode]
public class TextFitter : MonoBehaviour
{
    TMP_Text tmp;
    LayoutElement layout;
    [SerializeField, Tooltip("Minimum height of the text")] float minHeight = 50;

    private void OnEnable()
    {
        tmp = GetComponent<TMP_Text>();
        layout = GetComponent<LayoutElement>();

        tmp.OnPreRenderText -= OnTextChanged; // prevent adding multiple times
        tmp.OnPreRenderText += OnTextChanged;

        OnTextChanged(tmp.textInfo);
    }
    private void OnTextChanged(TMP_TextInfo info)
    {
        if (!this.enabled)
            return;

        if (tmp.enableAutoSizing == false)
            return;

        // get minimum height needed for min font size
        tmp.enableAutoSizing = false;
        tmp.fontSize = tmp.fontSizeMin;
        // set height of min font, but with a capped min size
        layout.preferredHeight = Mathf.Max(tmp.preferredHeight, minHeight);
        // restore behaviour
        tmp.enableAutoSizing = true;
    }
    private void OnDisable()
    {
        tmp.OnPreRenderText -= OnTextChanged;
    }
}