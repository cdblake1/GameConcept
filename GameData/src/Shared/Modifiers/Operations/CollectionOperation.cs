namespace GameData.src.Shared.Modifiers.Operations
{
    public sealed record CollectionOperation<T>
    {
        public IReadOnlyList<T> Items { get; }

        public CollectionOperationKind Operation { get; }

        public CollectionOperation(IReadOnlyList<T> items, CollectionOperationKind operation)
        {
            this.Items = items;
            this.Operation = operation;
        }
    }

    public enum CollectionOperationKind { Add, Set, Remove, Clear }
}