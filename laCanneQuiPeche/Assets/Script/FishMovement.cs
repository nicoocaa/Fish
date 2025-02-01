using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float minSpeed = 1f;  // Vitesse minimale
    public float maxSpeed = 3f;  // Vitesse maximale
    public float changeDirectionTime = 3f; // Temps avant de changer de direction
    public float rotationSpeed = 5f;  // Vitesse de rotation du poisson
    public float accelerationRate = 2f;  // Taux d'accélération/décélération
    public float catchRadius = 0.5f;  // Rayon dans lequel le poisson peut être attrapé

    private Vector3 moveDirection;
    private float currentSpeed;
    private float targetSpeed;
    private float timer;
    private Camera mainCamera;
    private float screenWidth;
    private float screenHeight;
    private SpriteRenderer spriteRenderer;
    private bool isBeingCaught = false;

    void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        CalculateScreenBounds();
        ChangeDirection();
    }

    void CalculateScreenBounds()
    {
        // Convertir les dimensions de l'écran en unités monde
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        screenWidth = cameraWidth;
        screenHeight = cameraHeight;
    }

    void Update()
    {
        if (!isBeingCaught)
        {
            // Comportement normal du poisson
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

            // Vérifier si le poisson est cliqué
            if (Input.GetMouseButtonDown(0))
            {
                CheckIfCaught();
            }
        }
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

        // Retourner le sprite en fonction de la direction
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
        isBeingCaught = true;
        // Vous pouvez ajouter ici un effet visuel, un son, etc.
        Debug.Log("Poisson attrapé !");
        
        // Optionnel : Ajouter un score ou autre logique de jeu
        
        // Détruire le poisson après un court délai
        Destroy(gameObject, 0.5f);
    }
}
