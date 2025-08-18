using UnityEngine;
using TMPro;

public class FlotacionTexto : MonoBehaviour
{
    [Header("Configuración de Flotación")]
    [Tooltip("Altura máxima de la flotación")]
    public float amplitud = 0.5f;
    
    [Tooltip("Velocidad de la flotación")]
    public float velocidad = 1f;
    
    [Tooltip("Offset del tiempo para variar el movimiento")]
    public float offsetTiempo = 0f;
    
    [Header("Configuración de Rotación (Opcional)")]
    [Tooltip("Si está activado, el texto también rotará ligeramente")]
    public bool rotar = false;
    
    [Tooltip("Ángulo máximo de rotación")]
    public float anguloRotacion = 5f;
    
    [Tooltip("Velocidad de rotación")]
    public float velocidadRotacion = 0.5f;

    private Vector3 posicionInicial;
    private TMP_Text texto;
    private float tiempoInicial;

    void Start()
    {
        texto = GetComponent<TMP_Text>();
        if (texto == null)
        {
            Debug.LogError("No se encontró el componente TMP_Text en este GameObject");
            return;
        }

        posicionInicial = transform.position;
        tiempoInicial = Time.time + offsetTiempo;
    }

    void Update()
    {
        if (texto == null) return;

        // Calcular movimiento vertical usando una función seno
        float nuevaY = posicionInicial.y + Mathf.Sin((Time.time - tiempoInicial) * velocidad) * amplitud;
        
        // Aplicar nueva posición
        transform.position = new Vector3(posicionInicial.x, nuevaY, posicionInicial.z);

        // Rotación opcional
        if (rotar)
        {
            float rotacionZ = Mathf.Sin((Time.time - tiempoInicial) * velocidadRotacion) * anguloRotacion;
            transform.rotation = Quaternion.Euler(0f, 0f, rotacionZ);
        }
    }

    // Métodos para ajustar parámetros durante el juego
    public void SetAmplitud(float nuevaAmplitud)
    {
        amplitud = nuevaAmplitud;
    }

    public void SetVelocidad(float nuevaVelocidad)
    {
        velocidad = nuevaVelocidad;
    }

    public void ResetPosicion()
    {
        transform.position = posicionInicial;
        transform.rotation = Quaternion.identity;
        tiempoInicial = Time.time + offsetTiempo;
    }
}