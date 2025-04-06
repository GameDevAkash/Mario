using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSoundManager : MonoBehaviour
{
    public static BackgroundSoundManager instance;
    public AudioClip Mario_Die, Mario_Win;
    public AudioSource MarioCollectionSound;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);

        MarioCollectionSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
