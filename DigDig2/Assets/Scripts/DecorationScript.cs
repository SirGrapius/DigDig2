using UnityEngine;

public class DecorationScript : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    [SerializeField] SpriteRenderer mySpriteRend;
    [SerializeField] SpriteRenderer playerSpriteRend;

    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("PlayerSprite").GetComponent<Transform>();
        playerSpriteRend = GameObject.FindGameObjectWithTag("PlayerSprite").GetComponent<SpriteRenderer>();
        mySpriteRend = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (playerPos.position.y < this.transform.position.y)
        {
            mySpriteRend.sortingOrder = playerSpriteRend.sortingOrder - 1;
        }
        else
        {
            mySpriteRend.sortingOrder = playerSpriteRend.sortingOrder + 1;
        }
    }
}
