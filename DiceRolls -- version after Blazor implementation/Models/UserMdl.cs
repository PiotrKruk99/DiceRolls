using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiceRolls.Models
{
    public class UserMdl
    {
        public int Id { get; set; }
        public string Dice { get; set; }
        public DateTime CreationDate { get; set; }
        //public string Login { get; set; }
    }
}
