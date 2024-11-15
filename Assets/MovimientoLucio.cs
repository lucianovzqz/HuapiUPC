using UnityEngine;

public class MovimientoLucio : MonoBehaviour
{
    private Rigidbody2D rb2D;

    [Header("Movimiento")]
    private float movimientoHorizontal = 0f;
    [SerializeField] private float velocidadDeMovimiento;
    [Range(0, 0.3f)][SerializeField] private float suavizadoDeMovimiento;
    private Vector3 velocidad = Vector3.zero;
    private bool mirandoDerecha = true;

    [Header("Salto")]
    [SerializeField] private float fuerzaDeSalto; // Fuerza base del salto
    [SerializeField] private LayerMask queEsSuelo;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private Vector3 dimensionesCaja;
    [SerializeField] private bool enSuelo;

    [SerializeField] private int maxSaltos = 1; // Número máximo de saltos
    private int saltosRestantes; // Contador de saltos disponibles
    private bool salto = false;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        saltosRestantes = maxSaltos; // Inicializar los saltos
    }

    private void Update()
    {
        movimientoHorizontal = Input.GetAxisRaw("Horizontal") * velocidadDeMovimiento;

        if (Input.GetButtonDown("Jump") && saltosRestantes > 0)
        {
            salto = true;
            saltosRestantes--; // Reducir saltos restantes al presionar el botón de salto
        }
    }

    private void FixedUpdate()
    {
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, queEsSuelo);

        if (enSuelo)
        {
            saltosRestantes = maxSaltos; // Reiniciar los saltos al tocar el suelo
        }

        // Limitar la velocidad vertical máxima
        if (rb2D.velocity.y > 6f) // Valor según la altura deseada
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, 6f);
        }

        Mover(movimientoHorizontal * Time.fixedDeltaTime, salto);
        salto = false; // Resetear el salto después de aplicarlo
    }

    private void Mover(float mover, bool saltar)
    {
        Vector3 velocidadObjetivo = new Vector2(mover, rb2D.velocity.y);
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, velocidadObjetivo, ref velocidad, suavizadoDeMovimiento);

        if (mover > 0 && !mirandoDerecha)
        {
            Girar();
        }
        else if (mover < 0 && mirandoDerecha)
        {
            Girar();
        }

        if (saltar)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, 0f); // Resetear velocidad vertical

            // Menos fuerza en el segundo salto
            float fuerzaActual = saltosRestantes == maxSaltos - 1 ? fuerzaDeSalto : fuerzaDeSalto * 0.75f;
            rb2D.AddForce(new Vector2(0f, fuerzaActual), ForceMode2D.Impulse);
        }
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCaja);
    }
}