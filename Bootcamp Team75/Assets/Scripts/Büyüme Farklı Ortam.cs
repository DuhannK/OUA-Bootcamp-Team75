using UnityEngine;

public class FlowersGrowth : MonoBehaviour
{
    public GameObject[] flowerStages; // �i�ek a�amalar�n� denetleyicide atay�n (beklenen 4 a�ama)
    public GameObject[] driedFlowerStages; // Kurumu� �i�ek a�amalar�n� denetleyicide atay�n (beklenen 4 a�ama)
    public float[] growthTimes; // B�y�me ge�i� s�relerini denetleyicide atay�n (beklenen 3 s�re)
    private int currentStage = 0; // Mevcut a�ama indeksi
    private float growthTimer = 0f; // B�y�me zamanlay�c�s�
    public Transform sunnyZone; // G�ne�li b�lgeyi denetleyicide atay�n

    private bool isInSunnyZone = false; // G�ne�li b�lgede olup olmad���n� belirtir
    private bool previouslyInSunnyZone = false; // Daha �nce g�ne�li b�lgede olup olmad���n� belirtir
    private float sunnyMultiplier = 3f; // G�ne�li b�lgede b�y�me h�zland�r�c� katsay�
    private Quaternion startRotation; // Ba�lang�� rotasyonu

    void Start()
    {
        startRotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);

        // growthTimes dizisinin do�ru say�da eleman i�erdi�inden emin ol
        if (growthTimes.Length != flowerStages.Length - 1)
        {
            Debug.LogError("B�y�me s�releri dizisi, �i�ek a�amalar� dizisinden bir eksik eleman i�ermelidir!");
            return;
        }

        // �lk tohum a�amas�n� olu�tur
        Instantiate(flowerStages[currentStage], transform.position, startRotation, transform);
    }

    void Update()
    {
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

            // Debugging i�in kalan s�reyi logla
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

        // Bir sonraki a�amay� olu�tur
        if (currentStage < flowerStages.Length)
        {
            Instantiate(flowerStages[currentStage], transform.position, startRotation, transform);
        }
    }

    public void AdjustGrowthTimer(float transitionFactor)
    {
        growthTimer *= transitionFactor;
    }

    // Bu metodu ekleyerek g�r�n�m� s�f�rla
    public void ResetAppearance()
    {
        // Mevcut a�ama GameObject'ini yok et
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Mevcut a�amay� olu�tur
        if (currentStage < flowerStages.Length)
        {
            Instantiate(flowerStages[currentStage], transform.position, startRotation, transform);
        }
    }

    // Bu metodu ekleyerek mevcut a�ama indeksini al
    public int GetCurrentStage()
    {
        return currentStage;
    }
}
