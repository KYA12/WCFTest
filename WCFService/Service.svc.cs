using Dapper;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFService
{
	public class Service : ServiceBase, IService
	{
		private readonly ILog log;
		private readonly string connection = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
		public Service()
		{
			//Get logger
			log = LogManager.GetLogger(typeof(Service));
		}

		/* Save_Cheques() saves cheque's object to the database*/
		public bool Save_Cheques(Cheque cheque)
		{
			try
			{	
				using (IDbConnection con = new SqlConnection(connection))
				{
					log.Info("Enter Save_Cheques()");
					// Check if connection is opened or closed, and open it
					if (con.State == ConnectionState.Closed)
						con.Open();
					string sqlParameter;
					// Convert articles from array of strings to the string with delimiter
					if (cheque.Articles != null)
					{
						sqlParameter = string.Join(";", cheque.Articles);
					}
					else sqlParameter = null;
					// Create value with parameters of the cheque's object for sql query
					DynamicParameters parameters = new DynamicParameters();
					parameters.Add("@cheque_id", cheque.Id);
					parameters.Add("@cheque_number", cheque.Number);
					parameters.Add("@summ", cheque.Summ);
					parameters.Add("@discount", cheque.Discount);
					parameters.Add("@articles", sqlParameter);
					// Execute stored procedure with Dapper
					if (con.Execute("SaveCheque", parameters, commandType: CommandType.StoredProcedure) > 0)
						return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				log.Error(ex);
				return false;
			}
		}

		/* Get_Cheques_Pack(int) returns collection of the last added cheques*/
		public ICollection<Cheque> Get_Cheques_Pack(int amount)
		{
			try
			{
				log.InfoFormat("Enter Get_Cheques_Pack(), value = {0}", amount);
				ICollection<Cheque> chequesList = new List<Cheque>();
				DynamicParameters parameters = new DynamicParameters();

				using (IDbConnection con = new SqlConnection(connection))
				{
					if (con.State == ConnectionState.Closed)
						con.Open();
					// Add parameter's value to the sql query
					parameters.Add("@amount", amount);
					// Execute sql query with mapping sql data with model's properties
					chequesList = con.Query<Guid, string, decimal, decimal, string, Cheque>
						("GetChequesPack", (a, b, c, d, e) =>
							new Cheque
							{
								Id = a,
								Number = b,
								Summ = c,
								Discount = d,
								Articles = e?.Split(';')
							}, parameters, splitOn: "*", commandType: CommandType.StoredProcedure).ToList();
					log.Info("Exit Get_Cheques_Pack()");
					return chequesList;
				}
			}
			catch (Exception ex)
			{
				log.Error(ex);
				return null;
			}
		}
	}
}
