using System.Collections.Generic;
using UnityEngine;

public class Chunk_Manager : MonoBehaviour
{
    public GameObject[] chunkPrefabs; // Array of possible chunk prefabs
    public Transform player; // Reference to the player's transform
    public int chunksAhead = 2; // Number of chunks ahead of the player
   [HideInInspector]  public List<GameObject> spawnedChunks = new List<GameObject>(); // List of currently spawned chunks
    public float spawnOffset = 10f; // Distance ahead of the player where chunks should spawn
    public Chunk firstChunk;
    private GameObject lastChunk; // The last spawned chunk
    private Vector3 lastExitPoint; // Exit point of the last spawned chunk
    private bool isfirstChunk = true;
    public GameObject [] startChunks;
    int startChunkIndex = 0;
    private Queue<int> lastChunksSpawnedQueue;


    void Start()
    {
        // Spawn initial chunks to ensure there are always two ahead
        SpawnInitialChunks();
       

    }

    void Update()
    {
       
    }

    void SpawnInitialChunks()
    {
        // Spawn initial chunks to ensure there are always chunks ahead
        for (int i = 0; i < startChunks.Length + 1; i++)
        {
            SpawnRandomChunk();
        }
    }

    public Chunk SpawnRandomChunk()
    {
        if (lastChunksSpawnedQueue == null)
        {
            lastChunksSpawnedQueue = new Queue<int>();

            lastChunksSpawnedQueue.Enqueue(0);
            lastChunksSpawnedQueue.Enqueue(0);
            lastChunksSpawnedQueue.Enqueue(0);
        }
        // Pick a random chunk prefab
        GameObject newChunk;
        //set first chunk for game start
        if (isfirstChunk == true)
        {
            newChunk = Instantiate(startChunks[startChunkIndex]);
            firstChunk = newChunk.GetComponent<Chunk>();
            player.gameObject.GetComponent<PlayerCollisionManager>().currentchunk = firstChunk;
            if (startChunkIndex == startChunks.Length-1)
                isfirstChunk = false;
            else
                startChunkIndex++;
        }
        else
        {
            int spawnIndex = Random.Range(0, chunkPrefabs.Length);
            while(lastChunksSpawnedQueue.Contains(spawnIndex) && chunkPrefabs.Length > 1 )
            {
                spawnIndex = Random.Range(0, chunkPrefabs.Length);
            }
           
            lastChunksSpawnedQueue.Enqueue(spawnIndex);
            lastChunksSpawnedQueue.Dequeue();
           
            newChunk = Instantiate(chunkPrefabs[Random.Range(0, chunkPrefabs.Length)]); 
        }
        newChunk.GetComponent<Chunk>().manager = this;
        // If this is the first chunk, place it at the origin
        if (lastChunk == null)
        {
            newChunk.transform.position = Vector3.zero;
        }
        else
        {
            // Align the new chunk's entry point with the last chunk's exit point
            Transform entryPoint = newChunk.transform.Find("EntryPoint");

            Vector3 entryPointWorldPos = entryPoint.position;

            // Get the world position of the Exit Point
            Vector3 exitPointWorldPos = lastExitPoint;

            // Calculate the offset (difference) between Entry Point and Exit Point
            Vector3 offset = exitPointWorldPos - entryPointWorldPos;

            // Move the Chunk by the offset to make Entry Point overlap Exit Point
            newChunk.transform.position += offset;
        }

        // Get the new chunk's exit point
        Transform exitPoint = newChunk.transform.Find("ExitPoint");
        lastExitPoint = exitPoint.position;

        // Update the lastChunk reference
        lastChunk = newChunk;

        // Add the new chunk to the list of spawned chunks
        spawnedChunks.Add(newChunk);

        return newChunk.GetComponent<Chunk>();
    }

    public void RemoveFirstChunk()
    {
        // Destroy the first chunk in the list and remove it
        GameObject firstChunk = spawnedChunks[0];
        spawnedChunks.RemoveAt(0);
        Destroy(firstChunk);
    }
}

