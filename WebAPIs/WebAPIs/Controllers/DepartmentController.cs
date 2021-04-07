using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using WebAPIs.Models;
namespace WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select Departmentid,DepartmentName from Department";
            DataTable table = new DataTable();
            string sql = _configuration.GetConnectionString("EmployeeAppConfig");
            SqlDataReader sqlDataReader;
            using(SqlConnection sqlConnection = new SqlConnection(sql))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlDataReader = sqlCommand.ExecuteReader();
                    table.Load(sqlDataReader);
                    sqlConnection.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Department department)
        {
            string query = @"insert into Department values('"+department.DepartmentName+@"')";
            DataTable table = new DataTable();
            string sql = _configuration.GetConnectionString("EmployeeAppConfig");
            SqlDataReader sqlDataReader;
            using (SqlConnection sqlConnection = new SqlConnection(sql))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlDataReader = sqlCommand.ExecuteReader();
                    table.Load(sqlDataReader);
                    sqlConnection.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Department department)
        {
            string query = @"update Department set DepartmentName = '" + department.DepartmentName + @"'
                            where DepartmentId="+department.DepartmentId+@"";
                            
            DataTable table = new DataTable();
            string sql = _configuration.GetConnectionString("EmployeeAppConfig");
            SqlDataReader sqlDataReader;
            using (SqlConnection sqlConnection = new SqlConnection(sql))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlDataReader = sqlCommand.ExecuteReader();
                    table.Load(sqlDataReader);
                    sqlConnection.Close();
                }
            }
            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"delete from  Department where DepartmentId=" + id+ @"";

            DataTable table = new DataTable();
            string sql = _configuration.GetConnectionString("EmployeeAppConfig");
            SqlDataReader sqlDataReader;
            using (SqlConnection sqlConnection = new SqlConnection(sql))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlDataReader = sqlCommand.ExecuteReader();
                    table.Load(sqlDataReader);
                    sqlConnection.Close();
                }
            }
            return new JsonResult("Updated Successfully");
        }

    }
}
