namespace ChatAppDemo.Data;

public class Chat
{
    public int Id { get; set; }
    
    public List<User> ChatMembers { get; set; }
    
    public List<Message> Messages { get; set; }
}