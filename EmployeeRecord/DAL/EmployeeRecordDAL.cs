using EmployeeRecord.Models;
using System.Data;
using System.Data.SqlClient;

namespace EmployeeRecord.DAL
{
    public interface IEmployeeRecordRepository
    {
        string AddEditEmployee(EmployeeRecords Emp);
        IEnumerable<EmployeeRecords> ManageEmployee(EmployeeRecords EMP);
        void DeleteEmployee(EmployeeRecords EMP);
    }
    public class EmployeeRecordDAL : IEmployeeRecordRepository
    {
        private readonly string _connectionString;

        public EmployeeRecordDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string AddEditEmployee(EmployeeRecords EMP)
        {
            string? ReturnID = "";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("AddEditEmployeeDetails", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@EmployeeID", EMP.EmployeeID);
                command.Parameters.AddWithValue("@EmployeeName", EMP.EmployeeName);
                command.Parameters.AddWithValue("@EmployeeAddress", EMP.EmployeeAddress);
                command.Parameters.AddWithValue("@EmployeePhone", EMP.EmployeePhone);
                command.Parameters.AddWithValue("@EmployeeEmail", EMP.EmployeeEmail);

                DataSet oDataSet = new DataSet();
                SqlDataAdapter oDataAdapter = new System.Data.SqlClient.SqlDataAdapter();
                oDataAdapter = new SqlDataAdapter(command);
                oDataAdapter.Fill(oDataSet);

                //command.ExecuteNonQuery();

                ReturnID = oDataSet.Tables[0].Rows[0]["EmployeeID"].ToString();
                connection.Close();
            }
            return ReturnID;
        }

        public void DeleteEmployee(EmployeeRecords EMP)
        {            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DeleteEmployee", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@EmployeeIDs", EMP.EmployeeIDs);
                command.ExecuteNonQuery();
                
                connection.Close();
            }            
        }


        public IEnumerable<EmployeeRecords> ManageEmployee(EmployeeRecords EMP)
        {
            var lstEmployeeReport = new List<EmployeeRecords>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("ManageEmployeeDetails", connection);
                SqlDataAdapter ODataAdapter = new SqlDataAdapter();
                DataSet oDataSet = new DataSet();
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@EmployeeID", EMP.EmployeeID);
                command.Parameters.AddWithValue("@EmployeeName", EMP.EmployeeName);
                command.Parameters.AddWithValue("@EmployeeAddress", EMP.EmployeeAddress);
                command.Parameters.AddWithValue("@EmployeePhone", EMP.EmployeePhone);
                command.Parameters.AddWithValue("@EmployeeEmail", EMP.EmployeeEmail);

                ODataAdapter = new SqlDataAdapter(command);
                ODataAdapter.Fill(oDataSet);

                lstEmployeeReport = (from r in oDataSet.Tables[0].AsEnumerable()
                               select new EmployeeRecords
                               {
                                   EmployeeID = r.Field<Guid>("EmployeeID").ToString(),
                                   EmployeeName = r.Field<string>("EmployeeName"),
                                   EmployeeAddress = r.Field<string>("EmployeeAddress"),
                                   EmployeePhone = r.Field<string>("EmployeePhone"),
                                   EmployeeEmail = r.Field<string>("EmployeeEmail"),                                   
                               }).ToList();
                connection.Close();
            }
            return lstEmployeeReport;
        }
    }
}
