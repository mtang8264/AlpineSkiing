using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetFunction : MonoBehaviour
{
    public KeyCode[] resetKey;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < resetKey.Length; i++)
        {
            if (Input.GetKeyDown(resetKey[i]))
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
