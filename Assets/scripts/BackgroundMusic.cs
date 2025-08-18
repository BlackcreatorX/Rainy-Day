using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [Header("Configuración de Música")]
    [Tooltip("Arrastra aquí el archivo de audio MP3")]
    public AudioClip musicClip; // El clip de audio que quieres reproducir
    
    [Range(0f, 1f)]
    public float volume = 0.5f; // Volumen ajustable entre 0 y 1
    
    private AudioSource audioSource;

    void Awake()
    {
        // Asegurarse de que el objeto no se destruya al cargar una nueva escena
        DontDestroyOnLoad(gameObject);
        
        // Obtener o añadir el componente AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Start()
    {
        if (musicClip != null)
        {
            // Configurar el AudioSource
            audioSource.clip = musicClip;
            audioSource.volume = volume;
            audioSource.loop = true; // Activar el bucle
            audioSource.Play(); // Comenzar la reproducción
        }
        else
        {
            Debug.LogWarning("No se ha asignado un clip de audio para la música de fondo.");
        }
    }

    // Método público para cambiar el volumen desde otros scripts
    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume); // Asegurar que el volumen está entre 0 y 1
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }
}