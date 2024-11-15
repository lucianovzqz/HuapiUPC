using UnityEngine;

public class ObjetoRecolectable : MonoBehaviour
{
    [SerializeField] private string tipoDeObjeto; // Objetos como el libro, frutas, etc

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Recolectar();
        }
    }

    private void Recolectar()
    {
       
        Debug.Log($"Objeto recolectado: {tipoDeObjeto}");

        // Desaparecer el objeto después de recolectarlo
        Destroy(gameObject);
    }
}

