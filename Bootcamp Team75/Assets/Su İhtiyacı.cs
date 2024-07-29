using UnityEngine;
using System;

public class WateringManager : MonoBehaviour
{
    public GameObject[] driedFlowerStages; // Her b�y�me a�amas�na kar��l�k gelen kurumu� �i�ek a�amalar�
    public float timeToDryOut = 86.400f; // �i�e�in kurumaya ba�layaca�� s�re (�rne�in, 24 saat)
    private DateTime lastWateredTime; // Son sulama zaman�
    private bool isDriedOut = false; // �i�e�in kurumu� olup olmad���n� belirtir

    private FlowersGrowth flowersGrowth; // FlowersGrowth bile�eni

    void Start()
    {
        flowersGrowth = GetComponent<FlowersGrowth>();

        // PlayerPrefs'ten son sulama zaman�n� y�kle
        if (PlayerPrefs.HasKey("LastWateredTime"))
        {
            string lastWateredTimeStr = PlayerPrefs.GetString("LastWateredTime");
            lastWateredTime = DateTime.Parse(lastWateredTimeStr);
        }
        else
        {
            // Kaydedilmi� bir sulama zaman� yoksa, �u anki zaman� son sulama zaman� olarak ayarla
            lastWateredTime = DateTime.Now;
            PlayerPrefs.SetString("LastWateredTime", lastWateredTime.ToString());
        }

        CheckWateringStatus(); // Sulama durumunu kontrol et
    }

    void Update()
    {
        // Sulama durumunu belirli aral�klarla kontrol et
        CheckWateringStatus();
    }

    public void WaterPlant()
    {
        // Son sulama zaman�n� �u anki zamanla g�ncelle
        lastWateredTime = DateTime.Now;
        PlayerPrefs.SetString("LastWateredTime", lastWateredTime.ToString());

        // Kuruma durumunu s�f�rla ve bitkinin g�r�n�m�n� g�ncelle
        isDriedOut = false;
        UpdatePlantAppearance();
    }

    private void CheckWateringStatus()
    {
        // Son sulamadan bu yana ge�en s�reyi hesapla
        TimeSpan timeSinceLastWatered = DateTime.Now - lastWateredTime;
        Debug.Log($"Son sulamadan bu yana ge�en s�re: {timeSinceLastWatered.TotalSeconds} saniye");

        // Ge�en s�re kurumaya kadar olan s�reyi a�arsa, bitki durumunu g�ncelle
        if (timeSinceLastWatered.TotalSeconds >= timeToDryOut)
        {
            isDriedOut = true;
        }

        UpdatePlantAppearance(); // Bitkinin g�r�n�m�n� g�ncelle
    }

    private void UpdatePlantAppearance()
    {
        if (isDriedOut)
        {
            // FlowersGrowth script'inden mevcut a�ama indeksini al
            int currentStage = flowersGrowth.GetCurrentStage();

            // Mevcut a�ama GameObject'ini yok et
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            // Kar��l�k gelen kurumu� �i�ek a�amas�n� olu�tur
            if (currentStage < driedFlowerStages.Length)
            {
                Instantiate(driedFlowerStages[currentStage], transform.position, Quaternion.identity, transform);
                Debug.Log("�i�ek kurudu. Kurumu� a�ama g�steriliyor.");
            }
        }
        else
        {
            // Mevcut b�y�me a�amas�n�n g�r�n�m�n� s�f�rla
            flowersGrowth.ResetAppearance();
            Debug.Log("�i�ek kurumam��. Mevcut a�ama g�steriliyor.");
        }
    }
}
