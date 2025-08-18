using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public enum DireccionMovimiento
    {
        Arriba,
        Abajo,
        Izquierda,
        Derecha,
        Personalizada
    }

    [Header("Configuración de Movimiento")]
    [Tooltip("Velocidad de movimiento en unidades por segundo")]
    public float velocidad = 5f;
    
    [Header("Dirección del Movimiento")]
    [Tooltip("Selecciona la dirección inicial del movimiento")]
    public DireccionMovimiento direccion = DireccionMovimiento.Abajo;
    
    [Tooltip("Dirección personalizada (solo se usa si dirección = Personalizada)")]
    public Vector2 direccionPersonalizada = Vector2.down;
    
    [Header("Tiempo de Vida")]
    [Tooltip("Tiempo en segundos antes de autodestruirse")]
    public float tiempoDeVida = 3f;

    private Vector2 direccionMovimiento;

    private void Start()
    {
        // Configurar la dirección basada en la selección del inspector
        switch (direccion)
        {
            case DireccionMovimiento.Arriba:
                direccionMovimiento = Vector2.up;
                break;
            case DireccionMovimiento.Abajo:
                direccionMovimiento = Vector2.down;
                break;
            case DireccionMovimiento.Izquierda:
                direccionMovimiento = Vector2.left;
                break;
            case DireccionMovimiento.Derecha:
                direccionMovimiento = Vector2.right;
                break;
            case DireccionMovimiento.Personalizada:
                direccionMovimiento = direccionPersonalizada.normalized;
                break;
        }

        // Programar la autodestrucción después del tiempo de vida
        Destroy(gameObject, tiempoDeVida);
    }

    private void Update()
    {
        // Mover el objeto en la dirección especificada con la velocidad dada
        transform.Translate(direccionMovimiento * velocidad * Time.deltaTime);
    }

    // Método para cambiar la dirección desde otro script si es necesario
    public void EstablecerDireccion(Vector2 nuevaDireccion)
    {
        direccionMovimiento = nuevaDireccion.normalized;
    }
}