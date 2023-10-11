namespace ChatAppDemo.Data;

public class User
{
    public int Id { get; set; }
    
    public string SocketId { get; set; }
    
    public string Name { get; set; }
    
    public List<User> Contacts { get; set; }
    
    public List<Chat> Chats { get; set; }
}