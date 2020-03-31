namespace Telemedicine.API.Models
{
    public class Select
    {
        // Both are UserIds, either the User that 'selects' another user 
        // and the user 'selected' by the other user
        public int SelectorId { get; set; }

        public int SelecteeId { get; set; }

        public User Selector { get; set; }
        public User Selectee { get; set; }
    }
}