using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_Level1 : MonoBehaviour
{
    public int enemyCount = 0;
    public float combatArea = 25f;
    public float spawnRange = 1.5f;

    private GameObject player;
    public LayerMask layer;

    // Level 1 References
    public BoxCollider2D BarrierL, BarrierR;
    public Camera Cam1, Cam2;
    public Transform SpawnSpot1;

    //for creating gameObjects
    public GameObject enemy_slime;

    private int gameState = 1;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        //player = GameObject.FindGameObjectWithTag("Player");
        //we'll want to start implementing the Tag system so we could have multiple player objects potentially
        //      For character switching, down the road

        Application.targetFrameRate = 60;   //important for game speed to be same across all computers
        CountEnemies();
        
        ControlledSpawn(enemy_slime, SpawnSpot1.position, 3, 0);    //now spawns with spawn point
        gameState = 1;

        Cam2.enabled = false;
        BarrierL = GameObject.Find("Left Barrier").GetComponent<BoxCollider2D>();
        BarrierR = GameObject.Find("Right Barrier").GetComponent<BoxCollider2D>();
        BarrierL.enabled = false;
        BarrierR.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x >= -12 && player.transform.position.x <= 5 && enemyCount > 0)
        {
            Cam2.enabled = true;
            Cam1.enabled = false;
            BarrierL.enabled = true;
            
        }
        else if (enemyCount <= 0)
        {
            BarrierL.enabled = false;
            BarrierR.enabled = false;
            Cam2.enabled = false;
            Cam1.enabled = true;
        }

        if (enemyCount <= 0 && gameState == 1)
        {
            //end game
            gameState = 0;

        }

        if (Input.GetKey(KeyCode.Return))
        {
            SpawnNearPlayer(enemy_slime);
        }

        CountEnemies();
    }

    //ControlledSpawn(GameObject, Vector2);
    //This version has a default offset of 1f
    void ControlledSpawn(GameObject enemy, Vector2 position, int amount)
    {
        float offset = 1f;
        float x = position.x;
        float y = position.y;
        for (int i = 0; i < amount; i++)
        {
            Vector2 Location = new Vector2(x + Random.Range(-offset, offset), y + Random.Range(-offset, offset));
            Instantiate(enemy, Location, player.transform.rotation);
        }
    }

    //This version takes a float offset to manually set the offset values
    void ControlledSpawn(GameObject enemy, Vector2 position, int amount, float offsetVal)
    {
        float offset = offsetVal;
        float x = position.x;
        float y = position.y;
        for (int i = 0; i < amount; i++)
        {
            Vector2 Location = new Vector2(x + Random.Range(-offset, offset), y + Random.Range(-offset, offset));
            Instantiate(enemy, Location, player.transform.rotation);
        }
    }

    //SpawnNearPlayer(GameObject);
    //This version will place one object near the player (default offset of .5)
    void SpawnNearPlayer(GameObject enemy)
    {
        float offset = .5f;
        float x = player.transform.position.x;
        float y = player.transform.position.y;
        Vector2 Location = new Vector2(x + Random.Range(-offset, offset), y + Random.Range(-offset, offset));
        Instantiate(enemy, Location, player.transform.rotation);
    }

    //This version allows you to specific how many of the object and an offset range
    void SpawnNearPlayer(GameObject enemy, int amount, float offsetVal)
    {
        float offset = offsetVal;
        float x = player.transform.position.x;
        float y = player.transform.position.y;
        for (int i = 0; i < amount; i++)
        {
            Vector2 Location = new Vector2(x + Random.Range(-offset, offset), y + Random.Range(-offset, offset));
            Instantiate(enemy, Location, player.transform.rotation);
        }
    }

    void CountEnemies()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, combatArea, layer);
        int count = 0;
        foreach (Collider2D enemy in enemies)
            count++;

        if (count != enemyCount)
            enemyCount = count;
    }

}