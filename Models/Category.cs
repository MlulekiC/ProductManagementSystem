using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagementSystem.Models
{
    public class Category
    {
        #region Private properties

        private System.String _sCode = "";
        private System.String _sName = "";
        private System.String _sStatus = "";

        #endregion Private properties

        #region Public properties
        public System.Int32 CategoryID { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]{3}\d{3}$", ErrorMessage = "Product Code must be 3 letters followed by 3 numbers (e.g., ABC123)")]
        public String CategoryCode
        {
            get { return _sCode; }
            set { _sCode = value; }
        }

        [Required]
        public System.String Name
        {
            get { return _sName; }
            set { _sName = value; }
        }
        public System.String Status
        {
            get { return _sStatus; }
            set { _sStatus = value; }
        }
        public System.DateTime CreateDate { get; set; }
        public System.Int32? CreateUser { get; set; }

        [NotMapped]
        public System.DateTime AmendDate { get; set; }
        #endregion Public properties
    }
}
