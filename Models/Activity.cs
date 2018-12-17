using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DojoActivity.Models;

namespace DojoActivity.Models
{
    public class Activity
    {
        [Key]
        public int ActivityId {get;set;}

            [Required]
            [MinLength(2, ErrorMessage="Title should be more than 2 characters!!")]       
        public string Title {get;set;}

            [Required]
        public DateTime Date {get;set;}

            [Required]
        public DateTime Time {get;set;}

            [Required]
            [RegularExpression(@"^(?:\d|[,\.])+$", ErrorMessage = "Must choose a positive number.")]
        public int Duration {get;set;}

        public string DurationForm{get;set;}

            [Required]
            [MinLength(10, ErrorMessage="Description should be more than 10 characters!!")]
        public string Description {get;set;}

        
        public DateTime CreatedAt {get;set;} = DateTime.Now;

        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public int UserId {get;set;}

        public User Creator {get;set;}
        public List<Participant> participant {get;set;}

    }
}