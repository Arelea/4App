using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp4A.Models;

namespace WebApp4A.Controllers
{
    public class BookController : Controller
    {

         private string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetBook()
        {
            DataTable tdbl = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter("ShowBook", connection);
                    dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dataAdapter.Fill(tdbl);
                }
                return View(tdbl);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new BookModel());
        }

        // Конец
        //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _

        // Начало. Пост создание новых полей лабораторного склада

        [HttpPost]
        public ActionResult Create(BookModel bookModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        SqlCommand command = new SqlCommand("AddBook", sqlConnection)
                        {
                            CommandType = CommandType.StoredProcedure
                        };

                        command.Parameters.AddWithValue("in_Name", bookModel.Name);
                        command.Parameters.AddWithValue("in_Author", bookModel.Author);
                        command.Parameters.AddWithValue("in_Year", bookModel.Year);
                        //command.Parameters.AddWithValue("in_Content", bookModel.Content);
                        command.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                return RedirectToAction(nameof(Create));
            }
            return View(ModelState.Values);
        }

        [HttpGet]
        public ActionResult EditDeleteAllData()
        {
            BookModel bookModel = new BookModel();
            try
            {               
                DataTable tdbl = new DataTable();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter sqlDA = new SqlDataAdapter("ShowBook", connection);
                    sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sqlDA.Fill(tdbl);
                }
                bookModel.listy = tdbl;
                return View(bookModel);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        public ActionResult EditTableRow(int? id)
        {
            BookModel book = new BookModel();
            if (id > 0)
            {
                book = FetchRecordById(id);
            }           
            return View(book);
        }

        [HttpPost]
        public ActionResult EditTableRow(BookModel bookModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        SqlCommand command = new SqlCommand("UpdateBook", sqlConnection)
                        {
                            CommandType = CommandType.StoredProcedure
                        };

                        command.Parameters.AddWithValue("in_Id", bookModel.Id);
                        command.Parameters.AddWithValue("in_Name", bookModel.Name);
                        command.Parameters.AddWithValue("in_Author", bookModel.Author);
                        command.Parameters.AddWithValue("in_Year", bookModel.Year);
                        //command.Parameters.AddWithValue("in_Content", bookModel.Content);
                        command.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                return RedirectToAction(nameof(EditDeleteAllData));
            }
            return View(ModelState.Values);
        }

        // Конец
        //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _

        // Начало. Вьюха удаления. Получаем значения той записи, котору. хотим удалить по Id

        [HttpGet]
        public ActionResult DeleteTableRow(int? id)
        {
            BookModel bookModel = FetchRecordById(id);
            return View(bookModel);
        }

        // Конец
        //_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _

        // Начало. Пост удаления

        [HttpPost, ActionName("DeleteTableRow")]
        public ActionResult DeleteTableRowConfirm(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        SqlCommand command = new SqlCommand("DeleteBook", sqlConnection)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        command.Parameters.AddWithValue("in_Id", id);
                        command.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                return RedirectToAction(nameof(EditDeleteAllData));
            }
            return View(ModelState.Values);
        }

        [NonAction]
        public BookModel FetchRecordById(int? id)
        {
            BookModel bookModel = new BookModel();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    DataTable dataTable = new DataTable();

                    sqlConnection.Open();
                    SqlDataAdapter sqlDA = new SqlDataAdapter("ViewRecordById", sqlConnection);
                    sqlDA.SelectCommand.Parameters.AddWithValue("in_Id", id);
                    sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sqlDA.Fill(dataTable);
                    if (dataTable.Rows.Count == 1)
                    {
                        bookModel.Id = Convert.ToInt32(dataTable.Rows[0]["Id"].ToString());

                        bookModel.Name = dataTable.Rows[0]["Name"].ToString();

                        bookModel.Author = dataTable.Rows[0]["Author"].ToString();

                        bookModel.Year = Convert.ToInt32(dataTable.Rows[0]["Year"].ToString());

                        //bookModel.Content = (System.Xml.XmlDocument)dataTable.Rows[0]["Content"];
                       
                    }
                    return bookModel;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

      
    }
}