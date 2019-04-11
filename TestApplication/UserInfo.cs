using System;
using AeroORMFramework;
using System.Collections.Generic;

namespace TestApplication
{
    public class UserInfo
    {
        [CanBeNull(false)]
        [PrimaryKey]
        public int ID { get; set; }
        [CanBeNull(false)]
        public string Login { get; set; }
        [CanBeNull(false)]
        public string Password { get; set; }
        [CanBeNull(false)]
        public string Name { get; set; }
        [CanBeNull(false)]
        public string Surname { get; set; }
        [CanBeNull(false)]
        public DateTime BirthDate { get; set; }
        [CanBeNull(false)]
        public string Email { get; set; }
        [CanBeNull(true)]
        [Json]
        public List<int> UserDocs { get; set; }
        [CanBeNull(true)]
        [Json]
        public List<int> SharedDocs { get; set; }
        [CanBeNull(true)]
        [Json]
        public byte[] UserPhoto { get; set; }

        public override string ToString()
        {
            return $"User: ID {ID}, Name {Name}, BirthDate {BirthDate}, " +
                $"Email {Email}, Pass {Password}";
        }
    }
}
