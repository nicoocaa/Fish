using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public SpriteRenderer transitionImage;
    public float fadeDuration = 1f;
    public string nextSceneName;
    public float rotationSpeed = 90f;

    private void Start()
    {
        if (transitionImage != null)
        {
            Color color = transitionImage.color;
            color.a = 0;
            transitionImage.color = color;
            transitionImage.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FadeAndSwitchScene());
        }
    }

    private IEnumerator FadeAndSwitchScene()
    {
        if (transitionImage != null)
        {
            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
                Color color = transitionImage.color;
                color.a = alpha;
                transitionImage.color = color;

                transitionImage.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

                yield return null;
            }
        }

        SceneManager.LoadScene(nextSceneName);
    }
}
