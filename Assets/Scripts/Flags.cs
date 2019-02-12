using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Flags : MonoBehaviour
{
    public Player player;

    public GameObject BlueHorizontal;
    public GameObject BlueVertical;
    public GameObject RedHorizontal;
    public GameObject RedVertical;

    public GameObject finish;

    public int numberOfGates = 55;

    public float verticalChance;
    public float firstDistance, minDistance, maxDistance, overrideDistance;
    public float verticalDistanceMultiplier = 2f;

    // Start is called before the first frame update
    void Start()
    {
        bool red = true;
        float distance = - 1 * firstDistance;

        for (int i = 0; i < numberOfGates - 1; i++)
        {
            bool vert = Random.value < verticalChance;
            if (i == 0)
            {
                vert = false;
            }
            else
            {
                if (overrideDistance > 0)
                {
                    distance -= overrideDistance;
                }
                else
                {
                    distance -= Random.Range(vert ? minDistance * verticalDistanceMultiplier : minDistance, vert ? maxDistance * verticalDistanceMultiplier : maxDistance);
                }
            }

            GameObject instance;

            if (red)
            {
                if (vert)
                {
                    instance = Instantiate(RedVertical);
                }
                else
                {
                    instance = Instantiate(RedHorizontal);
                }
            }
            else
            {
                if (vert)
                {
                    instance = Instantiate(BlueVertical);
                }
                else
                {
                    instance = Instantiate(BlueHorizontal);
                }
            }
            instance.GetComponent<Flag>().player = player;
            instance.transform.parent = transform;
            instance.transform.localPosition = new Vector3(Random.Range(-1.66f, 1.31f), distance);

            red = !red;
        }

        distance -= 10;

        GameObject f = Instantiate(finish);
        f.transform.parent = transform;
        f.transform.localPosition = new Vector3(0, distance);
        f.GetComponent<TextMeshPro>().color = player.GetComponent<SpriteRenderer>().color;
        f.GetComponent<Finish>().player = player;
    }

    // Update is called once per frame
    public void LateUpdate()
    {
        if(player.transform.position.y < 0f)
        {
            float offset = player.transform.position.y;
            player.transform.position = new Vector3(player.transform.position.x, 0);
            transform.Translate(0,-offset * 2, 0);
        }
    }
}
