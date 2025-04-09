using UnityEngine;

public class ParallaxBackGround : MonoBehaviour
{
    private GameObject cam;

    [SerializeField] private float parallaxEffect;

    private float xPosition;


    void Start()
    {
        cam = GameObject.Find("Main Camera");

        xPosition = transform.position.x;
    }


    void Update()
    {
        float distanceToMove = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector2(xPosition + distanceToMove, transform.position.y);
    }
}
