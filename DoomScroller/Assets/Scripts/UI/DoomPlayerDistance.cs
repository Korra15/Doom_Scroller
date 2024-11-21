using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml;
using static Codice.Client.Common.Servers.RecentlyUsedServers;

public class DoomPlayerDistance : MonoBehaviour
{
    public MovingDoom impendingDoom;
    public PlayerStateManager player;
    public Chunk_Manager chunkManager;
    public Slider distanceSlider;
    public float maxPlayerDoomDistance;

    public GameObject endPoint;
    public GameObject startPoint;
    public GameObject cap;

    private float doomPlayerDistance;
    private float sliderValue;
    private Chunk firstChunk;
    private Chunk lastChunk;
    private float minValue;
    private float maxValue;
    private float normalizedValue;


    private void Start()
    {
        //maxPlayerDoomDistance = Mathf.Abs(impendingDoom.transform.position.x - player.transform.position.x);
    }

    void Update()
    {
        doomPlayerDistance = Mathf.Abs(impendingDoom.transform.position.x - player.transform.position.x);
       
        firstChunk = chunkManager.spawnedChunks[0].GetComponent<Chunk>();
        minValue = firstChunk.startPoint.transform.position.x;

        lastChunk = chunkManager.spawnedChunks[chunkManager.spawnedChunks.Count-1].GetComponent<Chunk>();
        maxValue = lastChunk.endPoint.transform.position.x;

        //float normalizedValue = Mathf.InverseLerp(3f,maxPlayerDoomDistance, doomPlayerDistance);
        normalizedValue = Mathf.InverseLerp(minValue, maxValue, doomPlayerDistance);
        sliderValue = 1 - normalizedValue;
       
        distanceSlider.value = sliderValue;
        SetCap(sliderValue);

        //if (sliderValue > 0) distanceSlider.value = sliderValue;
        //else distanceSlider.value = 0.01f;

       
    }
    public void SetCap(float value)
    {
        float distance = Vector2.Distance(startPoint.transform.position, endPoint.transform.position);

        // Scale the distance
        distance = distance * value;

        // Calculate the difference vector
        Vector3 difference = endPoint.transform.position - startPoint.transform.position;

        // Normalize and scale the difference vector
        difference = difference.normalized * distance;

        // Translate the vector back to A 
        cap.transform.position = (startPoint.transform.position + difference);
    }
}
