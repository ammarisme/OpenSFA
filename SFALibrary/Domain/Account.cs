using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFALibrary.Common;

namespace SFALibrary.Domain
{
    [FileName("Accounts")]
    [Serializable()]
    public class Account
    {
        private string id = string.Empty;
        private string firstName = string.Empty;
        private string lastName = string.Empty;
        private string address = string.Empty;
        private string phoneNumber2 = string.Empty;
        private string status = string.Empty;
        private string designation = string.Empty;

        [DBField("Id")]
        [PrimaryKey]
        public string Id { get { return id; } set { id = value.Trim(); } }
        [DBField("FirstName")]
        public string FirstName { get { return firstName; } set { firstName= value.Trim(); } }
        [DBField("LastName")]
        public string LastName { get { return lastName; } set { lastName= value.Trim(); } }
        [DBField("Address")]
        public string Address { get { return address; } set { address= value.Trim(); } }
        [DBField("PhoneNumber2")]
        public string PhoneNumber2 { get { return phoneNumber2; } set { phoneNumber2= value.Trim(); } }
        [DBField("Designation")]
        public string Designation { get { return designation; } set { designation = value.Trim(); } }
    }
}
