using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

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
    public float p1End = -1f;
    public float p2End = -1f;

    TextMeshPro p1Time;
    TextMeshPro p2Time;
    TextMeshPro highscore;

    public bool winner;
    public string record = "";

    // Start is called before the first frame update
    void Start()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;

        if(File.Exists(Application.persistentDataPath + "/Record.ski"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Record.ski", FileMode.Open);
            Record r = (Record)bf.Deserialize(file);
            record = r.record;
            winner = r.winner;
        }
    }

    // Update is called once per frame
    void Update()
    {
        p1Finished = p1.finished;
        p2Finished = p2.finished;

        Debug.Log(Time.deltaTime);
        if(highscore != null)
        {
            if(record != "")
            {
                highscore.text = record;
                if(winner)
                {
                    highscore.color = p2.GetComponent<SpriteRenderer>().color;
                }
                else
                {
                    highscore.color = p2.GetComponent<SpriteRenderer>().color;
                }
            }
            else
            {
                highscore.text = "??.?";
                highscore.color = new Color(103f/255f, 103f / 255f, 103f / 255f);
            }
        }

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
            else
            {
                if(p1End < 0)
                {
                    p1End = Time.time - timerStart;
                }
                p1Time.text = "" + (p1End * 10 / 10);
                if (Time.time - timerStart < 10)
                {
                    p1Time.text = "0" + p1Time.text;
                }

                if (p1Time.text.Length <= 3)
                {
                    p1Time.text += ".0";
                }
            }
            if (p2Finished == false)
            {
                p2Time.text = "" + (Mathf.Floor((Time.time - timerStart) * 10) / 10);
                if (p2Time.text.Length <= 3)
                {
                    p2Time.text += ".0";
                }
            }
            else
            {
                if (p2End < 0)
                {
                    p2End = Time.time - timerStart;
                }
                p2Time.text = "" + (p2End * 10 / 10);
                if (p2Time.text.Length <= 3)
                {
                    p2Time.text += ".0";
                }
            }

            if(p1.misses > 0)
            {
                p1Time.text = "- " + p1.misses + " -";
            }
            if(p2.misses > 0)
            {
                p2Time.text = "- " + p2.misses + " -";
            }
        }

        if(p1.finished && p2.finished)
        {
            if(p1.misses == 0 && p2.misses == 0)
            {
                if(p1End < p2End)
                {
                    winner = false;
                    record = "" + p1End;
                }
                else
                {
                    winner = true;
                    record = "" + p2End;
                }
            }
            else if(p1.misses == 0 && p2.misses >0)
            {
                winner = false;
                record = "" + p1End;
            }
            else if(p1.misses > 0 && p2.misses == 0)
            {
                winner = true;
                record = "" + p2End;
            }
        }

        if(p1Finished && p2Finished)
        {
            Record r = new Record(record, winner);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/Record.ski");
            bf.Serialize(file, r);
            file.Close();
            SceneManager.LoadScene(0);
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

[System.Serializable]
public class Record
{
    public string record;
    public bool winner;
    public Record(string r, bool w)
    {
        record = r;
        winner = w;
    }
}