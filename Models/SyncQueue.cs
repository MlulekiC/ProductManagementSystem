using System.ComponentModel.DataAnnotations;

namespace ProductManagementSystem.Models
{
    public class SyncQueue
    {
        #region Private Properties
        private System.String _sUsername = "";
        private System.String _sURL = "";
        private System.String _sSyncData = "";
        private System.String _sStatus = "";
        #endregion Private Properties

        #region Public Properties
        [Key]
        public Int32 QueueID { get; set; }
        public String UserName
        {
            get { return _sUsername; }
            set { _sUsername = value; }
        }
        public String SyncURL
        {
            get { return _sURL; }
            set { _sURL = value; }
        }
        public String SyncData
        {
            get { return _sSyncData; }
            set { _sSyncData = value; }
        }
        public String Status
        {
            get { return _sStatus; }
            set { _sStatus = value; }
        }
        #endregion Public Properties
    }
}
