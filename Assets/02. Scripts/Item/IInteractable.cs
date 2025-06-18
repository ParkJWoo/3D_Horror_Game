using System;

public interface IInteractable
{
    void SetInterface(bool active);

    void OnInteraction();

    event Action<IInteractable> OnInteracted;
}
