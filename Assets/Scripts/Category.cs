using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static GamePlay;

public class Category : MonoBehaviour
{
    public string                  Name;
    public int                     Stats;
    public TMP_Text                NameText, StatsText;
    public Button                  Button;
    public UnityEvent<string, int> CategoryPressed;

    private Color _cachedColor;

    private void OnEnable()
    {
        SwitchButton.AddListener(OnSwitchButtonCalled);
    }

    private void OnDisable()
    {
        SwitchButton.RemoveListener(OnSwitchButtonCalled);
    }

    private void OnSwitchButtonCalled(bool arg0)
    {
        Button.interactable = arg0;
    }

    private void Start()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        var img = GetComponent<Image>();
        _cachedColor = img.color;
        img.color    = Color.green;
        Invoke(nameof(RevertColor), 1f);
    }

    private void RevertColor()
    {
        GetComponent<Image>()
            .color = _cachedColor;
    }

    public void UpdateUI()
    {
        NameText.text  = Name;
        StatsText.text = Stats.ToString();
    }

    public void OnButtonPressed()
    {
        CategoryPressed?.Invoke(Name, Stats);
    }
}