INSERT INTO RIAYATI_PENDING_REQUESTS(REQUEST_TYPE,FACILITY_ID,PAYLOAD)VALUES
('ERX-UPLOAD-TRANSACTION', 
6642,
UTL_RAW.CAST_TO_RAW('{
  "ErxRequest": {
    "Header": {
      "SenderID": "xxx",
      "ReceiverID": "xxxx",
      "TransactionDate": "03/04/2025 11:37",
      "RecordCount": 1,
      "DispositionFlag": "PRODUCTION",
      "PayerID": "INS020"
    },
    "Prescription": {
      "ID": "xxxxxxx",
      "Type": "eRxRequest",
      "ReferenceNumber": "xxxxxx",
      "Clinician": "xxxx",
      "Patient": {
        "MemberID": "xxx-xxxxx-xxxxxxx-x",
        "NationalIDNumber": "xxx-xxxxx-xxxxxxx-x",
        "DateOfBirth": "14/10/1975 00:00",
        "FullName": "xxxxx",
        "Gender": "Male",
        "ContactNumber": "xxxxxxx",
        "Weight": 60,
        "Email": "xxxxxxxxxx"
      },
      "Encounter": {
        "FacilityID": "8108",
        "Type": 1
      },
      "Diagnosis": [
        {
          "Type": "Principal",
          "Code": "R50.9"
        },
        {
          "Type": "Secondary",
          "Code": "R07.0"
        }
      ],
      "Activity": [
        {
          "ID": "8812676",
          "Type": "5",
          "Code": "J45-5625-01768-02",
          "Quantity": 6,
          "Duration": 3,
          "UnitId": 1,
          "Refills": 0,
          "RoutOfAdmin": "074",
          "Instructions": "NA",
          "Frequency": {
            "UnitPerFrequency": 1,
            "FrequencyValue": 2,
            "FrequencyType": "Day"
          },
          "Observation": [
            {
              "Type": "Text",
              "Code": "Description",
              "Value": "1.000 Qty 2 times a day For 3 Days",
              "ValueType": "Other"
            }
          ],
          "Start": "03/04/2025 00:00"
        },
        {
          "ID": "8812675",
          "Type": "5",
          "Code": "I53-5790-00581-01",
          "Quantity": 6,
          "Duration": 5,
          "UnitId": 1,
          "Refills": 0,
          "RoutOfAdmin": "074",
          "Instructions": "NA",
          "Frequency": {
            "UnitPerFrequency": 1,
            "FrequencyValue": 1,
            "FrequencyType": "Day"
          },
          "Observation": [
            {
              "Type": "Text",
              "Code": "Description",
              "Value": "1.000 Qty Once a day For 5 Days",
              "ValueType": "Other"
            }
          ],
          "Start": "03/04/2025 00:00"
        },
        {
          "ID": "8812679",
          "Type": "5",
          "Code": "A72-0164-04327-02",
          "Quantity": 1,
          "Duration": 1,
          "UnitId": 1,
          "Refills": 0,
          "RoutOfAdmin": "074",
          "Instructions": "PRN Details:if severe pain/fever , 6 hour apart , maximum 4 tablet per day",
          "Frequency": {
            "UnitPerFrequency": 1,
            "FrequencyValue": 1,
            "FrequencyType": "Day"
          },
          "Observation": [
            {
              "Type": "Text",
              "Code": "Description",
              "Value": "1.000 Qty PRN For 1 Days",
              "ValueType": "Other"
            }
          ],
          "Start": "03/04/2025 00:00"
        },
        {
          "ID": "8812680",
          "Type": "5",
          "Code": "G33-1311-01628-01",
          "Quantity": 1,
          "Duration": 5,
          "UnitId": 1,
          "Refills": 0,
          "RoutOfAdmin": "074",
          "Instructions": "NA",
          "Frequency": {
            "UnitPerFrequency": 1,
            "FrequencyValue": 1,
            "FrequencyType": "Day"
          },
          "Observation": [
            {
              "Type": "Text",
              "Code": "Description",
              "Value": "1.000 Qty Once a day (Hours of Sleep) For 5 Days",
              "ValueType": "Other"
            }
          ],
          "Start": "03/04/2025 00:00"
        },
        {
          "ID": "8812677",
          "Type": "5",
          "Code": "H68-5358-04849-02",
          "Quantity": 1,
          "Duration": 5,
          "UnitId": 1,
          "Refills": 0,
          "RoutOfAdmin": "074",
          "Instructions": "NA",
          "Frequency": {
            "UnitPerFrequency": 1,
            "FrequencyValue": 1,
            "FrequencyType": "Day"
          },
          "Observation": [
            {
              "Type": "Text",
              "Code": "Description",
              "Value": "1.000 Qty Once a day For 5 Days",
              "ValueType": "Other"
            }
          ],
          "Start": "03/04/2025 00:00"
        }
      ]
    }
  }
}')
);

INSERT INTO RIAYATI_PENDING_REQUESTS(REQUEST_TYPE,FACILITY_ID,PAYLOAD)VALUES
('ERX_UPLOAD_AUTH_TRANSACTION',
6642, 
UTL_RAW.CAST_TO_RAW('{
  "ErxAuthorization": {
    "Header": {
      "SenderID": "LC0",
      "ReceiverID": "INS00",
      "TransactionDate": "21/08/2018 12:13",
      "RecordCount": "2",
      "DispositionFlag": "PRODUCTION",
      "PayerID": "PYR0"
    },
    "Authorization": {
      "Result": "No",
      "ID": "PA_Test_2254125",
      "IDPayer": "45674114",
      "DenialCode": "AUTH-008",
      "Start": "21/08/2018 12:15",
      "End": "21/08/2018 12:15",
      "Limit": "7.005",
      "Comments": "Test Comment Example",
      "Activity": [
        {
          "ID": "Act9958",
          "Type": "3",
          "Code": "00192",
          "Quantity": "3.00",
          "Net": "47.495",
          "List": "22.5",
          "PatientShare": "9.894",
          "PaymentAmount": "57.233",
          "DenialCode": "MNEC-004",
          "Observation": [
            {
              "Type": "LOINC",
              "Code": "ActivityGross",
              "Value": "Test",
              "ValueType": "Test"
            }
          ],
          "Comments": "Service is not clinically indicated based on good clinical practice, without additional supporting diagnoses/activities"
        }
      ]
    }
  }
}')
);
commit;
