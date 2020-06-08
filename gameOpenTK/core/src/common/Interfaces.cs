using OpenTK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace gameOpenTK.common
{
    interface IGetters
    {
        Vector3[] GetVerts();
        Vector3[] GetColorData();
        Vector2[] GetTextureCoords();
        int[] GetIndices(int offset = 0);
    }

    interface ITransformations
    {
        void Scale(bool plus);

        void RotateX(bool dir);
        void RotateY(bool dir);
        void RotateZ(bool dir);

        void TraslateX(bool dir);
        void TraslateY(bool dir);
        void TraslateZ(bool dir);
    }

    interface IChildTransformations
    {
        void ScaleChild(string key, bool plus);

        void RotateXChild(string key, bool dir);
        void RotateYChild(string key, bool dir);
        void RotateZChild(string key, bool dir);

        void TraslateXChild(string key, bool dir);
        void TraslateYChild(string key, bool dir);
        void TraslateZChild(string key, bool dir);
    }

    interface IParamTransformations
    {
        void RotateX(float angle);
        void RotateY(float angle);
        void RotateZ(float angle);

        void TraslateX(float distance);
        void TraslateY(float distance);
        void TraslateZ(float distance);
    }
}
