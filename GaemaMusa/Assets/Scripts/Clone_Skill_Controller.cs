using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private float colorLoosingSpeed;
    [SerializeField] private float cloneDuration;
    private float cloneTimer;


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));
        }
    }


    public void SetupClone(Transform _newTransform)
    {
        transform.position = _newTransform.position;
        cloneTimer = cloneDuration;
    }
}
