using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroWait : MonoBehaviour
{
    public class Settings
    {
        public float SoundVal;
        public float MusicVal;
        public Resolution Resolution;
    }

    [SerializeField]
    private AudioMixer SoundMixer;
    [SerializeField]
    private AudioMixer MusicMixer;

    public float WaitTime;

    void Awake()
    {
        try
        {
            using StreamReader reader = new StreamReader("Settings.conf");
            XmlSerializer deserializer = new XmlSerializer(typeof(Settings));
            Settings settings = deserializer.Deserialize(reader) as Settings;

            SoundMixer.SetFloat("VolumeMaster", settings.SoundVal);
            MusicMixer.SetFloat("VolumeMaster", settings.MusicVal);
            Screen.SetResolution(settings.Resolution.width, settings.Resolution.height, Screen.fullScreen);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message + "\n Использование дефолтных настроек");
        }

        StartCoroutine(WaitOfVideo());
    }

    private IEnumerator WaitOfVideo()
    {
        transform.GetChild(0).GetComponent<VideoPlayer>().Play();
        yield return new WaitForSeconds(WaitTime);
        SceneManager.LoadScene(1);
    }
}