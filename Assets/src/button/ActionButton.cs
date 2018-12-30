using System;
using System.Collections.Generic;

// Note, sub buttons do not need an ID nor do they need to be in the list.
public class ActionButton {

    public static readonly ActionButton[] BUTTON_LIST = new ActionButton[64]; // There doesn't seem to be any limit to the size.

    // Functions to use for this.setShouldDisableFunction()
    private static bool functionIsImmutable(SidedEntity entity) { return entity.isImmutable(); }
    private static bool functionIsConstructing(SidedEntity entity) { return ((BuildingBase)entity).isConstructing(); }


    public static readonly ActionButton entityDestroy = new ActionButtonParent("Destroy", 0,
        new ActionButtonChild("Confirm")
        .setMainActionFunction((entity) => { entity.damage(null, int.MaxValue); }))
        .setShouldDisableFunction(functionIsImmutable);

    // Builder specific.
    public static readonly ActionButton builderBuild = new ActionButtonParent("Build", 1,
        new ActionButtonBuild("Camp", Registry.buildingCamp),
        new ActionButtonBuild("Producer", Registry.buildingProducer),
        new ActionButtonBuild("Workshop", Registry.buildingWorkshop),
        new ActionButtonBuild("Training House", Registry.buildingTrainingHouse),
        new ActionButtonBuild("Storeroom", Registry.buildingStoreroom),
        new ActionButtonBuild("Tower", Registry.buildingCannon),
        new ActionButtonBuild("Flag", Registry.buildingFlag)
    );

    public static readonly ActionButton builderHarvestResources = new ActionButton("Harvest", 2)
        .setMainActionFunction((unit) => {
            UnitBuilder ub = ((UnitBuilder)unit);
            ub.setTask(new TaskHarvestNearby(ub));
        });

    public static readonly ActionButton builderRepair = new ActionButtonRequireClick("Repair", 3)
        .setMainActionFunction((unit, target) => {
            UnitBuilder ub = ((UnitBuilder)unit);
            ub.setTask(new TaskRepair(ub, (BuildingBase)target)); //TODO
        })
        .setValidForActionFunction((team, entity) => {
            if(entity.getTeam() == team && entity is BuildingBase) {
                BuildingBase b = (BuildingBase)entity;
                return true;
                if(!b.isConstructing() && b.getHealth() < b.getMaxHealth()) {
                    return true;
                }
            }
            return false;
        })
        .setEntitySelecterFunction((list, clickedEntity) => {
            return Util.closestToPoint(clickedEntity.getPos(), list, (entity) => { return ((UnitBase)entity).getTask().cancelable(); });
        });

    // Fighting troop attacks.
    public static readonly ActionButton unitIdle = new ActionButton("Idle", 7)
        .setMainActionFunction((unit) => {
            ((UnitBase)unit).setTask(null);
        });

    public static readonly ActionButton unitAttackNearby = new ActionButton("Attack", 8)
        .setMainActionFunction((unit) => {
            UnitFighting uf = ((UnitFighting)unit);
            uf.setTask(new TaskAttackNearby(uf));
        });

    public static readonly ActionButton unitDefend = new ActionButton("Defend Point", 9)
        .setMainActionFunction((unit) => {
            UnitFighting uf = ((UnitFighting)unit);
            uf.setTask(new TaskDefendPoint(uf));
        });

    // Buildings (16-23)
    public static readonly ActionButton train = new ActionButtonParent("Train", 17,
        new ActionButtonTrain(Registry.unitSoldier),
        new ActionButtonTrain(Registry.unitArcher),
        new ActionButtonTrain(Registry.unitBuilder))
        .setShouldDisableFunction(functionIsConstructing);

    public static readonly ActionButton buildingRotate = new ActionButton("Rotate", 20)
        .setMainActionFunction((building) => {
            ((BuildingBase)building).rotateBuilding();
        }).setShouldDisableFunction((entity) => {
            return functionIsImmutable(entity) || functionIsConstructing(entity);
        });

    /// <summary> The mask of the action button, this is -1 on child buttons. </summary>
    private readonly int mask;
    private readonly int id;
    private readonly string displayName;

    private bool executeOnClient = false;

