namespace SFA.Apprenticeships.Infrastructure.Azure.Common
{
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using Microsoft.WindowsAzure.Storage.Queue;

    public class AzureMessageHelper
    {
        public static T DeserialiseQueueMessage<T>(CloudQueueMessage queueMessage)
        {
            T scheduledQueueMessage;

            var dcs = new XmlSerializer(typeof(T));

            using (var xmlstream = new MemoryStream(Encoding.Unicode.GetBytes(queueMessage.AsString)))
            {
                scheduledQueueMessage = (T)dcs.Deserialize(xmlstream);
            }

            return scheduledQueueMessage;
        }

        public static CloudQueueMessage SerialiseQueueMessage<T>(T queueMessage)
        {
            var serializer = new XmlSerializer(typeof(T));
            string message;

            using (var ms = new MemoryStream())
            {
                serializer.Serialize(ms, queueMessage);
                ms.Position = 0;
                var sr = new StreamReader(ms);
                message = sr.ReadToEnd();
            }

            var cloudMessage = new CloudQueueMessage(message);

            return cloudMessage;
        }
    }
}