using UnityEngine;

public class RndNumber : IRandomGenerator {

    private static RndNumber _instance;

    public static RndNumber instance {
        get {
            if(_instance == null) {
                _instance = new RndNumber();
            }
            return _instance;
        }
    }

    public bool rndBool() {
        return Random.Range(0, 2) == 1;
    }

    public float rndFloat(float min, float max) {
        // TODO is this leaving about a number because max is inclusive?
        return Random.Range(min, max);
    }

    public int rndInt(int min, int max) {
        return Random.Range(min, max);
    }

    public Quaternion rndYRot() {
        return Quaternion.Euler(0, Random.Range(0, 359), 0);
    }
}
