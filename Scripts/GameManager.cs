using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Samet;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using JetBrains.Annotations;

public class GameManager : MonoBehaviour
{

   
    public static int AnlikKarakterSayisi = 1;
    public List<GameObject> Karakterler;
    public List<GameObject> OlusmaEfektleri;
    public List<GameObject> YokOlmaEfektleri;
    public List<GameObject> AdamLekesiEfektleri;



    [Header("LEVEL VERİLERİ")]
    public List<GameObject> Dusmanlar;
    public int KacDusmanOlsun;
    public GameObject _AnaKarakter;
    
    public bool OyunBittimi;
    bool SonaGeldikmi;

    [Header("SAPKALAR")]
    public GameObject[] Sapkalar;

    [Header("SOPALAR")]
    public GameObject[] Sopalar;

    [Header("MATERYALLER")]
    public Material[] Materyaller;
    public SkinnedMeshRenderer  _Renderer;

    Matematiksel_islemler _Matematiksel_islemler = new Matematiksel_islemler();
    BellekYonetim _BellekYonetim = new BellekYonetim();

    Scene _Scene;

    [Header("GENEL")]

    public AudioSource OyunSes;
    public AudioSource ButonSes;
    public Slider OyunSesi;
    public GameObject YuklemeEkrani;
    public GameObject[] Paneller;


    private void Awake()
    {
        OyunSes.volume = _BellekYonetim.VeriOku_f("OyunSes");
        Destroy(GameObject.FindWithTag("MenuMusic"));
        ItemleriKontrolEt();
    }
    void Start()
    {
        DusmanlariOlustur();
        _Scene = SceneManager.GetActiveScene();
        // Debug.Log("" + GameObject.FindGameObjectsWithTag("Player").Length);
    }
    void Update()
    {
        AnlikKarakterSayisiniGuncelle();
    }
    public void DusmanlariOlustur(){
        for (int i = 0; i < KacDusmanOlsun; i++)
        {
            Dusmanlar[i].SetActive(true); 
        }
    }

    public void DusmanlariTetikle()
    {
        foreach (var item in Dusmanlar)
        {
            if (item.activeInHierarchy)
            {
              item.GetComponent<Dusman>().Animasyontetikle();
            }
        }    
        SonaGeldikmi = true; 
        AnlikKarakterSayisiniGuncelle();
        SavasDurumu();
        
       
    }
   


    public void AnlikKarakterSayisiniGuncelle()
    {
        int sahnedekiGercekKarakterSayisi = 1;

        foreach (var item in Karakterler)
        {
            if (item.activeInHierarchy) // Eğer karakter sahnedeyse
            {
                sahnedekiGercekKarakterSayisi++;
            }
        }

        AnlikKarakterSayisi = sahnedekiGercekKarakterSayisi;
         

    }

    public void DusmanSayisiniGuncelle()
    {
        int sahnedekiGercekDusmanSayisi = 0;

        foreach (var item in Dusmanlar)
        {
            if (item.activeInHierarchy) // Eğer karakter sahnedeyse
            {
                sahnedekiGercekDusmanSayisi++;
            }
        }
        KacDusmanOlsun = sahnedekiGercekDusmanSayisi;

    }


    public void SavasDurumu()
    {
        if (SonaGeldikmi)
        {
           AnlikKarakterSayisiniGuncelle();
           DusmanSayisiniGuncelle();
         Debug.Log("AnlikKarakterSayisi: " + AnlikKarakterSayisi + " / KacDusmanOlsun: " + KacDusmanOlsun);
            if (AnlikKarakterSayisi == 1 || KacDusmanOlsun <= 1  || AnlikKarakterSayisi == 2)

            {
                OyunBittimi = true;
                foreach (var item in Dusmanlar)
                {
                    if (item.activeInHierarchy)
                    {
                        item.GetComponent<Animator>().SetBool("Saldir", false);
                    }
                }
                foreach (var item in Karakterler)
                {
                    if (item.activeInHierarchy)
                    {
                        item.GetComponent<Animator>().SetBool("Saldir", false);
                    }
                }

                _AnaKarakter.GetComponent<Animator>().SetBool("Saldir", false);



                if (AnlikKarakterSayisi <= KacDusmanOlsun)
            {
                Debug.Log("Savaşi Kaybettiniz");
                Paneller[2].SetActive(true);
            }
            else
            {
                     if (AnlikKarakterSayisi > 5)
                    {
                        if(_Scene.buildIndex == _BellekYonetim.VeriOku_i("SonLevel")) 
                        {
                            _BellekYonetim.VeriKaydet_int("Puan", _BellekYonetim.VeriOku_i("Puan") + 50);
                            _BellekYonetim.VeriKaydet_int("SonLevel", _BellekYonetim.VeriOku_i("SonLevel") + 1);
                        }
                    }
                    else
                    {
                        if(_Scene.buildIndex == _BellekYonetim.VeriOku_i("SonLevel")) 
                        {
                        _BellekYonetim.VeriKaydet_int("Puan", _BellekYonetim.VeriOku_i("Puan") + 0);
                        _BellekYonetim.VeriKaydet_int("SonLevel", _BellekYonetim.VeriOku_i("SonLevel") + 1);
                        }
                    }   

                    Debug.Log("Savaşi Kazandiniz");
                    Paneller[1].SetActive(true);
            }
        }
        }
    }


