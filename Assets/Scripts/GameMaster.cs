using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private static GameMaster instance = null;
    public static GameMaster Instance { get { return instance; } }

    private int sceneIndex = 0;

    public void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetLevel();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            NextLevel();
        }
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void NextLevel()
    {
        sceneIndex++;
        SceneManager.LoadScene(sceneIndex);
    }
}
