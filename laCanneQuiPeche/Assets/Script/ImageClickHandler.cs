using UnityEngine;

public class ImageClickHandler : MonoBehaviour
{
    public int cost = 10;  // Le coût initial de l'action
    public TMPro.TextMeshProUGUI scoreText; // Texte qui affiche le score
    public TMPro.TextMeshProUGUI feedbackText; // Message de retour
    public FishSpawner fishSpawner; // Référence au script FishSpawner

// ...
    void OnMouseDown()
    {
        if (ScoreManager.CanAfford(cost)) // Vérifie si l'achat est possible
        {
            if (ScoreManager.Purchase(cost)) // Effectue l'achat
            {
                // Mise à jour immédiate du texte du score
                UpdateScoreText(); // Assurez-vous que le score est mis à jour avant d'afficher

                // Message de confirmation
                if (feedbackText != null)
                {
                    feedbackText.text = "Action effectuée ! Vous avez dépensé " + cost + " sous.";
                }

                // Doubler le coût de l'action après une réussite
                cost *= 2; // Cela double le coût à chaque fois

                // Augmenter le nombre de poissons de 5
                fishSpawner.IncreaseFishCount(5);
            }
        }
        else
        {
            if (feedbackText != null)
            {
                feedbackText.text = "Pas assez de sous pour acheter.";
            }
        }
    }
// ...

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "" + ScoreManager.GetScore();
        }
        else
        {
            Debug.LogError("⚠ Erreur : 'Score Text' n'est pas assigné dans l'inspecteur !");
        }
    }
}