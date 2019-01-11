using System;

public static class EntitySelecter {

    public static readonly Predicate<MapObject> isHarvestable = (MapObject obj) => { return obj is HarvestableObject; };
    public static readonly Predicate<MapObject> isBuilding = (MapObject obj) => { return obj is BuildingBase; };
    public static readonly Predicate<MapObject> isUnit = (MapObject obj) => { return obj is UnitBase; };
}
