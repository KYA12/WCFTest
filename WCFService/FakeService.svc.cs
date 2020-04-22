using log4net;
using log4net.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace WCFService
{
	public class FakeService : ServiceBase, IService
	{
		private readonly ILog log;
		public FakeService()
		{
			//Get logger
			log = LogManager.GetLogger(typeof(FakeService));
		}
		public bool Save_Cheques(Cheque cheque)
		{
			try
			{
				log.Info("Enter Save_Cheques()");
				string path = AppDomain.CurrentDomain.BaseDirectory;
				string directory = path + "\\App_Data";
				string fileName = ConfigurationManager.AppSettings["FileName"];
				string name = Path.Combine(directory, fileName);
				if (!Directory.Exists(directory))
					Directory.CreateDirectory(directory); 
				FileStream fs = File.Open(name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				using (StreamWriter writer = new StreamWriter(fs))
				{
					JsonSerializer serializer = new JsonSerializer();
					serializer.Serialize(writer, cheque);
				}
				log.Info("Exit Save_Cheque()");
				return true;
			}
			catch (Exception ex)
			{
				log.Error(ex);
				return false;
			}
		}
		public ICollection<Cheque> Get_Cheques_Pack(int amount)
		{
			try
			{
				log.InfoFormat("Enter Get_Cheques_Pack(), value: {0}", amount);
				ICollection<Cheque> chequesList = new List<Cheque>(amount);
				for (int i = 0; i < amount; i++)
				{
					Cheque cheque = new Cheque
					{
						Id = Guid.NewGuid(),
						Number = i.ToString(),
						Summ = 100,
						Discount = 100,
						Articles = new string[] { "one", "people", "bird" }
					};
					chequesList.Add(cheque);
				}
				log.Info("Exit Get_Cheques_Pack()");
				return chequesList;
			}
			catch (Exception ex)
			{
				log.Error(ex);
				return null;
			}
		}

	}
}