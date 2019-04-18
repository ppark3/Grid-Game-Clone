using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveSound : MonoBehaviour
{
    public AudioClip reviveSound;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlaySound()
    {
        audioSource.clip = reviveSound;
        audioSource.Play();
    }
}
