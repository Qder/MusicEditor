namespace DevBridge.Templates.WebProject.DataEntities
{
    public interface IEntity
    {
        object Id { get; set; }
    }

    public interface IEntity<TId> : IEntity where TId : struct
    {
        new TId Id { get; set; }
    }
}