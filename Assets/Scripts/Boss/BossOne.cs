using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Android;


enum BossPhases
{
    PHASE0, PHASE1,
}

public class BossOne : MonoBehaviour
{
    private BossPhases phases = BossPhases.PHASE0;
    private Agent agent;
    private Transform player;

    private WeaponParentController weaponParent;
    private ParticleSystem rage;

    public GameObject pillarFab;

    private List<Vector2> pillarList;

    private int tick = 0;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<Agent>();
        player = FindObjectOfType<PlayerController>().transform;
        weaponParent = GetComponentInChildren<WeaponParentController>();
        rage = GetComponentInChildren<ParticleSystem>();
        rage.gameObject.SetActive(false);

        pillarList = new List<Vector2>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player == null)
        {
            Debug.LogError("Huh player not found while boss is active... soemthing is wrong");
            return;
        }
        
        switch (phases)
        {
            case BossPhases.PHASE0:
                PhaseZeroUpdate();
                break;
            case BossPhases.PHASE1:
                PhaseOneUpdate();
                break;
        }
        tick++;

        AnimateCharacter();
    }

    void PhaseOneUpdate()
    {
        //rage mode
        if (tick < 60 * 2)
        {
            rage.gameObject.SetActive(true);
            agent.aimDirection = player.position;
            agent.movementDirection = (-transform.position + player.position).normalized;
            if (tick%10 == 0)
            {
                weaponParent.Attack();
            }
        } else if (tick < (60 * 2) + (60 * 3))
        {
            // Relaxers
            agent.movementDirection = new Vector2(0, 0);
            agent.statBlock.baseMoveSpeed = 0;
            rage.gameObject.SetActive(false);

        }
        else
        {
            tick = -1;
            
            if ((1 - (agent.healthSystem.GetCurrentValue() / (double) agent.healthSystem.GetMaxValue())) * 4 >= InitCounter)
            {
                phases = BossPhases.PHASE0;
            }
        }
    }

    int InitCounter = 0;

    void PhaseZeroInit()
    {
        // Magical Numbers

        int pillars = 6 + (InitCounter * 4);
        InitCounter++;


        for (int i = 0; i <  pillars; i++)
        {
            int tries = 30;
            Vector2 newPos;
            do
            {
                newPos = (Vector2)player.position + Random.insideUnitCircle * 10;
                tries--;

            } while (tries > 0 && checkBounds(newPos));
            pillarList.Add(newPos);

            Vector3 pillarActual = newPos;
            pillarActual.z = newPos.y*0.0001f;
            Instantiate(pillarFab, pillarActual, Quaternion.identity);
        }

        GetComponent<CapsuleCollider2D>().enabled = false;
    }

    void PhaseZeroUpdate()
    {
        if (tick == 0)
        {
            PhaseZeroInit();
        } else if (tick > 60 * 4)
        {
            tick = -1;
            phases = BossPhases.PHASE1;
            GetComponent<CapsuleCollider2D>().enabled = true;
        }
    }

    private bool checkBounds(Vector2 pos)
    {
        for (int i = 0; i < pillarList.Count; i++)
        {
            if ((pillarList[i] - pos).magnitude < 2)
            {
                return true;
            }
        }
        return false;
    }

    private void AnimateCharacter()
    {
        Vector2 lookDirection = player.position - transform.position;
        agent.agentAnimator.RotateToPointer(lookDirection);
        agent.agentAnimator.PlayWalkAnimation(agent.movementDirection);
    }

}
