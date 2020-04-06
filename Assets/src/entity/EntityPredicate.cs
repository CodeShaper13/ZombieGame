using System;

public static class EntityPredicate {

    public static readonly Predicate<MapObject> isHarvestable = (MapObject obj) => { return obj is HarvestableObject; };
    public static readonly Predicate<MapObject> isBuilding = (MapObject obj) => { return obj is BuildingBase; };
    public static readonly Predicate<MapObject> isUnit = (MapObject obj) => { return obj is UnitBase; };
    public static readonly Predicate<MapObject> isLiving = (MapObject obj) => { return obj is LivingObject; };
    public static readonly Predicate<MapObject> isSided = (MapObject obj) => { return obj is SidedEntity; };
}
