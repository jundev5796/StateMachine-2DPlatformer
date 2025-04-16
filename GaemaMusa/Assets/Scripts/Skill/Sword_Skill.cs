using UnityEngine;

public class Sword_Skill : Skill
{
    [Header("Skill Info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchDir;
    [SerializeField] private float swordGravity;


    private Vector2 finalDir;


    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDir = new Vector2(AimDirection().normalized.x * launchDir.x, AimDirection().normalized.y * launchDir.y);
        }   
    }


    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.transform.position, transform.rotation);
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();

        newSwordScript.SetupSword(finalDir, swordGravity);
    }


    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // bring mouse position to world
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }
}
