using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;


    [Header("Knockback")]
    [SerializeField] private float knockbackForce = 3f;
    [SerializeField] private float knockbackHeight = 1.5f;
    [SerializeField] private float knockbackDuration = 0.15f;


    private bool isKnockedBack;
    private float knockbackTimer;


    private PlayerInput inputActions;

    private Rigidbody rb;
    private SpriteRenderer spriteRenderer;
    public Animator anim;


    private PlayerStateMachine stateMachine;
    public PlayerStateMachine StateMachine => stateMachine;


    public Rigidbody RB => rb;


    public Vector2 MoveInput { get; private set; }


    public PlayerJump PlayerJump { get; private set; }
    public PlayerDash PlayerDash { get; private set; }


    // Inputs
    public bool JumpPressed { get; private set; }
    public bool JumpReleased { get; private set; }
    public bool DashPressed { get; private set; }


    public PlayerAttack PlayerAttack { get; private set; }


    public bool Attack1Pressed { get; private set; }
    public bool Attack2Pressed { get; private set; }


    public bool BlockPressed { get; private set; }
    public bool BlockHeld { get; private set; }


    public PlayerHealth PlayerHealth { get; private set; }


    public bool IsGrounded => PlayerJump.IsGrounded;

    public bool IsMoving =>
        Mathf.Abs(MoveInput.x) > 0.1f;



    private void Awake()
    {
        inputActions = new PlayerInput();


        rb = GetComponent<Rigidbody>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        anim = GetComponent<Animator>();


        PlayerJump = GetComponent<PlayerJump>();

        PlayerDash = GetComponent<PlayerDash>();

        PlayerAttack = GetComponent<PlayerAttack>();

        PlayerHealth = GetComponent<PlayerHealth>();


        stateMachine = new PlayerStateMachine();


        stateMachine.Initialize(
            new IdleState(
                this,
                stateMachine));
    }



    private void OnEnable()
    {
        inputActions.Enable();
    }


    private void OnDisable()
    {
        inputActions.Disable();
    }



    private void Update()
    {
        Vector2 input =
            inputActions.Player.Move.ReadValue<Vector2>();


        if (Mathf.Abs(input.x) < 0.2f)
        {
            input.x = 0f;
        }


        if (PlayerHealth != null &&
            PlayerHealth.IsBlocking)
        {
            input = Vector2.zero;
        }


        MoveInput = input;



        JumpPressed =
            inputActions.Player.Jump
            .WasPressedThisFrame();


        JumpReleased =
            inputActions.Player.Jump
            .WasReleasedThisFrame();



        DashPressed =
            inputActions.Player.Dash
            .WasPressedThisFrame();



        Attack1Pressed =
            inputActions.Player.Attack1
            .WasPressedThisFrame();


        Attack2Pressed =
            inputActions.Player.Attack2
            .WasPressedThisFrame();



        BlockPressed =
            inputActions.Player.Block
            .WasPressedThisFrame();


        BlockHeld =
            inputActions.Player.Block
            .IsPressed();



        stateMachine.Update();
    }



    private void FixedUpdate()
    {
        HandleKnockback();


        stateMachine.FixedUpdate();
    }



    public void Move()
    {
        if (isKnockedBack)
            return;


        rb.linearVelocity = new Vector3(
            MoveInput.x * moveSpeed,
            rb.linearVelocity.y,
            0f);


        Flip();
    }



    public void Stop()
    {
        rb.linearVelocity = new Vector3(
            0f,
            rb.linearVelocity.y,
            0f);
    }



    private void Flip()
    {
        if (MoveInput.x > 0)
            spriteRenderer.flipX = false;


        else if (MoveInput.x < 0)
            spriteRenderer.flipX = true;
    }



    public void ApplyKnockback(Vector3 attackerPosition)
    {
        isKnockedBack = true;

        knockbackTimer = knockbackDuration;

        float direction =
            Mathf.Sign(
                transform.position.x -
                attackerPosition.x);

        rb.linearVelocity = new Vector3(
            direction * knockbackForce,
            knockbackHeight,
            0f);
    }
    private void HandleKnockback()
    {
        if (!isKnockedBack)
            return;

        knockbackTimer -= Time.fixedDeltaTime;

        if (knockbackTimer <= 0f)
        {
            isKnockedBack = false;
        }
    }
    public void SetAnimationSpeed(float speed)
    {
        anim.SetFloat(
            "Speed",
            speed);
    }
    public void PlayJumpAnimation()
    {
        anim.SetTrigger("Jump");
    }
    public void PlayDashAnimation()
    {

    }
    public float GetVerticalVelocity()
    {
        return rb.linearVelocity.y;
    }
}