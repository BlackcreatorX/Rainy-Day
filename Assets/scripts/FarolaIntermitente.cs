using UnityEngine;
using System.Collections;

public class FarolaIntermitente : MonoBehaviour
{
    [Header("Configuración de Tiempos")]
    [Tooltip("Tiempo mínimo que la farola puede estar encendida")]
    public float minTiempoEncendido = 3f;
    
    [Tooltip("Tiempo máximo que la farola puede estar encendida")]
    public float maxTiempoEncendido = 10f;
    
    [Tooltip("Tiempo mínimo que la farola puede estar apagada")]
    public float minTiempoApagado = 0.5f;
    
    [Tooltip("Tiempo máximo que la farola puede estar apagada")]
    public float maxTiempoApagado = 5f;

    [Header("Referencias")]
    [Tooltip("El objeto que representa la luz de la farola (debe tener un SpriteRenderer)")]
    public GameObject luzFarola;

    private SpriteRenderer spriteLuz;
    private bool estaEncendida = true;

    void Start()
    {
        if (luzFarola == null)
        {
            Debug.LogError("No se ha asignado el objeto de la luz de la farola!");
            return;
        }

        spriteLuz = luzFarola.GetComponent<SpriteRenderer>();
        if (spriteLuz == null)
        {
            Debug.LogError("El objeto de la luz no tiene un componente SpriteRenderer!");
            return;
        }

        // Iniciar la corrutina de intermitencia
        StartCoroutine(CicloIntermitente());
    }

    IEnumerator CicloIntermitente()
    {
        while (true)
        {
            // Esperar un tiempo aleatorio antes de cambiar de estado
            float tiempoEspera;
            
            if (estaEncendida)
            {
                // Calcular tiempo aleatorio para apagar
                tiempoEspera = Random.Range(minTiempoEncendido, maxTiempoEncendido);
                yield return new WaitForSeconds(tiempoEspera);
                
                // Apagar la farola
                ApagarFarola();
            }
            else
            {
                // Calcular tiempo aleatorio para encender
                tiempoEspera = Random.Range(minTiempoApagado, maxTiempoApagado);
                yield return new WaitForSeconds(tiempoEspera);
                
                
                EncenderFarola();
            }
        }
    }

    void EncenderFarola()
    {
        spriteLuz.enabled = true;
        estaEncendida = true;
    }

    void ApagarFarola()
    {
        spriteLuz.enabled = false;
        estaEncendida = false;
    }

    // Opcional: Método para forzar el estado de la farola
    public void SetEstadoFarola(bool encender)
    {
        if (encender)
        {
            EncenderFarola();
        }
        else
        {
            ApagarFarola();
        }
    }
}