    public void AdamYonetimi(string islemturu, int GelenSayi, Transform Pozisyon)
    {
        switch (islemturu)
        {
            case "Carpma":
                _Matematiksel_islemler.Carpma(GelenSayi, Karakterler, Pozisyon,OlusmaEfektleri);

                break;


            case "Toplama":
                _Matematiksel_islemler.Toplama(GelenSayi, Karakterler, Pozisyon,OlusmaEfektleri);


                break;

            case "Cikartma":
                _Matematiksel_islemler.Cikartma(GelenSayi, Karakterler,YokOlmaEfektleri);


                break;


            case "Bolme":
                _Matematiksel_islemler.Bolme(GelenSayi, Karakterler,YokOlmaEfektleri);

                break;

            


        }
    }


    public void YokOlmaEfektiOlustur(Vector3 Pozisyon, bool Balyoz=false, bool Durum=false)
    {
        foreach (var item in YokOlmaEfektleri)
        {
            if (!item.activeInHierarchy)
            {
                item.SetActive(true);
                item.transform.position = Pozisyon;
                item.GetComponent<ParticleSystem>().Play();
                item.GetComponent<AudioSource>().Play();
                if(!Durum)
                AnlikKarakterSayisi--;
                else 
                KacDusmanOlsun--;
                break;
                
                

            }
        }
        
        if (Balyoz)
        {
           Vector3 yeniPoz = new Vector3(Pozisyon.x, .005f, Pozisyon.z);

            foreach (var item in AdamLekesiEfektleri)
        {
            if (!item.activeInHierarchy)
            {
                item.SetActive(true);
                item.transform.position = yeniPoz ;
                item.GetComponent<AudioSource>().Play();
                
                break;
                
                

            }
        }

        }
       if (!OyunBittimi)
            SavasDurumu();
    }

    public void ItemleriKontrolEt()
    {
        if (_BellekYonetim.VeriOku_i("AktifSapka") != -1)
            Sapkalar[_BellekYonetim.VeriOku_i("AktifSapka")].SetActive(true);
       
        if (_BellekYonetim.VeriOku_i("AktifSopa") != -1)
            Sopalar[_BellekYonetim.VeriOku_i("AktifSopa")].SetActive(true);
       
            if (_BellekYonetim.VeriOku_i("AktifTema") != -1)
            {
                Material[] mats = _Renderer.materials;
                    mats[0] = Materyaller[_BellekYonetim.VeriOku_i("AktifTema")];
                    _Renderer.materials = mats;
            }
            else
            {
                   Material[] mats = _Renderer.materials;
                    mats[0] = Materyaller[0];
                    _Renderer.materials = mats;
            }
           


    }

    public void OyunİciAyarlar(string islem)
    {
        if (islem == "OyunMenu")
        {
            ButonSes.Play();
            Time.timeScale = 0f; // Oyunu duraklat
            Paneller[0].SetActive(true);
        }
        else if (islem == "OyunDevam")
        {
            ButonSes.Play();
            Time.timeScale = 1f;
            Paneller[0].SetActive(false);
        }
        else if (islem == "OyunCikis")
        {
            ButonSes.Play();
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
            AnlikKarakterSayisi = 1;

        }
        else if (islem == "OyunTekrar")
        {
            ButonSes.Play();
            Time.timeScale = 1f;
            SceneManager.LoadScene(_Scene.buildIndex);
            AnlikKarakterSayisi = 1;

        }
        else if (islem == "OyunSes")
        {
            OyunSes.volume = OyunSesi.value;
            _BellekYonetim.VeriKaydet_float("OyunSes", OyunSesi.value);
        }

    }

    



    public void SonrakiLevel()
    {
        ButonSes.Play();
        Time.timeScale = 1f;
        StartCoroutine(LoadAsync(_Scene.buildIndex + 1));
        Debug.Log("Sonraki Level");
        AnlikKarakterSayisi = 1;

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
   

   
}
