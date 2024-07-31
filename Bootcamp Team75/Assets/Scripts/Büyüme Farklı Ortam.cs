using UnityEngine;

public class FlowersGrowth : MonoBehaviour
{
    public GameObject[] flowerStages; // �i�ek a�amalar�n� inspectorda atay�n (4 a�ama bekleniyor)
    public float[] growthTimes; // B�y�me ge�i� s�relerini inspectorda atay�n (3 s�re bekleniyor)
    private int currentStage = 0;
    private float growthTimer = 0f;
    public Transform sunnyZone; // G�ne�li b�lgeyi inspectorda atay�n
    private bool isInSunnyZone = false;
    private bool previouslyInSunnyZone = false;
    private float sunnyMultiplier = 3f;
    private Quaternion startRotation;
    private bool isGrowthPaused = false; // B�y�meyi duraklatmak i�in yeni bayrak
    public bool is2D = false; // 2D

    void Start()
    {
        if (is2D)
        {
            startRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            startRotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y, Camera.main.transform.rotation.eulerAngles.z);
        }

        // growthTimes dizisinin do�ru say�da eleman i�erdi�inden emin olun
        if (growthTimes.Length != flowerStages.Length - 1)
        {
            Debug.LogError("B�y�me s�releri dizisi, �i�ek a�amalar� dizisinden bir eksik element i�ermelidir!");
            return;
        }

        // �lk tohum a�amas�n� instantiate et
        Instantiate(flowerStages[currentStage], transform.position, startRotation, transform);
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
            // B�y�me zamanlay�c�s�n� g�ncelle
            float currentGrowthTime = growthTimes[currentStage];
            if (isInSunnyZone)
            {
                currentGrowthTime /= sunnyMultiplier;
            }

            // B�lgeler aras�nda ge�i� yaparken b�y�me zamanlay�c�s�n� ayarla
            if (isInSunnyZone && !previouslyInSunnyZone)
            {
                growthTimer /= sunnyMultiplier;
            }
            else if (!isInSunnyZone && previouslyInSunnyZone)
            {
                growthTimer *= sunnyMultiplier;
            }

            previouslyInSunnyZone = isInSunnyZone;

            growthTimer += Time.deltaTime;

            // Kalan s�reyi hesapla
            float remainingTime = currentGrowthTime - growthTimer;

            // Kalan s�reyi debug i�in logla
            Debug.Log($"Kalan S�re: {Mathf.CeilToInt(remainingTime)} saniye");

            if (growthTimer >= currentGrowthTime)
            {
                // Bir sonraki a�amaya ge�
                Grow();
                growthTimer = 0f; // Zamanlay�c�y� s�f�rla
            }
        }
    }

    public void Grow()
    {
        // Mevcut a�ama GameObject'ini yok et
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // A�amay� art�r
        currentStage++;

        // Bir sonraki a�amay� instantiate et
        if (currentStage < flowerStages.Length)
        {
            Instantiate(flowerStages[currentStage], transform.position, startRotation, transform);
        }
    }

    public void AdjustGrowthTimer(float transitionFactor)
    {
        growthTimer *= transitionFactor;
    }

    // G�r�n�m� s�f�rlamak i�in bu y�ntemi ekleyin
    public void ResetAppearance()
    {
        // Mevcut a�ama GameObject'ini yok et
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Mevcut a�amay� instantiate et
        if (currentStage < flowerStages.Length)
        {
            Instantiate(flowerStages[currentStage], transform.position, startRotation, transform);
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
        isGrowthPaused = true;
    }

    public void ResumeGrowth()
    {
        isGrowthPaused = false;
    }
}
