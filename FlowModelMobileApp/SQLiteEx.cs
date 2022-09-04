using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FlowModelMobileApp
{
   public interface ISQLite
   {
      SQLiteConnection GetConnection();
   }
}
