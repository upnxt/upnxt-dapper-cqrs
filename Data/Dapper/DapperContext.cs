using System;
using System.Data;
using System.Data.SqlClient;

namespace Data.Dapper
{
    public interface IDapperContext : IDisposable
    {
        IDbConnection Connection { get; }
        T Transaction<T>(Func<IDbTransaction, T> query);
        IDbTransaction BeginTransaction();
        void Commit();
        void Rollback();
    }


    public class DapperContext : IDapperContext
    {
        private readonly string _connectionString;
        private IDbConnection _connection;

        public DapperContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbTransaction _transaction { get; set; }

        /// <summary>
        ///     Get the current connection, or open a new connection
        /// </summary>
        public IDbConnection Connection
        {
            get
            {
                if (_connection == null)
                    _connection = new SqlConnection(_connectionString);

                if (string.IsNullOrWhiteSpace(_connection.ConnectionString))
                    _connection.ConnectionString = _connectionString;

                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();

                return _connection;
            }
        }

        /// <summary>
        ///     Start a new transaction if one is not already available
        /// </summary>
        public IDbTransaction BeginTransaction()
        {
            if (_transaction == null || _transaction.Connection == null)
                _transaction = Connection.BeginTransaction();

            return _transaction;
        }

        /// <summary>
        ///     Perform a transactionless query
        /// </summary>
        public T Transaction<T>(Func<IDbTransaction, T> query)
        {
            using (var connection = Connection)
            {
                using (var transaction = BeginTransaction())
                {
                    try
                    {
                        var result = query(transaction);
                        transaction.Commit();

                        return result;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        ///     Commit and dispose of the transaction
        /// </summary>
        public void Commit()
        {
            try
            {
                _transaction.Commit();
                _transaction.Dispose();
                _transaction = null;
            }
            catch (Exception ex)
            {
                if (_transaction != null && _transaction.Connection != null)
                    Rollback();

                throw new NullReferenceException("Tried Commit on closed Transaction", ex);
            }
        }

        /// <summary>
        ///     Rollback and dispose of the transaction
        /// </summary>
        public void Rollback()
        {
            try
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null;
            }
            catch (Exception ex)
            {
                throw new NullReferenceException("Tried Rollback on closed Transaction", ex);
            }
        }

        /// <summary>
        ///     Dispose of the transaction and close the connection
        /// </summary>
        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }

            if (_connection != null && _connection.State != ConnectionState.Closed)
            {
                _connection.Close();
                _connection = null;
            }
        }
    }
}