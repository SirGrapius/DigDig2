using UnityEngine;

public class Chiliscript : MonoBehaviour
{
    ClosestEnemy targeting;
    public Animator baseAnimator;
    float maxRange = 4;
    public bool growing;
    public float growthTimer;
    public float waterAmount = 1;
    public float waterAmountMin = 1;
    public float waterLoss = 1;
    [SerializeField] float stage1 = 30;
    [SerializeField] float stage2 = 60;
    [SerializeField] float stage3 = 90;
    private void Awake()
    {
        baseAnimator = GetComponent<Animator>();
        targeting = GetComponent<ClosestEnemy>();
    }
    void Update()
    {
        if (growing)
        {
            if (waterAmount > waterAmountMin)
            {
                waterAmount -= waterLoss * Time.deltaTime;
            }
            if (waterAmount < waterAmountMin)
            {
                waterAmount = waterAmountMin;
            }
            growthTimer += Time.deltaTime * waterAmount;

            if (growthTimer >= stage3)
            {
                baseAnimator.SetBool("Adult", true);
                gameObject.tag = "Plant";
                growing = false;
            }
            else if (growthTimer >= stage2)
            {
                baseAnimator.SetBool("Young", true);
            }
            else if (growthTimer >= stage1)
            {
                baseAnimator.SetBool("Child", true);
            }
        }
        if (!growing)
        {
            GameObject[] enemiesInRange = targeting.Target(maxRange, 5);
            if (enemiesInRange != null)
            {
                Debug.Log("NUKE!"); //Kaboom
            }
        }
    }
}
