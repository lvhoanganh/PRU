using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private List<Sprite> ControlSprite;
    [SerializeField] private AudioSource effectAudioSource;
    [SerializeField] private AudioSource defaultAudioSource;
    [SerializeField] private AudioSource bossAudioSource;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip reLoadClip;
    [SerializeField] private AudioClip energyClip;
    [SerializeField] private AudioClip Boom;
    [SerializeField] private GameObject Mute;
    [SerializeField] private GameObject Volume;
    private bool isMute = false;
    private GameManager gameManager;
    void Awake()
    {
        // Tìm GameUI trong scene
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogWarning("GameUI not found in the scene!");
        }
    }
    public void Start()
    {
        Mute.SetActive(true);
        Volume.SetActive(false);
    }
    public void playShootSound()
    {
        if (!isMute)
        {
            effectAudioSource.PlayOneShot(shootClip);
        }
    }
    public void playReLoadSound()
    {
        if (!isMute) // Chỉ phát âm thanh nếu isMute = false
        {
            effectAudioSource.PlayOneShot(reLoadClip);
        }
    }
    public void playEnergySound()
    {
        if (!isMute) // Chỉ phát âm thanh nếu isMute = false
        {
            effectAudioSource.PlayOneShot(energyClip);
        }
    }
    public void playExSound()
    {
        if (!isMute) // Chỉ phát âm thanh nếu isMute = false
        {
            effectAudioSource.PlayOneShot(Boom);
        }
    }
    public void playDefaultAudio()
    {
        if (!isMute) // Chỉ phát âm thanh nếu isMute = false
        {
            bossAudioSource.Stop();
            defaultAudioSource.Play();
        }
    }
    public void playBossAudio()
    {
        if (!isMute) // Chỉ phát âm thanh nếu isMute = false
        {
            defaultAudioSource.Stop();
            bossAudioSource.Play();
        }
    }
    public void stopAudioGame()
    {
        bossAudioSource.Stop();
        defaultAudioSource.Stop();
        effectAudioSource.Stop();
        isMute = true;
        Mute.SetActive(false);
        Volume.SetActive(true);
    }

    public void playAudioGame()
    {
        if (gameManager.getBoss())
        {
            bossAudioSource.Play();
            isMute = false;
            Mute.SetActive(true);
            Volume.SetActive(false);
        }
        else
        {
            defaultAudioSource.Play();
            isMute = false;
            Mute.SetActive(true);
            Volume.SetActive(false);
        }
        
    }
}
