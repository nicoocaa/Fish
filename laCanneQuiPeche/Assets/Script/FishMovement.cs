using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("Effets Visuels")]
    public Color normalColor = Color.white;
    public Color caughtColor = Color.green;
    public float colorTransitionSpeed = 5f;

    private Vector3 moveDirection;
    private float currentSpeed;
    private float targetSpeed;
    private float timer;
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;
    private bool isBeingCaught = false;

    private static int caughtFishCount = 0; // Variable pour compter les poissons attrapés
    private float gameTime = 5f; // Temps limite en secondes pour attraper les poissons

    void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = normalColor;
        ChangeDirection();
    }

    void Update()
    {
        // Réduit le temps restant
        gameTime -= Time.deltaTime;

        // Si le temps est écoulé ou si tous les poissons sont attrapés, on change de scène
        if (gameTime <= 0 || AllFishCaught())
        {
            // Changer de scène
            SceneManager.LoadScene("SampleScene");  // Remplacer "NextScene" par le nom de la scène suivante
            ScoreManager.SetScore(caughtFishCount);  // Enregistrer le score
            return;
        }

        if (!isBeingCaught)
        {
            NormalBehavior();

            if (Input.GetMouseButtonDown(0))
            {
                CheckIfCaught();
            }
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
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;
        float distance = Vector2.Distance(transform.position, mousePosition);

        if (distance <= catchRadius)
        {
            CatchFish();
            ScoreManager.AddScore(2); // Ajoute 2 au score pour chaque poisson attrapé
            UpdateScoreText(); // Mettez à jour le texte du score après l'ajout
        }
    }

    void CatchFish()
    {
        isBeingCaught = true;
        spriteRenderer.color = caughtColor;
        caughtFishCount++;  // Incrémente le compteur de poissons attrapés
        
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

    // Fonction qui vérifie si tous les poissons ont été attrapés
    bool AllFishCaught()
    {
        return GameObject.FindGameObjectsWithTag("Fish").Length == 0;
    }
    
    public void AddTime(float timeToAdd)
    {
        gameTime += timeToAdd;
    }


    void UpdateScoreText()
    {
        // Implémentation de la mise à jour du texte du score
    }
    public float GetGameTime()
    {
        return gameTime;
    }

}
