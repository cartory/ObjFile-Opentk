using gameOpenTK.common;
using OpenTK;
using OpenTK.Graphics.ES20;
using OpenTK.Graphics.OpenGL;
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
    class Object: ISetters, ITransformations, IParamTransformations
    {
        public string name;
        protected HashList<Part> parts;
        protected float step, scale, theta;
        protected Vector3 pos = Vector3.Zero;

        public Object(string name)
        {
            this.name = name;
            parts = new HashList<Part>();
            Part tmp = new Part(null);
            step = tmp.step;
            theta = tmp.theta;
            scale = tmp.scale;
            GC.Collect();
        }

        public void Add(Part part)
        {
            part.step = step;
            part.scale = scale;
            part.theta = theta;
            parts.Add(part.name, part);
        }

        public void Del(string key) => parts.Del(key);

        public Part[] GetArray()
        {
            Part[] array = new Part[parts.Count];
            parts.list.Values.CopyTo(array, 0);
            return array;
        }

        protected void updatePos(float dx, float dz) 
        {
            this.pos += new Vector3(dx, 0, dz);
        }

        public void setPos(float px, float pz) 
        {
            if (pos.X < px)
            {
                while (pos.X < px)
                {
                    pos.X += step;
                    TraslateX(step);
                }
            }
            else
            {
                while (pos.X > px) 
                {
                    pos.X -= step;
                    TraslateX(-step);
                }
            }

            if (pos.Z < pz) 
            {
                while (pos.Z < pz) 
                {
                    pos.Z += step;
                    TraslateZ(step);
                }
            }else
            {
                while (pos.Z > pz)
                {
                    pos.Z -= step;
                    TraslateZ(-step);
                }
            }
            foreach (DictionaryEntry e in parts)
            {
                parts.Get(e.Key).Position= pos;
            }
        }

        #region setters
        public virtual void setScale(float scale)
        {
            this.scale = scale;
            foreach (DictionaryEntry e in parts)
            {
                parts.Get(e.Key).scale = scale;
            }
        }

        public virtual void setStep(float step)
        {
            this.step = step;
            foreach (DictionaryEntry e in parts)
            {
                parts.Get(e.Key).step = step;
            }
        }

        public virtual void setTheta(float theta)
        {
            this.theta = theta;
            foreach (DictionaryEntry e in parts)
            {
                parts.Get(e.Key).theta = theta;
            }
        }
        #endregion

        #region Itransformations
        public virtual void Scale(bool plus)
        {
            foreach (DictionaryEntry e in parts)
            {
                parts.Get(e.Key).Scale(plus);
            }
        }

        public virtual void RotateX(bool dir)
        {
            foreach (DictionaryEntry e in parts)
            {
                parts.Get(e.Key).RotateX(dir);
            }
        }

        public virtual void RotateY(bool dir)
        {
            foreach (DictionaryEntry e in parts)
            {
                parts.Get(e.Key).RotateY(dir);
            }
        }

        public virtual void RotateZ(bool dir)
        {
            foreach (DictionaryEntry e in parts)
            {
                parts.Get(e.Key).RotateZ(dir);
            }
        }

        public virtual void TraslateX(bool dir)
        {
            foreach (DictionaryEntry e in parts)
            {
                parts.Get(e.Key).TraslateX(dir);
            }
        }

        public virtual void TraslateY(bool dir)
        {
            foreach (DictionaryEntry e in parts)
            {
                parts.Get(e.Key).TraslateY(dir);
            }
        }

        public virtual void TraslateZ(bool dir)
        {
            foreach (DictionaryEntry e in parts)
            {
                parts.Get(e.Key).TraslateZ(dir);
            }
        }
        #endregion

        #region IParamTransformations
        public virtual void RotateX(float angle)
        {
            foreach (DictionaryEntry e in parts)
            {
                parts.Get(e.Key).RotateX(angle);
            }
        }

        public virtual void RotateY(float angle)
        {
            foreach (DictionaryEntry e in parts)
            {
                parts.Get(e.Key).RotateY(angle);
            }
        }

        public virtual void RotateZ(float angle)
        {
            foreach (DictionaryEntry e in parts)
            {
                parts.Get(e.Key).RotateZ(angle);
            }
        }

        public virtual void TraslateX(float distance)
        {
            foreach (DictionaryEntry e in parts)
            {
                parts.Get(e.Key).TraslateX(distance);
            }
        }

        public virtual void TraslateY(float distance)
        {
            foreach (DictionaryEntry e in parts)
            {
                parts.Get(e.Key).TraslateY(distance);
            }
        }

        public virtual void TraslateZ(float distance)
        {
            foreach (DictionaryEntry e in parts)
            {
                parts.Get(e.Key).TraslateZ(distance);
            }
        }

        public virtual void Traslate(Vector3 vector) 
        {
            foreach (DictionaryEntry e in parts)
            {
                parts.Get(e.Key).Traslate(vector);
            }
        }
        #endregion
    }
}
