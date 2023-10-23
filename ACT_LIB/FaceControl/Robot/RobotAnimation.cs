using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Act.Lib.Robot
{
    [Serializable()]
    public class RobotAnimation
    {
        private String name;
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        private int repeatTimes;
        public int RepeatTimes
        {
            get { return repeatTimes; }
            set { repeatTimes = value; }
        }

        private int repeatFrequency;
        public int RepeatFrequency
        {
            get { return repeatFrequency; }
            set { repeatFrequency = value; }
        }

        private List<RobotMotion> motionsList;        
        public List<RobotMotion> FACEMotionsList
        {
            get { return motionsList; }
            set { motionsList = value; }
        }

        public RobotAnimation() {
            name = "";
            repeatTimes = 0;
            repeatFrequency = 0;
            motionsList = new List<RobotMotion>();
        }

        public RobotAnimation(List<RobotMotion> m, int times, int freq)
        {
            name = "";
            repeatTimes = times;
            repeatFrequency = freq;
            motionsList = m;
        }

        #region Save as/Load from XML Format

        /// <summary>
        /// Save object to a file in XML format. 
        /// </summary>
        /// <param name="objGraph"></param>
        /// <param name="fileName"></param>
        /// <exception cref="FaceException"></exception>
        public static void SaveAsXmlFormat(object objGraph, string fileName)
        {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(RobotAnimation), new Type[] { typeof(RobotMotion) });
            Stream fStream = null;
            StreamWriter xmlWriter = null;

            try
            {
                fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
                xmlWriter = new StreamWriter(fStream);
                xmlFormat.Serialize(xmlWriter, objGraph);
                xmlWriter.Close();
                fStream.Close();
            }
            catch (Exception ex)
            {
                xmlWriter.Close();
                fStream.Close();
                throw new RobotException("Some error occurs during save expression file.", ex, typeof(RobotAnimation).FullName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static RobotAnimation LoadFromXmlFormat(string fileName)
         {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(RobotAnimation), new Type[] { typeof(RobotMotion) });
            Stream filestream = null;

            try
            {
                filestream = new FileStream(fileName, FileMode.Open);
                RobotAnimation animation = xmlFormat.Deserialize(filestream) as RobotAnimation;
                filestream.Close();
                return animation;
            }
            catch (Exception ex)
            {
                filestream.Close();
                throw new RobotException("Some error occurs during load expression file.", ex, typeof(RobotAnimation).FullName);
            }
        }

        #endregion

    }
}
