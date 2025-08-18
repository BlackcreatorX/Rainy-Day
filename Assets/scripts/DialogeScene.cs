using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class DialogeScene : MonoBehaviour
{
    [Header("Dialog System")]
    public GameObject dialogSystem;

    [SerializeField] private TextMeshProUGUI Nombre;
    [SerializeField] private TextMeshProUGUI DialogoText;
    [SerializeField] private TextMeshProUGUI BotonTexto;
    [SerializeField] private GameObject Carlos;
    
    [Header("Configuración de Diálogos")]
    [SerializeField] private List<Dialogo> dialogos = new List<Dialogo>();
    private int indiceDialogoActual = 0;
    private bool dialogoActivo = false;

    [System.Serializable]
    public class Dialogo
    {
        public string nombrePersonaje;
        [TextArea(3, 10)]
        public string textoDialogo;
    }
    

    

    void Start()
    {
        // Inicializar sistema de diálogo
        if (dialogSystem != null)
        {
            dialogSystem.SetActive(false);
        }


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
            if (BotonTexto.enabled)
            {
                BotonTexto.enabled = false; // Desactivar el botón de texto
                Carlos.SetActive(true); // Activar el GameObject Carlos
            }
            AvanzarDialogo();
        }
    }

    public void IniciarDialogo(int indice)
    {
        if (indice >= 0 && indice < dialogos.Count)
        {
            // Verificar requisitos de misión
            Dialogo dialogo = dialogos[indice];

            

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




            MostrarDialogo(siguienteDialogo);
        }
        else
        {
            TerminarDialogo();
        }
    }
public void TerminarDialogo()
{
    // Desactivar el sistema de diálogo
    dialogSystem.SetActive(false);
    dialogoActivo = false;
    Time.timeScale = 1f; // Reanudar el juego
    
    // Calcular el índice de la siguiente escena
    int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
    
    // Verificar si existe la siguiente escena
    if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
    {
        SceneManager.LoadScene(nextSceneIndex);
    }
    else
    {
        // Si no hay más escenas, volver al inicio o cargar escena 1
        Debug.LogWarning("No hay más escenas. Volviendo a la escena 1.");
        SceneManager.LoadScene(0); // O usa 1 si tu escena inicial está en el índice 1
    }
}



    

    // Método para actualizar diálogo desde otros scripts
    public void ActualizarDialogo(string nuevoNombre, string nuevoTexto)
    {
        if (Nombre != null) Nombre.text = nuevoNombre;
        if (DialogoText != null) DialogoText.text = nuevoTexto;
    }

   



}