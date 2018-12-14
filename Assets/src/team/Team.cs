using System;
using UnityEngine;

// Teams are accessed through static fields for convenience and initialized in Map.Awake()
public class Team {

    public static Team NONE = new TeamNone(0);
    public static Team ORANGE = new Team(1, "orange", new Color(1f, 0.522f, 0.106f));
    public static Team BLUE = new Team(2, "blue", Color.blue);
    public static Team GREEN = new Team(3, "green", Color.green);
    public static Team PURPLE = new Team(4, "purple", new Color(0.40f, 0.07f, 0.54f));
    public static Team[] ALL_TEAMS = new Team[] { GREEN, PURPLE, ORANGE, BLUE };

    public readonly Predicate<MapObject> predicateThisTeam;
    public readonly Predicate<MapObject> predicateOtherTeam;

    private readonly int teamId;
    private readonly string teamName;
    private readonly Color color;
    private readonly EnumTeam enumTeam;

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
