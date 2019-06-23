using System;

namespace WeightApp.Db
{
    public class UserEntity
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public bool IsMale { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
