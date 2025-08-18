using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [Header("Dialog System")]
    public GameObject dialogSystem;

    [SerializeField] private TextMeshProUGUI Nombre;
    [SerializeField] private TextMeshProUGUI DialogoText;
    [SerializeField] private GameObject Mission1;
    [SerializeField] private GameObject Mission2;

    [Header("Configuración de Diálogos")]
    [SerializeField] private List<Dialogo> dialogos = new List<Dialogo>();
    private int indiceDialogoActual = 0;
    private bool dialogoActivo = false;

    [Header("Estado de Misiones")]
    public bool mision1Completada = false;
    public bool mision2Completada = false;

    [System.Serializable]
    public class Dialogo
    {
        public string nombrePersonaje;
        [TextArea(3, 10)]
        public string textoDialogo;
        public bool activarMision1;
        public bool activarMision2;
        public bool requiereMision1Completada;
        public bool requiereMision2Completada;
    }
    [Header("Mision1")]
    public BoxCollider2D CarroFaCollider;
    public BoxCollider2D colider1;

    [SerializeField] private string tagJugador = "Player"; // Tag del jugador
    [SerializeField] private LayerMask capaJugador; // Capa del jugador

    void Start()
    {
        // Inicializar sistema de diálogo
        if (dialogSystem != null)
        {
            dialogSystem.SetActive(false);
        }

        // Inicializar misiones
        if (Mission1 != null) Mission1.SetActive(false);
        if (Mission2 != null) Mission2.SetActive(false);

        // Comenzar con el primer diálogo si hay alguno
        if (dialogos.Count > 0)
        {
            IniciarDialogo(0);
        }
    }

    void Update()
    {
        // Detectar clic izquierdo del ratón para avanzar diálogo
        if (dialogoActivo && Input.GetMouseButtonDown(0))
        {
            AvanzarDialogo();
        }
    }

    public void IniciarDialogo(int indice)
    {
        if (indice >= 0 && indice < dialogos.Count)
        {
            // Verificar requisitos de misión
            Dialogo dialogo = dialogos[indice];

            if ((dialogo.requiereMision1Completada && !mision1Completada) ||
                (dialogo.requiereMision2Completada && !mision2Completada))
            {
                return;
            }

            indiceDialogoActual = indice;
            MostrarDialogo(dialogo);
            dialogSystem.SetActive(true);
            dialogoActivo = true;

            // Pausar el juego si es necesario
            Time.timeScale = 0f;
        }
    }

    private void MostrarDialogo(Dialogo dialogo)
    {
        Nombre.text = dialogo.nombrePersonaje;
        DialogoText.text = dialogo.textoDialogo;
    }



    public void AvanzarDialogo()
    {
        // Verificar si hay más diálogos
        if (indiceDialogoActual < dialogos.Count - 1)
        {
            indiceDialogoActual++;
            Dialogo siguienteDialogo = dialogos[indiceDialogoActual];

            // Verificar requisitos para el siguiente diálogo
            if ((!siguienteDialogo.requiereMision1Completada || mision1Completada) &&
                (!siguienteDialogo.requiereMision2Completada || mision2Completada))
            {
                MostrarDialogo(siguienteDialogo);

                // Activar misiones si corresponde
                if (siguienteDialogo.activarMision1 && Mission1 != null)
                {
                    Mission1.SetActive(true);
                }

                if (siguienteDialogo.activarMision2 && Mission2 != null)
                {
                    Mission2.SetActive(true);
                }
            }
            else
            {
                TerminarDialogo();
            }
        }
        else
        {
            TerminarDialogo();
        }
    }

    public void TerminarDialogo()
    {
        dialogSystem.SetActive(false);
        dialogoActivo = false;
        Time.timeScale = 1f; // Reanudar el juego

        if (mision1Completada && mision2Completada)
        {
            Debug.Log("Ambas misiones completadas");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Cambiar a la escena final
        }
    }

    public void CompletarMision1()
    {   
        colider1.enabled = false; // Desactivar el collider para evitar múltiples activaciones
    if (!mision1Completada) // Solo ejecutar si la misión no estaba completada
        {
            CarroFaCollider.enabled = true;

            mision1Completada = true;
        if (Mission1 != null) 
        {
            Mission1.SetActive(false);
            Debug.Log("Objeto de misión 1 desactivado");
        }

        // Buscar diálogos que requieran misión 1 completada
        for (int i = 0; i < dialogos.Count; i++)
        {
            if (dialogos[i].requiereMision1Completada && !dialogos[i].requiereMision2Completada)
            {
                Debug.Log($"Iniciando diálogo {i} después de completar misión 1");
                IniciarDialogo(i);
                break;
            }
        }
    }
}

    public void CompletarMision2()
    {
        mision2Completada = true;
        if (Mission2 != null) Mission2.SetActive(false);

        // Buscar diálogos que requieran misión 2 completada
        for (int i = 0; i < dialogos.Count; i++)
        {
            if (dialogos[i].requiereMision2Completada)
            {
                IniciarDialogo(i);
                break;
            }
        }
    }

    // Método para actualizar diálogo desde otros scripts
    public void ActualizarDialogo(string nuevoNombre, string nuevoTexto)
    {
        if (Nombre != null) Nombre.text = nuevoNombre;
        if (DialogoText != null) DialogoText.text = nuevoTexto;
    }

    // Para manejar la colisión, agrega este método en GameManager o en el script adjunto al objeto con el BoxCollider2D
    private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag(tagJugador)) // tagJugador = "Player"
        {

            if (mision1Completada == false)
            {
                Debug.Log("Trigger activado por el jugador");
                CompletarMision1();
            }
            else
            {

                Debug.Log("Trigger activado por el jugador");
                CompletarMision2();

                Debug.Log("Misión 2 completada");
            }
    }
}



}