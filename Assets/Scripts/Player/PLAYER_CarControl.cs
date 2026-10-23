using UnityEngine;

public class PLAYER_CarControl : MonoBehaviour
{
    PLAYER_AirborneCar air;
    AUDIO_Manager audioManager;
    SpriteRenderer carSprite;
    Camera cam;

    public Rigidbody2D carRigidBody2D;
    public float swerveStrength;
    public LogicScript logic;
    public bool carHasNotCrashed = true;
    public float playerSpeed = 10f;

    [Header("Oil Spin")]
    public bool isSpinning = false;
    public float spinDuration = 1.2f;
    private float accumulatedSpinAngle;
    private float previousRotation;
    private float spinStartAngle;
    private float spinDirection;

    private PLAYER_CarTilt PLAYER_CarTilt;
    private RigidbodyConstraints2D originalConstraints;
    private float originalFixedDeltaTime;

    private void Awake()
    {
        originalFixedDeltaTime = Time.fixedDeltaTime;

        PLAYER_CarTilt = GetComponent<PLAYER_CarTilt>();
        originalConstraints = carRigidBody2D.constraints;

        audioManager = GameObject.FindGameObjectWithTag("Audio")
            .GetComponent<AUDIO_Manager>();

        carSprite = GetComponentInChildren<SpriteRenderer>();
        cam = Camera.main;
        air = GetComponent<PLAYER_AirborneCar>();
    }

    void Start()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = originalFixedDeltaTime;

        logic = GameObject.FindGameObjectWithTag("Logic")
            .GetComponent<LogicScript>();
    }

    void Update()
    {
        if (!carHasNotCrashed) return;

        UpdateEnginePitch();
        InputHandling();
        UpdateOilSpin();
        OffScreenCheck();
    }

    void FixedUpdate()
    {
        if (!carHasNotCrashed) return;

        float yVelocity = carRigidBody2D.linearVelocity.y;

        if (air != null && air.IsAirborne)    // Temporarily disable the player's car gravity while it's in the air.
            yVelocity = 0f;

        carRigidBody2D.linearVelocity = new Vector2(playerSpeed, yVelocity);

        if (isSpinning)
        {
            carRigidBody2D.angularVelocity =
                spinDirection * (720f / spinDuration);
        }
    }

    private void UpdateEnginePitch()    // Control engine sound pitch, normalized for selected game difficulty.
    {
        float maxSpeed = GAME_Settings.CurrentDifficulty.maxSpeed;
        float normalizedSpeed = Mathf.Clamp01(playerSpeed / maxSpeed);

        float enginePitch = Mathf.Lerp(0.55f, 2.5f, normalizedSpeed);

        audioManager.SetEnginePitch(enginePitch);
    }

    bool IsFullyOffScreen()    // Checks if the car goes off screen if curb collisions are disabled.
    {
        float camTop = cam.transform.position.y + cam.orthographicSize;
        float camBottom = cam.transform.position.y - cam.orthographicSize;
        Bounds b = carSprite.bounds;

        return b.min.y > camTop || b.max.y < camBottom;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!carHasNotCrashed) return;

        Crash();
    }

    public void StartOilSpin(float direction)
    {
        isSpinning = true;
        accumulatedSpinAngle = 0f;

        spinStartAngle = carRigidBody2D.rotation;
        previousRotation = spinStartAngle;

        carRigidBody2D.constraints = RigidbodyConstraints2D.None;

        if (PLAYER_CarTilt != null)
            PLAYER_CarTilt.enabled = false;

        spinDirection = direction;
    }

    public void UpdateOilSpin()
    {
        if (!isSpinning) return;

        float currentRotation = carRigidBody2D.rotation;

        accumulatedSpinAngle += Mathf.Abs(
            Mathf.DeltaAngle(previousRotation, currentRotation)
        );

        previousRotation = currentRotation;

        if (accumulatedSpinAngle >= 720f)
        {
            isSpinning = false;

            carRigidBody2D.angularVelocity = 0f;
            carRigidBody2D.rotation = spinStartAngle;
            carRigidBody2D.constraints = originalConstraints;

            if (PLAYER_CarTilt != null)
                PLAYER_CarTilt.enabled = true;
        }
    }

    public void InputHandling()
    {
        bool canFlap = air == null || !air.IsAirborne;

        if (canFlap && ((Input.GetKeyDown(KeyCode.Space)) || Input.GetMouseButtonDown(0)))
        {
            carRigidBody2D.linearVelocity = new Vector2(playerSpeed, swerveStrength);
            audioManager.PlaySFX(audioManager.spacePress);
        }
    }

    public void OffScreenCheck()
    {
        if (IsFullyOffScreen())
        {
            carHasNotCrashed = false;
            audioManager.StopEngine();
            logic.offScreen();
            carRigidBody2D.simulated = false;
        }
    }

    public void Crash()
    {
        if (!carHasNotCrashed) return;

        carHasNotCrashed = false;

        // Disable scripted tilting and release rotation to physics.
        if (PLAYER_CarTilt != null)
        {
            PLAYER_CarTilt.enabled = false;
            carRigidBody2D.constraints &= ~RigidbodyConstraints2D.FreezeRotation;
        }

        carRigidBody2D.gravityScale = 0f;
        carRigidBody2D.linearVelocity = Vector2.zero;
        carRigidBody2D.angularVelocity = 0f;

        audioManager.PlaySFX(audioManager.crash);
        audioManager.StopEngine();

        Time.timeScale = 0.25f;
        logic.gameOver();
    }
}