using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationInsight.Services
{
    public interface IERXDHAService
    {
        Task<bool> UploadeRxTransaction(string pstrXMLFilename, string pstrXMLFileContent, string facilityLogin, string facilityPwd, string clinicianLogin, string clinicianPwd, int _eRxNo);// Funtion to Handle Uploading of eRx Files
        string GeteRxTransactions(string pstrlogin, string pstrpwd, string pstrmemberID, int _eRxReferenceNo);
        Task<string> SearcheRxTransactions(string pstrlogin, string pstrpwd, int _direction, string pstrFacilityLic, string pstrClinicianLic,
           string pstrmemberID, int? _eRxReferenceNo, int _transactionStatus, string pstrtransactionFromDate, string pstrtransactionToDate,
           int _minRecordCount, int _maxRecordCount);
        Task<string> DownloadeRxTransactions(string pstrlogin, string pstrpwd, string pstrfileID);
        int SeteRxTransactionsDownloaded(string pstrlogin, string pstrpwd, string pstrfileID);
    }
}
