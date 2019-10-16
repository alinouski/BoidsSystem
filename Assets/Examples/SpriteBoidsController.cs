using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteBoidsController : MonoBehaviour
{
    public SpriteBoid boidPrefab;
    public BoidConfiguration configuration;
    public BoidsBehaviourPreset preset;
    public Rect spawnArea;
    public bool unlimSpace;
    [Range(1, 100)]
    public int spawnBoidsCount;

    protected List<IBoid> boidsList = new List<IBoid>();

    private void Start()
    {
        for (int i = 0; i < spawnBoidsCount; i++)
        {
            Vector2 pos = new Vector2(UnityEngine.Random.Range(0, spawnArea.width), UnityEngine.Random.Range(0, spawnArea.height)) + spawnArea.position;
            Vector2 rot = new Vector2(UnityEngine.Random.Range(-1, 1.0f), UnityEngine.Random.Range(-1, 1.0f));

            CreateBoid(pos, rot);
        }
    }

    void Update()
    {
        for (int i = 0; i < boidsList.Count; i++)
        {
            if(Time.frameCount % 2 == 0)
                boidsList[i].UpdateVectors(boidsList, preset);

            boidsList[i].UpdatePosition();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rot = new Vector2(UnityEngine.Random.Range(-1, 1.0f), UnityEngine.Random.Range(-1, 1.0f));
            CreateBoid(Camera.main.ScreenToWorldPoint(Input.mousePosition), rot);
        }
    }

    protected void CreateBoid(Vector2 position, Vector2 velocity)
    {
        Boid2D boid = new Boid2D(position, velocity, configuration, spawnArea, unlimSpace);

        Instantiate(boidPrefab, position, Quaternion.identity).InitBoid(boid);

        boidsList.Add(boid);
    }

    private void OnValidate()
    {
        for (int i = 0; i < boidsList.Count; i++)
        {
            ((Boid2D)boidsList[i]).Config = configuration;
        }
    }
}


