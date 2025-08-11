public interface IStateSerializable<TDto, TSelf>
    where TSelf : IStateSerializable<TDto, TSelf>
{
    TDto Serialize();
    static abstract TSelf Restore(TDto dto);
}
