using System.Collections;
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
        Time.timeScale = 1f;
        Debug.Log("Transision Started");
        transitionAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(transisionTime + 1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        transitionAnimator.SetTrigger("End");
    }
}
