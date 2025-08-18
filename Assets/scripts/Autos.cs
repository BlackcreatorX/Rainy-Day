using UnityEngine;
using UnityEngine.SceneManagement;

public class Autos : MonoBehaviour
{
    [SerializeField] private string carTag = "Auto"; // Etiqueta de los objetos auto
    [SerializeField] private string AguaTag = "Agua"; // Etiqueta de los objetos agua


   
    
    // Método llamado cuando ocurre una colisión
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar si el objeto con el que colisionamos tiene la etiqueta de auto
        if (collision.gameObject.CompareTag(carTag))
        {
            GameOver();
        }
        
     if (collision.gameObject.CompareTag(AguaTag))
        {
            GameOver();
        }
    }

    // Método para manejar el Game Over
    private void GameOver()
    {
        Destroy(this.gameObject);
        // volver a cargar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}