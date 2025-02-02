using UnityEngine;

public class clickHandler : MonoBehaviour
{
    void OnMouseDown()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject) // Vérifie si on a cliqué sur cet objet
        {
            FishMovement[] fishMovements = FindObjectsOfType<FishMovement>();
            foreach (FishMovement fish in fishMovements)
            {
                fish.AddTime(5f);
                Debug.Log("Bouton cliqué ! La valeur de gameTime = " + fish.GetGameTime());
            }
        }
    }
}