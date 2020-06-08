using gameOpenTK.common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameOpenTK.models
{
    class Object : HashList<Part>, ITransformations, IParamTransformations, IChildTransformations
    {
        public string name;

        protected float step, scale, theta;
        public Object(string name)
        {
            this.name = name;
            Part tmp = new Part(null);
            step = tmp.step;
            scale = tmp.scale;
            theta = tmp.theta;
            GC.Collect();
        }
        public void Add(Part part) => Add(part.name, part);
        public Part[] GetArray()
        {
            Part[] array = new Part[list.Count];
            list.Values.CopyTo(array, 0);
            return array;
        }

        #region IChildTransformations
        public void ScaleChild(string key, bool plus) => ((Part)list[key]).Scale(plus);

        public void RotateXChild(string key, bool dir) => ((Part)list[key]).RotateX(dir);
        public void RotateYChild(string key, bool dir) => ((Part)list[key]).RotateY(dir);
        public void RotateZChild(string key, bool dir) => ((Part)list[key]).RotateZ(dir);

        public void TraslateXChild(string key, bool dir) => ((Part)list[key]).TraslateX(dir);
        public void TraslateYChild(string key, bool dir) => ((Part)list[key]).TraslateY(dir);
        public void TraslateZChild(string key, bool dir) => ((Part)list[key]).TraslateZ(dir);
        #endregion

        #region Itransformations
        public virtual void Scale(bool plus)
        {
            foreach (DictionaryEntry part in list)
            {
                ((Part)list[part.Key]).Scale(plus);
            }
        }

        public virtual void RotateX(bool dir)
        {
            foreach (DictionaryEntry part in list)
            {
                ((Part)list[part.Key]).RotateX(dir);
            }
        }

        public virtual void RotateY(bool dir)
        {
            foreach (DictionaryEntry part in list)
            {
                ((Part)list[part.Key]).RotateY(dir);
            }
        }

        public virtual void RotateZ(bool dir)
        {
            foreach (DictionaryEntry part in list)
            {
                ((Part)list[part.Key]).RotateZ(dir);
            }
        }

        public virtual void TraslateX(bool dir)
        {
            foreach (DictionaryEntry part in list)
            {
                ((Part)list[part.Key]).TraslateX(dir);
            }
        }

        public virtual void TraslateY(bool dir)
        {
            foreach (DictionaryEntry part in list)
            {
                ((Part)list[part.Key]).TraslateY(dir);
            }
        }

        public virtual void TraslateZ(bool dir)
        {
            foreach (DictionaryEntry part in list)
            {
                ((Part)list[part.Key]).TraslateZ(dir);
            }
        }
        #endregion

        #region IParamTransformations
        public virtual void RotateX(float angle)
        {
            foreach (DictionaryEntry entry in list)
            {
                ((Part)list[entry.Key]).RotateX(angle);
            }
        }

        public virtual void RotateY(float angle)
        {
            foreach (DictionaryEntry entry in list)
            {
                ((Part)list[entry.Key]).RotateY(angle);
            }
        }

        public virtual void RotateZ(float angle)
        {
            foreach (DictionaryEntry entry in list)
            {
                ((Part)list[entry.Key]).RotateZ(angle);
            }
        }

        public virtual void TraslateX(float distance)
        {
            foreach (DictionaryEntry entry in list)
            {
                ((Part)list[entry.Key]).TraslateX(distance);
            }
        }

        public virtual void TraslateY(float distance)
        {
            foreach (DictionaryEntry entry in list)
            {
                ((Part)list[entry.Key]).TraslateY(distance);
            }
        }

        public virtual void TraslateZ(float distance)
        {
            foreach (DictionaryEntry entry in list)
            {
                ((Part)list[entry.Key]).TraslateZ(distance);
            }
        }
        #endregion
    }
}
