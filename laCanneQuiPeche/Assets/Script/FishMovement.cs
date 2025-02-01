using UnityEngine;
using System.Collections;

public class FishMovement : MonoBehaviour
{
    [Header("Mouvement")]
    public float minSpeed = 0.3f;
    public float maxSpeed = 0.8f;
    public float changeDirectionTime = 3f;
    public float rotationSpeed = 2f;
    public float accelerationRate = 1f;
    
    [Header("Comportement")]
    public float catchRadius = 0.8f;
    public float panicSpeed = 0.6f;
    public float panicDuration = 1f;
    public float detectionRadius = 1f;
    public float panicChance = 0.5f;
    
    [Header("Effets Visuels")]
    public Color normalColor = Color.white;
    public Color panicColor = Color.red;
    public float colorTransitionSpeed = 5f;

    private Vector3 moveDirection;
    private float currentSpeed;
    private float targetSpeed;
    private float timer;
    private Camera mainCamera;
    private float screenWidth;
    private float screenHeight;
    private SpriteRenderer spriteRenderer;
    private bool isBeingCaught = false;
    private bool isPanicked = false;
    private float panicTimer = 0f;
    private Vector3 lastMousePosition;

    void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = normalColor;
        CalculateScreenBounds();
        ChangeDirection();
        lastMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    void CalculateScreenBounds()
    {
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        screenWidth = cameraWidth;
        screenHeight = cameraHeight;
    }

    void Update()
    {
        if (!isBeingCaught)
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z;
            float distanceToMouse = Vector2.Distance(transform.position, mousePosition);

            float mouseSpeed = Vector3.Distance(mousePosition, lastMousePosition) / Time.deltaTime;
            bool isMouseMovingFast = mouseSpeed > 15f;

            if (distanceToMouse < detectionRadius && isMouseMovingFast && !isPanicked)
            {
                if (Random.value < panicChance)
                {
                    StartPanic(mousePosition);
                }
            }

            if (isPanicked)
            {
                HandlePanicBehavior();
            }
            else
            {
                NormalBehavior();
            }

            spriteRenderer.color = Color.Lerp(spriteRenderer.color, 
                isPanicked ? panicColor : normalColor, 
                Time.deltaTime * colorTransitionSpeed);

            if (Input.GetMouseButtonDown(0))
            {
                CheckIfCaught();
            }

            lastMousePosition = mousePosition;
        }
    }

    void NormalBehavior()
    {
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * accelerationRate);
        
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        
        transform.position += moveDirection * currentSpeed * Time.deltaTime;
        WrapAroundScreen();

        timer += Time.deltaTime;
        if (timer >= changeDirectionTime)
        {
            ChangeDirection();
            timer = 0;
        }
    }

    void StartPanic(Vector3 mousePosition)
    {
        isPanicked = true;
        panicTimer = 0f;
        Vector3 awayFromMouse = (transform.position - mousePosition).normalized;
        moveDirection = awayFromMouse;
        currentSpeed = panicSpeed;
    }

    void HandlePanicBehavior()
    {
        panicTimer += Time.deltaTime;
        if (panicTimer >= panicDuration)
        {
            isPanicked = false;
            ChangeDirection();
            return;
        }

        moveDirection += new Vector3(
            Random.Range(-0.02f, 0.02f),
            Random.Range(-0.02f, 0.02f),
            0
        ).normalized;

        transform.position += moveDirection * panicSpeed * Time.deltaTime;
        WrapAroundScreen();
    }

    void WrapAroundScreen()
    {
        Vector3 viewPos = mainCamera.WorldToViewportPoint(transform.position);
        Vector3 newPosition = transform.position;

        if (viewPos.x < 0)
            newPosition.x = mainCamera.ViewportToWorldPoint(new Vector3(1, viewPos.y, viewPos.z)).x;
        else if (viewPos.x > 1)
            newPosition.x = mainCamera.ViewportToWorldPoint(new Vector3(0, viewPos.y, viewPos.z)).x;

        if (viewPos.y < 0)
            newPosition.y = mainCamera.ViewportToWorldPoint(new Vector3(viewPos.x, 1, viewPos.z)).y;
        else if (viewPos.y > 1)
            newPosition.y = mainCamera.ViewportToWorldPoint(new Vector3(viewPos.x, 0, viewPos.z)).y;

        transform.position = newPosition;
    }

    void ChangeDirection()
    {
        targetSpeed = Random.Range(minSpeed, maxSpeed);
        moveDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;

        if (moveDirection.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void CheckIfCaught()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;
        float distance = Vector2.Distance(transform.position, mousePosition);

        if (distance <= catchRadius)
        {
            CatchFish();
        }
    }

    void CatchFish()
    {
        if (isPanicked)
        {
            if (Random.value > 0.6f) return;
        }

        isBeingCaught = true;
        spriteRenderer.color = Color.green;
        Debug.Log("Poisson attrapé !");
        
        StartCoroutine(CatchAnimation());
    }

    System.Collections.IEnumerator CatchAnimation()
    {
        Vector3 originalScale = transform.localScale;
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            yield return null;
        }

        Destroy(gameObject);
    }
}
