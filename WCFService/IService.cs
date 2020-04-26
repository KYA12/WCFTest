using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFService
{
	[ServiceContract]
	public interface IService
	{
		[OperationContract]
		bool SaveCheques(Cheque cheque);

		[OperationContract]
		ICollection<Cheque> GetChequesPack(int size);
	}

	[DataContract]
	public class Cheque
	{
		[DataMember]
		public Guid Id { get; set; }

		[DataMember]
		public decimal Summ { get; set; }

		[DataMember]
		public string Number { get; set; }

		[DataMember]
		public decimal Discount { get; set; }

		[DataMember]
		public string[] Articles { get; set; }

	}
}