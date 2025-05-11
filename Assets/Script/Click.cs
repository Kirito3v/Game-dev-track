using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Click : MonoBehaviour
{
    public void ChangeScene(int scene) 
    {
        SceneManager.LoadScene(scene);
    }
}
