using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    public Animator transitionAnimator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator TransisionToScene(int transisionTime,string sceneName)
    {
        Debug.Log("Transision Started");
        transitionAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(transisionTime + 1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
        transitionAnimator.SetTrigger("End");
    }
}
