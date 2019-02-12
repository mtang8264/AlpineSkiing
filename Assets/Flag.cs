using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public Player player;
    public bool passed;
    public bool vert;
    public bool vertHappy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!passed)
        {
            if (!vert)
            {
                if (player.transform.position.y < transform.position.y)
                {
                    passed = true;
                    if (player.transform.position.x > transform.position.x - 2 && player.transform.position.x < transform.position.x + 2)
                    {

                    }
                    else
                    {
                        player.SendMessage("Miss");
                    }
                }
            }
            else
            {
                if(player.transform.position.y < transform.position.y - 2)
                {
                    passed = true;
                    if(vertHappy)
                    {

                    }
                    else
                    {
                        player.SendMessage("Miss");
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == player.gameObject.layer)
        {
            vertHappy = true;
        }
    }
}
