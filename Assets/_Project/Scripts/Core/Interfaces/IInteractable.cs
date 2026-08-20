namespace Game.Core
{
    public interface IInteractable
    {
        string PromptText { get; }   // "강화하기" 같은 안내 문구
        bool CanInteract { get; }
        void Interact();
    }
}