using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public static float maxTime = 4;
    public float minTime = 1;
    public GameObject spawnable;
    public GameObject alsospawnable;
    public GameObject alsospawnables;
    public GameObject shooterAlt;
    public GameObject jumperAlt;
    public GameObject dropperAlt;
    private float time = 0, time2 = 0;
    private float spawnTime;
    public int whereToSpawn = 13;
    public int whoToSpawn;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        whoToSpawn = Random.Range(1, 11);
        time += Time.deltaTime; //counts time
        time2 += Time.deltaTime;

        if (time >= spawnTime)
        {
            SpawnObject(whereToSpawn);
            SetRandomTime();
            time = 0;
        }
        if (maxTime >= 1.3)
        {
            if (time2 >= 1)
            {
                maxTime = maxTime - .1f;
                //print(maxTime);
                time2 = 0;
            }
        }
        if(scoreScript.scoreValue >= 125 && maxTime >= .82f)
        {
          
            if (time2 >= 1)
            {
                maxTime = maxTime -.02f;
                //print(maxTime);
                time2 = 0;
            }
        }
        //if (Input.GetKey(KeyCode.Y))
        //{
        //    scoreScript.scoreValue += 10;
        //}

    }
    private void SetRandomTime()
    {
        spawnTime = Random.Range(minTime, maxTime);
    }
    private void SpawnObject(int spawnX)
    {
        if (scoreScript.scoreValue <= 100)
        {
            if (whoToSpawn <= 6)
            {
                Instantiate(spawnable, new Vector3((Random.Range(-spawnX, spawnX + 1) > 0) ? spawnX : -spawnX, 0, 0), Quaternion.identity);
            }
            else if (whoToSpawn >= 9)
            {
                Instantiate(alsospawnable, new Vector3((Random.Range(-spawnX, spawnX + 1) > 0) ? spawnX : -spawnX, 0, 0), Quaternion.identity);
            }
            else if (whoToSpawn <= 7 || whoToSpawn > 9)
            {
                Instantiate(alsospawnables, new Vector3((Random.Range(-spawnX, spawnX + 1) > 0) ? spawnX : -spawnX, 0, 0), Quaternion.identity);
            }
        }
        if (scoreScript.scoreValue >= 100 && scoreScript.scoreValue <= 200)
        {
            if (whoToSpawn <= 3)
            {
                Instantiate(spawnable, new Vector3((Random.Range(-spawnX, spawnX + 1) > 0) ? spawnX : -spawnX, 0, 0), Quaternion.identity);
            }
            if (whoToSpawn == 4)
            {
                Instantiate(shooterAlt, new Vector3((Random.Range(-spawnX, spawnX + 1) > 0) ? spawnX : -spawnX, 0, 0), Quaternion.identity);
            }
            if (whoToSpawn >= 5 && whoToSpawn < 7)
            {
                Instantiate(alsospawnable, new Vector3((Random.Range(-spawnX, spawnX + 1) > 0) ? spawnX : -spawnX, 0, 0), Quaternion.identity);
            }
            if (whoToSpawn == 7 || whoToSpawn == 8)
            {
                Instantiate(dropperAlt, new Vector3((Random.Range(-spawnX, spawnX + 1) > 0) ? spawnX : -spawnX, 0, 0), Quaternion.identity);
            }
            if (whoToSpawn == 9)
            {
                Instantiate(alsospawnables, new Vector3((Random.Range(-spawnX, spawnX + 1) > 0) ? spawnX : -spawnX, 0, 0), Quaternion.identity);
            }
            if (whoToSpawn == 10)
            {
                Instantiate(jumperAlt, new Vector3((Random.Range(-spawnX, spawnX + 1) > 0) ? spawnX : -spawnX, 0, 0), Quaternion.identity);
            }
        }
        if (scoreScript.scoreValue >= 200)
        {
            if (whoToSpawn <= 1)
            {
                Instantiate(spawnable, new Vector3((Random.Range(-spawnX, spawnX + 1) > 0) ? spawnX : -spawnX, 0, 0), Quaternion.identity);
            }
            if (whoToSpawn >= 2 && whoToSpawn < 4)
            {
                Instantiate(shooterAlt, new Vector3((Random.Range(-spawnX, spawnX + 1) > 0) ? spawnX : -spawnX, 0, 0), Quaternion.identity);
            }
            if (whoToSpawn >= 5 && whoToSpawn < 6)
            {
                Instantiate(alsospawnable, new Vector3((Random.Range(-spawnX, spawnX + 1) > 0) ? spawnX : -spawnX, 0, 0), Quaternion.identity);
            }
            if (whoToSpawn >= 7 && whoToSpawn < 9)
            {
                Instantiate(dropperAlt, new Vector3((Random.Range(-spawnX, spawnX + 1) > 0) ? spawnX : -spawnX, 0, 0), Quaternion.identity);
            }
            if (whoToSpawn >= 8 && whoToSpawn <= 9)
            {
                Instantiate(alsospawnables, new Vector3((Random.Range(-spawnX, spawnX + 1) > 0) ? spawnX : -spawnX, 0, 0), Quaternion.identity);
            }
            if (whoToSpawn == 10)
            {
                Instantiate(jumperAlt, new Vector3((Random.Range(-spawnX, spawnX + 1) > 0) ? spawnX : -spawnX, 0, 0), Quaternion.identity);
            }



        }
    }
}

