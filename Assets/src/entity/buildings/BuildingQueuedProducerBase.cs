using fNbt;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuildingQueuedProducerBase : BuildingBase {

    private float trainingProgress;
    public List<RegisteredObject> trainingQueue;

    public override void onAwake() {
        base.onAwake();

        this.trainingQueue = new List<RegisteredObject>(this.getQueueSize());
    }

    protected override void preformTask(float deltaTime) {
        if(this.trainingQueue.Count != 0) {
            bool teamHasRoom = true; //TODO this.getTeam().getTroopCount() <= this.getTeam().getMaxTroopCount();

            if(teamHasRoom) {
                this.trainingProgress += deltaTime;
            }

            UnitBase nextInQueue = this.trainingQueue[0].getPrefab().GetComponent<UnitBase>();
            if(this.trainingProgress >= nextInQueue.getData().getProductionTime() && teamHasRoom) {
                Vector2 v = Random.insideUnitCircle * 2f;
                float i = 1.5f;
                v.x += (v.x < 0 ? -i : i);
                v.y += (v.x < 0 ? -i : i);

                Vector3 pos = this.transform.position + new Vector3(v.x, 0, v.y);
                RegisteredObject regObj = this.trainingQueue[0];
                this.trainingQueue.RemoveAt(0);

                SpawnInstructions<SidedEntity> instructions = this.map.spawnEntity<SidedEntity>(
                    regObj,
                    pos,
                    Quaternion.Euler(0, Random.Range(0, 359), 0));
                instructions.getObj().setTeam(this.getTeam());
                instructions.spawn();

                this.trainingProgress = 0;
            }
        }
    }

    public abstract int getQueueSize();

    /// <summary>
    /// Tries to add an object to the creation queue, retuning true if it was added
    /// or false if the queue was full.
    /// </summary>
    public bool tryAddToQueue(RegisteredObject obj) {
        if(this.trainingQueue.Count < this.getQueueSize()) {
            this.trainingQueue.Add(obj);
            return true;
        }
        else {
            return false;
        }
    }

    public float getTrainingProgress() {
        return this.trainingProgress;
    }

    public override void readFromNbt(NbtCompound tag) {
        base.readFromNbt(tag);

        this.trainingProgress = tag.getFloat("trainingProgress");

        NbtList list = tag.getList("trainingQueue");
        foreach(NbtInt integer in list) {
            this.trainingQueue.Add(Registry.getObjectFromRegistry(integer.Value));
        }
    }

    public override void writeToNbt(NbtCompound tag) {
        base.writeToNbt(tag);

        tag.setTag("trainingProgress", this.trainingProgress);

        //TODO check
        NbtList list = new NbtList("trainingQueue", NbtTagType.Int);
        foreach(RegisteredObject ro in this.trainingQueue) {
            list.Add(new NbtInt(ro.getId()));
        }
        tag.Add(list);
    }
}