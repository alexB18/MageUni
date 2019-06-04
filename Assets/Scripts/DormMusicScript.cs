using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DormMusicScript : MonoBehaviour
{
    public AudioClip song1;
    public AudioClip song2;
    public AudioClip song3;
    public AudioClip song4;
    public AudioClip song5;
    public AudioClip song6;
    public AudioClip song7;
    public AudioClip song8;
    public AudioClip song9;
    public AudioClip song10;

    private List<AudioClip> songs = new List<AudioClip>();

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        songs.Add(song1);
        songs.Add(song2);
        songs.Add(song3);
        songs.Add(song4);
        songs.Add(song5);
        songs.Add(song6);
        songs.Add(song7);
        songs.Add(song8);
        songs.Add(song9);
        songs.Add(song10);
        audioSource.clip = songs[Random.Range(0, songs.Count)];
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = songs[Random.Range(0, songs.Count)];
            audioSource.Play();
        }
    }
}
