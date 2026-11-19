using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Selecciona el primer boton al abrir la escena (para navegar con teclado o mando)
public class UISelectionHelper : MonoBehaviour
{
    [SerializeField] private Selectable firstSelectable;

    private IEnumerator Start()
    {
        yield return null;

        if (firstSelectable != null && EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(firstSelectable.gameObject);
    }
}
