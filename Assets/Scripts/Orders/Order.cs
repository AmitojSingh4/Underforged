
public class Order{
    public Item orderItem;
    public float orderTimer;

    public Order(Item item, float time){
        orderItem = item;
        orderTimer = time;
    }
}
// class that stores the data for orders,
// all orders are instances of this class