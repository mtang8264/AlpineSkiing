using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    [Header("Input Controls")]
    public Controls controls;
    public KeyCode down, up, right, left, action;

    [Header("Game Information")]
    public State state;
    public Direction direction = Direction.RIGHT;
    public float speed;
    public float horizontalMultiplier;
    public float actionMultiplier;

    [Header("Horizontal Specific Variables")]
    public bool crossSkiing;
    public float crossSkiTime;
    public float crossSkiDuration;

    [Header("Ski Variables")]
    public bool skiinng = false;
    public int angle = 0;
    public int lastAngle = 1;
    public bool bombing = false;
    public bool stopping = false;
    public float stopTimer;
    public float stopTime;

    [Header("Sprites")]
    public SkiSprite[] skiSprites;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        switch(controls)
        {
            case Controls.P1:
                down = KeyCode.DownArrow;
                up = KeyCode.UpArrow;
                left = KeyCode.LeftArrow;
                right = KeyCode.RightArrow;
                action = KeyCode.RightShift;
                break;
            case Controls.P2:
                down = KeyCode.F;
                up = KeyCode.R;
                left = KeyCode.D;
                right = KeyCode.G;
                action = KeyCode.W;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case State.HORIZONTAL:
                if(crossSkiing == false)
                {
                    if(Input.GetKey(right))
                    {
                        direction = Direction.RIGHT;
                        crossSkiing = true;
                        crossSkiTime = Time.time;
                    }
                    else if(Input.GetKey(left))
                    {
                        direction = Direction.LEFT;
                        crossSkiing = true;
                        crossSkiTime = Time.time;
                    }
                }
                else
                {
                    if(Time.time > crossSkiTime + crossSkiDuration)
                    {
                        crossSkiing = false;
                    }
                    else
                    {
                        switch(direction)
                        {
                            case Direction.RIGHT:
                                rb.MovePosition(transform.position + new Vector3(speed * horizontalMultiplier * Time.deltaTime,0));
                                break;
                            case Direction.LEFT:
                                rb.MovePosition(transform.position - new Vector3(speed * horizontalMultiplier * Time.deltaTime, 0));
                                break;
                        }
                    }
                }
                break;
            case State.WAIT:
                break;
            case State.SKI:
                if(!skiinng)
                {
                    if (crossSkiing == false)
                    {
                        if (Input.GetKey(right))
                        {
                            direction = Direction.RIGHT;
                            crossSkiing = true;
                            crossSkiTime = Time.time;
                        }
                        else if (Input.GetKey(left))
                        {
                            direction = Direction.LEFT;
                            crossSkiing = true;
                            crossSkiTime = Time.time;
                        }
                    }
                    else
                    {
                        if (Time.time > crossSkiTime + crossSkiDuration)
                        {
                            crossSkiing = false;
                        }
                        else
                        {
                            switch (direction)
                            {
                                case Direction.RIGHT:
                                    rb.MovePosition(transform.position + new Vector3(speed * horizontalMultiplier * Time.deltaTime, 0));
                                    break;
                                case Direction.LEFT:
                                    rb.MovePosition(transform.position - new Vector3(speed * horizontalMultiplier * Time.deltaTime, 0));
                                    break;
                            }
                        }
                    }

                    if (Input.GetKey(down))
                    {
                        skiinng = true;
                        angle = lastAngle;
                    }
                }
                else
                {
                    if (Input.GetKey(right) && !Input.GetKey(left))
                    {
                        angle = 1;
                        lastAngle = 1;
                    }
                    else if (Input.GetKey(left) && !Input.GetKey(right))
                    {
                        angle = -1;
                        lastAngle = -1;
                    }
                }

                bombing = Input.GetKey(down) && !Input.GetKey(up) && !Input.GetKey(left) && !Input.GetKey(right) ? true : false;

                if(Input.GetKey(up))
                {
                    rb.MovePosition(transform.position - new Vector3(0, speed * Time.deltaTime));
                    if(!stopping)
                    {
                        stopTimer = 0f;
                    }
                    stopping = true;
                }
                else
                {
                    stopping = false;
                }

                if (!stopping)
                {
                    if (bombing)
                    {
                        rb.MovePosition(transform.position + new Vector3(0, -speed * Time.deltaTime));
                    }
                    else if (angle == 1)
                    {
                        if (Input.GetKey(right) && !Input.GetKey(left))
                        {
                            rb.MovePosition(transform.position + new Vector3(speed * horizontalMultiplier * Time.deltaTime, -speed * Time.deltaTime));
                        }
                        else
                        {
                            rb.MovePosition(transform.position + new Vector3(speed * horizontalMultiplier * Time.deltaTime * horizontalMultiplier, -speed * Time.deltaTime));
                        }
                    }
                    else if (angle == -1)
                    {
                        if (Input.GetKey(left) && !Input.GetKey(right))
                        {
                            rb.MovePosition(transform.position + new Vector3(-1 * speed * horizontalMultiplier * Time.deltaTime, -speed * Time.deltaTime));
                        }
                        else
                        {
                            rb.MovePosition(transform.position + new Vector3(-1 * speed * horizontalMultiplier * Time.deltaTime * horizontalMultiplier, -speed * Time.deltaTime));
                        }
                    }
                    break;
                }
                else
                {
                    stopTimer += Time.deltaTime;
                    if(stopTime <= stopTimer)
                    {
                        angle = 0;
                        skiinng = false;
                    }
                }
                break;
        }

        SpriteUpdate();
    }

    public void SpriteUpdate()
    {
        if(!skiinng)
        {
            if (crossSkiing)
            {
                switch (direction)
                {
                    case Direction.RIGHT:
                        spriteRenderer.sprite = GetSprite("CrossHillRight");
                        break;
                    case Direction.LEFT:
                        spriteRenderer.sprite = GetSprite("CrossHillLeft");
                        break;
                }
            }
            else
            {
                switch (direction)
                {
                    case Direction.RIGHT:
                        spriteRenderer.sprite = GetSprite("Idle");
                        break;
                    case Direction.LEFT:
                        spriteRenderer.sprite = GetSprite("Idle");
                        break;
                }
            }
        }
        else
        {
            if(bombing)
            {
                spriteRenderer.sprite = GetSprite("SkiDown");
            }
            else if(angle == 1)
            {
                if(Input.GetKey(right) && !Input.GetKey(left))
                {
                    spriteRenderer.sprite = GetSprite("SkiRight");
                }
                else
                {
                    spriteRenderer.sprite = GetSprite("SkiDownRight");
                }
            }
            else if (angle == -1)
            {
                if (Input.GetKey(left) && !Input.GetKey(right))
                {
                    spriteRenderer.sprite = GetSprite("SkiLeft");
                }
                else
                {
                    spriteRenderer.sprite = GetSprite("SkiDownLeft");
                }
            }
        }

        if(stopping)
        {
            if(direction == Direction.LEFT)
            {
                spriteRenderer.sprite = GetSprite("CrossHillLeft");
            }
            else if(direction == Direction.RIGHT)
            {
                spriteRenderer.sprite = GetSprite("CrossHillRight");
            }
        }

        if(!skiinng && !crossSkiing)
        {
            spriteRenderer.sprite = GetSprite("Idle");
        }
        if(state == State.WAIT)
        {
            spriteRenderer.sprite = GetSprite("Wait");
        }

        if(spriteRenderer.sprite == GetSprite("Idle") && direction == Direction.LEFT)
        {
            transform.localScale = new Vector3(-1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1);
        }
    }

    public enum Controls { P1, P2, CUSTOM };
    public enum Direction { LEFT, RIGHT };
    public enum State { HORIZONTAL, WAIT, SKI, FINISH};

    public Sprite GetSprite(string n)
    {
        for (int i = 0; i < skiSprites.Length; i ++)
        {
            if(skiSprites[i].name.ToLower() == n.ToLower())
            {
                return skiSprites[i].sprite;
            }
        }
        return null;
    }
}

[System.Serializable]
public class SkiSprite
{
    public string name;
    public Sprite sprite;
}