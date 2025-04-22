using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;


    public override void UseSkill()
    {
        base.UseSkill();

        if (currentCrystal == null)
            currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        else
        {
            player.transform.position = currentCrystal.transform.position;
            Destroy(currentCrystal);
        }
    }
}
