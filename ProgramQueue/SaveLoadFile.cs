using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

using QueueRunner;

namespace ProgramQueue
{
    static class SaveLoadFile
    {
        public static void SaveQueue(string fileName, List<ProgramQueueItem> items)
        {
            var w = new XmlSerializer(typeof(List<ProgramQueueItem>));
            using (var s = new StreamWriter(fileName))
            {
                w.Serialize(s, items);
            }
        }

        public static List<ProgramQueueItem> LoadQueue(string fileName)
        {
            List<ProgramQueueItem> items;
            var w = new XmlSerializer(typeof(List<ProgramQueueItem>));

            using (var s = new StreamReader(fileName))
            {
                items = (List<ProgramQueueItem>)w.Deserialize(s);
            }

            return items;
        }
    }
}
