using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Samet;
using UnityEngine.UIElements;


public class AnaMenu_Manager : MonoBehaviour
{

    BellekYonetim _BellekYonetimi = new BellekYonetim();
    VeriYonetimi _VeriYonetimi = new VeriYonetimi();
    public GameObject CikisPaneli;
    public List<ItemBilgileri> _ItemBilgileri = new List<ItemBilgileri>();
    public AudioSource ButonSes;
    public GameObject YuklemeEkrani;
    void Start()
    {
        _BellekYonetimi.KontrolEtveTanimla();
       // _VeriYonetimi.IlkKurulumDosyaOlusturma(_ItemBilgileri);  
    }

    public void SahneYukle(int Index )
    {
        ButonSes.Play();
        SceneManager.LoadScene(Index);
    }

    public void Oyna()
    {
        ButonSes.Play();
        Time.timeScale = 1f;
        GameManager.AnlikKarakterSayisi = 1;
       // SceneManager.LoadScene(5);
        //_BellekYonetimi.VeriKaydet_int("SonLevel", 5);
       

        StartCoroutine(LoadAsync(_BellekYonetimi.VeriOku_i("SonLevel")));
        
        
    }

    IEnumerator LoadAsync(int SceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneIndex);
        YuklemeEkrani.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log(progress);
            yield return null;
        }
        
    }
   

    public void CikisPanelİslem(string durum)
    {
        ButonSes.Play();
        if (durum == "Evet")
        {
        Application.Quit();
        Debug.Log("Oyun Kapatılıyor...");
        }
        else if (durum == "Cikis")
        CikisPaneli.SetActive(true);
        else
        CikisPaneli.SetActive(false);
    }
    
    
}
