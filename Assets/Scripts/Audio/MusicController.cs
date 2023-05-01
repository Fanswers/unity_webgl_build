using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MusicController : MonoBehaviour
{
    public float fadeTimerDuration = 0.8f;
    public AnimationCurve fadeCurve;
    public AudioClip[] clips;
    private AudioSource source;
    private float maxVolume;
    public float cursor = 0f;
    public bool combatActive;


    private void Start()
    {
        source = GetComponent<AudioSource>();
        maxVolume = source.volume;
    }

    private bool playing = false;
    private float timer = 0f;

    private bool gameBehavior = false;

    private void Awake()
    {
        Bootstraper.instance.gameLoaded += () => gameBehavior = true;
        Bootstraper.instance.gameUnloaded += () => gameBehavior = false;
    }

    private void Update()
    {
        if (!gameBehavior) return;
        source.volume = Mathf.Lerp(0f, maxVolume, fadeCurve.Evaluate(cursor));
        if (cursor == 0f)
        {
            source.Stop();
            if (playing) playing = false;
        }
        else if (!source.isPlaying)
        {
            if (!playing) playing = true;
            source.clip = Select();
            source.Play();
        }

        combatActive = IsCombatActive();
        int dir = combatActive ? 1 : -1;
        float speed = 1 / fadeTimerDuration * Time.deltaTime;
        cursor += speed * dir;
        
        
        if (cursor > 1)
        {
            cursor = 1;
        }
        else if (cursor < 0)
        {
            cursor = 0;
        }
    }



    private int previouslySelected = -1;
    private AudioClip Select()
    {
        if (previouslySelected == -1)
        {
            int roll = Random.Range(0, clips.Length);
            previouslySelected = roll;
            return clips[roll];
        }
        else
        {
            int roll = Random.Range(0, clips.Length);
            while(roll == previouslySelected)
            {
                roll = Random.Range(0, clips.Length);
            }
            previouslySelected = roll;
            return clips[roll];
        }
    }

    private bool IsCombatActive() => TargetLibrary.IsAnyEnemyActive();
}
