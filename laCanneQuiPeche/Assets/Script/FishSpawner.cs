using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject[] fishPrefabs; // Tableau contenant plusieurs prefabs de poissons
    public int numberOfFish = 10; // Nombre initial de poissons
    public Vector2 spawnAreaSize = new Vector2(8f, 4.5f);

    void Start()
    {
        SpawnFish();
    }

    void SpawnFish()
    {
        for (int i = 0; i < numberOfFish; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            GameObject randomFishPrefab = fishPrefabs[Random.Range(0, fishPrefabs.Length)]; // Choix aléatoire du poisson
            Instantiate(randomFishPrefab, randomPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float y = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);
        return new Vector3(x, y, -1);
    }

    // Méthode pour augmenter le nombre de poissons
    public void IncreaseFishCount(int amount)
    {
        numberOfFish += amount; // Augmente le nombre de poissons
        SpawnFish(); // Re-spawn les poissons
    }
}