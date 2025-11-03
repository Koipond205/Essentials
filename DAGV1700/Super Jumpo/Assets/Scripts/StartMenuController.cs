using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartMenuController : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    public void OnNewGameClick()
    {
        LoadMapScene();
    }

    public void LoadMapScene()
    {
        StartCoroutine(LoadMap());
    }

    IEnumerator LoadMap()
    {
        //Play animation
        transition.SetTrigger("Start");
        //Wait
        yield return new WaitForSeconds(transitionTime);
        //Load scene
        SceneManager.LoadScene("MapScreen");
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
