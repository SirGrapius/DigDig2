using UnityEngine;

public class DecorationScript : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    [SerializeField] SpriteRenderer mySpriteRend;
    [SerializeField] SpriteRenderer playerSpriteRend;
    [SerializeField] GameStateManager gsManager;

    [SerializeField] bool hasSFX;


    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip soundEffect;

    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("PlayerSprite").GetComponent<Transform>();
        playerSpriteRend = GameObject.FindGameObjectWithTag("PlayerSprite").GetComponent<SpriteRenderer>();
        gsManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<GameStateManager>();
        mySpriteRend = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundEffect;
    }

    void Update()
    {
        audioSource.volume = gsManager.sfxVolume;
        if (playerPos.position.y < this.transform.position.y)
        {
            mySpriteRend.sortingOrder = playerSpriteRend.sortingOrder - 1;
        }
        else
        {
            mySpriteRend.sortingOrder = playerSpriteRend.sortingOrder + 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasSFX)
        {
            audioSource.PlayOneShot(soundEffect);
        }
    }
}
