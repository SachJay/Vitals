using UnityEngine;

public interface IInteractable
{
    public void Interact();

    public void EndInteraction();

    public GameObject GetGameObject();
}
