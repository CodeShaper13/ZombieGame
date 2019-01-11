using System;
using System.Collections.Generic;

public class TaskManager {

    private static TaskManager singleton;

    private readonly RegisteredTask taskIdle;
    private readonly RegisteredTask taskBuild;

    private List<RegisteredTask> allTasks;

    private TaskManager() {
        TaskManager.singleton = this;

        this.allTasks = new List<RegisteredTask>();

        this.taskIdle = this.register(0, typeof(TaskIdle));
        this.taskBuild = this.register(1, typeof(TaskConstructBuilding));
    }

    // Called from Main on startup.
    public static void bootstrap() {
        new TaskManager();
    }

    private RegisteredTask register(int id, Type type) {
        RegisteredTask task = new RegisteredTask(id, type);
        this.allTasks.Add(task);
        return task;
    }

    public static Type getTaskFromId(int id) {
        foreach(RegisteredTask rt in TaskManager.singleton.allTasks) {
            if(rt.id == id) {
                return rt.type;
            }
        }
        return typeof(TaskIdle);
    }

    public static int getIdFromTask(ITask task) {
        Type type = task.GetType();
        foreach(RegisteredTask rt in TaskManager.singleton.allTasks) {
            if(rt.type == type) {
                return rt.id;
            }
        }
        return 0;
    }

    private class RegisteredTask {

        public int id;
        public Type type;

        public RegisteredTask(int id, Type taskConstructBuilding) {
            this.id = id;
            this.type = taskConstructBuilding;
        }
    }
}
