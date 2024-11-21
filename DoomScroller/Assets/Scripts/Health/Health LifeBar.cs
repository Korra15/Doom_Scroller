using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthLifeBar : MonoBehaviour
{
    public GameObject lifePrefab;
    public Entity playerHealth;

    List<PlayerHealthier> lives = new List<PlayerHealthier>();
    
    // private void OnEnable(){
    //     PlayerHealth.OnPlayerDamaged += DrawLives;
    // }

    // private void OnDisable(){
    //     PlayerHealth.OnPlayerDamaged -= DrawLives;
    // }

    private void Start()
    {
        DrawLives();
    }
    public void DrawLives()
    {
        ClearLives();

        int maxHealthRemainder = playerHealth.MaxHealth % 2;
        int livesToMake = (playerHealth.MaxHealth / 2) + maxHealthRemainder;


        for(int i = 0; i < livesToMake; i++)
        {
            CreateEmptyLife();
        }

        for(int i = 0; i < lives.Count; i++){
            int lifeStatusRemainder = (int)Mathf.Clamp(playerHealth.CurrentHealth - (i*2),0,2);
            lives[i].SetLifeImage((LifeStatus)lifeStatusRemainder);
        }
    }

    public void CreateEmptyLife()
    {
        GameObject newLife = Instantiate(lifePrefab);
        newLife.transform.SetParent(transform);

        PlayerHealthier lifeComponent = newLife.GetComponent<PlayerHealthier>();
        lifeComponent.SetLifeImage(LifeStatus.Empty);
        lives.Add(lifeComponent);

    }
    public void ClearLives()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        lives = new List<PlayerHealthier>();
    }

    // Update is called once per frame
    void Update()
    {
        DrawLives();
    }
}
