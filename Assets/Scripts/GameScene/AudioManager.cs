using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class CustomClip
    {
        public AudioClip clip;
        public float start;
        public float end;
    }

    public static AudioManager Instance;

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource voiceSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioClip doorSlam;
    [SerializeField] private AudioClip stamp;
    [SerializeField] private AudioClip textPop;
    [SerializeField] private AudioClip susMeter;

    [SerializeField] private CustomClip[] voiceClips;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(Instance == null)
            Instance = this;

        //SceneManager.sceneLoaded += GameSceneLoaded;

        DontDestroyOnLoad(this);
    }
    void Destroy()
    {
        //SceneManager.sceneLoaded -= GameSceneLoaded;
    }
    /*
    public void GameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "GameScene")
        {
            bgmSource.Play();
            sfxSource.Play();
            sfxSource.PlayOneShot(doorSlam);
        }
    }
    */
    public void PlayGameStart()
    {
        bgmSource.Play();
        sfxSource.Play();
        sfxSource.PlayOneShot(doorSlam);
    }
    public void PlayVoiceSource()
    {
        int index = Random.Range(0,voiceClips.Length);
        AudioClip clip = voiceClips[index].clip;
        float start = voiceClips[index].start;
        float end = voiceClips[index].end;
        StartCoroutine(PlaySegment(voiceSource, clip, start, end));
    }

    IEnumerator PlaySegment(AudioSource source, AudioClip clip, float start, float end)
    {
        float clipLength = clip.length;
        //Debug.Log("Clip Length: " + clipLength);

        start = Mathf.Clamp(start, 0f, clipLength);
        end   = Mathf.Clamp(end, start, clipLength);

        //Debug.Log("Start: " + start + " End: " + end);

        source.clip = clip;
        source.time = start;
        source.Play();

        yield return new WaitForSeconds(end - start);
        
        source.Stop();
    }
    public void PlaySusMeter()
    {
        sfxSource.PlayOneShot(susMeter);
    }
    public void PlayTextPop()
    {
        //float prevVolume = sfxSource.volume;
        sfxSource.volume = 1f;
        sfxSource.PlayOneShot(textPop);
        //sfxSource.volume = prevVolume;
    }
    public void PlayStamp()
    {
        //float prevVolume = sfxSource.volume;
        sfxSource.volume = 1f;
        sfxSource.PlayOneShot(stamp);
        //sfxSource.volume = prevVolume;
    }
    public void StopAudioAfterGame()
    {
        bgmSource.Stop();
        sfxSource.Stop();
    }
    public void DoorSlam()
    {
        sfxSource.PlayOneShot(doorSlam);
    }
}
