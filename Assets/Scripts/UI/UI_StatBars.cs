using UnityEngine;
using UnityEngine.UI;

public class UI_StatBars : MonoBehaviour
{
    private Slider slider;
    private RectTransform rectTransform;

    [Header("Bar Options")]
    [SerializeField] protected bool scalebarLengthWithStats = true;
    [SerializeField] protected float widthScaleMultiplier = 1f;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        rectTransform = GetComponent<RectTransform>();
    }

    public virtual void SetStat(int value)
    {
        slider.value = value;
    }

    public virtual void SetMaxStat(int value)
    {
        slider.maxValue = value;
        slider.value = value;

        if(scalebarLengthWithStats)
        {
            rectTransform.sizeDelta = new Vector2(value * widthScaleMultiplier, rectTransform.sizeDelta.y);
            PlayerUI.Instance.playerUIHud.RefreshHUD();
        }
    }
}
