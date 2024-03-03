using UnityEngine.Events;

public class ChestInventory : InventoryHolder, IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public void Interact(Interactor interactor, out bool interactSuccesfull)
    {
        OnDynamicInventoryDisplayRequested?.Invoke(this.BasicInventorySytem);
        interactSuccesfull = true;
    }

    public void EndInteraction()
    {

    }
}
