using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;
using DataAccess;

namespace RIAYATIEPrescriptionAPI.Services
{
    public class ClsGenerateERXxml
    {
        private readonly ILog log = LogManager.GetLogger(typeof(ClsGenerateERXxml));
        public Task<string> GenerateXMLFile(int eRxNo)
        {
            string XmlCreated = "";
            try
            {
                var dsXmlData = GetDBSet(eRxNo);
                if (dsXmlData != null && dsXmlData.Tables["Header"].Rows.Count > 0)
                {
                    string strPath = @"C:\Concepts\eRx Request-" + DateTime.Now.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    if (!System.IO.Directory.Exists(strPath)) System.IO.Directory.CreateDirectory(strPath);
                    //dsXmlData.WriteXml(strPath+@"\DHA.xml");
                    string stXMLFileName = dsXmlData.Tables["Prescription"].Rows[0]["ID"].ToString().Trim() + ".xml";
                    XmlTextWriter writer = new XmlTextWriter(strPath + @"\" + stXMLFileName, System.Text.Encoding.UTF8);
                    writer.WriteStartDocument(true);
                    writer.Formatting = Formatting.Indented;
                    writer.Indentation = 2;
                    writer.WriteStartElement("eRx.Request"); // Start eRx.Request Tag
                                                             //Set The value for Arrary to Write  header
                    string[] strHeaderAry = new string[5];
                    strHeaderAry[0] = dsXmlData.Tables["Header"].Rows[0]["SenderID"].ToString().Trim();
                    strHeaderAry[1] = dsXmlData.Tables["Header"].Rows[0]["ReceiverID"].ToString().Trim();
                    //MessageBox.Show(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToString() + " " + System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern.ToString());
                    DateTime Headerdt = DateTime.Parse((dsXmlData.Tables["Header"].Rows[0]["TransactionDate"].ToString()));
                    if (Headerdt < DateTime.Now) Headerdt = DateTime.Now;
                    // MessageBox.Show(DateTime.Now.ToString("dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture));
                    // MessageBox.Show(DateTime.Now.Date.Day  + "/" + DateTime.Now.Date.Month  + "/" + DateTime.Now.Date.Year + " " + DateTime.Now.TimeOfDay.Minutes + ":" + DateTime.Now.TimeOfDay.Seconds);
                    strHeaderAry[2] = Headerdt.ToString("dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                    strHeaderAry[3] = dsXmlData.Tables["Header"].Rows[0]["RecordCount"].ToString().Trim();
                    strHeaderAry[4] = dsXmlData.Tables["Header"].Rows[0]["DispositionFlag"].ToString().Trim();
                    CreateXMLHeaderTag(strHeaderAry, writer); // Write XML Header

                    writer.WriteStartElement("Prescription"); // Start Prescription Tag

                    writer.WriteStartElement("ID"); //Start ID Tag
                    writer.WriteString(dsXmlData.Tables["Prescription"].Rows[0]["ID"].ToString().Trim());
                    writer.WriteEndElement(); //End ID Tag

                    writer.WriteStartElement("Type"); // Strat Type Tag
                    writer.WriteString(dsXmlData.Tables["Prescription"].Rows[0]["Type"].ToString().Trim());
                    writer.WriteEndElement(); // End Type Tag

                    writer.WriteStartElement("PayerID"); //Start PayerID Tag
                    writer.WriteString(dsXmlData.Tables["Prescription"].Rows[0]["PayerID"].ToString().Trim());
                    writer.WriteEndElement(); // End PayerID Tag

                    writer.WriteStartElement("Clinician");  // Start Clinician Tag
                    writer.WriteString(dsXmlData.Tables["Prescription"].Rows[0]["Clinician"].ToString().Trim());
                    writer.WriteEndElement(); // End Clinician tag

                    //Set The value for Arrary to Write  Patient
                    string[] strPatientTag = new string[5];
                    strPatientTag[0] = dsXmlData.Tables["Patient"].Rows[0]["MemberID"].ToString().Trim();
                    strPatientTag[1] = dsXmlData.Tables["Patient"].Rows[0]["EmiratesIDNumber"].ToString().Trim();
                    strPatientTag[2] = dsXmlData.Tables["Patient"].Rows[0]["DateOfBirth"].ToString().Trim();
                    strPatientTag[3] = dsXmlData.Tables["Patient"].Rows[0]["Weight"].ToString().Trim();
                    strPatientTag[4] = dsXmlData.Tables["Patient"].Rows[0]["Email"].ToString().Trim();

                    CreateXMLPatientTag(strPatientTag, writer); // Write Patient Tag Into XML 

                    //Set The value for Arrary to Write  Encounter Tag
                    string[] strEncounterTgVal = new string[2];
                    strEncounterTgVal[0] = dsXmlData.Tables["Encounter"].Rows[0]["FacilityID"].ToString().Trim();
                    strEncounterTgVal[1] = dsXmlData.Tables["Encounter"].Rows[0]["Type"].ToString().Trim();

                    CreateXMLEncounterTag(strEncounterTgVal, writer); // Write Encounter Tag into XML

                    for (int i = 0; i <= dsXmlData.Tables["Diagnosis"].Rows.Count - 1; i++) //Repeat Diagnosis Tag for Reach Row
                    {
                        writer.WriteStartElement("Diagnosis"); // Start Diagnosis Tag

                        writer.WriteStartElement("Type");  // Start Diagnosis Type Tag
                        writer.WriteString(dsXmlData.Tables["Diagnosis"].Rows[i]["Type"].ToString().Trim());
                        writer.WriteEndElement(); // Start Diagnosis Type Tag

                        writer.WriteStartElement("Code"); // Start Diagnosis Code Tag
                        writer.WriteString(dsXmlData.Tables["Diagnosis"].Rows[i]["Code"].ToString().Trim());
                        writer.WriteEndElement();  // Start Diagnosis Code Tag

                        writer.WriteEndElement(); // End Diagnosis Tag
                    }

                    for (int i = 0; i <= dsXmlData.Tables["Activity"].Rows.Count - 1; i++) //Repeat Diagnosis Tag for Reach Row
                    {
                        writer.WriteStartElement("Activity"); // Start Activity Tag

                        writer.WriteStartElement("ID"); // Start Activity Id Tag
                        writer.WriteString(dsXmlData.Tables["Activity"].Rows[i]["ID"].ToString().Trim());
                        writer.WriteEndElement();  // End Activity Id Tag

                        writer.WriteStartElement("Start"); // Start Activity Start Time Tag
                        DateTime Startdt = DateTime.Parse((dsXmlData.Tables["Activity"].Rows[i]["Start"].ToString()));
                        // DateTime Startdt = DateTime.ParseExact(dsXmlData.Tables["Activity"].Rows[i]["Start"].ToString().Trim(), "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                        writer.WriteString(Startdt.ToString("dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).Trim());
                        writer.WriteEndElement(); // End Activity Start Time Tag

                        writer.WriteStartElement("Type"); //Start Activity Type Tag
                        writer.WriteString(dsXmlData.Tables["Activity"].Rows[i]["Type"].ToString().Trim());
                        writer.WriteEndElement(); //End Activity Type Tag

                        writer.WriteStartElement("Code"); //Start Activity code Tag
                        writer.WriteString(dsXmlData.Tables["Activity"].Rows[i]["Code"].ToString().Trim());
                        writer.WriteEndElement(); //End Activity Code Tag

                        writer.WriteStartElement("Quantity"); //Start Activity qty Tag
                        writer.WriteString(dsXmlData.Tables["Activity"].Rows[i]["Quantity"].ToString().Trim());
                        writer.WriteEndElement(); //End Activity qty Tag

                        writer.WriteStartElement("Duration"); //Start Activity duration Tag
                        writer.WriteString(dsXmlData.Tables["Activity"].Rows[i]["Duration"].ToString().Trim());
                        writer.WriteEndElement(); //end Activity duration Tag

                        writer.WriteStartElement("Refills"); //Start Activity Refill Tag
                        writer.WriteString(dsXmlData.Tables["Activity"].Rows[i]["Refills"].ToString().Trim());
                        writer.WriteEndElement(); //End Activity Refill Tag

                        writer.WriteStartElement("RoutOfAdmin"); //Start Activity ROA Tag
                        writer.WriteString(dsXmlData.Tables["Activity"].Rows[i]["RoutOfAdmin"].ToString().Trim());
                        writer.WriteEndElement(); //End Activity ROA Tag

                        writer.WriteStartElement("Instructions"); //Start Activity Insturcation Tag
                        writer.WriteString(dsXmlData.Tables["Activity"].Rows[i]["Instructions"].ToString().Trim());
                        writer.WriteEndElement(); //End Activity Insturcation Tag


                        //Ignore Frequency Tag if Frequency is not mentioned
                        if (!string.IsNullOrEmpty(dsXmlData.Tables["Activity"].Rows[i]["UnitPerFrequency"].ToString().Trim()) &&
                           !string.IsNullOrEmpty(dsXmlData.Tables["Activity"].Rows[i]["FrequencyValue"].ToString().Trim()) &&
                           !string.IsNullOrEmpty(dsXmlData.Tables["Activity"].Rows[i]["FrequencyType"].ToString().Trim()))
                        {
                            writer.WriteStartElement("Frequency");  //Start Frequency Tag

                            writer.WriteStartElement("UnitPerFrequency");  //Start unit Per Frequency Tag
                            writer.WriteString(dsXmlData.Tables["Activity"].Rows[i]["UnitPerFrequency"].ToString().Trim());
                            writer.WriteEndElement();  //End unit Per Frequency Tag

                            writer.WriteStartElement("FrequencyValue"); //Start Frequency value Tag
                            writer.WriteString(dsXmlData.Tables["Activity"].Rows[i]["FrequencyValue"].ToString().Trim());
                            writer.WriteEndElement(); //End Frequency value Tag

                            writer.WriteStartElement("FrequencyType"); //Start Frequency Type Tag
                            writer.WriteString(dsXmlData.Tables["Activity"].Rows[i]["FrequencyType"].ToString().Trim());
                            writer.WriteEndElement(); ////End Frequency type tag

                            writer.WriteEndElement(); //End Frequency Tag
                        }

                        writer.WriteEndElement(); // End Activity Tag
                    }

                    writer.WriteEndElement(); // End Prescription Tag
                    writer.WriteEndElement(); // End eRx.Request Tag

                    writer.Close();
                    XmlCreated = strPath + @"\" + stXMLFileName;


                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return Task.FromResult(XmlCreated);
        }


        // Create Header Tag for the Xml
        private void CreateXMLHeaderTag(string[] strHeaderval, XmlTextWriter writer)
        {
            try
            {
                writer.WriteStartElement("Header");
                writer.WriteStartElement("SenderID");
                writer.WriteString(strHeaderval[0].Trim());
                writer.WriteEndElement();

                writer.WriteStartElement("ReceiverID");
                writer.WriteString(strHeaderval[1].Trim());
                writer.WriteEndElement();

                writer.WriteStartElement("TransactionDate");
                writer.WriteString(strHeaderval[2].Trim());
                writer.WriteEndElement();

                writer.WriteStartElement("RecordCount");
                writer.WriteString(strHeaderval[3].Trim());
                writer.WriteEndElement();

                writer.WriteStartElement("DispositionFlag");
                writer.WriteString(strHeaderval[4].Trim());
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
            catch (Exception ex) { log.Error(ex); }
        }

        // Create Patient Tag for the Xml
        private void CreateXMLPatientTag(string[] strPatval, XmlTextWriter writer)
        {
            try
            {
                writer.WriteStartElement("Patient");

                writer.WriteStartElement("MemberID");
                writer.WriteString(strPatval[0].Trim());
                writer.WriteEndElement();

                writer.WriteStartElement("EmiratesIDNumber");
                writer.WriteString(strPatval[1].Trim());
                writer.WriteEndElement();

                writer.WriteStartElement("DateOfBirth");
                writer.WriteString(strPatval[2].Trim());
                writer.WriteEndElement();

                writer.WriteStartElement("Weight");
                writer.WriteString(strPatval[3].Trim());
                writer.WriteEndElement();

                if (!string.IsNullOrEmpty(strPatval[4].Trim()))
                {
                    writer.WriteStartElement("Email");
                    writer.WriteString(strPatval[4].Trim());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }
            catch (Exception ex) { log.Error(ex); }
        }

        // Create Encounter Tag for the Xml
        private void CreateXMLEncounterTag(string[] strEncounterTgVal, XmlTextWriter writer)
        {
            try
            {

                writer.WriteStartElement("Encounter");

                writer.WriteStartElement("FacilityID");
                writer.WriteString(strEncounterTgVal[0].Trim());
                writer.WriteEndElement();

                writer.WriteStartElement("Type");
                writer.WriteString(strEncounterTgVal[1].Trim());
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
            catch (Exception ex) { log.Error(ex); }
        }

        public Task<string[]> GenerateXMLString(int eRxNo)
        {
            string[] XmlData = null;
            try
            {
                string strSenderID = null;
                DataSet dsXmlData = GetDBSet(eRxNo);
                if (dsXmlData != null && dsXmlData.Tables["Header"].Rows.Count > 0)
                {
                    string stXMLFileName = dsXmlData.Tables["Prescription"].Rows[0]["ID"].ToString().Trim() + ".xml";
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlNode xmldeclare = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    xmlDoc.AppendChild(xmldeclare);

                    XmlNode nodeeRx = xmlDoc.CreateElement("eRx.Request"); // Start eRx.Request Tag
                    xmlDoc.AppendChild(nodeeRx);

                    /*---------------------------------<Header>-------------------------------------*/

                    DateTime Headerdt = DateTime.Parse((dsXmlData.Tables["Header"].Rows[0]["TransactionDate"].ToString()));
                    if (Headerdt < DateTime.Now) Headerdt = DateTime.Now; //  IF Transaction Date is Less than  todays date then Todays Date 

                    XmlNode nodeHeader = xmlDoc.CreateElement("Header");// Start Request Header Tag
                    nodeeRx.AppendChild(nodeHeader);

                    XmlNode nodeSenderID = xmlDoc.CreateElement("SenderID");
                    nodeSenderID.InnerText = dsXmlData.Tables["Header"].Rows[0]["SenderID"].ToString().Trim();
                    strSenderID = dsXmlData.Tables["Header"].Rows[0]["SenderID"].ToString().Trim();
                    nodeHeader.AppendChild(nodeSenderID);

                    XmlNode nodeReceiverID = xmlDoc.CreateElement("ReceiverID");
                    nodeReceiverID.InnerText = dsXmlData.Tables["Header"].Rows[0]["ReceiverID"].ToString().Trim();
                    nodeHeader.AppendChild(nodeReceiverID);

                    XmlNode nodeTrnDate = xmlDoc.CreateElement("TransactionDate");
                    nodeTrnDate.InnerText = Headerdt.ToString("dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture); ;
                    nodeHeader.AppendChild(nodeTrnDate);

                    XmlNode nodeRecordCount = xmlDoc.CreateElement("RecordCount");
                    nodeRecordCount.InnerText = dsXmlData.Tables["Header"].Rows[0]["RecordCount"].ToString().Trim();
                    nodeHeader.AppendChild(nodeRecordCount);

                    XmlNode nodeDispFlg = xmlDoc.CreateElement("DispositionFlag");
                    nodeDispFlg.InnerText = dsXmlData.Tables["Header"].Rows[0]["DispositionFlag"].ToString().Trim();
                    nodeHeader.AppendChild(nodeDispFlg); //Request Header Tag Completed

                    /*-------------------------------</Header>--------------------------------------*/

                    /*--------------------------<Prescription>------------------------------------*/

                    XmlNode nodeAth = xmlDoc.CreateElement("Prescription");// Start Authorization Tag
                    nodeeRx.AppendChild(nodeAth);

                    XmlNode nodeType = xmlDoc.CreateElement("ID");
                    nodeType.InnerText = dsXmlData.Tables["Prescription"].Rows[0]["ID"].ToString().Trim();
                    nodeAth.AppendChild(nodeType);

                    XmlNode nodeID = xmlDoc.CreateElement("Type");
                    nodeID.InnerText = dsXmlData.Tables["Prescription"].Rows[0]["Type"].ToString().Trim();
                    nodeAth.AppendChild(nodeID);

                    XmlNode nodePayerID = xmlDoc.CreateElement("PayerID");
                    nodePayerID.InnerText = dsXmlData.Tables["Prescription"].Rows[0]["PayerID"].ToString().Trim();
                    nodeAth.AppendChild(nodePayerID);

                    XmlNode nodeEIDNum = xmlDoc.CreateElement("Clinician");
                    nodeEIDNum.InnerText = dsXmlData.Tables["Prescription"].Rows[0]["Clinician"].ToString().Trim();
                    nodeAth.AppendChild(nodeEIDNum);

                    /*--------------------------</Prescription>-------------------------------*/

                    /*--------------------------<Patient>-----------------------------------*/

                    XmlNode nodeEnc = xmlDoc.CreateElement("Patient");// Start Encounter Tag
                    nodeAth.AppendChild(nodeEnc);

                    XmlNode nodeFID = xmlDoc.CreateElement("MemberID");
                    nodeFID.InnerText = dsXmlData.Tables["Patient"].Rows[0]["MemberID"].ToString().Trim();
                    nodeEnc.AppendChild(nodeFID);

                    XmlNode nodeFType = xmlDoc.CreateElement("EmiratesIDNumber");
                    nodeFType.InnerText = dsXmlData.Tables["Patient"].Rows[0]["EmiratesIDNumber"].ToString().Trim();
                    nodeEnc.AppendChild(nodeFType);

                    XmlNode nodeDOB = xmlDoc.CreateElement("DateOfBirth");
                    nodeDOB.InnerText = dsXmlData.Tables["Patient"].Rows[0]["DateOfBirth"].ToString().Trim();
                    nodeEnc.AppendChild(nodeDOB);
                    if (dsXmlData.Tables["Patient"].Rows[0]["Weight"] != null && !string.IsNullOrEmpty(dsXmlData.Tables["Patient"].Rows[0]["Weight"].ToString().Trim()))
                    {
                        XmlNode nodeWeight = xmlDoc.CreateElement("Weight");
                        nodeWeight.InnerText = dsXmlData.Tables["Patient"].Rows[0]["Weight"].ToString().Trim();
                        nodeEnc.AppendChild(nodeWeight);
                    }

                    if (dsXmlData.Tables["Patient"].Rows[0]["Email"] != null && !string.IsNullOrEmpty(dsXmlData.Tables["Patient"].Rows[0]["Email"].ToString().Trim()))
                    {
                        XmlNode nodeEmail = xmlDoc.CreateElement("Email");
                        nodeEmail.InnerText = dsXmlData.Tables["Patient"].Rows[0]["Email"].ToString().Trim();
                        nodeEnc.AppendChild(nodeEmail);
                    }
                    /*--------------------------</Patient>---------------------------------*/
                    /*--------------------------</Patient>---------------------------------*/

                    XmlNode nodeEc = xmlDoc.CreateElement("Encounter");// Start Encounter Tag
                    nodeAth.AppendChild(nodeEc);

                    XmlNode nodeEFacID = xmlDoc.CreateElement("FacilityID");
                    nodeEFacID.InnerText = dsXmlData.Tables["Encounter"].Rows[0]["FacilityID"].ToString().Trim();
                    nodeEc.AppendChild(nodeEFacID);

                    XmlNode nodeEcType = xmlDoc.CreateElement("Type");
                    nodeEcType.InnerText = dsXmlData.Tables["Encounter"].Rows[0]["Type"].ToString().Trim();
                    nodeEc.AppendChild(nodeEcType);//Completed Encounter Tag   

                    /*--------------------------</Patient>---------------------------------*/

                    /*---------------------------<Diagnosis>---------------------------------*/

                    for (int i = 0; i <= dsXmlData.Tables["Diagnosis"].Rows.Count - 1; i++) //Repeat Diagnosis Tag for Reach Row
                    {
                        XmlNode nodeDiagnosis = xmlDoc.CreateElement("Diagnosis");// Start Diagnosis Tag
                        nodeAth.AppendChild(nodeDiagnosis);

                        XmlNode nodeDiagType = xmlDoc.CreateElement("Type");
                        nodeDiagType.InnerText = dsXmlData.Tables["Diagnosis"].Rows[i]["Type"].ToString().Trim();
                        nodeDiagnosis.AppendChild(nodeDiagType);

                        XmlNode nodeDiagCode = xmlDoc.CreateElement("Code");
                        nodeDiagCode.InnerText = dsXmlData.Tables["Diagnosis"].Rows[i]["Code"].ToString().Trim();
                        nodeDiagnosis.AppendChild(nodeDiagCode);//Completed Diagnosis Tag     
                    }

                    /*--------------------------</Diagnosis>---------------------------------*/

                    /*---------------------------<Activity>---------------------------------*/
                    for (int i = 0; i <= dsXmlData.Tables["Activity"].Rows.Count - 1; i++) //Repeat Activity Tag for Reach Row
                    {
                        XmlNode nodeActivity = xmlDoc.CreateElement("Activity");// Start Diagnosis Tag
                        nodeAth.AppendChild(nodeActivity);

                        XmlNode nodeActid = xmlDoc.CreateElement("ID");
                        int actID = Convert.ToInt16(dsXmlData.Tables["Activity"].Rows[i]["ID"].ToString().Trim() ?? "0");
                        nodeActid.InnerText = actID.ToString().Trim();
                        nodeActivity.AppendChild(nodeActid);

                        DateTime Startdt = DateTime.Parse((dsXmlData.Tables["Activity"].Rows[i]["Start"].ToString()));

                        XmlNode nodeAcrStartDt = xmlDoc.CreateElement("Start");
                        nodeAcrStartDt.InnerText = Startdt.ToString("dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).Trim();
                        nodeActivity.AppendChild(nodeAcrStartDt);

                        XmlNode nodeActType = xmlDoc.CreateElement("Type");
                        nodeActType.InnerText = dsXmlData.Tables["Activity"].Rows[i]["Type"].ToString().Trim();
                        nodeActivity.AppendChild(nodeActType);

                        XmlNode nodeActCode = xmlDoc.CreateElement("Code");
                        nodeActCode.InnerText = dsXmlData.Tables["Activity"].Rows[i]["Code"].ToString().Trim();
                        nodeActivity.AppendChild(nodeActCode);

                        XmlNode nodeActQty = xmlDoc.CreateElement("Quantity");
                        nodeActQty.InnerText = dsXmlData.Tables["Activity"].Rows[i]["Quantity"] != null ? dsXmlData.Tables["Activity"].Rows[i]["Quantity"].ToString().Trim() : "0";
                        nodeActivity.AppendChild(nodeActQty);

                        XmlNode nodeDuration = xmlDoc.CreateElement("Duration");
                        nodeDuration.InnerText = dsXmlData.Tables["Activity"].Rows[i]["Duration"] != null ? dsXmlData.Tables["Activity"].Rows[i]["Duration"].ToString().Trim() : "0";
                        nodeActivity.AppendChild(nodeDuration);

                        XmlNode nodeRefills = xmlDoc.CreateElement("Refills");
                        nodeRefills.InnerText = dsXmlData.Tables["Activity"].Rows[i]["Refills"] != null ? dsXmlData.Tables["Activity"].Rows[i]["Refills"].ToString().Trim() : "0";
                        nodeActivity.AppendChild(nodeRefills);

                        XmlNode nodeRoutOfAdmin = xmlDoc.CreateElement("RoutOfAdmin");
                        nodeRoutOfAdmin.InnerText = dsXmlData.Tables["Activity"].Rows[i]["RoutOfAdmin"] != null ? dsXmlData.Tables["Activity"].Rows[i]["RoutOfAdmin"].ToString().Trim() : "";
                        nodeActivity.AppendChild(nodeRoutOfAdmin);

                        XmlNode nodeInstructions = xmlDoc.CreateElement("Instructions");
                        nodeInstructions.InnerText = dsXmlData.Tables["Activity"].Rows[i]["Instructions"] != null ? dsXmlData.Tables["Activity"].Rows[i]["Instructions"].ToString().Trim() : "";
                        nodeActivity.AppendChild(nodeInstructions);

                        //Ignore Frequency Tag if Frequency is not mentioned
                        if (!string.IsNullOrEmpty(dsXmlData.Tables["Activity"].Rows[i]["UnitPerFrequency"].ToString().Trim()) &&
                           !string.IsNullOrEmpty(dsXmlData.Tables["Activity"].Rows[i]["FrequencyValue"].ToString().Trim()) &&
                           !string.IsNullOrEmpty(dsXmlData.Tables["Activity"].Rows[i]["FrequencyType"].ToString().Trim()))
                        {

                            XmlNode nodeFreq = xmlDoc.CreateElement("Frequency");// Start Diagnosis Tag
                            nodeActivity.AppendChild(nodeFreq);

                            XmlNode nodeUPF = xmlDoc.CreateElement("UnitPerFrequency");
                            nodeUPF.InnerText = dsXmlData.Tables["Activity"].Rows[i]["UnitPerFrequency"].ToString().Trim();
                            nodeFreq.AppendChild(nodeUPF);

                            XmlNode nodeFV = xmlDoc.CreateElement("FrequencyValue");
                            nodeFV.InnerText = dsXmlData.Tables["Activity"].Rows[i]["FrequencyValue"].ToString().Trim();
                            nodeFreq.AppendChild(nodeFV);

                            XmlNode nodeFT = xmlDoc.CreateElement("FrequencyType");
                            nodeFT.InnerText = dsXmlData.Tables["Activity"].Rows[i]["FrequencyType"].ToString().Trim();
                            nodeFreq.AppendChild(nodeFT);

                        }
                        /*--------------------------<Observation>---------------------------------*/
                        DataTable dtobs = OracleDataAccessRepository.GetInstance.GetPatRxObservationTable(eRxNo, actID);
                        if (dtobs != null && dtobs.Rows.Count > 0)
                        {
                            XmlNode nodeObservation = xmlDoc.CreateElement("Observation");// Start Diagnosis Tag
                            nodeActivity.AppendChild(nodeObservation);

                            XmlNode nodeobsType = xmlDoc.CreateElement("Type");
                            nodeobsType.InnerText = dtobs.Rows[0]["Type"].ToString().Trim();
                            nodeObservation.AppendChild(nodeobsType);

                            XmlNode nodeObsCode = xmlDoc.CreateElement("Code");
                            nodeObsCode.InnerText = dtobs.Rows[0]["Code"].ToString().Trim();
                            nodeObservation.AppendChild(nodeObsCode);

                            XmlNode nodeObsValue = xmlDoc.CreateElement("Value");
                            nodeObsValue.InnerText = dtobs.Rows[0]["Value"].ToString().Trim();
                            nodeObservation.AppendChild(nodeObsValue);

                            XmlNode nodeObsValType = xmlDoc.CreateElement("ValueType");
                            nodeObsValType.InnerText = dtobs.Rows[0]["ValueType"].ToString().Trim();
                            nodeObservation.AppendChild(nodeObsValType);
                        }
                        /*--------------------------</Observation>---------------------------------*/
                    }
                    /*--------------------------</Activity>---------------------------------*/

                    XmlData = new string[3];
                    XmlData[0] = stXMLFileName.ToUpper();
                    XmlData[1] = xmlDoc.OuterXml;
                    XmlData[2] = strSenderID;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return Task.FromResult(XmlData);
        }


        public static byte[] UnzipRarFile(byte[] bytRar, out string FileName)
        {
            using (MemoryStream ms1 = new MemoryStream(bytRar))
            {
                using (ZipInputStream zipInputStream = new ZipInputStream((Stream)ms1))
                {
                    ZipEntry nextEntry;
                    if ((nextEntry = zipInputStream.GetNextEntry()) != null)
                    {
                        FileName = nextEntry.Name;
                        using (MemoryStream ms2 = new MemoryStream())
                        {
                            byte[] buffer = new byte[2048];
                            while (true)
                            {
                                int count = zipInputStream.Read(buffer, 0, buffer.Length);
                                if (count > 0)
                                    ms2.Write(buffer, 0, count);
                                else
                                    break;
                            }
                            return ms2.ToArray();
                        }
                    }
                }
            }
            FileName = (string)null;
            return (byte[])null;
        }

        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            //int cntr = 0;
            string newrow = null;
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!string.IsNullOrEmpty((newrow = sr.ReadLine()))) //!sr.EndOfStream
                {
                    string[] rows = newrow.Split(',');
                    string[] combinedError = new string[7];
                    if (rows.Length >= 7)
                    {
                        combinedError[0] = rows[0];
                        combinedError[1] = rows[1];
                        combinedError[2] = rows[2];
                        combinedError[3] = rows[3];
                        combinedError[4] = rows[4];
                        combinedError[5] = rows[5];
                        combinedError[6] = rows[6];
                    }
                    else
                    {
                        combinedError = rows;
                    }
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = combinedError[i].Replace("\"", "");
                    }
                    dt.Rows.Add(dr);

                }

            }

            return dt;
        }

        public static DataSet GetDBSet(int eRxNo)
        {
            OracleDataAccessRepository.GetInstance.GetPatRxHeader(eRxNo);
            DataSet dsXmlData = new DataSet();
            dsXmlData.Tables.Add(OracleDataAccessRepository.GetInstance.GetPatRxHeaderTable(eRxNo));
            dsXmlData.Tables.Add(OracleDataAccessRepository.GetInstance.GetPatPrescriptionHeaderTable(eRxNo));
            dsXmlData.Tables.Add(OracleDataAccessRepository.GetInstance.GetPatRxPatientTagTable(eRxNo));
            dsXmlData.Tables.Add(OracleDataAccessRepository.GetInstance.GetPatRxEncounterTable(eRxNo));
            dsXmlData.Tables.Add(OracleDataAccessRepository.GetInstance.GetPatRxDiagnosisTable(eRxNo));
            dsXmlData.Tables.Add(OracleDataAccessRepository.GetInstance.GetPatRxActivityTable(eRxNo));
            return dsXmlData;
        }
    }
}
