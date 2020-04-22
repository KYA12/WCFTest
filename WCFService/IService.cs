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
		bool Save_Cheques(Cheque cheque);

		[OperationContract]
		ICollection<Cheque> Get_Cheques_Pack(int pack_size);
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