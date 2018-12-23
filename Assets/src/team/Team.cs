using System;
using UnityEngine;

// Teams are accessed through static fields for convenience and initialized in Map.Awake()
public class Team {

    public static Team NONE = new TeamNone(0);
    public static Team ORANGE = new Team(1, "orange", new Color(1f, 0.522f, 0.106f));
    public static Team BLUE = new Team(2, "blue", Color.blue);
    public static Team GREEN = new Team(3, "green", Color.green);
    public static Team PURPLE = new Team(4, "purple", new Color(0.40f, 0.07f, 0.54f));
    public static Team[] ALL_TEAMS = new Team[] { NONE, ORANGE, BLUE, GREEN, PURPLE };
    public readonly Predicate<MapObject> predicateThisTeam;
    public readonly Predicate<MapObject> predicateOtherTeam;

    private readonly int teamId;
    private readonly string teamName;
    private readonly Color color;
    private readonly EnumTeam enumTeam;

    // Server side only from here.
    private Transform orginPoint;
    /// <summary> The number of resources the team has. </summary>
    private int resources;

    private Team(int teamId, string name, Color color) {
        this.teamId = teamId;
        this.teamName = char.ToUpper(name[0]) + name.Substring(1);
        this.color = color;
        this.enumTeam = (EnumTeam)this.teamId;

        this.predicateThisTeam = (MapObject obj) => { return obj is SidedEntity && ((SidedEntity)obj).getTeam() == this; };
        this.predicateOtherTeam = (MapObject obj) => { return obj is SidedEntity && ((SidedEntity)obj).getTeam() != this; };
    }

    /// <summary>
    /// Returns the name of the team.
    /// </summary>
    public string getTeamName() {
        return this.teamName;
    }

    public Color getTeamColor() {
        return this.color;
    }

    public EnumTeam getEnum() {
        return this.enumTeam;
    }

    public int getTeamId() {
        return this.teamId;
    }

    /// <summary>
    /// Returns the current number of resources that this team has.
    /// </summary>
    public int getResources() {
        return this.resources;
    }

    /// <summary>
    /// Sets the Team's resources, clamping it between 0 and the maximum number the player can have.  Any overflow is discarded.
    /// </summary>
    public void setResources(int amount) {
        this.resources = Mathf.Clamp(amount, 0, this.getMaxResourceCount());
    }

    /// <summary>
    /// Reduces the Team's resources by the passed amount.
    /// </summary>
    public void reduceResources(int amount) {
        this.setResources(this.resources - amount);
    }

    public void increaseResources(int amount) {
        this.setResources(this.resources + amount);
    }

    public void setOrgin(Transform t) {
        this.orginPoint = t;
    }

    public Vector3 getOrginPos() {
        return this.orginPoint == null ? Vector3.zero : this.orginPoint.position;
    }

    public Quaternion getOrginRotation() {
        return this.orginPoint == null ? Quaternion.identity : this.orginPoint.rotation;
    }

    /// <summary>
    /// Returns the maximum amount of resources that this Team can have.
    /// </summary>
    public int getMaxResourceCount() {
        int maxResources = Constants.STARTING_RESOURCE_CAP;
        foreach(SidedEntity o in Map.instance.findMapObjects(this.predicateThisTeam)) {
            if(o is BuildingStoreroom) {
                maxResources += Constants.BUILDING_STOREROOM_RESOURCE_BOOST;
            }
        }
        return maxResources;
    }

    /// <summary>
    /// Returns the total number of troops this team can have.
    /// </summary>
    public int getMaxTroopCount() {
        int i = Constants.STARTING_TROOP_CAP;
        foreach(SidedEntity o in Map.instance.findMapObjects(this.predicateThisTeam)) {
            if(o is BuildingCamp) {
                BuildingCamp camp = (BuildingCamp)o;
                if(!camp.isConstructing()) {
                    i += Constants.BUILDING_CAMP_TROOP_BOOST;
                }
            }
        }
        return i;
    }

    public override bool Equals(object obj) {
        if(obj is Team) {
            return this == (Team)obj;
        } else {
            return false;
        }
    }

    public override int GetHashCode() {
        return this.teamId.GetHashCode();
    }

    public static bool operator ==(Team team1, Team team2) {
        return team1.teamId == team2.teamId;
    }

    public static bool operator !=(Team team1, Team team2) {
        return !(team1 == team2);
    }

    /// <summary>
    /// Returns the Team with the passed ID, or Team.None if the ID does not point to a team.
    /// </summary>
    public static Team getTeamFromId(int id) {
        foreach (Team team in Team.ALL_TEAMS) {
            if (team.teamId == id) {
                return team;
            }
        }
        return Team.NONE;
    }

    public static Team getTeamFromEnum(EnumTeam enumTeam) {
        switch (enumTeam) {
            case EnumTeam.GREEN: return Team.GREEN;
            case EnumTeam.PURPLE: return Team.PURPLE;
            case EnumTeam.ORANGE: return Team.ORANGE;
            case EnumTeam.BLUE: return Team.BLUE;
            case EnumTeam.NONE: default: return Team.NONE;
        }
    }

    private class TeamNone : Team {

        public TeamNone(int teamId) : base(teamId, "None", Color.white) { }
    }
}
