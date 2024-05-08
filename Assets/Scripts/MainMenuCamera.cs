using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour {
    public float speed;
    public float cameraPauseTime;

    public Transform topPoint;
    public Transform bottomPoint;
    private Transform endPoint;

    private float timer;

    // Update is called once per frame
    void Start () {
        endPoint = topPoint;
        timer = cameraPauseTime;
    }

    void Update() {
        timer += Time.deltaTime;

        if (timer >= cameraPauseTime) {
            transform.position = Vector3.MoveTowards(transform.position, endPoint.position, speed * Time.deltaTime);

            if (transform.position.y >= topPoint.position.y) {
                endPoint = bottomPoint;
                timer = 0;
            }
            else if (transform.position.y <= bottomPoint.position.y) {
                endPoint = topPoint;
                timer = 0;
            }
        }
    }

    IEnumerator StallCamera() {
        yield return new WaitForSeconds(cameraPauseTime);
    }
}
