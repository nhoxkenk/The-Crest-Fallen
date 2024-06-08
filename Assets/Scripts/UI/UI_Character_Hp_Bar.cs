using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Character_Hp_Bar : UI_StatBars
{
    [SerializeField] private AICharacterManager characterManager;
    [SerializeField] private Image healthImageSlider;
    [SerializeField] private float visibleTime = 3f;

    private float maxValue;
    private Camera cam;
    private CountdownTimer timer;


    protected override void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        characterManager = GetComponentInParent<AICharacterManager>();
    }

    protected override void Start()
    {
        base.Start();

        cam = Camera.main;
        GetComponentInParent<Canvas>().worldCamera = cam;
        timer = new CountdownTimer(visibleTime);

        characterManager.characterStat.CurrentHealthChange += HandleCurrentHealthChange;
    }
    
    private void HandleCurrentHealthChange(float maxHealth, float currentHealth)
    {
        gameObject.SetActive(true);

        if(timer.IsRunning)
        {
            timer.Reset();
        }
        else
        {
            timer.Start();
        }
        
        float healthPercent = (float)currentHealth / maxHealth;

        healthImageSlider.fillAmount = healthPercent;

        if (currentHealth < 0)
        {
            characterManager.characterStat.CurrentHealthChange -= HandleCurrentHealthChange;
            Destroy(gameObject);
        }
    }


    public override void SetStat(int value)
    {
        healthImageSlider.fillAmount = (float)value / maxValue;
    }

    public override void SetMaxStat(int value)
    {
        maxValue = value;
        healthImageSlider.fillAmount = (float) value / maxValue;
        if (scalebarLengthWithStats)
        {
            rectTransform.sizeDelta = new Vector2(value * widthScaleMultiplier, rectTransform.sizeDelta.y);
            PlayerUI.Instance.playerUIHud.RefreshHUD();
        }
    }

    private void LateUpdate()
    {
        if (timer.IsFinished())
        {
            gameObject.SetActive(false);
        }
        else
        {
            timer.Tick(Time.deltaTime);
        }

        transform.LookAt(transform.position + cam.transform.forward);
    }
}
