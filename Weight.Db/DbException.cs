using System;

namespace WeightApp.Db
{
    public class DbException : Exception
    {
        public int Code { get; set; }

        public DbException()
        {
        }

        public DbException(string message) : base(message)
        {
        }

        public DbException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}