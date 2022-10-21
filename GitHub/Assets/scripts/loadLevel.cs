using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadLevel : MonoBehaviour
{
    public void loadlvl (int levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
