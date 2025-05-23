using System.Data;
using Dungeon.Game.Helper;

namespace Dungeon.Game.Core.Utils;

public class EntitySystemMapper : IEquatable<IEnumerable<ComponentType>>, IEquatable<EntitySystemMapper>
{
    private readonly HashSet<ComponentType> _filterRules;
    private readonly HashSet<Entity> _entities;
    private readonly HashSet<System> _systems;

    public EntitySystemMapper(ISet<ComponentType> filterRules)
    {
        _filterRules = [..filterRules];
        _entities = [];
        _systems = [];
    }
    
    public EntitySystemMapper(IEnumerable<ComponentType> filterRules)
    {
        _filterRules = [..filterRules];
        _entities = [];
        _systems = [];
    }

    public EntitySystemMapper()
    {
        _filterRules = [];
        _entities = [];
        _systems = [];
    }

    public bool Add(System system)
    {
        if (!_systems.Add(system))
            return false;
        
        _entities.ForEach(system.TriggerOnAdd);
        return true;
    }

    public bool Remove(System system)
    {
        if (!_systems.Remove(system))
            return false;
        
        _entities.ForEach(system.TriggerOnRemove);
        return true;
    }

    public bool Add(Entity entity)
    {
        if (!_entities.Contains(entity) && Accept(entity))
        {
            _entities.Add(entity);
            _systems.ForEach(x => x.TriggerOnAdd(entity));
            return true;
        }

        return false;
    }

    public bool Remove(Entity entity)
    {
        if (_entities.Contains(entity))
        {
            _entities.Remove(entity);
            _systems.ForEach(x => x.TriggerOnRemove(entity));
            return true;
        }

        return false;
    }

    public void Update(Entity entity)
    {
        if (Accept(entity))
            Add(entity);
        else
            Remove(entity);
    }
    
    public IEnumerable<Entity> Entities() => _entities;
    
    private bool Accept(Entity entity)
    {
        // Guess based on UnitTest result. Actual code does not imply that it must have a component
        if (!entity.Components.Any())
            return false;
        
        foreach (var filter in _filterRules)
        {
            if (!entity.IsPresent(filter))
                return false;
        }
        return true;
    }

    public bool FilterEquals(IEnumerable<ComponentType> other)
    {
        return _filterRules.SetEquals(other);
    }
    
    public bool Equals(IEnumerable<ComponentType>? other)
    {
        if (other is null)
            return false;
        return FilterEquals(other);
    }

    public bool Equals(EntitySystemMapper? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return _filterRules.SetEquals(other._filterRules);
    }

    public override bool Equals(object? obj)
    {
        if (obj is EntitySystemMapper esm)
            return Equals(esm);
        return false;
    }
}