using AeroORMFramework;
using System.Collections.Generic;
using AeroORMFramework.CustomTypes;
using System;


namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Connector connector = new Connector(DataBaseSettings.ConnectionString);

            connector.AddTable<UserInfo>();
            connector.UpdateRecord<UserInfo>(new UserInfo()
            {
                ID = 6,
                Login = "login",
                Name = "Pablo",
                Surname = "surname",
                BirthDate = DateTime.Today,
                Password = "123456",
                Email = "example@hmail.com"
            });
            UserInfo userInfo = connector.GetRecord<UserInfo>("Name", "John");

            Console.WriteLine(userInfo);

            Console.ReadKey(true);
        }
    }
}
