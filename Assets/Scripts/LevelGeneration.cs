using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{

    public GameObject room;
    public int amountOfRoom = 200;

    List<GameObject> rooms = new List<GameObject>();
    List<int> sizes = new List<int>();

    public Vector4 startColor = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
    public Vector4 endColor = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);

    private long tick = 0l;

    private bool hasFinished = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < amountOfRoom; i++) 
        {
            GameObject temp = Instantiate(room, new Vector3((int) Random.Range(-4.0f, 4.0f), (int) Random.Range(-4.0f, 4.0f), 0), Quaternion.identity);
            temp.GetComponentInChildren<SpriteRenderer>().color = Vector4.Lerp(startColor, endColor, i/(float)amountOfRoom);

            temp.transform.localScale = new Vector3((int) Mathf.Ceil(Random.Range(0.0f, 10.0f)), (int) Mathf.Ceil(Random.Range(0.0f, 10.0f)), 1);
            rooms.Add(temp);
            sizes.Add((int) temp.transform.localScale.x * (int) temp.transform.localScale.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        tick++;
        //if (tick % 20 != 0) return;
        if (!hasFinished) {
            int counter2 = 0;
            for (int i = 0; i < amountOfRoom; ++i) {
                int counter = 0;
                Vector3 sum = rooms[i].transform.position;
                for (int j = 0; j < amountOfRoom; ++j) {
                    if (i == j) continue;
                    if (DoesIntersect(rooms[i], rooms[j]))
                    {
                        counter++;
                        sum += rooms[j].transform.position;
                    }
                }
                if (counter == 0) continue;
                counter2++;
                sum = sum / (counter + 1);
                rooms[i].transform.position -= (sum - rooms[i].transform.position).normalized;
                //rooms[i].transform.position = new Vector3(Mathf.Round(rooms[i].transform.position.x), Mathf.Round(rooms[i].transform.position.y), rooms[i].transform.position.z);
            }
            //Debug.Log(counter2);
            if (counter2 == 0) hasFinished = true;
        } else {
            sizes.Sort();
            for (int i = 0; i < amountOfRoom; ++i) {
                Vector3 roomScale = rooms[i].transform.localScale;
                if (roomScale.x * roomScale.y >= sizes[amountOfRoom - 11]) {
                        rooms[i].GetComponentInChildren<SpriteRenderer>().color = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
                } else {
                        rooms[i].GetComponentInChildren<SpriteRenderer>().color = new Vector4(0.7f, 0.7f, 0.7f, 1.0f);
                }
            }
        }
    }

    bool DoesIntersect(GameObject a, GameObject b)
    {
        float aLeft = a.transform.position.x - a.transform.localScale.x / 2;
        float aRight = a.transform.position.x + a.transform.localScale.x / 2;
        float aTop = a.transform.position.y + a.transform.localScale.y / 2;
        float aBottom = a.transform.position.y - a.transform.localScale.y / 2;

        float bLeft = b.transform.position.x - b.transform.localScale.x / 2;
        float bRight = b.transform.position.x + b.transform.localScale.x / 2;
        float bTop = b.transform.position.y + b.transform.localScale.y / 2;
        float bBottom = b.transform.position.y - b.transform.localScale.y / 2;

        return (aLeft < bRight &&
                aRight > bLeft &&
                aTop > bBottom &&
                aBottom < bTop);
    }
}
