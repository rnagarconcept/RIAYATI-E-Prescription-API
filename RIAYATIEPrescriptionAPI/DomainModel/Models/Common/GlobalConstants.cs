using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomainModel
{
    public class GlobalConstants
    {
        public const string DOWNLOAD_TRANSACTION_FILE_SOAP_ENEVLOP = "downloadTransactionFileSoapBody";
        public const string GET_NEW_TRANSACTIONS_SOAP_ENEVLOP = "getNewTransactionsSoapBody";
        public const string GETE_RX_TRANSACTION_SOAP_ENEVLOP = "geteRxTransactionSoapBody";
        public const string SEARCH_TRANSACTIONS_SOAP_ENEVLOP = "searchTransactionsSoapBody";
        public const string SET_TRANSACTION_DOWNLOADED_SOAP_ENEVLOP = "setTransactionDownloadedSoapBody";
        public const string UPLOAD_ERX_AUTHORIZATION_SOAP_ENEVLOP = "uploadERxAuthorizationSoapBody";
        public const string UPLOAD_ERX_REQUEST_SOAP_ENEVLOP = "uploadErxRequestSoapBody";

        public const string CONFIG_SETTINGS = "CONFIG_SETTINGS";
        public const string LICENSE_DETAILS = "LICENSE_DETAILS";
        public const string FACILITY_LICENSES = "FACILITY_LICENSES";
        public const string PAT_ERX_ENCOUNTERS = "PAT_ERX_ENCOUNTERS";
        public const string PHYSICIAN_CREDENTIALS = "PHYSICIAN_CREDENTIALS";

        public const string AUTH_TOKEN = "AUTH_TOKEN";
    }
}