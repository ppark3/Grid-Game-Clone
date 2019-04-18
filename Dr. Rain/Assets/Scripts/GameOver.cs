using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Text text1;
    public Color text1Color;
    public Text text2;
    public Color text2Color;
    public Text text3;
    public Color text3Color;
    public Text score;
    public Color scoreColor;

    public AudioClip endMusic;
    public AudioSource audioSource;

    public float fadeTime;
    public Color fadeOutColor;

    public GameObject soundManager;

    // Start is called before the first frame update
    void Start()
    {
        score.text = "" + GameManager.score;
        audioSource.clip = endMusic;
        audioSource.Play();

        fadeTime = 7;
        fadeOutColor = Color.clear;

        text1.color = fadeOutColor;
        text2.color = fadeOutColor;
        text3.color = fadeOutColor;
        score.color = fadeOutColor;
        StartCoroutine(FadeInText());

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Revive());
        }
    }

    IEnumerator Revive()
    {
        soundManager.SendMessage("PlaySound");
        text1.gameObject.SetActive(false);
        text2.gameObject.SetActive(false);
        text3.gameObject.SetActive(false);
        score.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        text1.gameObject.SetActive(true);
        text2.gameObject.SetActive(true);
        text3.gameObject.SetActive(true);
        score.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        text1.gameObject.SetActive(false);
        text2.gameObject.SetActive(false);
        text3.gameObject.SetActive(false);
        score.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.15f);
        text1.gameObject.SetActive(true);
        text2.gameObject.SetActive(true);
        text3.gameObject.SetActive(true);
        score.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        text1.gameObject.SetActive(false);
        text2.gameObject.SetActive(false);
        text3.gameObject.SetActive(false);
        score.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        text1.gameObject.SetActive(true);
        text2.gameObject.SetActive(true);
        text3.gameObject.SetActive(true);
        score.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Game");
    }

    private IEnumerator FadeInText()
    {
        for (float t = 0.0f; t < fadeTime; t += Time.deltaTime)
        {
            text1.color = Color.Lerp(text1.color, text1Color, Mathf.Min(1, t / fadeTime));
            text2.color = Color.Lerp(text2.color, text2Color, Mathf.Min(1, t / fadeTime));
            text3.color = Color.Lerp(text3.color, text3Color, Mathf.Min(1, t / fadeTime));
            score.color = Color.Lerp(score.color, scoreColor, Mathf.Min(1, t / fadeTime));
            yield return null;
        }
    }
}
