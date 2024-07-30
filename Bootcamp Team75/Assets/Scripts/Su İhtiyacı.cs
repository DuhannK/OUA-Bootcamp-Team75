using UnityEngine;
using System;

public class WateringManager : MonoBehaviour
{
    public GameObject[] driedFlowerStages; // Her b�y�me a�amas�na kar��l�k gelen kurumu� �i�ek a�amalar�
    public GameObject emptyPotPrefab; // Bo� saks� prefab�
    public float timeToDryOut = 86400f; // �i�e�in kurumas� i�in gereken s�re (�rne�in, 24 saat)
    public float timeToEmptyPot = 86400f; // Kuruduktan sonra �i�e�in bo� saks�ya d�n��mesi i�in gereken s�re
    private DateTime lastWateredTime;
    private DateTime driedOutTime;
    private bool isDriedOut = false;
    private bool isEmptyPot = false;

    private FlowersGrowth flowersGrowth;

    void Start()
    {
        flowersGrowth = GetComponent<FlowersGrowth>();

        // PlayerPrefs'den son sulama zaman�n� y�kle
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

        CheckWateringStatus();
    }

    void Update()
    {
        // Sulama durumunu periyodik olarak kontrol et
        CheckWateringStatus();
    }

    public void WaterPlant()
    {
        // Son sulama zaman�n� �u anki zaman olarak g�ncelle
        lastWateredTime = DateTime.Now;
        PlayerPrefs.SetString("LastWateredTime", lastWateredTime.ToString());

        // Kuruma ve bo� saks� durumunu s�f�rla ve �i�ek g�r�n�m�n� g�ncelle
        isDriedOut = false;
        isEmptyPot = false;
        flowersGrowth.ResumeGrowth(); // B�y�me duraklat�lm��sa devam et
        UpdatePlantAppearance();
    }

    private void CheckWateringStatus()
    {
        if (isEmptyPot) return; // �i�ek zaten bo� saks�ysa kontrol� atla

        // Son sulamadan bu yana ge�en s�reyi hesapla
        TimeSpan timeSinceLastWatered = DateTime.Now - lastWateredTime;
        Debug.Log($"Son sulamadan bu yana ge�en s�re: {(int)timeSinceLastWatered.TotalSeconds} saniye");

        if (isDriedOut)
        {
            // �i�ek kuruduktan bu yana ge�en s�reyi hesapla
            TimeSpan timeSinceDriedOut = DateTime.Now - driedOutTime;
            if (timeSinceDriedOut.TotalSeconds >= timeToEmptyPot)
            {
                TurnIntoEmptyPot();
            }
        }
        else
        {
            // E�er ge�en s�re kuruma s�resini a�arsa, bitki durumunu g�ncelle
            if (timeSinceLastWatered.TotalSeconds >= timeToDryOut)
            {
                isDriedOut = true;
                driedOutTime = DateTime.Now;
                flowersGrowth.PauseGrowth(); // �i�ek kurudu�unda b�y�meyi duraklat
                UpdatePlantAppearance();
            }
        }
    }

    private void UpdatePlantAppearance()
    {
        if (isDriedOut)
        {
            // FlowersGrowth scriptinden mevcut a�ama indeksini al
            int currentStage = flowersGrowth.GetCurrentStage();

            // Mevcut a�ama GameObject'ini yok et
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            // Kar��l�k gelen kurumu� �i�ek a�amas�n� instantiate et
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
            Debug.Log("�i�ek kurumad�. Mevcut a�ama g�steriliyor.");
        }
    }

    private void TurnIntoEmptyPot()
    {
        isEmptyPot = true;

        // Mevcut a�ama GameObject'ini yok et
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Bo� saks�y� instantiate et
        Instantiate(emptyPotPrefab, transform.position, Quaternion.identity, transform);
        Debug.Log("�i�ek bo� bir saks�ya d�n��t�.");
    }

    public float GetTimeUntilEmptyPot()
    {
        if (isDriedOut)
        {
            TimeSpan timeSinceDriedOut = DateTime.Now - driedOutTime;
            return (float)(timeToEmptyPot - timeSinceDriedOut.TotalSeconds);
        }
        else
        {
            return timeToEmptyPot;
        }
    }
}
