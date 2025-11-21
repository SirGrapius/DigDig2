using UnityEngine;

public class PotatoScript : MonoBehaviour
{
    public Animator baseAnimator;
    public bool growing;
    public float growthTimer;
    public float waterAmount = 1;
    public float waterAmountMin = 1;
    public float waterLoss = 1;
    [SerializeField] float stage1 = 30;
    [SerializeField] float stage2 = 60;
    [SerializeField] float stage3 = 90;
    public int damageValue = 1;
    private void Awake()
    {
        baseAnimator = GetComponent<Animator>();    
        gameObject.tag = "GrowingPlant";
        growing = true;
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
    }
    public void BecomeBaby()
    {
        baseAnimator.SetBool("Child", false);
        baseAnimator.SetBool("Young", false);
        baseAnimator.SetBool("Adult", false);
        growing = true;
        growthTimer = 0;
    }
}
