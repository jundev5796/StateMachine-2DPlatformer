using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    private Entity entity;
    private RectTransform myTransform;
    private Slider slider;


    void Start()
    {
        entity = GetComponentInParent<Entity>();
        slider = GetComponent<Slider>();
        myTransform = GetComponent<RectTransform>();

        entity.onFlipped += FlipUI;
    }


    private void FlipUI() => myTransform.Rotate(0, 180, 0);


    private void OnDisable() => entity.onFlipped -= FlipUI;
}
