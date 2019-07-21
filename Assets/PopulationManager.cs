using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour {

    public GameObject botPrefab;
    public int populationSize = 50;
    List<GameObject> population = new List<GameObject>();
    public static float elapsed = 0;
    public float elapsedView = elapsed;
    public float trialTime = 5;
    public int generation = 1;

    public float radius = 3;

    private void Start()
    {
        for(int i = 0; i <populationSize; i++)
        {
            Vector3 startingPos = new Vector3(this.transform.position.x + Random.Range(-radius, radius), this.transform.position.y, this.transform.position.z + Random.Range(-radius, radius));

            GameObject newBot = Instantiate(botPrefab, startingPos, this.transform.rotation);
            newBot.GetComponent<Brain>().Init();
            population.Add(newBot);
            
        }
    }

    private GameObject Breed(GameObject parent1, GameObject parent2)
    {
        Vector3 startingPos = new Vector3(this.transform.position.x + Random.Range(-radius, radius), this.transform.position.y, this.transform.position.z + Random.Range(-radius, radius));
        GameObject newBot = Instantiate(botPrefab, startingPos, this.transform.rotation);
        Brain botBrain = newBot.GetComponent<Brain>();
        if(Random.Range(0,100) == 1)
        {
            botBrain.Init();
            botBrain.dna.Mutate();
        }
        else
        {
            botBrain.Init();
            botBrain.dna.Combine(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna);
        }
        return newBot;
    }

    private void BreedNewPopulation()
    {
        List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<Brain>().timeAlive).ToList();
        population.Clear();

        for(int i = (int) (sortedList.Count/ 2.0f) - 1; i < sortedList.Count -1; i++)
        {
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i + 1], sortedList[i]));
        }

        for (int i = 0; i <sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }
        generation++;
    }

    private void FixedUpdate()
    {
        elapsedView = elapsed;
        elapsed += Time.deltaTime;
        if(elapsed >= trialTime)
        {
            BreedNewPopulation();
            elapsed = 0;
        }

    }
}
