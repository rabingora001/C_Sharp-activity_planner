
using System.ComponentModel.DataAnnotations;
using DojoActivity.Models;

namespace DojoActivity.Models
{
    public class Participant
    {
        [Key]
        public int ParticipantId {get;set;}
        public int UserId {get;set;}
        public int ActivityId {get;set;}
        public User User {get;set;}
        

    }
}