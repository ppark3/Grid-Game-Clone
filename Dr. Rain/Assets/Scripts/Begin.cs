using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Begin : MonoBehaviour
{
    public Text title;
    public Text instruction;

    public AudioClip titleMusic;
    public AudioSource audioSource;

    public float fadeTime;
    public Color fadeOutColor;
    public bool go;             // starts Fade() when true;
    public bool chill;          // keeps Fade() from happening every time Update runs


    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = titleMusic;
        audioSource.Play();
        fadeTime = 3;
        fadeOutColor = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            go = true;
        }

        if (title.color == fadeOutColor)
        {
            SceneManager.LoadScene("Game");
        }

        if (go && !chill)
        {
            StartCoroutine(FadeText());
            StartCoroutine(FadeOut(audioSource, fadeTime));
            chill = true;
        }
    }

    private IEnumerator FadeText()
    {
        for (float t = 0.0f; t < fadeTime; t += Time.deltaTime)
        {
            title.color = Color.Lerp(title.color, fadeOutColor, Mathf.Min(1, t / fadeTime));
            instruction.color = Color.Lerp(instruction.color, fadeOutColor, Mathf.Min(1, t / fadeTime));
            yield return null;
        }
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
