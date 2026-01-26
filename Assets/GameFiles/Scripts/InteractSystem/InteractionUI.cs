using TMPro;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private TextMeshProUGUI text;

    public void Show(InteractionType type, string description)
    {
        if (type == InteractionType.None)
        {
            Hide();
            return;
        }

        root.SetActive(true);
        text.text = $"ֽאזלטעו <b>E</b>, קעמבû <b>{description}</b>";
    }

    public void Hide()
    {
        root.SetActive(false);
    }
}
