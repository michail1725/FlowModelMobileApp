using SQLite;

namespace FlowModelMobileApp
{
   [Table("Materials")]
   public class Materials
   {
      [PrimaryKey, AutoIncrement]
      public int MaterialId { get; set; }
      public string MaterialName { get; set; }
   }

   [Table("Properties")]
   public class Properties
   {
      [PrimaryKey, AutoIncrement]
      public int PropertyId { get; set; }
      public string PropertyName { get; set; }
      public string PropertyUnit { get; set; }
      public string PropertyType { get; set; }

   }

   [Table("Material_has_Properties")]
   public class Material_has_Properties
   {
      [PrimaryKey, AutoIncrement]
      public int LinkId { get; set; }
      public int MaterialId { get; set; }
      public int PropertiesId { get; set; }
      public double Value { get; set; }
   }
}
