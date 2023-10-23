using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Act.Lib.Control
{
    public class Percept
    {
        private ObjectInfo objInfo;
        public ObjectInfo ObjInfo
        {
            get { return objInfo; }
            set { objInfo = value; }
        }

        public Percept(ObjectInfo info)
        {
            objInfo = info;
        }
    }


    public class ObjectInfo
    {
        private List<Rect> facesList;
        public List<Rect> FacesList
        {
            get { return facesList; }
            set { facesList = value; }
        }

        //private BrainModules module;
        //public BrainModules Module
        //{
        //    get { return module; }
        //    set { module = value; }
        //}

        //public ObjectInfo(List<Rect> l, BrainModules mod)
        //{
        //    facesList = l;
        //    module = mod;
        //}
    }
}
