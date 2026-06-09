using UnityEngine;

public class SelectedColorMarker : MonoBehaviour
{
    [SerializeField] private GameObject selectedColor;

    public void MoveToButton()
    {
        selectedColor.SetActive(true);
        selectedColor.transform.position = transform.position;
    }
}
