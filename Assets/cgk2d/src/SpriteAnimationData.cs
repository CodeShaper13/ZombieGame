using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "CGK/2d/Sprite Animation Data", order = 1)]
public class SpriteAnimationData : ScriptableObject {

    public Sprite unmovingFront;
    public Sprite unmovingBack;

    public Sprite[] walkingFrontFrames;
    public Sprite[] walkingBackFrames;

    public Sprite getWalkingFrontSprite(int frame) {
        return this.walkingFrontFrames[frame];
    }

    public Sprite getWalkingBackSprite(int frame) {
        return this.walkingBackFrames[frame];
    }
}
