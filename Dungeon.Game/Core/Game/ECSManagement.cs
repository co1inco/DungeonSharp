using System.Collections.Specialized;
using CSharpFunctionalExtensions;
using Dungeon.Game.Core.Utils;
using Dungeon.Game.Helper;
using Serilog;

namespace Dungeon.Game.Core;

public interface ILevel {}

public record struct NullLevel() : ILevel;

public sealed class ECSManagement
{
    private static readonly ILogger Log = Serilog.Log.ForContext<ECSManagement>();
    private static readonly OrderedDictionary Systems = new();
    private static readonly Dictionary<ILevel, ISet<EntitySystemMapper>> LevelStorageMap;
    private static ISet<EntitySystemMapper> _activeEntityStorage = new HashSet<EntitySystemMapper>();

    static ECSManagement()
    {
        LevelStorageMap = new Dictionary<ILevel, ISet<EntitySystemMapper>>
        {
            { new NullLevel(), _activeEntityStorage }
        };
        _activeEntityStorage.Add(new EntitySystemMapper());
    }

    public static void InformAboutChanges(Entity entity)
    {
        // throw new NotImplementedException();
    }


    public static void Add(Entity entity)
    {
        foreach (var entitySystemMapper in _activeEntityStorage)
        {
            entitySystemMapper.Add(entity);
        }
        // _activeEntityStorage.ForEach((x) -> x.Add(entity));
        Log.Information("Entity: {Entity} will be added to the Game.", entity);
    }

    public static void Remove(Entity entity)
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<Entity> Entities() =>
        Entities([]);

    public static IEnumerable<Entity> Entities(System system) =>
        Entities(system.FilterRules);

    public static IEnumerable<Entity> Entities(IEnumerable<ComponentType> withComponents)
    {
        if (_activeEntityStorage
            .Where(x => x.FilterEquals(withComponents))
            .TryFirst()
            .TryGetValue(out var rf))
        {
            return rf.Entities();
        }
        else
        {
            return CreateEntitySystemMapper(withComponents).Entities();
        }
    }

    private static EntitySystemMapper CreateEntitySystemMapper(IEnumerable<ComponentType> filter)
    {
        var mapper = new EntitySystemMapper(filter);
        _activeEntityStorage.Add(mapper);
        Entities().ForEach(x => mapper.Add(x));
        return mapper;
    }
}