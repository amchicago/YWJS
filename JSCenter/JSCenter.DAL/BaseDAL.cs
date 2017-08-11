using FluentData;

namespace JSCenter.DAL
{
   public abstract class BaseDAL
    {
        //http://fluentdata.codeplex.com/documentation
        private volatile static IDbContext _instance = null;
        private static readonly object lockHelper = new object();
        protected static IDbContext Db
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockHelper)
                    {
                        if (_instance == null)
                            _instance = new DbContext().ConnectionStringName("SqliteConn", new SqliteProvider());
                    }
                }
                return _instance;
            }
        }
    }
}
