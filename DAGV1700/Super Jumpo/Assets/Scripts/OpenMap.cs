using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class OpenMap : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    public AudioSource musicManager;

    public void MapOpen()
    {
        LoadMapScene();
        musicManager.Pause();
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicManager.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
