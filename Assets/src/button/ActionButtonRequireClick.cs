using System;
using System.Collections.Generic;


public class ActionButtonRequireClick : ActionButton {

    private Action<SidedEntity, SidedEntity> mainFunctionDelayed;
    private Func<IEnumerable<SidedEntity>, SidedEntity, SidedEntity> entitySelecterFunctionDelayed;

    /// <summary>
    /// A function that checks if the passed entity is a valid target for this action.
    /// </summary>
    private Func<Team, SidedEntity, bool> validForActionFunction;

    public ActionButtonRequireClick(string actionName, int id) : base(actionName, id) { }

    public ActionButtonRequireClick setMainActionFunction(Action<SidedEntity, SidedEntity> function) {
        this.mainFunctionDelayed = function;
        return this;
    }

    #region Constructor methods:

    public ActionButtonRequireClick setValidForActionFunction(Func<Team, SidedEntity, bool> function) {
        this.validForActionFunction = function;
        return this;
    }

    public ActionButtonRequireClick setEntitySelecterFunction(Func<IEnumerable<SidedEntity>, SidedEntity, SidedEntity> function) {
        this.entitySelecterFunctionDelayed = function;
        return this;
    }

    #endregion

    public bool isValidForAction(Team team, SidedEntity entity) {
        return this.validForActionFunction(team, entity);
    }

    // Overloads of parent class functions to provide the clicked argument.

    [ServerSideOnly]
    public void callFunction(BuildingBase building, SidedEntity clicked) {
        this.mainFunctionDelayed.Invoke(building, clicked);
    }

    [ServerSideOnly]
    public void callFunction<T>(List<T> list, SidedEntity clickedObject) where T : SidedEntity {
        List<SidedEntity> candidates = this.getCandidates(list);

        if(this.entitySelecterFunctionDelayed == null) {
            // Call function on all.
            foreach(SidedEntity entity in candidates) {
                this.mainFunctionDelayed.Invoke(entity, clickedObject);
            }
        }
        else {
            SidedEntity e = this.entitySelecterFunctionDelayed.Invoke(candidates, clickedObject);
            this.mainFunctionDelayed.Invoke(e, clickedObject);
        }
    }
}
