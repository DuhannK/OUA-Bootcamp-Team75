using UnityEngine;
using UnityEngine.UI;
using System;

public class FlowersGrowth : MonoBehaviour
{
    private Quaternion startRotation;
    public Sprite[] flowerStages; // �i�ek a�amalar�n�n sprite'lar�n� inspectorda atay�n (4 a�ama bekleniyor)
    public float[] growthTimes; // B�y�me ge�i� s�relerini inspectorda atay�n (3 s�re bekleniyor)
    private int currentStage = 0;
    private DateTime startTime;
    public Transform sunnyZone; // G�ne�li b�lgeyi inspectorda atay�n
    private bool isInSunnyZone = false;
    private float sunnyMultiplier = 3f;
    private bool isGrowthPaused = false; // B�y�meyi duraklatmak i�in yeni bayrak
    private Image flowerImage; // �i�e�in image bile�eni
    private TimeSpan pausedDuration = TimeSpan.Zero; // B�y�me duraklatma s�resi
    private DateTime? pauseStartTime = null; // B�y�me duraklat�ld���nda zaman� kaydetmek i�in de�i�ken
    public bool is2dFlower = false; // 2D �i�ekler i�in bayrak
    private SpriteRenderer spriteRenderer; // 3D �i�ekler i�in spriteRenderer bile�eni
    void Start()
    {
        // growthTimes dizisinin do�ru say�da eleman i�erdi�inden emin olun
        if (growthTimes.Length != flowerStages.Length - 1)
        {
            Debug.LogError("B�y�me s�releri dizisi, �i�ek a�amalar� dizisinden bir eksik element i�ermelidir!");
            return;
        }

        // Ba�lang�� zaman�n� ayarla
        startTime = DateTime.Now;

        // �lk tohum a�amas�n� ayarla
        if(is2dFlower)
        {
            startRotation = Quaternion.Euler(0f, 0f, 0f);

            flowerImage = GetComponent<Image>();
            flowerImage.sprite = flowerStages[currentStage];
            flowerImage.preserveAspect = true;
        }
        else
        {

            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y, Camera.main.transform.rotation.eulerAngles.z));

            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = flowerStages[currentStage];
        }
    }

    void Update()
    {
        if (isGrowthPaused) return; // B�y�me duraklat�lm��sa update'i atla

        if (sunnyZone != null)
        {
            // �i�e�in g�ne�li b�lgede olup olmad���n� kontrol et
            isInSunnyZone = Vector3.Distance(transform.position, sunnyZone.position) < sunnyZone.localScale.x / 2;
        }

        if (currentStage < growthTimes.Length)
        {
            // Toplam b�y�me s�resini hesapla
            TimeSpan totalGrowthTime = TimeSpan.FromSeconds(growthTimes[currentStage]);
            if (isInSunnyZone)
            {
                totalGrowthTime = TimeSpan.FromSeconds(growthTimes[currentStage] / sunnyMultiplier);
            }

            // Ge�en s�reyi hesapla
            TimeSpan elapsedTime = (DateTime.Now - startTime) - pausedDuration;

            // Kalan s�reyi hesapla
            TimeSpan remainingTime = totalGrowthTime - elapsedTime;

            // Kalan s�reyi debug i�in logla
            Debug.Log($"Kalan S�re: {Mathf.CeilToInt((float)remainingTime.TotalSeconds)} saniye");

            if (elapsedTime >= totalGrowthTime)
            {
                // Bir sonraki a�amaya ge�
                Grow();
                startTime = DateTime.Now; // Ba�lang�� zaman�n� g�ncelle
                pausedDuration = TimeSpan.Zero; // Duraklama s�resini s�f�rla
            }
        }
    }

    public void Grow()
    {
        // A�amay� art�r
        currentStage++;

        // Bir sonraki a�amay� ayarla
        if (currentStage < flowerStages.Length)
        {
            if (is2dFlower)
            {
                flowerImage.sprite = flowerStages[currentStage];
            }
            else
            {
                spriteRenderer.sprite = flowerStages[currentStage];
            }
        }
    }

    public void AdjustGrowthTimer(float transitionFactor)
    {
        // Ge�erli a�ama i�in b�y�me s�resini ayarla
        growthTimes[currentStage] *= transitionFactor;
    }

    // G�r�n�m� s�f�rlamak i�in bu y�ntemi ekleyin
    public void ResetAppearance()
    {
        // Mevcut a�amay� ayarla
        if (currentStage < flowerStages.Length)
        {
            if (is2dFlower)
            {
                flowerImage.sprite = flowerStages[currentStage];
            }else
            {
                spriteRenderer.sprite = flowerStages[currentStage];
            }
        }
    }

    // Mevcut a�ama indeksini almak i�in bu y�ntemi ekleyin
    public int GetCurrentStage()
    {
        return currentStage;
    }

    // B�y�meyi duraklatmak ve devam ettirmek i�in bu y�ntemleri ekleyin
    public void PauseGrowth()
    {
        if (!isGrowthPaused)
        {
            isGrowthPaused = true;
            pauseStartTime = DateTime.Now;
        }
    }

    public void ResumeGrowth()
    {
        if (isGrowthPaused)
        {
            isGrowthPaused = false;
            if (pauseStartTime.HasValue)
            {
                pausedDuration += DateTime.Now - pauseStartTime.Value;
            }
            pauseStartTime = null;
        }
    }
}
