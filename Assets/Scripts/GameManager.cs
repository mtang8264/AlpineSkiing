using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Information about this game")]
    public GameModes gameMode;
    public bool begun = false;
    public Player p1, p2;

    [Header("Information for the timer")]
    public GameObject scorePrefab;
    float secretTime;
    public float waitTime;
    public float timerStart;
    public bool timer;

    [Header("Who has finished?")]
    public bool p1Finished, p2Finished;

    TextMeshPro p1Time;
    TextMeshPro p2Time;
    TextMeshPro highscore;

    // Start is called before the first frame update
    void Start()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(begun)
        {
            if(Time.time - secretTime > waitTime && !timer)
            {
                timerStart = Time.time;
                timer = true;
                begun = false;
                p1.state = Player.State.SKI;
                p2.state = Player.State.SKI;
            }
        }

        if(timer)
        {
            if(p1Finished == false)
            {
                p1Time.text = "" + (Mathf.Floor((Time.time - timerStart) * 10) / 10);
                if(Time.time - timerStart <10)
                {
                    p1Time.text = "0" + p1Time.text;
                }

                if(p1Time.text.Length <= 3)
                {
                    p1Time.text += ".0";
                }
            }
            if (p2Finished == false)
            {
                p2Time.text = "" + (Mathf.Floor((Time.time - timerStart) * 10) / 10);
                if (Time.time - timerStart < 10)
                {
                    p2Time.text = "0" + p2Time.text;
                }

                if (p2Time.text.Length <= 3)
                {
                    p2Time.text += ".0";
                }
            }
        }
    }

    void BeginRace()
    {
        GameObject temp = Instantiate(scorePrefab);
        p1Time = GameObject.Find("p1Time").GetComponent<TextMeshPro>();
        p2Time = GameObject.Find("p2Time").GetComponent<TextMeshPro>();
        highscore = GameObject.Find("HighScore").GetComponent<TextMeshPro>();
        secretTime = Time.time;
        begun = true;

        p1.state = Player.State.WAIT;
        p2.state = Player.State.WAIT;
    }

    public enum GameModes {UNDECIDED, SLALOM, GIANT_SLALOM, DOWN_HILL}
}
