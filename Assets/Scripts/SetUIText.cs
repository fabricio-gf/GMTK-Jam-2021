using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUIText : MonoBehaviour
{
    public TMPro.TMP_Text text;

    public void SetText(int id)
    {
        text.text = id.ToString();
    }

    public void SetText(string stringVal)
    {
        text.text = stringVal;
    }

    public void SetText(Slider slider)
    {
        text.text = slider.value.ToString();
    }
}
