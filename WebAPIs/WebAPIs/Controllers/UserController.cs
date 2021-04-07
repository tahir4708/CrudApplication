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
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserController(IConfiguration configuration, IWebHostEnvironment web)
        {
            _configuration = configuration;
            _webHostEnvironment = web;
        }
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select EmployeeId,EmployeeName,Department,convert(varchar(10),DateOfJoining,120),PhotoFileName from Employee";
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
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Employee employee)
        {
            string query = @"insert into Employee(EmployeeName,Department,DateOfJoining,PhotoFileName) 
            values('" + employee.EmployeeName + @"','" + employee.Department + @"','" + employee.DateOfjoining+ @"',
                   '" + employee.PhotoFileName + @"')";

            
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
        public JsonResult Put(Employee employee)
        {
            string query = @"update Employee set EmployeeName = '" + employee.EmployeeName+ @"',
                                    Department = '" + employee.Department+ @"',
                                    DateOfJoining = '" + employee.DateOfjoining+ @"',
                                    PhotoFileName = '" + employee.PhotoFileName+ @"'
                                    where EmployeeId=" + employee.EmployeeId+ @"";

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
            string query = @"delete from  Employee where EmployeeId=" + id + @"";

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
            return new JsonResult("Deleted Successfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = _webHostEnvironment.ContentRootPath + "/photos/" + fileName;
                using(var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(fileName);
            }
            catch (Exception)
            {

                return new JsonResult("anonymous.png");
            }
        }

        [Route("GetAllDepartmentName")]
        [HttpPost]
        public JsonResult GetAllDepartmentName()
        {
            string query = @"select Departmentid,DepartmentName from Department";
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
            return new JsonResult(table);
        }
        [Route("getSpData/{id}")]
        [HttpGet]
        public JsonResult GetDataFromSP(int id)
        {
            string query = @"GetEmployeData";
            DataTable table = new DataTable();
            string sql = _configuration.GetConnectionString("EmployeeAppConfig");
            SqlDataReader sqlDataReader;
            using (SqlConnection sqlConnection = new SqlConnection(sql))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(new SqlParameter("@id", id));
                    sqlDataReader = sqlCommand.ExecuteReader();
                    table.Load(sqlDataReader);
                    sqlConnection.Close();
                }
            }
            return new JsonResult(table);
        }
    }
}
