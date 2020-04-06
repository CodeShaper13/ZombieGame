using System;
using UnityEngine;

// Teams are accessed through static fields for convenience and initialized in Map.Awake()
public class Team {

    public static Team NONE = new TeamNone(0);
    public static Team SURVIVORS_1 = new Team(1, "orange", EnumTeam.ORANGE, new Color(1f, 0.522f, 0.106f));
    public static Team SURVIVORS_2 = new Team(2, "blue", EnumTeam.BLUE, Color.blue);
    public static Team SURVIVORS_3 = new Team(3, "green", EnumTeam.GREEN, Color.green);
    public static Team SURVIVORS_4 = new Team(4, "purple", EnumTeam.PURPLE, new Color(0.40f, 0.07f, 0.54f));
    public static Team ZOMBIES = new TeamBlack(5);

    /// <summary> An array of all the teams. </summary>
    public static Team[] ALL_TEAMS = new Team[] { NONE, SURVIVORS_1, SURVIVORS_2, SURVIVORS_3, SURVIVORS_4, ZOMBIES };
    public static Team[] ALL_PLAYABLE_TEAMS = new Team[] { SURVIVORS_1, SURVIVORS_2, SURVIVORS_3, SURVIVORS_4 };

    public readonly Predicate<MapObject> predicateThisTeam;
    public readonly Predicate<MapObject> predicateThisTeamUnit;
    public readonly Predicate<MapObject> predicateOtherTeam;

    private readonly int teamId;
    private readonly string teamName;
    private readonly Color color;
    private readonly EnumTeam enumTeam;

    private Team(int teamId, string name, EnumTeam enumTeam, Color color) {
        this.teamId = teamId;
        this.teamName = char.ToUpper(name[0]) + name.Substring(1);
        this.color = color;
        this.enumTeam = enumTeam;

        this.predicateThisTeam = (MapObject obj) => { return obj is SidedEntity && ((SidedEntity)obj).getTeam() == this; };
        this.predicateThisTeamUnit = (MapObject obj) => { return this.predicateThisTeam(obj) && obj is UnitBase; };
        this.predicateOtherTeam = (MapObject obj) => { return obj is SidedEntity && ((SidedEntity)obj).getTeam() != this; };
    }

    /// <summary>
    /// Returns the name of the team.
    /// </summary>
    public string getTeamName() {
        return this.teamName;
    }

    public Color getColor() {
        return this.color;
    }

    public EnumTeam getEnum() {
        return this.enumTeam;
    }

    public int getId() {
        return this.teamId;
    }

    /// <summary>
    /// Returns the maximum amount of resources that this Team can have.
    /// </summary>
    public virtual int getMaxResourceCount(MapBase map) {
        int maxResources = int.MaxValue;
        foreach(SidedEntity o in map.findMapObjects(this.predicateThisTeam)) {
            if(o is BuildingStoreroom) {
                maxResources += Constants.BUILDING_STOREROOM_RESOURCE_BOOST;
            }
        }
        return maxResources;
    }

    public int getCurrentTroopCount(MapBase map) {
        return map.findMapObjects(this.predicateThisTeamUnit).Count;
    }

    /// <summary>
    /// Returns the total number of troops this team can have.
    /// </summary>
    public virtual int getMaxTroopCount(MapBase map) {
        int i = Constants.STARTING_TROOP_CAP;
        foreach(SidedEntity o in map.findMapObjects(this.predicateThisTeam)) {
            if(o is BuildingCamp) {
                BuildingCamp camp = (BuildingCamp)o;
                if(!camp.isConstructing()) {
                    i += Constants.BUILDING_CAMP_TROOP_BOOST;
                }
            }
        }
        return i;
    }


    public bool Equals(Team p) {
        // If parameter is null, return false.
        if(System.Object.ReferenceEquals(p, null)) {
            return false;
        }

        // Optimization for a common success case.
        if(System.Object.ReferenceEquals(this, p)) {
            return true;
        }

        // If run-time types are not exactly the same, return false.
        if(this.GetType() != p.GetType()) {
            return false;
        }

        // Return true if the fields match.
        // Note that the base class is not invoked because it is
        // System.Object, which defines Equals as reference equality.
        return (this.teamId == p.teamId);
    }

    public override int GetHashCode() {
        return this.teamId.GetHashCode();
    }

    public static bool operator ==(Team lhs, Team rhs) {
        // Check for null on left side.
        if(System.Object.ReferenceEquals(lhs, null)) {
            if(System.Object.ReferenceEquals(rhs, null)) {
                // null == null = true.
                return true;
            }

            // Only the left side is null.
            return false;
        }
        // Equals handles case of null on right side.
        return lhs.Equals(rhs);
    }

    public static bool operator !=(Team lhs, Team rhs) {
        return !(lhs == rhs);
    }

    public override bool Equals(object obj) {
        if(obj is Team) {
            return this == (Team)obj;
        } else {
            return false;
        }
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
            case EnumTeam.GREEN: return Team.SURVIVORS_3;
            case EnumTeam.PURPLE: return Team.SURVIVORS_4;
            case EnumTeam.ORANGE: return Team.SURVIVORS_1;
            case EnumTeam.BLUE: return Team.SURVIVORS_2;
            case EnumTeam.BLACK: return Team.ZOMBIES;
            case EnumTeam.NONE: default: return Team.NONE;
        }
    }

    private class TeamNone : Team {

        public TeamNone(int teamId) : base(teamId, "None", EnumTeam.NONE, Color.gray) { }
    }

    private class TeamBlack : Team {

        public TeamBlack(int teamId) : base(teamId, "Black", EnumTeam.BLACK, Color.black) { }

        public override int getMaxTroopCount(MapBase map) {
            return int.MaxValue;
        }

        public override int getMaxResourceCount(MapBase map) {
            return int.MaxValue;
        }
    }
}
