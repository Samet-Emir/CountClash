using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Samet;

public class Ayarlar_Manager : MonoBehaviour
{
    public AudioSource Butonses;
    public Slider MenuSes;
    public Slider OyunSes;
    BellekYonetim _BellekYonetim = new BellekYonetim();
    void Start()
    {
        MenuSes.value = _BellekYonetim.VeriOku_f("MenuSes");
        OyunSes.value = _BellekYonetim.VeriOku_f("OyunSes");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SesAyarla(string HangiSes)
    {
        switch (HangiSes)
        {
            case "MenuSes":
               _BellekYonetim.VeriKaydet_float("MenuSes", MenuSes.value);
                break;
            case "OyunSes":
                _BellekYonetim.VeriKaydet_float("OyunSes", OyunSes.value);
                break;
        }
    }
    public void GeriDon()
    {
        Butonses.Play();
        SceneManager.LoadScene(0);
    }
}
