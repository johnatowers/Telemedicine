using System; 
using System.Linq; 

namespace Telemedicine.API.Models
{
    //
    public class Document
    {
        public int id {get; set;}

        public string Url {get; set;}

        public string Description {get; set;}

        public DateTime DateAdded {get; set;}

        public string PublicId {get; set;}

        public User User {get; set;}

        public int UserId {get; set;}

        public string Type { get; set; }
    }
}
