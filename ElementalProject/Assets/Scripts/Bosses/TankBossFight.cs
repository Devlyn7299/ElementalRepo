using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBossFight : MonoBehaviour
{
    //private Components
    private Rigidbody2D body;
    private Animator animator;
    private EnemyController controller;
    private ParticleSystem particles;
    private GameObject player;
    private Transform cannonPoint, flamePoint_0, flamePoint_1, flamePoint_2;

    //public variables
    public Transform bossFightArea;
    public GameObject bulletPrefab, Flame_projectile;
    public float healthState = 5f;
    public float barrageInterval = .3f;
    public float FlameDelay = 0.1f;
    public float FlameAttackRange = 10.0f;
    public float FlameAmount = 100f;
    // Health Bar???


    //private variables for coding
    private bool fightStarted = false;
    private bool cannonDone = false;
    private bool flameDone = false;
    private bool fightEnded = false;

    private BoxCollider2D[] traps;
    private Animator[] anim;

    // Start is called before the first frame update
    void Start()
    {
        if (bossFightArea == null)
        {
            bossFightArea = this.transform;
        }
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        controller = GetComponent<EnemyController>();
        particles = GetComponent<ParticleSystem>();
        player = GameObject.FindGameObjectWithTag("Player");

        //get components in traps (3) children
        traps = transform.GetChild(3).GetComponentsInChildren<BoxCollider2D>();
        anim = transform.GetChild(3).GetComponentsInChildren<Animator>();

        flamePoint_0 = transform.GetChild(2).GetChild(1).gameObject.transform;
        flamePoint_1 = transform.GetChild(2).GetChild(2).gameObject.transform;
        flamePoint_2 = transform.GetChild(2).GetChild(3).gameObject.transform;
        cannonPoint = transform.GetChild(2).GetChild(0).gameObject.transform;

        //start spikes safe
        foreach (BoxCollider2D trap in traps)
        {
            trap.enabled = false;
        }
    }

    private void Update()
    {
        if (startBattle() && !fightStarted)
        {
            fightStarted = true;
            StartCoroutine(BossBattle());
        }

        if(fightEnded)
        {
            Debug.Log("You defeated the Tank Boss!!");
        }
    }

    public bool startBattle()
    {
        //if you step near the boss zone,
        if (Vector2.Distance(bossFightArea.position, player.transform.position) <= 10f)
        {
            return true;
        }
        else
            return false;
    }

    IEnumerator BossBattle()
    {
        fightEnded = false;

        //begin looping between flame up, cannon, flame down, flamethrower
        while (controller.isAlive)
        {
            //put up defenses
            foreach (Animator trap in anim)
            {
                trap.SetTrigger("extend");
            }
            foreach (BoxCollider2D trap in traps)
            {
                trap.enabled = true;
            }

            //shoot a barrage of energy bullets at the player
            cannonDone = false;
            StartCoroutine(CannonAttack());
            while (!cannonDone)
            {
                yield return null;
            }

            //drop defenses
            foreach (Animator trap in anim)
            {
                trap.SetTrigger("retract");
            }
            foreach (BoxCollider2D trap in traps)
            {
                trap.enabled = false;
            }

            //shoot a jet of flame
            flameDone = false;
            StartCoroutine(FlameAttack());
            while (!flameDone)
            {
                yield return null;
            }

            //trigger defenses to warn player, wait for 2 seconds
            foreach (Animator trap in anim)
            {
                trap.SetTrigger("trigger");
            }
            yield return new WaitForSeconds(2f);
        }

        //end
        fightEnded = true;
    }

    IEnumerator CannonAttack()
    {
        //launch a barrage of energy bullets at the player
        int randomNum = Random.Range(5, 10);
        for (int i = 0; i < randomNum; i++)
        {
            Instantiate(bulletPrefab, cannonPoint.position, transform.rotation);
            yield return new WaitForSeconds(barrageInterval);
        }

        yield return new WaitForSeconds(3f);

        cannonDone = true;
    }

    IEnumerator FlameAttack()
    {
        //shoot flames for a few seconds
        //TODO

        float seperation = Vector2.Distance(body.transform.position, player.transform.position);

        if (seperation <= FlameAttackRange)
        {
            //play animation, wait for animation to finish
            //animator.SetTrigger("attack");
            for (int i = 0; i < FlameAmount; i++)
            {
               // print(seperation + "of seperation and " + FlameAttackRange + "is FlameAttackRange");
                //fire projectile, wait for attack delay, after allow attacking again

                GameObject flame1 = Instantiate(Flame_projectile, flamePoint_0.position, transform.rotation);
                flame1.GetComponent<Projectile>().BounceFreq = Random.Range(1, 3);
                GameObject flame2 = Instantiate(Flame_projectile, flamePoint_1.position, transform.rotation);
                flame2.GetComponent<Projectile>().BounceFreq = Random.Range(1, 3);
                GameObject flame3 = Instantiate(Flame_projectile, flamePoint_2.position, transform.rotation);
                flame3.GetComponent<Projectile>().BounceFreq = Random.Range(1, 3);
                print("FIRE");
                yield return new WaitForSeconds(FlameDelay);
            }
                
            
        }
        //end
        flameDone = true;
    }
}
