using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.SpriteMask;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] bool isMoving;
    [SerializeField] float baseSpeed = 5;
    [SerializeField] public float currentSpeed;
    [SerializeField] bool sprinting;
    [SerializeField] float lastDirection = 1; //3 = side, 2 = up, 1 = down

    Vector2 playerInput;

    [Header("Target Settings")]
    [SerializeField] float damage = 2.5f;
    [SerializeField] BoxCollider2D hitbox;
    [SerializeField] float cooldown = 1;
    [SerializeField] bool onCooldown;
    [SerializeField] bool usingTool;
    [SerializeField] bool chargingAttack;
    [SerializeField] Enemy currentEnemy;

    [Header("Tool Settings")]
    [SerializeField] TileScript inventory;
    [SerializeField] float maxShovelCharge;
    [SerializeField] float currentShovelCharge;
    [SerializeField] GameObject shovelHitboxObject;


    [Header("Audio")]
    // audio controller script here
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource shovelSource;
    [SerializeField] AudioClip[] soundEffects;
    [Header("Anim Settings")]
    [SerializeField] Animator animator;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameStateManager gsManager;

    private void Awake()
    {
        gsManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<GameStateManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        currentSpeed = baseSpeed;
        gsManager.OnGameStateChange += OnGameStateChanged;
        audioSource = GetComponent<AudioSource>();
    }

    void OnDestroy()
    {
        gsManager.OnGameStateChange -= OnGameStateChanged;
    }

    void Update()
    {
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); //if player presses a movement key the player will move

        if (!isMoving)
        {
            switch (lastDirection)
            {
                case 1: //down
                    {
                        animator.SetBool("Idle", true);
                        break;
                    }
                case 2: //up
                    {
                        animator.SetBool("IdleU", true);
                        break;
                    }
                case 3: //side
                    {
                        animator.SetBool("IdleS", true);
                        break;
                    }
            }
        }
        else
        {
            animator.SetBool("Idle", false);
            animator.SetBool("IdleU", false);
            animator.SetBool("IdleS", false);
        }

        if (playerInput.x != 0) //change animations of player if moving up or down.
        {
            isMoving = true;
            animator.SetBool("WalkS", true);
            if (rb.linearVelocityX < 0 && !chargingAttack)
            {
                shovelHitboxObject.transform.rotation = Quaternion.Euler(0, 0, 270);
                rb.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (rb.linearVelocityX > 0 && !chargingAttack)
            {
                shovelHitboxObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                rb.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            lastDirection = 3;
        }
        else
        {
            animator.SetBool("WalkS", false);
        }

        if (playerInput.y != 0) //change animations of player if moving up or down.
        {
            isMoving = true;
            if (rb.linearVelocityY < 0 && !chargingAttack)
            {
                rb.transform.rotation = Quaternion.Euler(0, 0, 0);
                shovelHitboxObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                animator.SetBool("WalkD", true);
                animator.SetBool("WalkU", false);
                lastDirection = 1;
            }
            if (rb.linearVelocityY > 0 && !chargingAttack)
            {
                rb.transform.rotation = Quaternion.Euler(0, 0, 0);
                shovelHitboxObject.transform.rotation = Quaternion.Euler(0, 0, 180);
                animator.SetBool("WalkU", true);
                animator.SetBool("WalkD", false);
                lastDirection = 2;
            }
        }
        else
        {
            animator.SetBool("WalkU", false);
            animator.SetBool("WalkD", false);
        }

        if (isMoving)
        {

        }
        else
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
        }

        if (rb.linearVelocity == new Vector2(0, 0))
        {
            isMoving = false;
        }

        if (Input.GetKeyDown(KeyCode.Space)) //use your current tool
        {
            if (inventory.selectedTool == 3 && !onCooldown) //if you're holding the shovel charge attack
            {
                chargingAttack = true;
                animator.SetBool("WalkU", false);
                shovelSource.PlayOneShot(soundEffects[1]);
                switch (lastDirection)
                {
                    case 1: //down
                        {
                            animator.SetBool("ChargingD", true);
                            break;
                        }
                    case 2: //up
                        {
                            animator.SetBool("ChargingU", true);
                            break;
                        }
                    case 3: //side
                        {
                            animator.SetBool("ChargingS", true);
                            break;
                        }
                }
            }
            if (inventory.selectedTool == 2) //if you're holding the watering can
            {
                usingTool = true;
            }
            if (inventory.selectedTool == 1) //if you're using the hoe
            {
                usingTool = true;
            }
        }

        if (chargingAttack && currentShovelCharge < maxShovelCharge) //if charging attack, count time charging
        {
            currentShovelCharge += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space) && chargingAttack) //unleash attack upon letting go of space
        {
            usingTool = false;
            if (chargingAttack)
            {
                chargingAttack = false;
                shovelSource.PlayOneShot(soundEffects[2]);
                switch (lastDirection)
                {
                    case 1: //down
                        {
                            animator.SetBool("ChargingD", false);
                            break;
                        }
                    case 2: //up
                        {
                            animator.SetBool("ChargingU", false);
                            break;
                        }
                    case 3: //side
                        {
                            animator.SetBool("ChargingS", false);
                            break;
                        }
                }
                if (currentEnemy != null)
                {
                    shovelSource.PlayOneShot(soundEffects[3]);
                    currentEnemy.Damage(Mathf.RoundToInt(damage * ((1 + currentShovelCharge) / 0.75f)));
                }
                currentShovelCharge = 0;
                StartCoroutine(CooldownDuration());
            }
        }

        if (rb.linearVelocityX != 0 && rb.linearVelocityY != 0) //reduce velocity when moving diagonally for more realistic movement
        {
            rb.linearVelocityX = rb.linearVelocityX * Time.deltaTime / Mathf.Sqrt(2);
            rb.linearVelocityY = rb.linearVelocityY * Time.deltaTime / Mathf.Sqrt(2);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !sprinting || Input.GetKeyDown(KeyCode.RightShift) && !sprinting) //check if player is sprinting
        {
            sprinting = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)) //stop sprinting
        {
            sprinting = false;
        }

        if (sprinting) //increase speed when sprinting
        {
            currentSpeed = baseSpeed * 1.5f;
        }
        else //revert speed when not sprinting
        {
            currentSpeed = baseSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && inventory.selectedTool == 3) //check if you're attacking and there's an enemy in your hitbox and do damage.
        {
            currentEnemy = collision.gameObject.GetComponent<Enemy>();
            Debug.Log("bam, boom, wop");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.gameObject.GetComponent<Enemy>() == currentEnemy)
        {
            currentEnemy = null;
        }
    }

    private IEnumerator CooldownDuration() //start cooldown
    {
        shovelSource.Stop();
        Debug.Log("on cooldown");
        onCooldown = true;
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
        Debug.Log("off cooldown");
        yield return null;
    }

    private void FixedUpdate() //add a velocity when player is moving
    {
        rb.linearVelocityX = playerInput.x * currentSpeed;
        rb.linearVelocityY = playerInput.y * currentSpeed;
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }

    public void Save(ref PlayerSaveData data)
    {
        Debug.Log("fucking save!!");
        data.Position = transform.position;
    }

    public void Load(PlayerSaveData data)
    {
        transform.position = data.Position;
    }
}

[System.Serializable]
public struct PlayerSaveData
{
    public Vector3 Position;
}
