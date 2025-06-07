namespace GameData.src.Shared.Modifier
{
    public sealed record ScalarOperation
    {
        private ScalarOperation(OperationKind operation, int value)
        {
            ModifierOperation = operation;
            Value = value;
        }

        public OperationKind ModifierOperation { get; }

        public int Value { get; }

        public enum OperationKind { Add, Mult }

        public static ScalarOperation Create(OperationKind operation, int value)
        {
            return new ScalarOperation(operation, value);
        }
    }
}