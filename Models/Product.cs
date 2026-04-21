using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagementSystem.Models
{
    public class Product
    {
        #region Private properties

        private System.String _sCode = "";
        private System.String _sProductCode = "";
        private System.String _sName = "";
        private System.String _sDescription = "";
        private System.Int32 _iCategoryID = 0;
        private System.String _sCategory = "";

        #endregion Private properties

        #region Public properties
        public System.Int32 ProductID{ get; set; }
        public System.String ProductCode
        {
            get { return _sProductCode; }
            set { _sProductCode = value; }
        }

        [Required]
        public System.String ProductName
        {
            get { return _sName; }
            set { _sName = value; }
        }

        [Required]
        [StringLength(500, MinimumLength = 10)]
        public System.String Description
        {
            get { return _sDescription; }
            set { _sDescription = value; }
        }
        public System.Int32 CategoryID
        {
            get { return _iCategoryID; }
            set { _iCategoryID = value; }
        }

        [NotMapped]
        public System.String Category
        {
            get { return _sCategory; }
            set { _sCategory = value; }
        }
        [Required]
        public System.Decimal Price { get; set; }
        public String? Image { get; set; }

        public System.DateTime CreateDate { get; set; }

        public System.Int32? CreateUser { get; set; }
        [NotMapped]
        public System.DateTime AmendDate { get; set; }

        [NotMapped]
        public IEnumerable<Category>? Categories { get; set; }

        [NotMapped]
        [Display(Name = "Upload Image")]
        public IFormFile? ImageFile { get; set; }

        #endregion Public properties
    }
}
