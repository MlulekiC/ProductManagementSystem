using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagementSystem.Models
{
    public class User
    {
        private System.String _sUsername = "";
        private System.String _sPassword1 = "";
        private System.String _sPassword2 = "";
        private System.String _sStatus = "";

        public System.Int32 UserId { get; set; }

        [Required]
        [EmailAddress]
        public String UserName
        {
            get { return _sUsername; }
            set { _sUsername = value; }
        }

        [Required]
        [DataType(DataType.Password)]
        public String Password
        {
            get { return _sPassword1; }
            set { _sPassword1 = value; }
        }

        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public String ConfirmPassword
        {
            get { return _sPassword2; }
            set { _sPassword2 = value; }
        }
        public String Status
        {
            get { return _sStatus; }
            set { _sStatus = value; }
        }
        public System.DateTime CreateDate { get; set; }
        public System.Int32 CreateUser { get; set; }
    }
}
