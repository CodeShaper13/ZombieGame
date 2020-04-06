// T is the Player, or whatever is clicking/
public interface IClickable<T>  {

    bool onClick(T player);
}