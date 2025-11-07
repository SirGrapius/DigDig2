using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] bool isMoving;
    [SerializeField] float baseSpeed = 5;
    [SerializeField] float currentSpeed;
    [SerializeField] bool sprinting;

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


    [Header("Audio")]
    // audio controller script here
    [SerializeField] AudioSource walkSource;
    [Header("Anim Settings")]
    [SerializeField] Animator animator;

    [SerializeField] Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInParent<Animator>();
        currentSpeed = baseSpeed;
    }
    void Update()
    {
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); //if player presses a movement key the player will move

        if (playerInput.x != 0) //change animations of player if moving up or down.
        {
            isMoving = true;
            if (rb.linearVelocityX < 0)
            {
                rb.transform.rotation = Quaternion.Euler(0, 0, 270);
            }
            if (rb.linearVelocityX > 0)
            {
                rb.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
        }

        if (playerInput.y != 0) //change animations of player if moving up or down.
        {
            if (rb.linearVelocityY < 0)
            {
                rb.transform.rotation = Quaternion.Euler(0, 0, 0);
                //change animation to move downwards
            }
            if (rb.linearVelocityY > 0)
            {
                rb.transform.rotation = Quaternion.Euler(0, 0, 180);
                //change animation to move upwards
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) //use your current tool
        {
            if (inventory.selectedTool == 3 && !onCooldown) //if you're holding the shovel charge attack
            {
                chargingAttack = true;
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
        if (Input.GetKeyUp(KeyCode.Space) && chargingAttack) //unleash attack upon letting go of space
        {
            usingTool = false;
            if (chargingAttack)
            {
                chargingAttack = false;
                currentEnemy.Damage(1);
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
}
