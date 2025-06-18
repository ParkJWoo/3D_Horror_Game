using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public TextMeshProUGUI interactionText;

    public virtual void Init() 
    {
        interactionText.enabled = false;
    }

    public void SetInteractionText(bool active, string str = "")
    {
        interactionText.enabled = active;
        interactionText.text = str;
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
