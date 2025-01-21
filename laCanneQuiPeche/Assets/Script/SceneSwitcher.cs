using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public SpriteRenderer transitionImage; // Référence au SpriteRenderer de l'image PNG
    public float fadeDuration = 1f; // Durée du fondu (en secondes)
    public string nextSceneName; // Nom de la scène à charger après la transition
    public float rotationSpeed = 90f; // Vitesse de rotation (degrés par seconde)

    private void Start()
    {
        // Assure que l'image commence avec une transparence totale
        if (transitionImage != null)
        {
            // Initialisation de l'alpha à 0 (image invisible)
            Color color = transitionImage.color;
            color.a = 0;
            transitionImage.color = color;

            // Rendre l'objet actif si ce n'est pas déjà fait
            transitionImage.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        // Lorsque l'utilisateur appuie sur la touche Espace
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FadeAndSwitchScene());
        }
    }

    private IEnumerator FadeAndSwitchScene()
    {
        // Transition de fondu (faire apparaître l'image)
        if (transitionImage != null)
        {
            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Lerp(0, 1, timer / fadeDuration); // Calcul de l'alpha de 0 à 1
                Color color = transitionImage.color;
                color.a = alpha;
                transitionImage.color = color; // Appliquer l'alpha à l'image

                // Faire tourner l'image pendant la transition
                transitionImage.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime); // Rotation autour de l'axe Z

                yield return null;
            }
        }

        // Une fois le fondu terminé, change la scène
        SceneManager.LoadScene(nextSceneName);
    }
}
