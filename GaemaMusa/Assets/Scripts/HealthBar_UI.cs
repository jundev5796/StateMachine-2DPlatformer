using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    private Entity entity;
    private CharacterStats myStats;
    private RectTransform myTransform;
    private Slider slider;


    private void Start()
    {
        myTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        myStats = GetComponentInParent<CharacterStats>();
        slider = GetComponentInChildren<Slider>();

        entity.onFlipped += FlipUI;

        myStats.onHealthChanged += UpdateHealthUI;

        UpdateHealthUI();

        Debug.Log("Calling Health Bar");
    }


    // private void Update()
    // {
    //     UpdateHealthUI();
    // }


    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;
    }


    public void FlipUI() => myTransform.Rotate(0, 180, 0);


    private void OnDisable() 
    {
        entity.onFlipped -= FlipUI;
        myStats.onHealthChanged -= UpdateHealthUI;
    }    
}
