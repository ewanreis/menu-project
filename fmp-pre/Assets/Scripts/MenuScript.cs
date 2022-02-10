using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class MenuScript : MonoBehaviour
{
    private bool buttonAvailable = true, isCreepy = false;
    private int level = 1, pIsFullscreen, pGraphicsPreset;
    private float pMasterVolume;

    public Slider volumeSlider;
    public GameObject settingsMenu, howToPlayScreen;

    public AudioClip[] clips;
    public AudioSource source;
    public AudioMixer audioMixer;

    enum Sounds
    {
        continueHover, // 0
        continueClick, // 1
        clickDeny, // 2
        startClick, // 3
        hoverDeny, // 4
        startHover, // 5
        quitClick, // 6
        menuMusic, // 7
        menuMusicCreepy, // 8
        genericClick // 9
    }

    void Start() 
    {
        Cursor.lockState = CursorLockMode.None;
        pMasterVolume = PlayerPrefs.GetFloat("playerMasterVolume",0);
        pIsFullscreen = PlayerPrefs.GetInt("playerFullscreen",0);
        Screen.fullScreen = (pIsFullscreen == 0) ? false : true;
        audioMixer.SetFloat("masterVolume",pMasterVolume);
        volumeSlider.value = pMasterVolume;
        settingsMenu.SetActive(false);
        howToPlayScreen.SetActive(false);
        Sounds music = (isCreepy) ? Sounds.menuMusicCreepy : Sounds.menuMusic;
        source.PlayOneShot(clips[(int)music],0.2f);
    }

    public void ContinueGame() 
    {
        if (buttonAvailable)
        {
            source.PlayOneShot(clips[(int)Sounds.continueClick]);
            Invoke("LoadScene", 1.5f);
        }
        else
            source.PlayOneShot(clips[(int)Sounds.clickDeny]);
    }

    public void StartGame() 
    {
        level = 2;
        source.PlayOneShot(clips[(int)Sounds.startClick]);
        Invoke("LoadScene", 1.5f);
    }

    public void QuitGame()
    {
        source.PlayOneShot(clips[(int)Sounds.quitClick]);
        Invoke("EndGame", 1f);
    }

    public void SetVolume(float volume) 
    {
        PlayerPrefs.SetFloat("playerMasterVolume", volume);
        audioMixer.SetFloat("masterVolume",volume);
    }

    public void SetGraphicsPreset(int preset) => print(preset);
    public void SetFullscreen(bool isFullscreen) => Screen.fullScreen = isFullscreen;
    public void ContinueHover() => source.PlayOneShot(clips[(buttonAvailable) ? (int)Sounds.continueHover : (int)Sounds.hoverDeny]);
    public void StartHover() => source.PlayOneShot(clips[(int)Sounds.startHover]);
    public void GenericClick() => source.PlayOneShot(clips[(int)Sounds.genericClick]);

    private void EndGame() => Application.Quit();
    private void LoadScene() => SceneManager.LoadScene(level); 
}