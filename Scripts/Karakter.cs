using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Karakter : MonoBehaviour
{
    public GameManager _GameManager;
    public Kamera _Kamera;
    public bool SonaGeldikmi;
    public GameObject Gidecegiyer;

    public Slider _Slider;
    public GameObject GecisNoktasi;
    
  
   
   
   
    private void Start()
    {
         float Fark = Vector3.Distance(transform.position, GecisNoktasi.transform.position);
         _Slider.maxValue = Fark;
         
         

    }
    private void FixedUpdate()
    {
        if (!SonaGeldikmi)
            transform.Translate(Vector3.forward * .5f * Time.deltaTime);
    }
    
   

    void Update()
    {
        if(Time.timeScale!=0){

        if (SonaGeldikmi)
        {
             transform.position = Vector3.Lerp(transform.position, Gidecegiyer.transform.position , .005f);
            if(_Slider.value!=0)
            _Slider.value -= .01f;

        }
        else
        {
            float Fark = Vector3.Distance(transform.position, GecisNoktasi.transform.position);
            _Slider.value = Fark;
            

            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (Input.GetAxis("Mouse X") < 0)
                {
                    transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x - .1f, transform.position.y, transform.position.z), .3f);
                }
                if (Input.GetAxis("Mouse X") > 0)
                {
                    transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x + .1f, transform.position.y, transform.position.z), .3f);
                }
            }
        }

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Carpma") || other.CompareTag("Toplama") || other.CompareTag("Bolme") || other.CompareTag("Cikartma"))
        {
            int sayi = int.Parse(other.name);
            _GameManager.AdamYonetimi(other.tag, sayi, other.transform);
        }

        else if (other.CompareTag("Sontetikleyici"))
        {
            _Kamera.SonaGeldikmi = true;
           _GameManager.DusmanlariTetikle();
            SonaGeldikmi=true;
            
        }
         else if (other.CompareTag("BosKarakter"))
        {
            _GameManager.Karakterler.Add(other.gameObject);
            
            
        }
       

    }
    private void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.CompareTag("Engel_Takilma") || collision.gameObject.CompareTag("Testere"))
        {
            if (transform.position.x < 0)
            transform.position = new Vector3(transform.position.x + 0.4f, transform.position.y, transform.position.z);
            else
            transform.position = new Vector3(transform.position.x - 0.4f, transform.position.y, transform.position.z);
        }
        
    }
}
