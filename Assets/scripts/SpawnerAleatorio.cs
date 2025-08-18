using UnityEngine;
using System.Collections.Generic;

public class SpawnerAleatorio : MonoBehaviour
{
    [System.Serializable]
    public class SpawnableObject
    {
        public GameObject prefab;
        [Range(0, 100)] 
        public int probabilidadRelativa = 50;
    }

    [Header("Configuración de Objetos")]
    [Tooltip("Lista de objetos que pueden spawnear")]
    public List<SpawnableObject> objetosParaSpawnear = new List<SpawnableObject>();

    [Header("Configuración de Spawn")]
    [Tooltip("Frecuencia mínima de spawn en segundos")]
    public float frecuenciaMinima = 1f;

    [Tooltip("Frecuencia máxima de spawn en segundos")]
    public float frecuenciaMaxima = 5f;

    [Tooltip("Si está activado, el spawner comenzará a funcionar automáticamente")]
    public bool spawnAutomatico = true;

    [Header("Límites de Spawn")]
    [Tooltip("Radio alrededor del spawner donde pueden aparecer los objetos")]
    public float radioSpawn = 3f;

    [Tooltip("Si está activado, los objetos se spawnearán en la posición exacta del spawner")]
    public bool spawnEnPosicionExacta = false;

    private float tiempoParaSiguienteSpawn;
    private int totalProbabilidad;

    void Start()
    {
        CalcularTotalProbabilidad();

        if (spawnAutomatico)
        {
            CalcularNuevoTiempoSpawn();
        }
    }

    void Update()
    {
        if (!spawnAutomatico) return;

        tiempoParaSiguienteSpawn -= Time.deltaTime;

        if (tiempoParaSiguienteSpawn <= 0f)
        {
            SpawnearObjeto();
            CalcularNuevoTiempoSpawn();
        }
    }

    void CalcularTotalProbabilidad()
    {
        totalProbabilidad = 0;
        foreach (var obj in objetosParaSpawnear)
        {
            totalProbabilidad += obj.probabilidadRelativa;
        }
    }

    void CalcularNuevoTiempoSpawn()
    {
        tiempoParaSiguienteSpawn = Random.Range(frecuenciaMinima, frecuenciaMaxima);
    }

    public void SpawnearObjeto()
    {
        if (objetosParaSpawnear.Count == 0 || totalProbabilidad <= 0)
        {
            Debug.LogWarning("No hay objetos configurados para spawnear o la probabilidad total es 0");
            return;
        }

        // Seleccionar un objeto aleatorio basado en las probabilidades
        int randomPoint = Random.Range(0, totalProbabilidad);
        int accumulatedProbability = 0;
        GameObject objetoASpawnear = null;

        foreach (var obj in objetosParaSpawnear)
        {
            accumulatedProbability += obj.probabilidadRelativa;
            if (randomPoint < accumulatedProbability)
            {
                objetoASpawnear = obj.prefab;
                break;
            }
        }

        if (objetoASpawnear != null)
        {
            // Calcular posición de spawn
            Vector3 posicionSpawn = transform.position;
            if (!spawnEnPosicionExacta)
            {
                Vector2 randomCircle = Random.insideUnitCircle * radioSpawn;
                posicionSpawn += new Vector3(randomCircle.x, randomCircle.y, 0);
            }

            // Instanciar el objeto
            Instantiate(objetoASpawnear, posicionSpawn, Quaternion.identity);
        }
    }

    // Método para iniciar/detener el spawn automático
    public void ToggleSpawnAutomatico(bool activar)
    {
        spawnAutomatico = activar;
        if (activar)
        {
            CalcularNuevoTiempoSpawn();
        }
    }

    // Método para spawnear un objeto específico por índice
    public void SpawnearObjetoEspecifico(int indice)
    {
        if (indice >= 0 && indice < objetosParaSpawnear.Count)
        {
            Instantiate(objetosParaSpawnear[indice].prefab, transform.position, Quaternion.identity);
        }
    }

    // Dibujar gizmo para ver el radio de spawn en el editor
    void OnDrawGizmosSelected()
    {
        if (!spawnEnPosicionExacta)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, radioSpawn);
        }
    }
}