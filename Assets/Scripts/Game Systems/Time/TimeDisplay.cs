using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeDisplay : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    void Update()
    {
        if (TimeManager.Instance != null)
        {
            timeText.text = TimeManager.Instance.GetTimeString();
        }
    }
}
