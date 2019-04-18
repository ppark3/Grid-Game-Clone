using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject water;
    public float cameraHeight;
    public float cameraWidth;
    public bool startTimer;
    public float totalTimerTime;
    public float waterValue;
    public Vector3 center;
    public Vector3 startPosition;
    public float t;

    public Color dark;
    public Color gray;

    public static int score;
    public Text scoreText;

    public GameObject musicManager;

    public static bool lost;

    // Start is called before the first frame update
    void Start()
    {
        totalTimerTime = 10f;
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        startPosition = new Vector3(0f, -height);
        water.transform.position = startPosition;
        newScaleY(water, height);
        newScaleX(water, width * 1.5f);
        center = new Vector3(0, 0);
        score = 0;
        lost = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (water.transform.position == center)
        {
            lost = true;
            StartCoroutine(FadeOut(musicManager.GetComponent<MusicManager>().audioSource));
        }
        else
        {
            t += Time.deltaTime / totalTimerTime;
            water.transform.position = Vector3.Lerp(startPosition, center, t);
        }
        scoreText.text = "" + score;
    }

    public void newScaleY(GameObject theGameObject, float newSize)
    {
        float size = theGameObject.GetComponent<Renderer>().bounds.size.y;
        Vector3 rescale = theGameObject.transform.localScale;
        rescale.y = newSize * rescale.y / size;
        theGameObject.transform.localScale = rescale;
    }

    public void newScaleX(GameObject theGameObject, float newSize)
    {
        float size = theGameObject.GetComponent<Renderer>().bounds.size.x;
        Vector3 rescale = theGameObject.transform.localScale;
        rescale.x = newSize * rescale.x / size;
        theGameObject.transform.localScale = rescale;
    }

    public void Restart()
    {
        water.transform.position = startPosition;
        t = 0;
    }

    public static IEnumerator FadeOut(AudioSource audioSource)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("GameOver");
    }
}
