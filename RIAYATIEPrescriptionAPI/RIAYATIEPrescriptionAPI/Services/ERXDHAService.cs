using DomainModel.Models.Request;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RIAYATIEPrescriptionAPI.Services
{
    public class ERXDHAService : ERXDHAServiceBase, IERXDHAService
    { 
        public Task<bool> UploadeRxTransaction(string pstrXMLFilename, string pstrXMLFileContent, string facilityLogin, string facilityPwd, string clinicianLogin, string clinicianPwd, int _eRxNo)
        {
            //bool UpldStatus = false;
            //try
            //{
            //    if (!string.IsNullOrEmpty(pstrXMLFilename) && !string.IsNullOrEmpty(facilityLogin) && !string.IsNullOrEmpty(facilityPwd)
            //        && !string.IsNullOrEmpty(clinicianLogin) && !string.IsNullOrEmpty(clinicianPwd))
            //    {
            //        byte[] fileContent = Encoding.UTF8.GetBytes(pstrXMLFileContent);
            //        string fileName = Path.GetFileName(pstrXMLFilename);
            //        int eRxReferenceNo; // Output
            //        string errorMessage; //Output 
            //        byte[] errorReport;//Output 

            //       OracleDataAccessRepository.GetInstance.InsertErXHistory(_eRxNo, Path.GetFileNameWithoutExtension(pstrXMLFilename), 0);
            //        int i = OracleDataAccessRepository.GetInstance.UploadERxRequest(facilityLogin, facilityPwd, clinicianLogin, clinicianPwd,
            //            fileContent, fileName, out eRxReferenceNo, out errorMessage, out errorReport);


            //        clsEvntvwrLogging.fnMsgWritter(MessageResponse(i));
            //        if (i == 1 || i == 0)
            //        {
            //            ClsGetRxData.InsertErXHistory(_eRxNo, Path.GetFileNameWithoutExtension(pstrXMLFilename), 1);
            //            ClsGetRxData.UpdateStatusCode(_eRxNo, 1);
            //            if (eRxReferenceNo != 0)
            //            {
            //                bool res = ClsGetRxData.UpdateResponseCode(_eRxNo, 1, eRxReferenceNo);
            //                if (res)
            //                {
            //                    ClsGetRxData.InsertErXHistory(_eRxNo, Path.GetFileNameWithoutExtension(pstrXMLFilename), 1);
            //                    ClsGetRxData.InsertXMlData(_eRxNo, 1, pstrXMLFilename, pstrXMLFileContent, "File uploaded successfully");
            //                }
            //                else if (!res)
            //                {
            //                    ClsGetRxData.InsertErXHistory(_eRxNo, Path.GetFileNameWithoutExtension(pstrXMLFilename), -1);
            //                    ClsGetRxData.InsertXMlData(_eRxNo, -1, pstrXMLFilename, pstrXMLFileContent, "Error in file uploading");
            //                }
            //            }
            //            UpldStatus = true;
            //        }
            //        else
            //        {
            //            ClsGetRxData.UpdateStatusCode(_eRxNo, -1);
            //            ClsGetRxData.InsertErXHistory(_eRxNo, Path.GetFileNameWithoutExtension(pstrXMLFilename), -1);
            //            ClsGetRxData.InsertXMlData(_eRxNo, -1, pstrXMLFilename, pstrXMLFileContent, "Error in file uploading");
            //        }

            //        if (!string.IsNullOrEmpty(errorMessage))
            //        {
            //            clsEvntvwrLogging.fnMsgWritter(errorMessage);
            //        }
            //        if (errorReport != null && errorReport.Length > 0)
            //        {
            //            string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // System.Reflection.Assembly.GetExecutingAssembly().Location;
            //            string errorpath = Path.GetDirectoryName(tempPath) + @"\Error Report";
            //            if (!Directory.Exists(errorpath)) Directory.CreateDirectory(errorpath);
            //            string CompletePath = System.IO.Path.Combine(errorpath, fileName + System.Guid.NewGuid().ToString() + ".csv"); // , fileName + System.Guid.NewGuid().ToString() + ".zip"
            //            if (File.Exists(CompletePath))
            //            {
            //                File.Delete(CompletePath);
            //            }
            //            System.IO.File.WriteAllBytes(CompletePath, errorReport);

            //            if (System.IO.Directory.Exists(errorpath))
            //            {
            //                DirectoryInfo di = new DirectoryInfo(errorpath);
            //                /* foreach (FileInfo fi in di.GetFiles("*.zip"))
            //                 {
            //                     string filename = fi.FullName;
            //                     string strErrorFileName = Path.GetFileNameWithoutExtension(filename);
            //                     string originalFileName = "";
            //                     byte[] data = File.ReadAllBytes(filename);
            //                     byte[] NewFile = ClsGenerateERXxml.UnzipRarFile(data, out originalFileName);
            //                     File.Delete(filename);
            //                     string CSVPrfix = Path.GetFileName(strErrorFileName);
            //                     string FileNameWithPath = System.IO.Path.Combine(errorpath, CSVPrfix + "_" + originalFileName);
            //                     System.IO.File.WriteAllBytes(FileNameWithPath, NewFile);


            //                     DataTable dtError = ClsGenerateERXxml.ConvertCSVtoDataTable(FileNameWithPath);

            //                     if (dtError != null && dtError.Rows.Count > 0)
            //                     {
            //                         foreach (DataRow dr in dtError.Rows)
            //                         {
            //                             string StrFileName = pstrXMLFilename;
            //                             string StrTrn = dr["Transaction"].ToString();
            //                             string StrType = dr["Type"].ToString(); 
            //                             string StrObjName = dr["ObjectName"].ToString();
            //                             string StrHaadFld = dr["DhpoField"].ToString();
            //                             string StrFldVal = dr["FieldValue"].ToString();
            //                             string StrAddRef = dr["AdditionalReference"].ToString();
            //                             string StrErrorTxt = dr["ErrorText"].ToString();

            //                             int e = ClsGetRxData.InsertXMlError(_eRxNo, StrFileName, StrTrn, StrType, 
            //                                 StrObjName, StrHaadFld, StrFldVal, StrAddRef, StrErrorTxt);
            //                         }

            //                     }
            //                     File.Delete(filename);
            //                 }*/

            //                foreach (FileInfo fi in di.GetFiles("*.csv"))
            //                {
            //                    string filename = fi.FullName;
            //                    DataTable dtError = ClsGenerateERXxml.ConvertCSVtoDataTable(filename);

            //                    if (dtError != null && dtError.Rows.Count > 0)
            //                    {
            //                        foreach (DataRow dr in dtError.Rows)
            //                        {
            //                            string StrFileName = pstrXMLFilename;
            //                            string StrTrn = dr["Transaction"].ToString();
            //                            string StrType = dr["Type"].ToString();
            //                            string StrObjName = dr["ObjectName"].ToString();
            //                            string StrHaadFld = dr["DhpoField"].ToString();
            //                            string StrFldVal = dr["FieldValue"].ToString();
            //                            string StrAddRef = dr["AdditionalReference"].ToString();
            //                            string StrErrorTxt = dr["ErrorText"].ToString();

            //                            int e = ClsGetRxData.InsertXMlError(_eRxNo, StrFileName, StrTrn, StrType,
            //                                StrObjName, StrHaadFld, StrFldVal, StrAddRef, StrErrorTxt);
            //                        }

            //                    }
            //                    File.Delete(filename);
            //                }
            //            }


            //        }

            //    }
            //    return Task.FromResult(UpldStatus);
            //}
            //catch (Exception ex)
            //{
            //    Public.Library.ErrorHandeling.clsEvntvwrLogging.fnLogWritter(ex);
            //}
            //return Task.FromResult(UpldStatus);
            return null;
        }      
        public string GeteRxTransactions(ApiRequestModel model)
        {
            try
            {
                var service = new ErxDHAWebRequestService();
                var response = service.Post(model);
            }
            catch (Exception ex)
            {
               
            }

            return null;
        }

        public Task<string> SearcheRxTransactions(string pstrlogin, string pstrpwd, int _direction, string pstrFacilityLic, string pstrClinicianLic,
            string pstrmemberID, int? _eRxReferenceNo, int _transactionStatus, string pstrtransactionFromDate, string pstrtransactionToDate,
            int _minRecordCount, int _maxRecordCount)
        {
            //try
            //{
            //    /*
            //     * Method  Signature
            //     *  int SearchTransactions( string login,string pwd,int direction,sting callerLicense,sting clinicianLicense,sting memberID,int eRxReferenceNo,
            //                              int transactionStatus,string transactionFromDate,string transactionToDate,int minRecordCount,
            //                              int maxRecordCount,out string foundTransactions,out string errorMessage)
            //     * direction values 1 (sent only) or 2 (received only);
            //     * transactionStatus values 1 (new only) or 2 (already downloaded only);
            //     * 
            //     */

            //    string xmlTransactions;
            //    string errorMessage;
            //    int i = objeRxValidateTransactionSoapClient.SearchTransactions(pstrlogin, pstrpwd, _direction, pstrFacilityLic, pstrClinicianLic, pstrmemberID,
            //        _eRxReferenceNo, _transactionStatus, pstrtransactionFromDate, pstrtransactionToDate, _minRecordCount, _maxRecordCount, out xmlTransactions, out errorMessage);

            //    //return MessageResponse(i);
            //    return Task.FromResult(xmlTransactions);
            //}
            //catch (Exception ex)
            //{
            //    clsEvntvwrLogging.fnLogWritter(ex);
            //}

            return null;
        }
       
        public Task<string> DownloadeRxTransactions(string pstrlogin, string pstrpwd, string pstrfileID)
        {
            //try
            //{
            //    string fileName;
            //    byte[] file;
            //    string errorMessage;
            //    int i = objeRxValidateTransactionSoapClient.DownloadTransactionFile(pstrlogin, pstrpwd, pstrfileID, out fileName, out file, out errorMessage);
            //    if (i == 0) SeteRxTransactionsDownloaded(pstrlogin, pstrpwd, pstrfileID);
            //    clsEvntvwrLogging.fnMsgWritter(MessageResponse(i));
            //    if (!string.IsNullOrEmpty(errorMessage)) clsEvntvwrLogging.fnMsgWritter(errorMessage);

            //    if (file != null && file.Length > 0)
            //    {
            //        string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //        string Downloadpath = Path.GetDirectoryName(tempPath) + @"\Download Request";
            //        if (!Directory.Exists(Downloadpath)) Directory.CreateDirectory(Downloadpath);
            //        string dfName = Downloadpath + @"\" + Path.GetFileNameWithoutExtension(fileName) + Path.GetExtension(fileName);
            //        if (File.Exists(dfName)) File.Delete(dfName);
            //        File.WriteAllBytes(dfName, file);
            //        string downloadedxml = null;
            //        if (System.IO.Directory.Exists(Path.GetDirectoryName(dfName)))
            //        {
            //            if (Path.GetExtension(dfName).ToLower() == ".zip")
            //            {
            //                string filename = dfName;
            //                string strErrorFileName = Path.GetFileNameWithoutExtension(filename);
            //                string originalFileName = "";
            //                byte[] data = File.ReadAllBytes(filename);
            //                byte[] NewFile = ClsGenerateERXxml.UnzipRarFile(data, out originalFileName);
            //                File.Delete(filename);
            //                string CSVPrfix = Path.GetFileName(strErrorFileName);
            //                string FileNameWithPath = System.IO.Path.Combine(Downloadpath, CSVPrfix + "_" + originalFileName);
            //                System.IO.File.WriteAllBytes(FileNameWithPath, NewFile);
            //                downloadedxml = FileNameWithPath;
            //            }
            //            else if (Path.GetExtension(dfName).ToLower() == ".xml") downloadedxml = dfName;

            //            DataSet dsxml = new DataSet();
            //            dsxml.ReadXml(downloadedxml);
            //            if (dsxml != null && dsxml.Tables.Count > 0)
            //            {
            //                string[] strAuth = new string[5];
            //                strAuth[0] = (dsxml.Tables["Authorization"].Columns.Contains("ID") ? dsxml.Tables["Authorization"].Rows[0]["ID"].ToString() : null);
            //                strAuth[1] = (dsxml.Tables["Authorization"].Columns.Contains("Result") ? dsxml.Tables["Authorization"].Rows[0]["Result"].ToString() : null);
            //                strAuth[2] = (dsxml.Tables["Authorization"].Columns.Contains("IDPayer") ? dsxml.Tables["Authorization"].Rows[0]["IDPayer"].ToString() : null);
            //                strAuth[3] = (dsxml.Tables["Authorization"].Columns.Contains("DenialCode") ? dsxml.Tables["Authorization"].Rows[0]["DenialCode"].ToString() : null);
            //                strAuth[4] = (dsxml.Tables["Authorization"].Columns.Contains("Comments") ? dsxml.Tables["Authorization"].Rows[0]["Comments"].ToString() : null);

            //                bool isdone = ClsGetRxData.UpdateAuthorizationCode(strAuth, dsxml.Tables["Activity"]);
            //                if (!string.IsNullOrEmpty(strAuth[0]))
            //                {
            //                    Int32 erx_no = ClsGetRxData.GetRequestErxNo(strAuth[0]);
            //                    string xmlcontent = System.IO.File.ReadAllText(downloadedxml); //xmldoc.OuterXml;
            //                    ClsGetRxData.InsertXMlData(erx_no, 2, Path.GetFileName(dfName), xmlcontent, "File Downloaded successfully");
            //                }
            //            }
            //            File.Delete(downloadedxml);
            //        }
            //    }

            //    return Task.FromResult(MessageResponse(i));
            //}
            //catch (Exception ex)
            //{
            //    clsEvntvwrLogging.fnLogWritter(ex);
            //}

            return null;
        }
       
        public int SeteRxTransactionsDownloaded(string pstrlogin, string pstrpwd, string pstrfileID)
        {
            int downldcount = -1;
            //try
            //{
            //    string errorMessage;
            //    downldcount = objeRxValidateTransactionSoapClient.SetTransactionDownloaded(pstrlogin, pstrpwd, pstrfileID, out errorMessage);
            //    if (!string.IsNullOrEmpty(errorMessage)) clsEvntvwrLogging.fnMsgWritter(errorMessage);
            //}
            //catch (Exception ex)
            //{ clsEvntvwrLogging.fnLogWritter(ex); }

            return downldcount;

        }
       
        private string MessageResponse(int _sr)
        {
            switch (_sr)
            {
                case 0:
                    return "Operation is successful.";
                case 1:
                    return "E-claim transaction validation succeeded with warnings.";
                case 2:
                    return "No new prior authorization transactions are available for download.";
                case 3:
                    return "Member has no approved trade drugs, hence Prescription transaction is not returned.";
                case 4:
                    return "DRG grouping is performed using default patient gender and age (female 21 years old)";
                case -1:
                    return "Login failed for the user.";
                case -2:
                    return "E-claim transaction validation is failed with errors.";
                case -3:
                    return "Required input parameter for the web service is empty, or null, or contains invalid value.";
                case -4:
                    return "Unexpected error occurred.";
                case -5:
                    return "If difference between date from and date to parameters is longer than 100 days.";
                case -6:
                    return "The specified file is not found.";
                case -7:
                    return "Transaction is not supported";
                case -8:
                    return "DRG Grouper is busy serving other requests; if you get this error code please try to call the web service again in 5-10 minutes";
                case -9:
                    return "Error occurred while running DRG Grouper";
                case -10:
                    return "No search criteria is found.";
                default:
                    return string.Empty;

            }
        }       

        private async Task<string> CallSoapService(string url, string action, string soapBody)
        {
            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(soapBody, Encoding.UTF8, "text/xml");

                // Set required headers
                content.Headers.Add("SOAPAction", action);

                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode(); // Throws if not successful

                var soapResponse = await response.Content.ReadAsStringAsync();
                return soapResponse;
            }
        }

        public string GeteRxTransactions(string pstrlogin, string pstrpwd, string pstrmemberID, int _eRxReferenceNo)
        {
            throw new NotImplementedException();
        }
    }
}

