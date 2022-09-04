using SQLite;

namespace FlowModelMobileApp
{
   [Table("users")]
   public class Users
   {
      [PrimaryKey, AutoIncrement]
      public int UserId { get; set; }
      public string Login { get; set; }
      public string Password { get; set; }
      public string Role { get; set; }
   }
}

