using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniMenuController : MonoBehaviour
{
    public GameObject miniMenuUI; // Mini men� UI referans�
    public GameObject hintsBookUI; // �pu�lar� kitab� UI referans�

    // Mini men�y� a�an fonksiyon
    public void OpenMiniMenu()
    {
        miniMenuUI.SetActive(true);
        Time.timeScale = 0f; // Oyunu duraklat
    }

    // Mini men�y� kapatan fonksiyon
    public void CloseMiniMenu()
    {
        miniMenuUI.SetActive(false);
        Time.timeScale = 1f; // Oyunu devam ettir
    }

    // �pu�lar� kitab�n� a�an fonksiyon
    public void OpenHintsBook()
    {
        hintsBookUI.SetActive(true);
        Time.timeScale = 0f; // Oyunu duraklat
    }

    // �pu�lar� kitab�n� kapatan fonksiyon
    public void CloseHintsBook()
    {
        hintsBookUI.SetActive(false);
        Time.timeScale = 1f; // Oyunu devam ettir
    }

    // Ana men�ye d�n fonksiyonu
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Oyunu devam ettir
        SceneManager.LoadScene("Menu"); // Ana men� sahnesine d�n
    }
}
