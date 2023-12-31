﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Act.Lib.Robot
{
    [global::System.Serializable]
    public class RobotException : Exception
    {
        /// <summary>
        /// The name of the class generating the exception
        /// </summary>
        private string module;
        public string Module
        {
            get { return module; }
            set { module = value; }
        }

        /// <summary>Initializes a new instance of the FaceException</summary>
        public RobotException() { }

        /// <summary>Initializes a new instance of the FaceException</summary>
        /// <param name="message">Exception text</param>
        public RobotException(string message) : base(message) { }

        /// <summary>Initializes a new instance of the FaceException</summary>
        /// <param name="message">Exception text</param>
        /// <param name="inner">The inner exception as reason of this message</param>
        public RobotException(string message, Exception inner) : base(message, inner) { }

        /// <summary>Initializes a new instance of the FaceException</summary>
        /// <param name="inner">The inner exception as reason of this message</param>
        public RobotException(Exception inner) : base(String.Empty, inner) { }

        /// <summary>Initializes a new instance of the FaceException</summary>
        /// <param name="inner">The inner exception as reason of this message</param>
        /// <param name="moduleName">The name of the class raising the exception</param>
        public RobotException(Exception inner, string moduleName)
            : base(String.Empty, inner)
        {
            module = moduleName;
        }

        /// <summary>Initializes a new instance of the FaceException</summary>
        /// <param name="message">Exception text</param>
        /// <param name="inner">The inner exception as reason of this message</param>
        /// <param name="moduleName">The name of the class raising the exception</param>
        public RobotException(string message, Exception inner, string moduleName)
            : base(message, inner)
        {
            module = moduleName;
        }

#if !WindowsCE
        /// <summary>Used for serialization of the exception</summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected RobotException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
#endif


    }
}