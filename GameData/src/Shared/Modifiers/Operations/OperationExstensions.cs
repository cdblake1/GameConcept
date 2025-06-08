using GameData.src.Shared;
using GameData.src.Shared.Modifiers.Operations;

public static class OperationExtensions
{
    // public static Duration ApplyDurationModifier(this DurationOperation operation, Duration duration)
    // {
    //     if (duration.Type != operation.Kind)
    //     {
    //         throw new InvalidOperationException("cannot modify durations of different types");
    //     }

    //     if (duration.Type == Duration.Kind.Turns && operation.Kind == Duration.Kind.Turns)
    //     {
    //         if (operation.Turns is not null)
    //         {
    //             return Duration.FromTurns(duration.Turns + operation.Turns.ApplyScalarModifier(duration.Turns));
    //         }
    //         else
    //         {
    //             throw new InvalidOperationException("Turns cannot be null when DurationKind is set to Turns.");
    //         }
    //     }
    //     else if (operation.Kind == Duration.Kind.Permanent)
    //     {
    //         return Duration.Permanent();
    //     }
    //     else
    //     {
    //         if (operation.ExpiresWith is Duration.ExpiresWith ex)
    //         {
    //             return Duration.FromExpiry(ex);
    //         }
    //         else
    //         {
    //             throw new InvalidOperationException("ExpiresWith cannot be null when DurationKind is set to ExpiresWith.");
    //         }
    //     }
    // }

    public static List<T> ApplyCollectionModifier<T>(this CollectionOperation<T> collection, List<T> items)
    {
        switch (collection.Operation)
        {
            case CollectionOperationKind.Add:
                items.AddRange(collection.Items);
                break;
            case CollectionOperationKind.Remove:
                foreach (var item in items)
                {
                    items.Remove(item);
                }
                break;
            case CollectionOperationKind.Clear:
                items.Clear();
                break;
            case CollectionOperationKind.Set:
                items = [.. collection.Items];
                break;
            default:
                break;
        }

        return items;
    }
}