using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PillarScript : MonoBehaviour
{

    public GameObject particleSystem;
    public GameObject particleSystem2;
    public GameObject pillarAsset;

    public int hp = 30;

    // Start is called before the first frame update
    void Start()
    {

        pillarAsset.SetActive(false);

        StartCoroutine(DelayPillar());
    }

    private IEnumerator DelayPillar()
    {
        yield return new WaitForSeconds(1.5f);


        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, 1.5f))
        {
            // Get users and targets
            PlayerController playerTarget = collider.GetComponent<PlayerController>();

            if (collider.tag == "Enemy") { continue; }

            // Deal damage to target
            if (playerTarget != null)
            {
                // Roll i-frames
                if (playerTarget.rollFrameCounter != 0) { continue; }
                playerTarget.ApplyDamage(5);
            }
        }

        particleSystem.GetComponent<ParticleSystem>().loop = false;
        particleSystem2.GetComponent<ParticleSystem>().Play();
        pillarAsset.gameObject.SetActive(true);
    }

    public void ApplyDamage(int damage)
    {
        hp -= damage;

        if (hp < 0)
        {
            Destroy(gameObject);
        }
    }
}