    /// <summary>
    /// The function that is run when the button is clicked.
    /// </summary>
    protected Action<SidedEntity> mainFunction;
    /// <summary>
    /// A function used to pick what single entity to call the function on.  This can be
    /// null and if it is, the main function is called on all.
    /// </summary>
    private Func<IEnumerable<SidedEntity>, SidedEntity> entitySelecterFunction;
    /// <summary>
    /// Called on every button every Update when they are visable.
    /// Use this to disable buttons if they should not be clicked.
    /// </summary>
    private Func<SidedEntity, bool> shouldDisableFunction;

    public ActionButton(string actionName, int id) {
        this.displayName = actionName;
        this.id = id;
        this.mask = (1 << this.id);

        // Only add parent ActionButtons to this list.
        if(id >= 0) {
            ActionButton.BUTTON_LIST[this.id] = this;
        }
    }

    #region Constructor methods:

    public ActionButton setMainActionFunction(Action<SidedEntity> function) {
        this.mainFunction = function;
        return this;
    }

    public ActionButton setEntitySelecterFunction(Func<IEnumerable<SidedEntity>, SidedEntity> function) {
        this.entitySelecterFunction = function;
        return this;
    }

    public ActionButton setShouldDisableFunction(Func<SidedEntity, bool> function) {
        this.shouldDisableFunction = function;
        return this;
    }

    public ActionButton setExecuteOnClientSide() {
        this.executeOnClient = true;
        return this;
    }

    #endregion

    /// <summary>
    /// Returns true if this ActionButton's interactability should be disabled.
    /// </summary>
    public bool shouldDisable(SidedEntity thisEntity) {
        return this.shouldDisableFunction == null ? false : this.shouldDisableFunction.Invoke(thisEntity);
    }

    /// <summary>
    /// Calls this button's function on the passed SidedEntity.
    /// </summary>
    [ServerSideOnly]
    public void callFunction(BuildingBase entity) {
        // No need to check masks, as this is a building and there is only ever one selected at a time.
        this.mainFunction.Invoke(entity);
    }

    /// <summary>
    /// Calls this button's function on the passed SidedEntities.
    /// 
    /// If this button's entitySelector function is null,
    ///     this button's function is called on all of the SidedEntities.
    /// If it is not null
    ///     this.button's function is called on the SidedEntity that is returned from the entitySelector function.
    /// </summary>
    [ServerSideOnly]
    public virtual void callFunction<T>(List<T> list) where T : SidedEntity {
        List<SidedEntity> candidates = this.getCandidates(list);

        if(this.entitySelecterFunction == null) {
            // Call function on all.
            foreach(SidedEntity entity in candidates) {
                this.mainFunction.Invoke(entity);
            }
        }
        else {
            // Call function on a specific SidedEntity.
            SidedEntity e = this.entitySelecterFunction.Invoke(candidates);
            this.mainFunction.Invoke(e);
        }
    }

    /// <summary>
    /// If true, the action function should be called on the client side, not the server.
    /// </summary>
    public bool executeOnClientSide() {
        return this.executeOnClient;
    }

    /// <summary>
    /// Returns the mask of this button, or if this is a child button, it's parent's mask.
    /// </summary>
    public virtual int getMask() {
        return this.mask;
    }

    /// <summary>
    /// Returns the text to display on the button.  Override to provide fancier text.
    /// </summary>
    public virtual string getText() {
        return this.displayName;
    }

    /// <summary>
    /// Returns the ID of this button.
    /// </summary>
    public int getID() {
        return this.id;
    }

    /// <summary>
    /// Weeds out the SidedEntities that can't have this buttons function called on them by looking at their button mask.
    /// The valid SidedEntities are returned.
    /// </summary>
    protected List<SidedEntity> getCandidates<T>(List<T> list) where T : SidedEntity {
        int buttonMask = this.getMask();
        List<SidedEntity> candidates = new List<SidedEntity>(list.Count);

        // Weed out the ones that can't call this action by looking at the mask.
        foreach(SidedEntity entity in list) {
            if((entity.getButtonMask() & buttonMask) != 0) {
                candidates.Add(entity);
            }
        }

        return candidates;
    }
}