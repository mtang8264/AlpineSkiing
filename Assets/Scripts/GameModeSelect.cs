using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameModeSelect : MonoBehaviour
{
    public TextMeshPro text;

    string[] strings = { "SLALOM", "GIANT SLALOM", "DOWN HILL"};
    int current = 0;
    int lastSecond = 0;

    bool dying = false;
    float beginTime;
    int count = 0;
    public int flickers = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!dying)
        {
            if ((int)(Time.time / 2) != lastSecond)
            {
                current++;
            }

            if (current >= 3)
            {
                current = 0;
            }

            text.text = strings[current];

            lastSecond = (int)(Time.time / 2);

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                switch (current)
                {
                    case 0:
                        GameManager.instance.gameMode = GameManager.GameModes.SLALOM;
                        break;
                    case 1:
                        GameManager.instance.gameMode = GameManager.GameModes.GIANT_SLALOM;
                        break;
                    case 2:
                        GameManager.instance.gameMode = GameManager.GameModes.DOWN_HILL;
                        break;
                }

                dying = true;
                beginTime = Time.time;
            }
        }
        else
        {
            if((int)((Time.time - beginTime) * 1.5 )%2 != 0)
            {
                if(text.enabled)
                {
                    count++;
                }
                text.enabled = false;
            }
            else
            {
                text.enabled = true;
            }

            if (count > flickers)
            {
                GameManager.instance.SendMessage("BeginRace");
                Destroy(gameObject);
            }
        }
    }
}
