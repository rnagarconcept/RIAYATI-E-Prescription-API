using DomainModel.Models.Common;
using System;
using System.Collections.Generic;
using System.IO;

namespace RIAYATIEPrescriptionAPI.Services
{
    public class SoapMessageEnevlopService
    {
        public static List<MessageEnevlop> ReadAllMessages()
        {
            var messageEnevlops = CacheManagerService.Instance.GetCache<List<MessageEnevlop>>("MESSAGES_ENEVLOPS");
            if(messageEnevlops == null || messageEnevlops.Count == 0)
            {
                messageEnevlops = new List<MessageEnevlop>();
                string dataFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Services", "SoapMessages");
                // Check if the folder exists
                if (!Directory.Exists(dataFolderPath))
                {
                    return messageEnevlops;
                }

                // Get all XML files from the folder
                string[] xmlFiles = Directory.GetFiles(dataFolderPath, "*.xml");
                foreach (var item in xmlFiles)
                {
                    var obj = new MessageEnevlop();
                    var fileInfo = new FileInfo(item);
                    obj.Key = fileInfo.Name.Replace(".xml","");
                    obj.Message = File.ReadAllText(item);
                    messageEnevlops.Add(obj);
                }
            }           

            return messageEnevlops;
        }
    }
}